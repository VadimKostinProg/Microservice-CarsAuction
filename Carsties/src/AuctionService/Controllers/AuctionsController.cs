using AuctionService.DataAccess;
using AuctionService.DTO;
using AuctionService.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Contracts;
using MassTransit;
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
        private readonly IPublishEndpoint _publishEndpoint;

        public AuctionsController(AuctionDbContext context, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<ActionResult<List<AuctionResponse>>> GetAllAuctions([FromQuery] string date)
        {
            var query = _context.Auctions.OrderBy(x => x.Item.Make).AsQueryable();

            var allAuctions = await query.ToListAsync();

            if (!string.IsNullOrEmpty(date))
            {
                query = query.Where(x => x.UpdatedAt > DateTime.Parse(date).ToUniversalTime());

                var auctions = await query.ToListAsync();
            }

            return Ok(await query.ProjectTo<AuctionResponse>(_mapper.ConfigurationProvider).ToListAsync());
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

            var newAuction = _mapper.Map<AuctionResponse>(auction);

            await _publishEndpoint.Publish(_mapper.Map<AuctionCreated>(newAuction));

            var result = await _context.SaveChangesAsync() > 0;

            if (!result)
                return BadRequest("Cannot save changes to the Database.");

            return CreatedAtAction(nameof(GetAuctionById), 
                new { auction.Id }, newAuction);
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

            await _publishEndpoint.Publish(_mapper.Map<AuctionUpdated>(auction));

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

            var auctionDeleted = new AuctionDeleted() { Id = auction.Id.ToString() };

            _context.Auctions.Remove(auction);

            await _publishEndpoint.Publish(auctionDeleted);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return BadRequest("Cannot save changes to the Database.");

            return Ok();
        }
    }
}
