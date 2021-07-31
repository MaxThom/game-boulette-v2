using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBoulette.Shared
{
    public class Configuration
    {
        public string Name { get; set; } = "";
        public string Theme { get; set; } = "";
        public int NumberOfPaperPerPerson { get; set; }
        public int NumberOfRound { get; set; }
        public int TimePerTurn { get; set; }
        public string VideoUrl { get; set; } = "";
    }
}
