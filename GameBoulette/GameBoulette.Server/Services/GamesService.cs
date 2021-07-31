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
        private Dictionary<string, GameRoom> games = new Dictionary<string, GameRoom>();

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

            games.Add(game.Code, game);
            return game;
        }

        public GameRoom JoinLobby(string gameCode, Player newPlayer)
        {
            if (!GameUtility.IsGameCodeValid(gameCode) || !games.ContainsKey(gameCode))
                return null;

            // Check if it was a disconnected player.
            var disconnectedPlayer = GameUtility.FindCorrespondingPlayer(newPlayer.Id, games[gameCode]);
            if (disconnectedPlayer != null)
                return games[gameCode];

            // Else add it to a team.
            if (games[gameCode].TeamOne.Players.Count <= games[gameCode].TeamTwo.Players.Count)
                games[gameCode].TeamOne.Players.Add(newPlayer);
            else
                games[gameCode].TeamTwo.Players.Add(newPlayer);
            
            return games[gameCode];
        }
    }
}
