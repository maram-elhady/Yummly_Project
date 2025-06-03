using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyRecipeApp.Models;
using Yummly.DTO.Post;
using Yummly.Helper;
using Yummly.Models.DbModels;

namespace Yummly.Services
{
    public class PostService : IPostService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public PostService(ApplicationDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Response<int>> CreatePostAsync(CreatePostDto postDto)
        {
            if (postDto == null)
                return new Response<int> { Success = false, Message = "Invalid post data" };

            var category = await _context.Categories.FindAsync(postDto.CategoryId);
            if (category == null)
                return new Response<int> { Success = false, Message = "Category not found" };

            if (postDto.ImageFile != null)
            {
                var result = await UploadImageAsync(postDto.ImageFile);
                if (!result.Status)
                    return new Response<int> { Success = false, Message = result.Message };
                postDto.ImageUrl = result.ImagePath;
            }

            var newPost = _mapper.Map<PostModel>(postDto);
            newPost.CreatedAt = DateTime.UtcNow;

            _context.Posts.Add(newPost);
            await _context.SaveChangesAsync();
            return new Response<int> { Success = true, Message = "Post created successfully", Data = newPost.Id };
        }

        public async Task<Response<List<GetUserPostsDto>>> GetUserPostsAsync(string userId)
        {
            var posts = await _context.Posts
                        .Where(p => p.UserId == userId)
                        .ToListAsync();

            if (!posts.Any())
                return new Response<List<GetUserPostsDto>>
                {
                    Success = false,
                    Message = "No posts found for this user",
                    Data = null
                };
            var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/";

            var getPosts = _mapper.Map<List<GetUserPostsDto>>(posts, opt =>
            {
                opt.Items["BaseUrl"] = baseUrl;
            }); ;

            return new Response<List<GetUserPostsDto>> { Success = true, Message = "Posts Retrived successfully", Data = getPosts };
        }

        public async Task<Response<bool>> DeletePostAsync(int postId)
        {
            var post = await _context.Posts.FindAsync(postId);

            if (post == null)
                return new Response<bool> { Success = false, Message = "Post not found", Data = false };

            _context.Posts.Remove(post); 
            await _context.SaveChangesAsync();

            return new Response<bool> { Success = true, Message = "Post deleted successfully", Data = true };
        }



        public async Task<PhotoResponse> UploadImageAsync(IFormFile imageFile)
        {
            try
            {
                if (imageFile == null || imageFile.Length == 0)
                    return new PhotoResponse { Status = false, Message = "No file uploaded." };

                // extensions
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var fileExtension = Path.GetExtension(imageFile.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                    return new PhotoResponse { Status = false, Message = "Invalid file type. Only JPG, JPEG, PNG, and WEBP are allowed." };

                // Size
                long maxFileSize = 5 * 1024 * 1024; // 5MB
                if (imageFile.Length > maxFileSize)
                    return new PhotoResponse { Status = false, Message = "File size exceeds the maximum limit of 5MB." };


                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Yummly Posts");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                // unique file name
                string fileName = $"{Guid.NewGuid()}{fileExtension}";
                string imagePath = Path.Combine("Yummly Posts", fileName);
                string fileFullPath = Path.Combine(uploadsFolder, fileName);

                // Save file
                using (var fileStream = new FileStream(fileFullPath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                return new PhotoResponse
                {
                    Status = true,
                    ImagePath = imagePath,
                };
            }
            catch (Exception ex)
            {
                return new PhotoResponse { Status = false, Message = $"File upload failed: {ex.Message}" };
            }
        }

        




    }
}
