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

            await DB.InitAsync("SearchDB", MongoClientSettings
                .FromConnectionString(app.Configuration.GetConnectionString("MongoDBConnection")));

            await DB.Index<Item>()
                .Key(x => x.Make, KeyType.Text)
                .Key(x => x.Model, KeyType.Text)
                .Key(x => x.Color, KeyType.Text)
                .CreateAsync();

            // Seed data
            var count = await DB.CountAsync<Item>();

            if (count == 0)
            {
                var itemsData = await File.ReadAllTextAsync("Data/auctions.json");

                var options = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                };

                var items = JsonSerializer.Deserialize<List<Item>>(itemsData, options);

                await DB.SaveAsync(items);

                logger.LogInformation("Data base has been initialized successfully.");
            }

            // Adding last updated items
            var lastUpdatedDate = await DB.Find<Item, string>()
                .Sort(x => x.Descending(x => x.UpdatedAt))
                .Project(x => x.UpdatedAt.ToString())
                .ExecuteFirstAsync();

            var auctionService = scope.ServiceProvider.GetRequiredService<IAuctionsService>();

            var newItems = await auctionService.GetAuctions(lastUpdatedDate);

            if (newItems.Any())
            {
                int newItemsCount = newItems.Count();
                logger.LogInformation($"New {newItemsCount} items has been inserted to the data base successfully.");

                await DB.SaveAsync(newItems);
            }
        }
    }
}
