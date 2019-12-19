using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bot.Controllers
{
    [ApiController]
    public class BotController : ControllerBase
    {
        private readonly ILogger<BotController> _logger;

        public BotController(ILogger<BotController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("start")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> Start([FromForm] StartGame start)
        {
            return Ok(start);
        }

        [HttpGet]
        [Route("move")]
        public string Get()
        {
            return "CALL";
        }

        [HttpPost]
        [Route("update")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> Update([FromForm] UpdateGame update)
        {
            return Ok(update);
        }
    }

    public class StartGame
    {
        public string OPPONENT_NAME { get; set; }
        public int STARTING_CHIP_COUNT { get; set; }
        public int HAND_LIMIT { get; set; }
    }

    public class UpdateGame
    {
        public string COMMAND { get; set; }
        public string DATA { get; set; }
    }
}
