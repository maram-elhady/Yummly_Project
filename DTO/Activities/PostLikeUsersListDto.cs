using Yummly.DTO.Like;

namespace Yummly.DTO.Activities
{
    public class PostLikeUsersListDto
    {
        public List<LikesDto> LikeList { get; set; }
        public List<CommentsDto> CommentList { get; set; }

        public class LikesDto
        {
            public string UserId { get; set; }
        }

        public class CommentsDto
        {
            public string UserId { get; set; }
            public string Content { get; set; }
        }

    }
}
