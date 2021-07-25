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
            },
            CreateGamePage = new CreateGamePage()
            {
                Title = "Création d'une partie!",
                Description = "Configurer les rèles de la partie selon vos désirs.",
                GameName = "Nom de la partie",
                GameTheme = "Thème de la partie",
                NumberOfRound = "Nombre de manche",
                Rounds = "manches",
                TimePerTurn = "Temps par tour",
                Seconds = "secondes",
                NumberOfPaper = "Nombre de papier par personne",
                Papers = "papiers",
                VideoURL = "URL de l'appel vidéo",
                Return = "Retour",
                Restart = "Refaire",
                Create = "Créer la partie!",
                MissingField = "Ce champ est requis!"
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
            },
            CreateGamePage = new CreateGamePage()
            {
                Title = "Game creation !",
                Description = "Configure game rules at your liking.",
                GameName = "Game name",
                GameTheme = "Game theme",
                NumberOfRound = "Number of round",
                Rounds = "rounds",
                TimePerTurn = "Time per turn",
                Seconds = "seconds",
                NumberOfPaper = "Number of papers per person",
                Papers = "papers",
                VideoURL = "Video call URL",
                Return = "Return",
                Restart = "Restart",
                Create = "Create the game!",
                MissingField = "This field is required!"
            }
        };
    }
}
