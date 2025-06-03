using Microsoft.AspNetCore.Mvc;
using System.Web.Http.ModelBinding;
using Yummly.DTO.Authentication;
using Yummly.Helper;
using Yummly.Services.Authentication;

namespace Yummly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDTO model)
        {
            if (!ModelState.IsValid)
                return StatusCode(400, new { ModelState });

            var result = await _authService.RegisterAsync(model);

            if (!result.IsAuthenticated)
                return StatusCode(400, new { result.Message });

            return StatusCode(200, new { result });
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO model)
        {
            if (!ModelState.IsValid)
                return StatusCode(400, new { ModelState });

            var result = await _authService.LoginAsync(model);

            if (!result.IsAuthenticated)
                return StatusCode(400, new { result.Message });

            return StatusCode(200, new { result });
        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPasswordAsync([FromBody] ForgetpasswordDTO model)
        {
            if (!ModelState.IsValid)
                return StatusCode(400, new { ModelState });
            var message = new EmailMessage(new string[] { "Yummly102.com" }, "Verify Your Email", "This is the content from our email.");
            var result = await _authService.ForgetPassword(model, message);

            if (result .StartsWith( "Email is Incorrect"))
                return BadRequest(new { Message = result });

            if (result .StartsWith( "Failed to send verification email. Please try again."))
                return BadRequest(new { Message = result });

            return StatusCode(200, new { result });
        }

        [HttpPost("ForgetPassVerfication")]
        public async Task<IActionResult> ForgetPassVerficationAsync([FromBody] ForgetpassVerficationDTO model)
        {
            if (!ModelState.IsValid)
                return StatusCode(400, new { ModelState });
            
            var result = await _authService.ForgetPassVerfication(model.Email,model.VerficationCode);

            if (result .StartsWith("Verfication code or Requset is expired"))
                return BadRequest(new { Message = result });

            if (result.StartsWith( "Incorrect Verfication Code"))
                return BadRequest(new { Message = result });

            return StatusCode(200, new { result });
        }

        [HttpPut("ForgetPasschange")]
        public async Task<IActionResult> ForgetPassChangeAsync([FromBody] ForgetPassChangeDTO model)
        {
            if (!ModelState.IsValid)
                return StatusCode(400, new { ModelState });

            var result = await _authService.ForgetPasssChangeAsync(model);

            if (!result.IsAuthenticated)
                return StatusCode(400, new { result.Message });

            return StatusCode(200, new { result.Message });
        }
    }
}
