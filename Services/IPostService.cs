using Yummly.DTO.Post;
using Yummly.Helper;

namespace Yummly.Services
{
    public interface IPostService
    {
        Task<Response<int>>CreatePostAsync(CreatePostDto postDto);
        Task<Response<List<GetUserPostsDto>>> GetUserPostsAsync(string postId);
        Task<Response<bool>> DeletePostAsync(int postId);
    }
}
