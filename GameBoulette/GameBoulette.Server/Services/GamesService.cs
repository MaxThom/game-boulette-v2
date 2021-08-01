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
                    CurrentState = GameState.Lobby
                }
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
            var disconnectedPlayer = GameUtility.FindCorrespondingPlayer(newPlayer.Id, Games[gameCode]);
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
    }
}
