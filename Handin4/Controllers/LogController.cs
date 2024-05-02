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

        [HttpGet("search-logs")]
        [Authorize(Policy = "AdminAccess")]
        public async Task<ActionResult> SearchLogs()
        {

            var logs = await _logService.SearchLogs();
            logs.ToString();
            return Ok(logs);
        }
    }
}
