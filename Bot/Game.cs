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

        public Game(int chipCount, int handLimit)
        {
            _chipCount = chipCount;
            _handLimit = handLimit;
        }


        public string Move()
        {
            var ratio = _ratio[_card];
            if (ratio == 0.0)
            {
                return "FOLD";
            }

            var bet = (int)(ratio * _chipCount);
            _chipCount -= bet;
            return $"BET:{bet}";
        }

        public void SetCard(string updateData)
        {
            _card = updateData;
        }
    }
}
