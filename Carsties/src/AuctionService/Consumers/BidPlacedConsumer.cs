using AuctionService.DataAccess;
using Contracts;
using MassTransit;

namespace AuctionService.Consumers
{
    public class BidPlacedConsumer : IConsumer<BidPlaced>
    {
        private readonly AuctionDbContext _context;
        private readonly ILogger<BidPlacedConsumer> _logger;

        public BidPlacedConsumer(AuctionDbContext context, ILogger<BidPlacedConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<BidPlaced> context)
        {
            _logger.LogInformation("--> Consumed bid placed: " + context.Message.Id);

            var auction = await _context.Auctions.FindAsync(context.Message.AuctionId);

            if (auction.CurrentHighBid == null ||
               (context.Message.BidStatus.Contains("Accepted") && 
                context.Message.Amount > auction.CurrentHighBid))
            {
                auction.CurrentHighBid = context.Message.Amount;
                await _context.SaveChangesAsync();
            }
        }
    }
}
