using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyRecipeApp.Models;
using Yummly.DTO.Post;
using Yummly.Services;

namespace Yummly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IPostService _postservice;
        public PostController(ApplicationDbContext context,IPostService postService)
        {
            _context = context;
            _postservice = postService;
        }

        [HttpGet("categorylist")]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        {
            var category = await _context.Categories
                                              .Select(c => new CategoryDto { Id=c.Id,Name=c.Name,photo=c.Photo})
                                              .ToListAsync();
            return StatusCode(200,category);
        }

        [HttpPost("createpost")]
        public async Task<IActionResult> CreatePost([FromForm]CreatePostDto postDto)
        {
            if (!ModelState.IsValid)
                return StatusCode(400, new { ModelState });

            var result = await _postservice.CreatePostAsync(postDto);

            if (!result.Success)
                return BadRequest(new { result.Message });

            return StatusCode(200, new { result.Message,result.Data });
        }

        [HttpGet("get-user-posts/{userId}")]
        public async Task<IActionResult> GetPost([FromRoute] string userId)
        {
            if (!ModelState.IsValid)
                return StatusCode(400, new { ModelState });

            var result = await _postservice.GetUserPostsAsync(userId);

            if (!result.Success)
                return BadRequest(new { result.Message });

            return StatusCode(200, new { result.Message, result.Data });
        }

        [HttpDelete("deletepost/{postId}")]
        public async Task<IActionResult> DeletePost([FromRoute] int postId)
        {
            if (!ModelState.IsValid)
                return StatusCode(400, new { ModelState });

            var result = await _postservice.DeletePostAsync(postId);

            if(!result.Success)
                return NotFound(new { result.Message });

            return StatusCode(200, new { result.Message });
        }


    }
}
