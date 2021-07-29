using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBoulette.Shared
{
    public class Team
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public List<Player> Players { get; set; }


    }
}
