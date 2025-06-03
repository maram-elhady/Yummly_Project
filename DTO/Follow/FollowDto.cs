using System.ComponentModel.DataAnnotations;

namespace Yummly.DTO.Follow
{
    public class FollowDto
    {
        public string FollowingUserId { get; set; }
        public string FollowedUserId { get; set; }

    }
}
