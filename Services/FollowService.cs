using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyRecipeApp.Models;
using Yummly.DTO.Follow;
using Yummly.DTO.Like;
using Yummly.Helper;
using Yummly.Models.DbModels;

namespace Yummly.Services
{
    public class FollowService : IFollowService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public FollowService(ApplicationDbContext context, IMapper mapper = null)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Response<bool>> FollowaAync(FollowDto followDto)
        {
            if(followDto.FollowedUserId == followDto.FollowingUserId)
            {
                return new Response<bool> { Success = false, Message = "You cannot follow yourself." };
            }

            var Following = await _context.Users.AnyAsync(u => u.Id == followDto.FollowingUserId);
            var Followed = await _context.Users.AnyAsync(u => u.Id == followDto.FollowedUserId);

            if (!Following || !Followed )
            {
                return new Response<bool> { Success = false, Message = "User Not Found" };
            }

            var existingFollow = await _context.follows
                              .FirstOrDefaultAsync(f => f.FollowingUserId == followDto.FollowingUserId && f.FollowedUserId == followDto.FollowedUserId);

            if (existingFollow != null)
            {
                _context.follows.Remove(existingFollow);

            }
            else
            {
                var newFollow = _mapper.Map<FollowModel>(followDto);
                _context.follows.Add(newFollow);
            }

            await _context.SaveChangesAsync();
            return new Response<bool>
            {
                Success = true,
                Message = existingFollow != null ? "UnFollowed Successfully" : "Followed Successfully"
            };
        }


        public async Task<Response<List<UsersFullNameDto>>> GetFollowingListAsync(string userId)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
            {
                return new Response<List<UsersFullNameDto>> { Success = false, Message = "User not found", Data = null };
            }

            var followingList = await _context.follows
                .Where(f => f.FollowingUserId == userId) // Find people the user follows
                .Select(f => new UsersFullNameDto
                {
                    FullName = f.FollowedUser.FullName, //the one who being followed by user
                })
                .ToListAsync();

            return new Response<List<UsersFullNameDto>>
            {
                Success = true,
                Message = "Following list retrieved successfully",
                Data = followingList
            };
        }

        public async Task<Response<List<UsersFullNameDto>>> GetFollowersListAsync(string userId)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
            {
                return new Response<List<UsersFullNameDto>> { Success = false, Message = "User not found", Data = null };
            }

            var followersList = await _context.follows
                .Where(f => f.FollowedUserId == userId) 
                .Select(f => new UsersFullNameDto
                {
                    FullName = f.FollowingUser.FullName, //the one who follows the user
                })
                .ToListAsync();

            return new Response<List<UsersFullNameDto>>
            {
                Success = true,
                Message = "Following list retrieved successfully",
                Data = followersList
            };
        }

        public async Task<Response<FollowNumbersDto>> GetFollowersAndFollowingNumberAsync(string userId)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
            {
                return new Response<FollowNumbersDto> { Success = false, Message = "User not found" };
            }

            var followingCount = await _context.follows.CountAsync(f => f.FollowingUserId == userId);
            var followersCount = await _context.follows.CountAsync(f => f.FollowedUserId == userId);

            var followNumbers = new FollowNumbersDto
            {
                FollowingNumbers = followingCount,
                FollowersNumbers = followersCount
            };

            return new Response<FollowNumbersDto>
            {
                Success = true,
                Message = "Following list retrieved successfully",
                Data = followNumbers
            };
        }



    }
}
