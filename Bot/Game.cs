using System.Collections.Generic;
using Serilog;

namespace Bot
{
    public class Game
    {
        private int _chipCount;
        private int _remainingHands;
        private readonly int _startingChips;
        private string _opponentName;
        private string _card;
        private string _opponentMove;
        private int _opponentChipCount;
        private int _totalBetThisHand;

        public Game(int chipCount, int remainingHands, string opponentName)
        {
            _chipCount = chipCount;
            _startingChips = chipCount;
            _opponentChipCount = chipCount;
            _remainingHands = remainingHands;
            _opponentName = opponentName;
            Log.Information($"New game started. ChipCount: {chipCount}, HandLimit: {remainingHands}, OpponentName: {opponentName}");
        }

        public void ReceiveButton()
        {
            _opponentMove = null;
            Log.Information("Opponent posted blind (1)");
        }

        public void PostBlind()
        {
            _chipCount -= 1;
            _totalBetThisHand += 1;
            _opponentMove = null;
            Log.Information("Posted blind (1)");
        }

        public void SetCard(string card)
        {
            _card = card;
            _remainingHands -= 1;
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
                    Log.Information($"Crap card {_card}, Our go first, Bet minimum (2) to try and force them to fold");
                    Log.Information("BET");
                    _totalBetThisHand += 2;
                    _chipCount -= 2;
                    return "BET";
                }

                Log.Information($"Crap card {_card}, Opponent has bet, Folding");
                return "FOLD";
            }

            var maxWillingToBet = (int)(ratio * _chipCount);
            if (_totalBetThisHand >= maxWillingToBet)
            {
                return "FOLD";
            }

            _totalBetThisHand += maxWillingToBet;
            _chipCount -= maxWillingToBet;
            Log.Information($"Betting: {maxWillingToBet}");
            return $"BET:{maxWillingToBet}";
        }

        public void ReceiveChips(string chips)
        {
            var chipsWon = int.Parse(chips);
            _chipCount += chipsWon;
            Log.Information($"Received chips: {chips}");
            if (_chipCount == 0)
            {
                Log.Information($"**GAME LOST** {_remainingHands}");
            }
            else if (_chipCount >= _startingChips * 2)
            {
                Log.Information($"**GAME WON** {_remainingHands}");
            }
            else if (_remainingHands == 0 && _chipCount > _startingChips)
            {
                Log.Information($"**GAME WON** {_remainingHands}");
            }

            _totalBetThisHand = 0;
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
