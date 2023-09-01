using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.ServiceContracts;
using System.Text.Json;

namespace SearchService.Data
{
    public static class DBInitializer
    {
        public static async Task InitDB(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var logger = scope.ServiceProvider.GetRequiredService<ILogger<WebApplication>>();

            var context = scope.ServiceProvider.GetRequiredService<AuctionsContext>();

            // Seed data
            var isDataBaseFilled = context.Items.Find(x => true).Any();

            if (!isDataBaseFilled)
            {
                var itemsData = await File.ReadAllTextAsync("Data/auctions.json");

                var options = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                };

                var items = JsonSerializer.Deserialize<List<Item>>(itemsData, options);

                await context.Items.InsertManyAsync(items);

                logger.LogInformation("Data base has been initialized successfully.");
            }

            // Adding last updated items
            var lastUpdatedDate = context.Items.Find(x => true)
                .ToList()
                .Max(x => x.UpdatedAt)
                .ToString();

            var syncCommunicationService = scope.ServiceProvider.GetRequiredService<ISyncServiceCommunicator>();

            var newItems = await syncCommunicationService.GetNewestAuctionsAsync(lastUpdatedDate);

            if (newItems.Any())
            {
                int newItemsCount = newItems.Count();
                logger.LogInformation($"New {newItemsCount} items has been inserted to the data base successfully.");

                foreach(var item in newItems)
                {
                    if(context.Items.Find(x => x.Id == item.Id).Any())
                    {
                        await context.Items.ReplaceOneAsync(Builders<Item>.Filter.Eq(x => x.Id, item.Id), item);
                    }
                    else
                    {
                        await context.Items.InsertOneAsync(item);
                    }
                }
            }
        }
    }
}
