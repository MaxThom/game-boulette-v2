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
            AppBar = new AppBar()
            {
                Title = "Boulette virtuelle"
            },
            IndexPage = new IndexPage()
            {
                Title = "Le jeu de boulette en ligne !",
                Description = "Verrat de saint-ciboire de purée de viande à chien de boswell de cochonnerie de baptême de sacristi de crucifix de torrieux de sainte-viarge de bout d'ciarge de sapristi de cibouleau de saint-ciarge de mautadine.",
                JoinGame = "Rejoindre une partie",
                JoinGameSub = "24 joueurs en ligne !",
                CreateGame = "Créer une partie",
                CreateGameSub = "Facile et intuitif !",
                AvatarTitle = "Votre avatar !",
                AvatarInput = "Entrez votre nom",
                Instruction = "Règles",
            }
        };

        public static Language English = new Language()
        {
            Name = "English",
            EnglishName = "English",
            ShortName = "EN",
            AppBar = new AppBar()
            {
                Title = "Virtual Boulet"
            },
            IndexPage = new IndexPage()
            {
                Title = "The boulet game online !",
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. ",
                JoinGame = "Join game",
                JoinGameSub = "24 online players !",
                CreateGame = "Create game",
                CreateGameSub = "Easy & Simple !",
                AvatarTitle = "Your avatar !",
                AvatarInput = "Enter your name",
                Instruction = "Rules",
            }
        };
    }
}
