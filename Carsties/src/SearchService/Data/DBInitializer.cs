using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;
using System.Text.Json;

namespace SearchService.Data
{
    public static class DBInitializer
    {
        public static async Task InitDB(this WebApplication app)
        {
            await DB.InitAsync("SearchDB", MongoClientSettings
                .FromConnectionString(app.Configuration.GetConnectionString("MongoDBConnection")));

            await DB.Index<Item>()
                .Key(x => x.Make, KeyType.Text)
                .Key(x => x.Model, KeyType.Text)
                .Key(x => x.Color, KeyType.Text)
                .CreateAsync();

            // Seed data
            var count = await DB.CountAsync<Item>();

            if (count > 0)
                return;

            var itemsData = await File.ReadAllTextAsync("Data/auctions.json");

            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };

            var items = JsonSerializer.Deserialize<List<Item>>(itemsData, options);

            await DB.SaveAsync(items);
        }
    }
}
