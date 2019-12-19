using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bot
{
    public class Game
    {
        private readonly int _chipCount;
        private readonly int _handLimit;

        public Game(int chipCount, int handLimit)
        {
            _chipCount = chipCount;
            _handLimit = handLimit;
        }


        public string Move()
        {
            return "CALL";
        }
    }
}
