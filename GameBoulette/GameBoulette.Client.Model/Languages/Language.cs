using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBoulette.Client.Model.Languages
{
    public class Language
    {
        public string ShortName { get; set; }
        public string Name { get; set; }
        public string EnglishName { get; set; }
        public IndexPage Index { get; set; }
    }

    public class IndexPage
    {
        public string Title { get; set; }
    }
}
