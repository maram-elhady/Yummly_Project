using System.ComponentModel.DataAnnotations;

namespace Yummly.DTO.Authentication
{
    public class ForgetpasswordDTO
    {
        [Required(ErrorMessage = "This field is Required")]
        public string Email { get; set; }
    }
}
