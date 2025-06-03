using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Yummly.Models.DbModels;

namespace MyRecipeApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        //any additinal data wrote here
        [MaxLength(100)]
        public string FullName { get; set; }

        //Nav prop
        public ICollection<FollowModel> Followings { get; set; } 
        public ICollection<FollowModel> Followers { get; set; } 
        public ICollection<PostModel> Posts { get; set; } 
        public ICollection<PostLikeModel> PostLikes { get; set; } 
        public ICollection<PostCommentModel> PostComments { get; set; }


    }
}
