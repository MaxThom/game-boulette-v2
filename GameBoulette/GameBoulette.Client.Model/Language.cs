using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBoulette.Client.Model
{
    public class Language
    {
        public string ShortName { get; set; } = "";
        public string Name { get; set; } = "";
        public string EnglishName { get; set; } = "";
        public AppBar AppBar {  get; set; } = new AppBar();
        public IndexPage IndexPage { get; set; } = new IndexPage();
        public CreateGamePage CreateGamePage { get; set; } = new CreateGamePage();
        public GameLobbyPage GameLobbyPage { get; set; } = new GameLobbyPage();
        public GamePage GamePage { get; set; } = new GamePage();
        public ScorePage ScorePage { get; set; } = new ScorePage();
        public RulesComponent RulesComponent { get; set; } = new RulesComponent();
    }

    public class AppBar
    {
        public string Title { get; set; } = "";
        public string Footer { get; set; } = "";
    }

    public class IndexPage
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string AvatarTitle { get; set; } = "";
        public string AvatarInput { get; set; } = "";
        public string AvatarInputHelper { get; set; } = "";
        public string JoinGame { get; set; } = "";
        public string JoinGameSub { get; set; } = "";
        public string JoinGameCode { get; set; } = "";
        public string CreateGame { get; set; } = "";
        public string CreateGameSub { get; set; } = "";
        public string Instruction { get; set; } = "";
        public string GameTheme { get; set; } = "";
        public string SnackConnected { get; set; } = "";
        public string SnackConnectedLong { get; set; } = "";
        public string SnackNotConnected { get; set; } = "";
        public string SnackNotConnectedLong { get; set; } = "";
    }

    public class CreateGamePage
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string GameName { get; set; } = "";
        public string GameTheme { get; set; } = "";
        public string NumberOfRound { get; set; } = "";
        public string Rounds { get; set; } = "";
        public string TimePerTurn { get; set; } = "";
        public string Seconds { get; set; } = "";
        public string NumberOfPaper { get; set; } = "";
        public string Papers { get; set; } = "";
        public string VideoURL { get; set; } = "";
        public string Return { get; set; } = "";
        public string Restart { get; set; } = "";
        public string Create { get; set; } = "";
        public string MissingField { get; set; } = "";
        public string GameUrlCopied { get; set; } = "";
        public string VideoUrlCopied { get; set; } = "";
    }

    public class GameLobbyPage
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string GameUrl { get; set; } = "";
        public string ThemeLabel { get; set; } = "";
        public string EnterWords { get; set; } = "";
        public string Word { get; set; } = "";
        public string NotReady { get; set; } = "";
        public string Ready { get; set; } = "";
        public string StartGame { get; set; } = "";
        public string TeamNameOne { get; set; } = "";
        public string TeamNameTwo { get; set; } = "";
        public string JoinTeam { get; set; } = "";
        public string Leave { get; set; } = "";
        public string ConfigurationLabel { get; set; } = "";
        public string RulesLabel { get; set; } = "";
        public string SnackGameStarted { get; set; } = "";
    }

    public class GamePage
    {
        public string RoundLabel { get; set; } = "";
        public string ScoreLabel { get; set; } = "";
        public string SkipWord { get; set; } = "";
        public string FoundWord { get; set; } = "";
        public string RemaningWords { get; set; } = "";
        public string StartTurn { get; set; } = "";
        public string MsgYourTurn { get; set; } = "";
        public string MsgNextRound { get; set; } = "";
        public string Latest { get; set; } = "";
    }

    public class ScorePage
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Tie { get; set; } = "";
        public string Mvp { get; set; } = "";
        public string WordFound { get; set; } = "";
        public string WordSkip { get; set; } = "";
        public string TeamChampion { get; set; } = "";
        public string StartTurn { get; set; } = "";
        public string WordFoundLegend { get; set; } = "";
        public string WordSkipLegend { get; set; } = "";
        public string TableTitle { get; set; } = "";
        public string TableWord { get; set; } = "";
        public string TableWrittenBy { get; set; } = "";
        public string TableTimesSkipped { get; set; } = "";

    }

    public class RulesComponent
    {
        public string SetupHeader { get; set; } = "";
        public string Setup { get; set; } = "";
        public string UnfoldingHeader { get; set; } = "";
        public string Unfolding { get; set; } = "";
        public string RoundsHeader { get; set; } = "";
        public string Rounds { get; set; } = "";
    }
}
