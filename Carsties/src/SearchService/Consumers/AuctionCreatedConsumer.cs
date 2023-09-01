using AutoMapper;
using Contracts;
using MassTransit;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.ServiceContracts;

namespace SearchService.Consumers
{
    public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
    {
        private readonly ILogger<AuctionCreatedConsumer> _logger;
        private readonly IAuctionsService _auctionsService;

        public AuctionCreatedConsumer(ILogger<AuctionCreatedConsumer> logger, IAuctionsService auctionsService)
        {
            _logger = logger;
            _auctionsService = auctionsService;
        }

        public async Task Consume(ConsumeContext<AuctionCreated> context)
        {
            _logger.LogInformation("--> Consumed auction created:" + context.Message.Id);

            await _auctionsService.AddNewItemAsync(context.Message);
        }
    }
}