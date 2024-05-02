using Handin4.Models;

namespace Handin4.Services.Interfaces
{
    public interface ILogService
    {
        Task<List<LogEntry>> GetAllLogs();
    }
}
