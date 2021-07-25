using GameBoulette.Client.Model.Languages;

using MudBlazor;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameBoulette.Client.Constants
{
    public static class Languages
    {
        public static Language French = new Language()
        {
            Name = "Francais",
            EnglishName = "French",
            ShortName = "FR",
            Index = new IndexPage()
            {
                Title = "Boulette virtuelle"
            }
        };

        public static Language English = new Language()
        {
            Name = "English",
            EnglishName = "English",
            ShortName = "EN",
            Index = new IndexPage()
            {
                Title = "Virtual Boulet"
            }
        };
    }
}
