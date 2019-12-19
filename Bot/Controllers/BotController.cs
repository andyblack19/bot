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
        public async Task<IActionResult> Start(StartGame startGame)
        {
            return Ok(startGame);
        }

        [HttpGet]
        [Route("move")]
        public string Get()
        {
            return "CALL";
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update()
        {
            return Ok();
        }
    }

    public class StartGame
    {
        [JsonPropertyName("OPPONENT_NAME")]
        public string OpponentName { get; set; }

        [JsonPropertyName("STARTING_CHIP_COUNT")]
        public int ChipCount { get; set; }

        [JsonPropertyName("HAND_LIMIT")]
        public int HandLimit { get; set; }
    }
}
