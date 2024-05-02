using Handin4.Services.Interfaces;
using Handin4.Models;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Handin4.Services
{
    public class LogService : ILogService
    {
        private readonly IMongoCollection<BsonDocument> _logCollection;

        public LogService(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("logs");
            _logCollection = database.GetCollection<BsonDocument>("log");
        }

        public async Task<List<LogEntry>> GetAllLogs()
        {
            var logs = await _logCollection.Find(new BsonDocument()).ToListAsync();
            var logEntries = new List<LogEntry>();

            foreach (var log in logs)
            {
                if (log["Properties"].IsBsonDocument &&
                    log["Properties"].AsBsonDocument.Contains("LogInfo") &&
                    log["Properties"]["LogInfo"].IsBsonDocument)
                {
                    var logInfo = log["Properties"]["LogInfo"].AsBsonDocument;

                    if (logInfo.Contains("TimeStamp") && logInfo["TimeStamp"] is BsonDateTime timestampBson)
                    {
                        var timestamp = timestampBson.ToUniversalTime(); // Convert to DateTime
                        var operationtype = logInfo.Contains("OperationType") && !logInfo["OperationType"].IsBsonNull ? logInfo["OperationType"].AsString : "";
                        var username = logInfo.Contains("Username") && !logInfo["Username"].IsBsonNull ? logInfo["Username"].AsString : "";

                        var logEntry = new LogEntry
                        {
                            TimeStamp = timestamp,
                            OperationType = operationtype,
                            Username = username,
                        };
                        logEntries.Add(logEntry);
                    }
                }
            }
            return logEntries;
        }
    }
}
