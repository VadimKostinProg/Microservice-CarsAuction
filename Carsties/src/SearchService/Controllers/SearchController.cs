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
        private readonly ILogger<SearchController> _logger;

        public SearchController(IAuctionsService auctionsService, ILogger<SearchController> logger)
        {
            _auctionsService = auctionsService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<SearchResponse>> SearchItems([FromQuery] SearchParams searchParams)
        {
            var response = await _auctionsService.SearchItemsAsync(searchParams);
            _logger.LogInformation($"--> Search result - page count: {response.PageCount}; total count: {response.TotalCount}");
            return Ok(response);
        }
    }
}
