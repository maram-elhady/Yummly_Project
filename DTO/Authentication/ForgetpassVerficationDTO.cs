using System.ComponentModel.DataAnnotations;

namespace Yummly.DTO.Authentication
{
    public class ForgetpassVerficationDTO
    {
        [Required(ErrorMessage = "This field is Required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field is Required")]
        public string VerficationCode { get; set; }
    }
}
