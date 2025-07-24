using System.ComponentModel.DataAnnotations.Schema;

namespace Yummly.DTO.Search
{
    public class SearchCategoryDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
         //public string Link { get; set; }
        public DateTime CreatedAt { get; set; } 
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
    }
}
