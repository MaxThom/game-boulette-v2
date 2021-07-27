using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBoulette.Client.Model.Game
{
    public class GameConfig
    {
        public string GameName { get; set; } = "";
        public string GameTheme { get; set; } = "";
        public int NumberOfRound { get; set; }
        public int TimePerTurn { get; set; }
        public int NumberOfPaper { get; set; }
        public string VideoUrl { get; set; } = "";
        public string GameCode { get; set; } = "";
    }
}
