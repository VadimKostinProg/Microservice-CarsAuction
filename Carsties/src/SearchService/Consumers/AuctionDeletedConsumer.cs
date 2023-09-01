using AutoMapper;
using Contracts;
using MassTransit;
using SearchService.ServiceContracts;

namespace SearchService.Consumers
{
    public class AuctionDeletedConsumer : IConsumer<AuctionDeleted>
    {
        private readonly ILogger<AuctionDeletedConsumer> _logger;
        private readonly IAuctionsService _auctionsService;

        public AuctionDeletedConsumer(ILogger<AuctionDeletedConsumer> logger, IAuctionsService auctionsService)
        {
            _logger = logger;
            _auctionsService = auctionsService;
        }

        public async Task Consume(ConsumeContext<AuctionDeleted> context)
        {
            _logger.LogInformation("--> Consumed auction deleted:" + context.Message.Id);

            await _auctionsService.DeleteItemAsync(context.Message.Id);
        }
    }
}
