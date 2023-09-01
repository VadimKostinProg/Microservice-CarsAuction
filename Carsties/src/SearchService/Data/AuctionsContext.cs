using MongoDB.Driver;
using SearchService.Models;

namespace SearchService.Data
{
    public class AuctionsContext
    {
        public IMongoCollection<Item> Items { get; set; }

        public AuctionsContext(IConfiguration configuration)
        {
            // Initializing data base
            string connectionString = configuration.GetConnectionString("MongoDbConnection");

            var mongoUrl = MongoUrl.Create(connectionString);
            var mongoClient = new MongoClient(mongoUrl);
            var dataBase = mongoClient.GetDatabase(mongoUrl.DatabaseName);

            Items = dataBase.GetCollection<Item>("items");

            // Adding search indexes
            var keys = Builders<Item>.IndexKeys
                .Text("Make")
                .Text("Model")
                .Text("Color");

            var indexModel = new CreateIndexModel<Item>(keys);
            Items.Indexes.CreateOne(indexModel);
        }
    }
}
