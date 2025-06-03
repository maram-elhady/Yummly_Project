using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Yummly.DTO.Authentication
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Username is Required"), MaxLength(100)]
        public string FullName { get; set; }

        //[Required(ErrorMessage = "Email is Required"), MaxLength(100), RegularExpression(@"^[^@\s]+@[^@\s]+\.(com)$", ErrorMessage = "Email Is Invalid")]
        [Required(ErrorMessage = "Email is Required"), MaxLength(100), RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Email Is Invalid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is Required"), MaxLength(15), RegularExpression(@"^\+[0-9]{1,3}[0-9]{4,14}$", ErrorMessage = "Phone Number Is Invalid")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password is Required"), MaxLength(100)]
        public string Password { get; set; }
    }
}
