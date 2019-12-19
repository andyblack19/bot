using System.Collections.Generic;

namespace Bot
{
    public class Game
    {
        private int _chipCount;
        private readonly int _handLimit;
        private string _card;
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

        private string _opponentMove;

        public Game(int chipCount, int handLimit)
        {
            _chipCount = chipCount;
            _handLimit = handLimit;
        }

        public void ReceiveButton()
        {
            _opponentMove = null;
        }

        public void PostBlind()
        {
            _chipCount -= 1;
            _opponentMove = null;
        }

        public void SetCard(string card)
        {
            _card = card;
        }

        public void OpponentMove(string move)
        {
            _opponentMove = move;
        }

        public string Move()
        {
            var ratio = _ratio[_card];
            if (ratio == 0.0)
            {
                return "FOLD";
            }

            var bet = (int)(ratio * _chipCount);
            if (bet > _handLimit)
            {
                bet = _handLimit;
            }

            _chipCount -= bet;

            return $"BET:{bet}";
        }

        public void ReceiveChips(string chips)
        {
            _chipCount += int.Parse(chips);
        }

        public void OpponentCard(string card)
        {
        }
    }
}
