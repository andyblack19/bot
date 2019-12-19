using System.Collections.Generic;
using Serilog;

namespace Bot
{
    public class Game
    {
        private int _chipCount;
        private readonly int _handLimit;
        private string _opponentName;
        private string _card;
        private string _opponentMove;
        private int _opponentChipCount;

        public Game(int chipCount, int handLimit, string opponentName)
        {
            _chipCount = chipCount;
            _opponentChipCount = chipCount;
            _handLimit = handLimit;
            _opponentName = opponentName;
            Log.Information($"New game started. ChipCount: {chipCount}, HandLimit: {handLimit}, OpponentName: {opponentName}");
        }

        public void ReceiveButton()
        {
            _opponentMove = null;
            Log.Information("Opponent posted blind (1)");
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
            var ratio = BettingLimits.Ratio[_card];

            if (ratio == 0.0)
            {
                if (_opponentMove == null)
                {
                    Log.Information("Crap card, Our go first, Bet minimum (1) to try and force them to fold");
                    return "BET";
                }

                Log.Information("Crap card, Opponent has bet, Folding");
                return "FOLD";
            }

            var maxWillingToBet = (int)(ratio * _chipCount);

            _chipCount -= maxWillingToBet;
            Log.Information($"Betting: {maxWillingToBet}");
            return $"BET:{maxWillingToBet}";
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

    public static class BettingLimits
    {
        public static readonly Dictionary<string, double> Ratio = new Dictionary<string, double>
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
    }
}
