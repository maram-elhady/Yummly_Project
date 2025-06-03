using MyRecipeApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yummly.Models.DbModels
{
    public class PostLikeModel
    {
        public string UserId { get; set; }

        [ForeignKey("PostId")]
        public int PostId { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public PostModel Post { get; set; }
    }
}
