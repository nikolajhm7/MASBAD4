using Handin4.Services.Interfaces;
using Handin4.Models;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Handin4.Services
{
    public class LogService : ILogService
    {
        private readonly IMongoCollection<LogEntry> _logCollection;

        public LogService(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("logs");
            _logCollection = database.GetCollection<LogEntry>("log");
        }

        //public async Task<List<LogEntry>> SearchLogs(string username, DateTime startTime, DateTime endTime, string operationType)
        
        public async Task<List<LogEntry>> SearchLogs() =>
            await _logCollection.Find(_ => true).ToListAsync();

        //public async Task<List<LogEntry>> SearchLogs()
        //{
        //    //var filterBuilder = Builders<LogEntry>.Filter;
        //    //var filter = filterBuilder.Empty;

        //    //// Apply filters based on provided parameters
        //    //if (!string.IsNullOrEmpty(username))
        //    //{
        //    //    filter &= filterBuilder.Eq(x => x.Username, username);
        //    //}

        //    ////filter &= filterBuilder.Gte(x => x.Timestamp, startTime);
        //    ////filter &= filterBuilder.Lte(x => x.Timestamp, endTime);

        //    //if (!string.IsNullOrEmpty(operationType))
        //    //{
        //    //    filter &= filterBuilder.Eq(x => x.OperationType, operationType);
        //    //}

        //    // Execute the search query
        //    //var result = await _logCollection.Find(_ => true).ToListAsync();
        //    var result = await _logCollection.Find(_ => true).ToListAsync();
        //    return result;
        //}
    }
}
