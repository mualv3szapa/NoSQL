using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace minimalApiMongo.Domains
{
    public class Client
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]

        public string? Id { get; set; }

        [BsonElement("userName")]
        
        public string? UserId { get; set; }

        [BsonElement("cpf")]
        public string? CPF { get; set;}

        [BsonElement("phone")]
        public string? Phone { get; set; }

        [BsonElement("Address")]
        public string? Address { get; set; }

        public Dictionary<string, string> AddicionalAttributes { get; set; }

        public Client()
        {
            AddicionalAttributes = new Dictionary<string, string>();
        }
    }
}
