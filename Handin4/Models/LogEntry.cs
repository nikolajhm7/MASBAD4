using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Handin4.Models
{
    [BsonIgnoreExtraElements]
    public class LogEntry
    {
        public LogEntry(string username, DateTime utcTimeStamp, string operationType)
        {
            Username = username;
            UtcTimeStamp = utcTimeStamp;
            OperationType = operationType;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Username")]
        public string Username { get; set; } = "hej";


        [BsonElement("UtcTimeStamp")]
        public DateTime UtcTimeStamp { get; set; } = DateTime.MinValue;

        [BsonElement("OperationType")]
        public string OperationType { get; set; } = "hej";

        public override string ToString()
        {
            return "Entry(" +
                "Id" + Id + "," +
                "Username" + Username + "," +
                "UtcTimeStamp" + UtcTimeStamp + "," +
                "OperationType" + OperationType +
                ")";
        }

    }
}
