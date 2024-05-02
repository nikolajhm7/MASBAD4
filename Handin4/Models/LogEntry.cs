using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Handin4.Models
{
    [BsonIgnoreExtraElements]
    public class LogEntry
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Username")]
        public string Username { get; set; }

        [BsonElement("TimeStamp")]
        public DateTime TimeStamp { get; set; }

        [BsonElement("OperationType")]
        public string OperationType { get; set; }

        public override string ToString()
        {
            return "Entry(" +
                "Id" + Id + "," +
                "Username" + Username + "," +
                "UtcTimeStamp" + TimeStamp + "," +
                "OperationType" + OperationType +
                ")";
        }

    }
}
