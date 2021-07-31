using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBoulette.Shared
{
    public class Player
    {
        public Guid Id { get; set; }
        public bool IsHost { get; set; }
        public string Name { get; set; } = "";
        public string Color { get; set; } = "";
        public int WordFound { get; set; }
        public int WordSkipped { get; set; }
        public bool IsPlaying { get; set; }
        public List<Word> WrittenWords { get; set; } = new List<Word>();
    }
}
