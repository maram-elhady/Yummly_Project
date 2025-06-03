using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;

namespace Yummly.DTO.Activities
{
    public class AddCommentDto
    {
        [BindNever]
        [JsonIgnore]
        public int CommentId { get; set; }
        public string UserId { get; set; }
        public int PostId { get; set; }
        public string Body { get; set; }
    }
}
