using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyRecipeApp.Models;
using System.Drawing.Printing;
using Yummly.DTO.Search;
using Yummly.Services;

namespace Yummly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ISearchService _searchservice;
        public SearchController(ApplicationDbContext context, ISearchService searchService)
        {
            _context = context;
            _searchservice = searchService;
        }

        [HttpGet("search-users")]
        public async Task<IActionResult> SearchUsers( string userId,string? search, int page = 1, int pageSize = 10)
        {
            if (!ModelState.IsValid)
                return StatusCode(400, new { ModelState });

            var result = await _searchservice.SearchAsync(userId, search, page , pageSize );

            return StatusCode(200, new { result });
        }


        //[HttpGet("search-recipes")]
        //public async Task<IActionResult> Searchrecipes()
        //{


        //    return StatusCode(200, new {  });
        //}
        [HttpGet("search-categories") ]
        public async Task<IActionResult> Searchrecipes(int id, int page = 1, int pageSize = 10)
        {
            if (!ModelState.IsValid)
                return StatusCode(400, new { ModelState });

            var result = await _searchservice.SearchCategoryAsync(id,page,pageSize);


            return StatusCode(200, new {result });
        }
    }
}
