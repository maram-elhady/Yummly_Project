using MyRecipeApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yummly.Models.DbModels
{
    public class PostCommentModel
    {
        public int CommentId { get; set; }
        public string UserId { get; set; }

        [ForeignKey("PostId")]
        public int PostId { get; set; }
        public string Body { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public PostModel Post { get; set; }
    }
}
