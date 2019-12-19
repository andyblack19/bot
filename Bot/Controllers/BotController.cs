using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bot.Controllers
{
    [ApiController]
    public class BotController : ControllerBase
    {
        private static Game CurrentGame;

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
            CurrentGame = new Game(start.STARTING_CHIP_COUNT, start.HAND_LIMIT,start.OPPONENT_NAME);

            return Ok(start);
        }

        [HttpGet]
        [Route("move")]
        public string Get()
        {
            return CurrentGame.Move();
        }

        [HttpPost]
        [Route("update")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> Update([FromForm] UpdateGame update)
        {
            if (update.COMMAND == "CARD")
            {
                CurrentGame.SetCard(update.DATA);
            }

            if (update.COMMAND == "RECEIVE_BUTTON")
            {
                CurrentGame.ReceiveButton();
            }

            if (update.COMMAND == "POST_BLIND")
            {
                CurrentGame.PostBlind();
            }
            
            if (update.COMMAND == "OPPONENT_MOVE")
            {
                CurrentGame.OpponentMove(update.DATA);
            }
            
            if (update.COMMAND == "RECEIVE_CHIPS")
            {
                CurrentGame.ReceiveChips(update.DATA);
            }
            
            if (update.COMMAND == "OPPONENT_CARD")
            {
                CurrentGame.OpponentCard(update.DATA);
            }

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
