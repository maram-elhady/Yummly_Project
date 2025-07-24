using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using MyRecipeApp.Helper;
using MyRecipeApp.Models;
using System.Configuration;
using System.Text;
using Yummly.Helper;
using Yummly.Services;
using Yummly.Services.Authentication;

namespace MyRecipeApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var configuration = builder.Configuration; //
            // Add services to the container.
            builder.Services.Configure<JWT>(configuration.GetSection("JWT"));//new1 (maps keys to class)
            builder.Services.Configure<EmailConfiguration>(configuration.GetSection("EmailConfiguration"));//new
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders(); //DI
            
            builder.Services.AddMemoryCache();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("cs"))
            );//

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IPostService, PostService>();//
            builder.Services.AddScoped<IActivityService, ActivityService>();//
            builder.Services.AddScoped<IFollowService, FollowService>();
            builder.Services.AddScoped<ISearchService, SearchService>();

            //  builder.Services.AddSingleton(sp => sp.GetRequiredService<IWebHostEnvironment>()); //
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddHttpContextAccessor(); //dynamically construct the full URL


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false; //better to be true to force secure communication
                o.SaveToken = false;
                o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidAudience = configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
                };
            });   //new1
            var key = builder.Configuration["JWT:Key"];
            Console.WriteLine("JWT Key: " + key);
            var app = builder.Build();

            // Configure the HTTP request pipeline.
           // if (app.Environment.IsDevelopment())
           // {
                app.UseSwagger();
                app.UseSwaggerUI();
           // }

            app.UseHttpsRedirection();

            app.UseAuthentication(); //New1 req

            app.UseAuthorization();

            app.UseStaticFiles();

            app.MapControllers();

            app.Run();
        }
    }
}
