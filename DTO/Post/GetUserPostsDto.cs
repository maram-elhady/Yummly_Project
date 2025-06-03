namespace Yummly.DTO.Post
{
    public class GetUserPostsDto
    {
        public string UserId { get; set; }
        public int PostId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Link { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
    }
}
