using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBoulette.Client.Model
{
    public class ScoreTableRow
    {
        public string Label { get; set; } = "";
        public string WrittenBy { get; set; } = "";
        public int TimesSkipped { get; set; }
    }
}
