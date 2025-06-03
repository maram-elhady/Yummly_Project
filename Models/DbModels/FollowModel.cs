using MyRecipeApp.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Yummly.Models.DbModels
{
    public class FollowModel
    {
        [Key]
        public string FollowingUserId { get; set; }

        [Key]
        public string FollowedUserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        //Nav prop
        [ForeignKey("FollowingUserId")]
        public ApplicationUser FollowingUser { get; set; } //one who follow

        [ForeignKey("FollowedUserId")]
        public ApplicationUser FollowedUser { get; set; } //one being followed
    }
}
