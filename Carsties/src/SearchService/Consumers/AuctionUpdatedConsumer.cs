using AutoMapper;
using Contracts;
using MassTransit;
using SearchService.Models;
using SearchService.ServiceContracts;

namespace SearchService.Consumers
{
    public class AuctionUpdatedConsumer : IConsumer<AuctionUpdated>
    {
        private readonly ILogger<AuctionUpdatedConsumer> _logger;
        private readonly IAuctionsService _auctionsService;

        public AuctionUpdatedConsumer(ILogger<AuctionUpdatedConsumer> logger, IAuctionsService auctionsService)
        {
            _logger = logger;
            _auctionsService = auctionsService;
        }

        public async Task Consume(ConsumeContext<AuctionUpdated> context)
        {
            _logger.LogInformation("--> Consumed auction updated:" + context.Message.Id);

            await _auctionsService.UpdateItemAsync(context.Message);
        }
    }
}
