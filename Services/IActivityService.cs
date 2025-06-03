using Yummly.DTO.Activities;
using Yummly.DTO.Like;
using Yummly.Helper;

namespace Yummly.Services
{
    public interface IActivityService
    {
        Task<Response<bool>> LikeAsync(LikeDto likeDto);
        Task<Response<AddCommentDto>> AddCommentAsync(AddCommentDto commentDto);
        Task<Response<string>> EditCommentAsync(EditCommentDto commentDto);
        Task<Response<bool>> DeleteCommentAsync(DeleteCommentDto commentDto);
        
    }
}
