using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBoulette.Shared
{
    public class Configuration
    {
        public string Name { get; set; }
        public string Theme { get; set; }
        public int NumberOfPaperPerPerson { get; set; }
        public string NumberOfRound { get; set; }
        public string TimePerTurn { get; set; }
        public string VideoUrl { get; set; }
    }
}
