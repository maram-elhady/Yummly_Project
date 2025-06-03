using System.ComponentModel.DataAnnotations;

namespace Yummly.DTO.Authentication
{
    public class ForgetPassChangeDTO
    {
        [Required(ErrorMessage = "This field is Required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field is Required")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "This field is Required")]
        [Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
