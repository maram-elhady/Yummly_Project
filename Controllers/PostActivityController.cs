using Microsoft.AspNetCore.Mvc;
using Yummly.DTO.Activities;
using Yummly.DTO.Like;
using Yummly.Services;

namespace Yummly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostActivityController : ControllerBase
    {
        private readonly IActivityService _activityservice;
        public PostActivityController(IActivityService LikeService)
        {
            _activityservice = LikeService;
        }

        [HttpPost("like/unlike")]
        public async Task<IActionResult> LikePost([FromBody] LikeDto likeDto)
        {
            if (!ModelState.IsValid)
                return StatusCode(400, new { ModelState });

            var result = await _activityservice.LikeAsync(likeDto);

            if (!result.Success)
                return BadRequest(new { result.Message });

            return StatusCode(200, new { result.Message });
        }

        [HttpPost("add-comment")]
        public async Task<IActionResult> AddComment([FromBody] AddCommentDto commentDto)
        {
            if (!ModelState.IsValid)
                return StatusCode(400, new { ModelState });

            var result = await _activityservice.AddCommentAsync(commentDto);

            if (!result.Success)
                return BadRequest(new { result.Message });

            return StatusCode(200, new { result.Message ,result.Data.CommentId,result.Data.Body });
        }

        [HttpPut("edit-comment")]
        public async Task<IActionResult> Editcomment([FromBody] EditCommentDto commentDto)
        {
            if (!ModelState.IsValid)
                return StatusCode(400, new { ModelState });

            var result = await _activityservice.EditCommentAsync(commentDto);

            if (!result.Success)
                return NotFound(new { result.Message });

            return StatusCode(200, new { result.Message, result.Data });
        }


        [HttpDelete("delete-comment")]
        public async Task<IActionResult> Deletecomment([FromBody] DeleteCommentDto commentDto)
        {
            if (!ModelState.IsValid)
                return StatusCode(400, new { ModelState });

            var result = await _activityservice.DeleteCommentAsync(commentDto);

            if (!result.Success)
                return NotFound(new { result.Message });

            return StatusCode(200, new { result.Message });
        }

       
        
    }
}
