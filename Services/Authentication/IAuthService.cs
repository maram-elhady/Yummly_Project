using Yummly.DTO.Authentication;
using Yummly.Helper;

namespace Yummly.Services.Authentication
{
    public interface IAuthService
    {
        Task< AuthModel> RegisterAsync(RegisterDTO model );
        Task<AuthModel> LoginAsync(LoginDTO model);
        Task<string> ForgetPassword(ForgetpasswordDTO model,EmailMessage message);
        Task<string> ForgetPassVerfication(string email,string password);
        Task<AuthModel> ForgetPasssChangeAsync(ForgetPassChangeDTO model);


    }
}
