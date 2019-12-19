using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bot.Controllers
{
    [ApiController]
    [Route("")]
    public class BotController : ControllerBase
    {
        private readonly ILogger<BotController> _logger;

        public BotController(ILogger<BotController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public Response Get()
        {
            return new Response();
        }
    }
}
