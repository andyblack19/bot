using System.Collections.Generic;
using Serilog;

namespace Bot
{
    public class Game
    {
        private int _chipCount;
        private readonly int _handLimit;
        private int _handNumber;
        private readonly int _startingChips;
        private string _opponentName;
        private string _card;
        private string _opponentMove;
        private int _opponentChipCount;
        private int _totalBetThisHand;

        public Game(int chipCount, int handLimit, string opponentName)
        {
            _chipCount = chipCount;
            _startingChips = chipCount;
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
            _totalBetThisHand += 1;
            _opponentMove = null;
            Log.Information("Posted blind (1)");
        }

        public void SetCard(string card)
        {
            _card = card;
            _handNumber += 1;
            Log.Information($"Hand number: {_handNumber}. Received card: {card}");
        }

        public void OpponentMove(string move)
        {
            _opponentMove = move;
            Log.Information($"Opponent move: {move}");
        }

        public string Move()
        {
            var aheadBy = _chipCount - _startingChips;
            var handsRemaining = (_handLimit - _handNumber) / 2;
            if (aheadBy > handsRemaining)
            {
                Log.Information($"Folding to hold out for a win. Ahead by {aheadBy} Rounds remaining: {handsRemaining}");
//                return "FOLD";
            }

            var ratio = BettingLimits.Ratio[_card];

            if (_chipCount < _startingChips * 0.5 && _handLimit - _handNumber < 10)
            {
                ratio *= 2;
            }

            if (ratio == 0.0)
            {
                if (_opponentMove == null)
                {
                    Log.Information($"Crap card {_card}, Our go first, Bet minimum (2) to try and force them to fold");
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
                if (_opponentName == "HanYolo")
                {
                    Log.Information("Calling against HanYolo");
                    return "CALL";
                }

                if (_opponentMove == "BET")
                {
                    Log.Information($"They bet minimum above we're willing to bet: {_opponentMove}");
                    return "CALL";
                }
                Log.Information($"They bet a lot more than our maximum: {_opponentMove}");
                return "FOLD";
            }

            _totalBetThisHand += maxWillingToBet;
            _chipCount -= maxWillingToBet;
            Log.Information($"Betting: {maxWillingToBet} ({ratio * 100}%)");
            return $"BET:{maxWillingToBet}";
        }

        public void ReceiveChips(string chips)
        {
            var chipsWon = int.Parse(chips);
            _chipCount += chipsWon;
            Log.Information($"Received chips: {chips}, Total: {_chipCount}");
            if (_chipCount == 0)
            {
                Log.Information($"**GAME LOST** {_handNumber}, Chips: {_chipCount}");
            }
            else if (_chipCount >= _startingChips * 2)
            {
                Log.Information($"**GAME WON** {_handNumber}, Won all the chips {_chipCount}");
            }
            else if (_handNumber == _handLimit && _chipCount > _startingChips)
            {
                Log.Information($"**GAME WON** {_handNumber}, Round limit reached. Chips: {_chipCount}");
            }
            else if (_handNumber == _handLimit && _chipCount < _startingChips)
            {
                Log.Information($"**GAME LOST** {_handNumber}, Round limit reached. Chips: {_chipCount}");
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
