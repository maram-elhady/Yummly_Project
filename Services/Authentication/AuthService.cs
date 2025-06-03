using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using MyRecipeApp.Helper;
using MyRecipeApp.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Yummly.DTO.Authentication;
using Yummly.Helper;

namespace Yummly.Services.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;
        private readonly EmailConfiguration _emailConfig;
        private readonly IMemoryCache _cache;
        public AuthService(UserManager<ApplicationUser> userManager, IOptions<JWT> jwt, RoleManager<IdentityRole> roleManager, IOptions<EmailConfiguration> emailconfig, IMemoryCache memoryCache)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
            _roleManager = roleManager;
            _emailConfig = emailconfig.Value;
            _cache = memoryCache;
        }
        public async Task<AuthModel> RegisterAsync(RegisterDTO model)
        {
            if(await _userManager.FindByEmailAsync(model.Email) is not null) 
                return new AuthModel {Message="Email is already registered"};

            var user = new ApplicationUser
            {
                UserName = Guid.NewGuid().ToString(),
                FullName = model.FullName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
            };

            var result= await _userManager.CreateAsync(user,model.Password);
            if(!result.Succeeded)
            {
                var errors=string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description},";  
                }
               return new AuthModel { Message=errors};
            }
            await _userManager.AddToRoleAsync(user,"User");

            var JwtSecurityToken = await CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            return new AuthModel
            {
                Email = model.Email,
                UserId= await _userManager.GetUserIdAsync(user),
                ExpiresOn = JwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = rolesList.ToList(),
                Token = new JwtSecurityTokenHandler().WriteToken(JwtSecurityToken),
               // UserName = user.UserName
            };
        }


        public async Task<AuthModel> LoginAsync(LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return new AuthModel { Message = "Email Or Password is Incorrect" };
            
            var JwtSecurityToken = await CreateJwtToken(user);
            return new AuthModel
            {
                Email = model.Email,
                UserId = await _userManager.GetUserIdAsync(user),
                ExpiresOn = JwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(JwtSecurityToken),
               // UserName = user.UserName

            };
        }


        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            
            if (string.IsNullOrEmpty(_jwt.Key))
            {
                throw new ArgumentNullException(nameof(_jwt.Key), "JWT Key is missing in the configuration.");
            }

            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        public async Task<string> ForgetPassword(ForgetpasswordDTO model,EmailMessage message)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null )
                return  "Email is Incorrect" ;

            var verificationCode = new Random().Next(1000, 9999).ToString();

            string passwordCacheKey= "ForgetPassword:"+user.Email;

            var cacheEntryOptions=new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10))
                .SetAbsoluteExpiration(TimeSpan.FromHours(1)).SetPriority(CacheItemPriority.Normal);

            _cache.Set(passwordCacheKey, verificationCode,cacheEntryOptions);

            var emailMessage = CreateEmailMessage(model.Email,message,verificationCode);

            try
            {
                await Send(emailMessage);
            }
            catch
            {
                return "Failed to send verification email. Please try again.";
            }
            return "Verfication Code sent.Please Check Your Email";
        }


        public async Task<string> ForgetPassVerfication(string email,string code)
        {
            var cacheKey = "ForgetPassword:" + email;
            if (!_cache.TryGetValue(cacheKey, out string? cachedCode))
                return "Verfication code or Requset is expired";

            if (cachedCode != code)
                return "Incorrect Verfication Code";

            string emailCacheKey = "email:" + email;
            string resetToken = Guid.NewGuid().ToString();
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(20))
                .SetAbsoluteExpiration(TimeSpan.FromHours(1)).SetPriority(CacheItemPriority.Normal);

            _cache.Set(emailCacheKey, resetToken, cacheEntryOptions);

            return "Email Verfied Succefully";
        }

        public async Task<AuthModel> ForgetPasssChangeAsync(ForgetPassChangeDTO model)
        {
            var cacheKey = "email:" + model.Email;
            if (!_cache.TryGetValue(cacheKey, out string? cachedEmail))
                return new AuthModel { Message = "Requset Is Expired" };


            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return new AuthModel { Message = "User not found." };

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user); //
            var result = await _userManager.ResetPasswordAsync(user, resetToken,model.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(",", result.Errors.Select(e => e.Description));
                return new AuthModel { Message = errors };
            }

            _cache.Remove(resetToken);
            var JwtSecurityToken = await CreateJwtToken(user);
           // var rolesList = await _userManager.GetRolesAsync(user);
            return new AuthModel
            {
               IsAuthenticated = true,
               Message= "Your password has been reset successfully."
            };
        }




        private MimeMessage CreateEmailMessage(string email, EmailMessage message,string verificationCode)
        {
            
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Yummly",_emailConfig.From));
            emailMessage.To.Add(new MailboxAddress("",email));
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text =  $"<!DOCTYPE html>\r\n<html>\r\n<head>\r\n   " +
                $" <title>Verification Code</title>\r\n    <style>\r\n       " +
                $" body {{\r\n            " +
                $"font-family: Arial, sans-serif;\r\n           " +
                $" background-color: #f8f9fa;\r\n            " +
                $"margin: 0;\r\n           " +
                $" padding: 0;\r\n        }}\r\n        ." +
                $"container {{\r\n            " +
                $"max-width: 500px;\r\n          " +
                $"  margin: 20px auto;\r\n           " +
                $" background: #fff;\r\n            " +
                $"padding: 20px;\r\n            " +
                $"border-radius: 10px;\r\n          " +
                $"  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);\r\n           " +
                $" text-align: center;\r\n        }}\r\n        " +
                $".logo {{\r\n            width: 80px;\r\n        }}\r\n  " +
                $"      h2 {{\r\n            color: #333;\r\n        }}\r\n        " +
                $".code {{\r\n            display: inline-block;\r\n           " +
                $" font-size: 24px;\r\n           " +
                $" font-weight: bold;\r\n         " +
                $"   color: #fff;\r\n           " +
                $" background: #ff6b6b;\r\n           " +
                $" padding: 10px 20px;\r\n           " +
                $" border-radius: 8px;\r\n       " +
                $"     letter-spacing: 3px;\r\n    " +
                $"        margin: 10px 0;\r\n        }}\r\n      " +
                $"  p {{\r\n       " +
                $"     color: #555;\r\n        }}\r\n       " +
                $" .footer {{\r\n          " +
                $"  font-size: 12px;\r\n            " +
                $"color: #888;\r\n           " +
                $" margin-top: 20px;\r\n        }}\r\n   " +
                $" </style>\r\n</head>\r\n<body>\r\n    " +
                $"<div class=\"container\">\r\n       " +
                $" <img src=\"https://cdn-icons-png.flaticon.com/512/1057/1057240.png\" " +
                $"alt=\"Lock Icon\" class=\"logo\">\r\n      " +
                $"  <h2>🔐 Your Verification Code</h2>\r\n     " +
                $"   <p>Hello,</p>\r\n       " +
                $" <p>Your verification code for Yummly is:</p>\r\n       " +
                $" <div class=\"code\">{verificationCode}</div>\r\n      " +
                $"  <p>This code is valid for 10 minutes. Please do not share it with anyone.</p>\r\n       " +
                $" <div class=\"footer\">If you did not request this, please ignore this email.</div>\r\n " +
                $"   </div>\r\n</body>\r\n</html>\r\n" };
            
            return emailMessage;
        }
        private async Task Send(MimeMessage mailMessage)
        {
        
                using (var client = new SmtpClient())
                {
                    try
                    {
                        await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                        client.AuthenticationMechanisms.Remove("XOAUTH2");
                        await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);
                        await client.SendAsync(mailMessage);
                    }
                    catch
                    {

                     Console.WriteLine("Failed to send verification email. Please try again.");
                    }
                    finally
                    {
                        await client.DisconnectAsync(true);
                        client.Dispose();
                    }
                }
            


        }




    }
}
