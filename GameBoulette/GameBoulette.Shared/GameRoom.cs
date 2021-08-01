using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBoulette.Shared
{
    public class GameRoom
    {
        public string Code { get; set; } = "";
        public Player Host {  get; set; }
        public Configuration Config { get; set; }
        public List<Word> Words { get; set; }
        public Team TeamOne { get; set; }
        public Team TeamTwo { get; set; }
        public Game CurrentGame { get; set; }
        public GameState CurrentState { get; set; }
    }
}
