using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MyRecipeApp.Models;
using System.Drawing.Printing;
using Yummly.DTO.Follow;
using Yummly.DTO.Search;
using Yummly.Helper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Yummly.Services
{
    public class SearchService : ISearchService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SearchService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<SearchUserResponseDto>> SearchAsync(string userId, string? search, int page = 1, int pageSize = 10)
        {
            string currentUserId = userId;

            // Get Ids of users the current user follows
            var followingIds = await _context.follows
                .Where(f => f.FollowingUserId == currentUserId)
                .Select(f => f.FollowedUserId)
                .ToListAsync();

            // Get Ids of users who follow the current user
            var followerIds = await _context.follows
                .Where(f => f.FollowedUserId == currentUserId)
                .Select(f => f.FollowingUserId)
                .ToListAsync();

            // all users excluding the current user
            var allMatchedUsersQuery = _context.Users
                .Where(u => u.Id != currentUserId);

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                allMatchedUsersQuery = allMatchedUsersQuery
                    .Where(u => u.FullName.ToLower().Contains(search));
            }

            // Fetch matched users 
            var matchedUsers = await allMatchedUsersQuery
                .Select(u => new SearchUserDto
                {
                    UserId = u.Id,
                    FullName = u.FullName,
                    // UserImage = u.UserImage,
                    IsFollowing = followingIds.Contains(u.Id)
                })
                .ToListAsync();

            // Apply priority sorting (followed by user,followers,rest of users)
            var followedUsers = matchedUsers
                .Where(u => followingIds.Contains(u.UserId))
                .ToList();

            var followersOnly = matchedUsers
                .Where(u => !followingIds.Contains(u.UserId) && followerIds.Contains(u.UserId))
                .ToList();

            var otherUsers = matchedUsers
                .Where(u => !followingIds.Contains(u.UserId) && !followerIds.Contains(u.UserId))
                .ToList();

            var orderedUsers = followedUsers
                .Concat(followersOnly)
                .Concat(otherUsers)
                .ToList();

            var totalCount = orderedUsers.Count;

            if (totalCount == 0)
            {
                return new Response<SearchUserResponseDto>
                {
                    Success = true,
                    Message = "No users found",
                    Data = null
                };
            }

            // Pagination
            var paginatedUsers = orderedUsers
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var responseData = new SearchUserResponseDto
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Users = paginatedUsers
            };

            return new Response<SearchUserResponseDto>
            {
                Success = true,
                Message = "Users retrieved successfully",
                Data = responseData
            };
        }

        public async Task<Response<SearchCategoryResponseDto>> SearchCategoryAsync(int id, int page = 1, int pageSize = 10)
        {
            var query = _context.Posts.Where(p => p.CategoryId == id);

            var totalCount = await query.CountAsync(); 

            if (totalCount == 0)
            {
                return new Response<SearchCategoryResponseDto>
                {
                    Success = false,
                    Message = "No posts found for this category",
                    Data = null
                };
            }

            var posts = await query
                .OrderByDescending(p => p.CreatedAt) 
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/";

            var result = posts.Select(p => new SearchCategoryDto
            {
                Id = p.Id,
                UserId = p.UserId,
                CategoryId = p.CategoryId,
                Title = p.Title,
                Description = p.Description,
                ImageUrl = baseUrl + p.ImageUrl,
                CreatedAt = p.CreatedAt,
                LikeCount = p.LikeCount,
                CommentCount = p.CommentCount
            }).ToList();

            var responseData = new SearchCategoryResponseDto
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Categories = result
            };

            return new Response<SearchCategoryResponseDto>
            {
                Success = true,
                Message = "Posts retrieved successfully" ,
                Data = responseData
            };
        }



    }
}
