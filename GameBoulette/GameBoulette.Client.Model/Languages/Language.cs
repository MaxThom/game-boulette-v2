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
        public AppBar AppBar {  get; set; }
        public IndexPage IndexPage { get; set; }
    }

    public class AppBar
    {
        public string Title { get; set; }
    }

    public class IndexPage
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string AvatarTitle { get; set; }
        public string AvatarInput { get; set; }
        public string JoinGame { get; set; }
        public string JoinGameSub { get; set; }
        public string CreateGame { get; set; }
        public string CreateGameSub { get; set; }
        public string Instruction { get; set; }
    }
}
