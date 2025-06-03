using System.ComponentModel.DataAnnotations;

namespace Yummly.DTO.Authentication
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email is Required"), MaxLength(100), RegularExpression(@"^[^@\s]+@[^@\s]+\.(com)$", ErrorMessage = "Email Is Invalid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Required"), MaxLength(100)]
        public string Password { get; set; }
    }
}
