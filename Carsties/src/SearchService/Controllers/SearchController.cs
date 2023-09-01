using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.RequestHelpers;
using SearchService.ServiceContracts;

namespace SearchService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IAuctionsService _auctionsService;

        public SearchController(IAuctionsService auctionsService)
        {
            _auctionsService = auctionsService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Item>>> SearchItems([FromQuery] SearchParams searchParams)
        {
            return Ok(await _auctionsService.SearchItemsAsync(searchParams));
        }
    }
}
