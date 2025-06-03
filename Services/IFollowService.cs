using Yummly.DTO.Follow;
using Yummly.Helper;

namespace Yummly.Services
{
    public interface IFollowService
    {
        Task<Response<bool>> FollowaAync(FollowDto followDto);
        Task<Response<List<UsersFullNameDto>>> GetFollowingListAsync(string userId);
        Task<Response<List<UsersFullNameDto>>> GetFollowersListAsync(string userId);
        Task<Response<FollowNumbersDto>> GetFollowersAndFollowingNumberAsync(string userId);
    }
}
