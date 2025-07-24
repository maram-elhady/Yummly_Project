using Yummly.DTO.Search;
using Yummly.Helper;

namespace Yummly.Services
{
    public interface ISearchService
    {
        Task<Response<SearchUserResponseDto>> SearchAsync(string userId, string? search, int page = 1, int pageSize = 10);
        Task<Response<SearchCategoryResponseDto>> SearchCategoryAsync(int id, int page = 1, int pageSize = 10);
    }
}
