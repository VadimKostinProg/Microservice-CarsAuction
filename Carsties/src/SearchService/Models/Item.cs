using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

namespace SearchService.Models
{
    [Serializable]
    public class Item
    {
        [BsonId]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.Int32)]
        public int ReservePrice { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string Seller { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string Winner { get; set; }

        [BsonRepresentation(BsonType.Int32)]
        public int? SoldAmount { get; set; }

        [BsonRepresentation(BsonType.Int32)]
        public int? CurrentHighBid { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedAt { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime UpdatedAt { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime AuctionEnd { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string Status { get; set; }

        //Item Info
        [BsonRepresentation(BsonType.String)]
        public string Make { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string Model { get; set; }

        [BsonRepresentation(BsonType.Int32)]
        public int Year { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string Color { get; set; }

        [BsonRepresentation(BsonType.Int32)]
        public int Mileage { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string ImageUrl { get; set; }
    }
}
