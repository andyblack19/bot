using System.Collections.Generic;
using Serilog;

namespace Bot
{
    public class Game
    {
        private int _chipCount;
        private readonly int _handLimit;
        private string _card;
        private string _opponentMove;

        private readonly Dictionary<string, double> _ratio = new Dictionary<string, double>
        {
            {"2", 0.0},
            {"3", 0.0},
            {"4", 0.0},
            {"5", 0.0},
            {"6", 0.0},
            {"7", 0.05},
            {"8", 0.07},
            {"9", 0.08},
            {"T", 0.1},
            {"J", 0.3},
            {"Q", 0.5},
            {"K", 0.7},
            {"A", 1.0}
        };

        public Game(int chipCount, int handLimit, string opponentName)
        {
            _chipCount = chipCount;
            _handLimit = handLimit;
            Log.Information($"New game started against: {opponentName}");
        }

        public void ReceiveButton()
        {
            _opponentMove = null;
            Log.Information("Received button");
        }

        public void PostBlind()
        {
            _chipCount -= 1;
            _opponentMove = null;
            Log.Information("Posted blind (1)");
        }

        public void SetCard(string card)
        {
            _card = card;
            Log.Information($"Received card: {card}");
        }

        public void OpponentMove(string move)
        {
            _opponentMove = move;
            Log.Information($"Opponent move: {move}");
        }

        public string Move()
        {
            var ratio = _ratio[_card];
            if (ratio == 0.0)
            {
                Log.Information("Folding");
                return "FOLD";
            }

            //if (_opponentMove.Contains("BET:"))
            //{
            //    var opponenent
            //}

            var bet = (int)(ratio * _chipCount);
            if (bet > _handLimit)
            {
                bet = _handLimit;
            }

            _chipCount -= bet;

            Log.Information($"Betting: {bet}");
            return $"BET:{bet}";
        }

        public void ReceiveChips(string chips)
        {
            _chipCount += int.Parse(chips);
            Log.Information($"Received chips: {chips}");
        }

        public void OpponentCard(string card)
        {
            Log.Information($"Opponent card: {card}");
        }
    }
}
