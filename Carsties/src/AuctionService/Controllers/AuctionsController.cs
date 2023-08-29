using AuctionService.DataAccess;
using AuctionService.DTO;
using AuctionService.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionsController : ControllerBase
    {
        private readonly AuctionDbContext _context;
        private readonly IMapper _mapper;

        public AuctionsController(AuctionDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<AuctionResponse>>> GetAllAuctions()
        {
            var auctions = await _context.Auctions
                .Include(x => x.Item)
                .OrderBy(x => x.Item.Make)
                .ToListAsync();

            return Ok(_mapper.Map<List<AuctionResponse>>(auctions));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuctionResponse>> GetAuctionById([FromRoute] Guid id)
        {
            var auction = await _context.Auctions
                .Include(x => x.Item)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (auction is null)
                return NotFound();

            return Ok(_mapper.Map<AuctionResponse>(auction));
        }

        [HttpPost]
        public async Task<ActionResult<AuctionResponse>> AddAuction([FromBody] AuctionAddRequest request)
        {
            var auction = _mapper.Map<Auction>(request);

            auction.Seller = "text seller";

            _context.Auctions.Add(auction);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result)
                return BadRequest("Cannot save changes to the Database.");

            return CreatedAtAction(nameof(GetAuctionById), 
                new { auction.Id }, _mapper.Map<AuctionResponse>(auction));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAuction([FromRoute] Guid id, [FromBody] AuctionUpdateRequest request)
        {
            var auction = await _context.Auctions
                .Include(x => x.Item)
                .FirstOrDefaultAsync(x => x.Id == id);

            if(auction is null) return NotFound();

            //TODO: check seller == username

            auction.Item.Make = request.Make ?? auction.Item.Make;
            auction.Item.Model = request.Model ?? auction.Item.Model;
            auction.Item.Color = request.Color ?? auction.Item.Color;
            auction.Item.Mileage = request.Mileage ?? auction.Item.Mileage;
            auction.Item.Year = request.Year ?? auction.Item.Year;

            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return BadRequest("Cannot save changes to the Database.");

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAuction([FromRoute] Guid id)
        {
            var auction = await _context.Auctions.FindAsync(id);

            if(auction is null) return NotFound();

            // TODO: check seller == username

            _context.Auctions.Remove(auction);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return BadRequest("Cannot save changes to the Database.");

            return Ok();
        }
    }
}
