using Contracts;
using MassTransit;
using SearchService.ServiceContracts;

namespace SearchService.Consumers
{
    public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
    {
        private readonly ILogger<AuctionFinishedConsumer> _logger;
        private readonly IAuctionsService _auctionsService;

        public AuctionFinishedConsumer(ILogger<AuctionFinishedConsumer> logger, IAuctionsService auctionsService)
        {
            _logger = logger;
            _auctionsService = auctionsService;
        }

        public async Task Consume(ConsumeContext<AuctionFinished> context)
        {
            _logger.LogInformation("--> Consumed auction finished" + context.Message.AuctionId);
            await _auctionsService.FinishItemAsync(context.Message);
        }
    }
}
