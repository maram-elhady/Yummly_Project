using MyRecipeApp.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yummly.Models.DbModels
{
    public class PostModel
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }

        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }
        public string Title { get; set; }
       // public string Ingredients { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Link { get; set; }
        public DateTime CreatedAt { get; set; }= DateTime.UtcNow;
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }

        // Nav Prop
        
        public ApplicationUser User { get; set; }
        public ICollection<PostLikeModel> PostLikes { get; set; } 
        public ICollection<PostCommentModel> PostComments { get; set; } 
        public CategoryModel Category { get; set; }
    }
}
