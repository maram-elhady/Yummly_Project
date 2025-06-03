using Microsoft.AspNetCore.Mvc;
using Yummly.DTO.Follow;
using Yummly.DTO.Like;
using Yummly.Services;

namespace Yummly.Controllers
{
    public class FollowController : ControllerBase
    {
        private readonly IFollowService _followservice;
        public FollowController(IFollowService followService)
        {
            _followservice = followService;
        }

        [HttpPost("follow/unfollow")]
        public async Task<IActionResult> LikePost([FromBody] FollowDto followDto)
        {
            if (!ModelState.IsValid)
                return StatusCode(400, new { ModelState });

            var result = await _followservice.FollowaAync(followDto);

            if (!result.Success)
                return BadRequest(new { result.Message });

            return StatusCode(200, new { result.Message });
        }

        [HttpGet("user-following-list/{userId}")]
        public async Task<IActionResult> GetFollowingList(string userId)
        {
            var result = await _followservice.GetFollowingListAsync(userId);
            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpGet("user-followers-list/{userId}")]
        public async Task<IActionResult> GetFollowersList(string userId)
        {
            var result = await _followservice.GetFollowersListAsync(userId);
            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpGet("followers-following-numbers/{userId}")]
        public async Task<IActionResult> GetNumbers(string userId)
        {
            if (!ModelState.IsValid)
                return StatusCode(400, new { ModelState });

            var result = await _followservice.GetFollowersAndFollowingNumberAsync(userId);

            if (!result.Success)
                return BadRequest(new { result });

            return StatusCode(200, new { result });
        }


    }
}
