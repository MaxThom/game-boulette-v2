using GameBoulette.Server.Hubs;
using GameBoulette.Shared;
using GameBoulette.Shared.Utilities;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;

namespace GameBoulette.Server.Services
{
    public class GamesService
    {
        private readonly IHubContext<GameHub, IGameClient> _gameHub;
        private Dictionary<string, GameRoom> games = new Dictionary<string, GameRoom>();

        public GamesService(IHubContext<GameHub, IGameClient> hubContext)
        {
            _gameHub = hubContext;                   
        }

        public GameRoom CreateLobby(Configuration config, Player host)
        {
            var game = new GameRoom()
            {
                Code = GameCodeUtility.GenerateGameCode(),
                Host = host,
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
            game.TeamOne.Players.Add(host);

            games.Add(game.Code, game);
            return game;
        }

        public GameRoom JoinLobby(string gameCode, Player newPlayer)
        {
            if (!GameCodeUtility.IsGameCodeValid(gameCode) || !games.ContainsKey(gameCode))
                return null;

            if (games[gameCode].TeamOne.Players.Count <= games[gameCode].TeamTwo.Players.Count)
                games[gameCode].TeamOne.Players.Add(newPlayer);
            else
                games[gameCode].TeamTwo.Players.Add(newPlayer);
            
            return games[gameCode];
        }
    }
}
