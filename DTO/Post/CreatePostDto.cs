using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Yummly.DTO.Post
{
    public class CreatePostDto
    {
        
        [BindNever]
        public int PostId { get; set; }

        [Required(ErrorMessage ="UserId is required")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "CategoryId is required")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [JsonIgnore]
        [BindNever]
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Image is required")]
        public IFormFile ImageFile {  get; set; }

        [Url(ErrorMessage = "Invalid URL format")]
        public string? Link { get; set; }

    }
}
