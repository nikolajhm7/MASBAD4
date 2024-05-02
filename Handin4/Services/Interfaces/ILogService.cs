using Handin4.Models;

namespace Handin4.Services.Interfaces
{
    public interface ILogService
    {
        //Task<List<LogEntry>> SearchLogs(string username, DateTime startTime, DateTime endTime, string operationType);
        Task<List<LogEntry>> SearchLogs();
    }
}
