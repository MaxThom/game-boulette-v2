using GameBoulette.Server.Hubs;
using GameBoulette.Shared;
using GameBoulette.Shared.Utilities;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;

namespace GameBoulette.Server.Services
{
    public class GamesService
    {
        //private readonly IHubContext<GameHub, IGameClient> _gameHub;
        public Dictionary<string, GameRoom> Games { get; set; } = new Dictionary<string, GameRoom>();

        public GamesService(IHubContext<GameHub, IGameClient> hubContext)
        {
            //_gameHub = hubContext;                   
        }

        public GameRoom CreateLobby(Configuration config, Player host)
        {
            var game = new GameRoom()
            {
                Code = GameUtility.GenerateGameCode(),
                Config = config,
                //Host = host,
                TeamOne = new Team()
                {
                    Name = "Team 1",
                    Players = new List<Player>()
                },
                TeamTwo = new Team()
                {
                    Name = "Team 2",
                    Players = new List<Player>()
                },
                Words = new List<Word>(),
                CurrentGame = new Game()
                {
                    
                },
                CurrentState = GameState.Lobby
            };
            host.IsHost = true;
            game.TeamOne.Players.Add(host);

            Games.Add(game.Code, game);
            return game;
        }

        public GameRoom JoinLobby(string gameCode, Player newPlayer)
        {
            if (!GameUtility.IsGameCodeValid(gameCode) || !Games.ContainsKey(gameCode))
                return null;

            // Check if it was a disconnected player.
            var (disconnectedPlayer, isTeamOne) = GameUtility.FindCorrespondingPlayer(newPlayer.Id, Games[gameCode]);
            if (disconnectedPlayer != null)
            {
                disconnectedPlayer.ConnectionId = newPlayer.ConnectionId;
                return Games[gameCode];
            }

            // Else add it to a team.
            if (Games[gameCode].TeamOne.Players.Where(x => !string.IsNullOrEmpty(x.ConnectionId)).ToList().Count <= Games[gameCode].TeamTwo.Players.Where(x => !string.IsNullOrEmpty(x.ConnectionId)).ToList().Count)
                Games[gameCode].TeamOne.Players.Add(newPlayer);
            else
                Games[gameCode].TeamTwo.Players.Add(newPlayer);

            return Games[gameCode];
        }

        public GameRoom ChangeTeam(string gameCode, Player playerRequest)
        {
            var (player, isTeamOne) = GameUtility.FindCorrespondingPlayer(playerRequest.Id, Games[gameCode]);
            if (isTeamOne == true)
            {
                Games[gameCode].TeamOne.Players.Remove(player);
                Games[gameCode].TeamTwo.Players.Add(player);
            }
            else
            {
                Games[gameCode].TeamTwo.Players.Remove(player);
                Games[gameCode].TeamOne.Players.Add(player);
            }

            return Games[gameCode];
        }

        public GameRoom Ready(string gameCode, Player playerRequest)
        {
            var (player, isTeamOne) = GameUtility.FindCorrespondingPlayer(playerRequest.Id, Games[gameCode]);
            player.IsReady = true;
            player.WrittenWords = playerRequest.WrittenWords;
            return Games[gameCode];
        }

        public GameRoom NotReady(string gameCode, Player playerRequest)
        {
            var (player, isTeamOne) = GameUtility.FindCorrespondingPlayer(playerRequest.Id, Games[gameCode]);
            player.IsReady = false;

            return Games[gameCode];
        }

        public GameRoom ChangeTeamName(string gameCode, string nameTeamOne, string nameTeamTwo)
        {
            Games[gameCode].TeamOne.Name = nameTeamOne;
            Games[gameCode].TeamTwo.Name = nameTeamTwo;

            return Games[gameCode];
        }

        public GameRoom StartGame(string gameCode)
        {
            var teamOneWords = Games[gameCode].TeamOne.Players.SelectMany(player => player.WrittenWords).Where(x => !string.IsNullOrEmpty(x.Label)).ToList();
            var teamTwoWords = Games[gameCode].TeamTwo.Players.SelectMany(player => player.WrittenWords).Where(x => !string.IsNullOrEmpty(x.Label)).ToList();
            var words = new List<Word>();
            words.AddRange(teamOneWords);
            words.AddRange(teamTwoWords);
            Games[gameCode].Words = words.GroupBy(x => x.Label).Select(x => x.First()).ToList();
            foreach (var word in Games[gameCode].Words)
                word.Label = word.Label.Trim().ToLower();

            Games[gameCode].CurrentGame = new Game()
            {
                RemainingWords = Games[gameCode].Words,
                CurrentPlayer = Games[gameCode].TeamOne.Players.Where(x => !string.IsNullOrEmpty(x.ConnectionId)).FirstOrDefault(),
                CurrentRound = 1,
                CurrentTurn = 0,
                TurnState = TurnState.Waiting,
            };
            Games[gameCode].CurrentState = GameState.Playing;
            return Games[gameCode];
        }
        public GameRoom StartTurn(string gameCode)
        {
            Games[gameCode].CurrentGame.TurnState = TurnState.Playing;
            Games[gameCode].CurrentGame.CurrentTurn += 1;

            return Games[gameCode];
        }

        public GameRoom WordFound(string gameCode, List<Word> remainingWords, int scoreTeamOne, int scoreTeamTwo)
        {
            Games[gameCode].CurrentGame.RemainingWords = remainingWords;
            Games[gameCode].TeamOne.Score = scoreTeamOne;
            Games[gameCode].TeamTwo.Score = scoreTeamTwo;

            return Games[gameCode];
        }
    }
}
