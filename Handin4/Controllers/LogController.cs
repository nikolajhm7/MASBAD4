using Handin4.Models;
using Handin4.Services;
using Handin4.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Handin4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ILogService _logService;

        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpGet("get-all-logs")]
        [Authorize(Policy = "AdminAccess")]
        public async Task<List<LogEntry>> GetAllLogs()
        {
            return await _logService.GetAllLogs();
        }


        [HttpGet("get-logs")]
        [Authorize(Policy = "AdminAccess")]
        public async Task<List<LogEntry>> GetLogs(string? username, string? operation, DateTime? startTime, DateTime? endTime)
        {
            var logs = await _logService.GetAllLogs();

            var filtLogs = logs.Where(log =>
                (string.IsNullOrEmpty(username)) || log.Username == username &&
                (string.IsNullOrEmpty(operation) || log.OperationType == operation) &&
                (!startTime.HasValue || log.TimeStamp >= startTime) &&
                (!endTime.HasValue || log.TimeStamp <= endTime)
            ).ToList();

            return filtLogs;
        }
    }
}
