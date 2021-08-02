using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBoulette.Shared
{
    public class Game
    {
        public Player CurrentPlayer { get; set; }
        public List<Word> RemainingWords { get; set; }
        public int CurrentRound { get; set; }
        public int CurrentTurn { get; set; }
        public TurnState TurnState { get; set; }
    }
}
