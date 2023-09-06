using Contracts;
using MassTransit;
using SearchService.ServiceContracts;

namespace SearchService.Consumers
{
    public class BidPlacedConsumer : IConsumer<BidPlaced>
    {
        private readonly ILogger<BidPlacedConsumer> _logger;
        private readonly IAuctionsService _auctionsService;

        public BidPlacedConsumer(ILogger<BidPlacedConsumer> logger, IAuctionsService auctionsService)
        {
            _logger = logger;
            _auctionsService = auctionsService;
        }

        public async Task Consume(ConsumeContext<BidPlaced> context)
        {
            _logger.LogInformation("--> Consumed bid placed: " + context.Message.Id);
            await _auctionsService.PlaceBidAsync(context.Message);
        }
    }
}
