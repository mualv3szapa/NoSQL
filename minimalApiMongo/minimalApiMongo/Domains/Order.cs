using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace minimalApiMongo.Domains
{
    public class Order
    {

        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]

        public string? Id { get; set; }

        [BsonElement("Date")]
        public DateTime Date { get; set; }

        [BsonElement("status")]
        public string? Status { get; set; }

        /// <summary>
        /// Referencia aos produtos do pedido
        /// </summary>

        [BsonElement("productId")]
        public List<string>? ProductId { get; set; }

        //[BsonIgnore]
        public List<Product>? Products { get; set; }

        /// <summary>
        /// Referencia ao cliente que está fazendo o pedido
        /// </summary>

        [BsonElement("clientId")]
        public string? ClientId { get; set; }

        //[BsonIgnore]
        public Client? Client { get; set; }
    }
}
