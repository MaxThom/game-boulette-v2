using GameBoulette.Server.Controllers;
using GameBoulette.Server.Services;
using GameBoulette.Shared;
using GameBoulette.Shared.Utilities;

using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameBoulette.Server.Hubs
{
    public class GameHub : Hub<IGameClient>
    {
        private readonly ILogger<GameHub> _logger;
        public GamesService _gamesService { get; set; }

        public GameHub(GamesService gamesService, ILogger<GameHub> logger) : base()
        {
            _gamesService = gamesService;
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation($"New user connected: {Context.ConnectionId}-{Context.UserIdentifier}");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _logger.LogInformation($"User disconnected: {Context.ConnectionId}-{Context.UserIdentifier}");
            var (disconnectedPlayer, game, isTeamOne) = GameUtility.FindCorrespondingPlayerAndGame(Context.ConnectionId, _gamesService.Games);
            if (disconnectedPlayer != null)
            {
                // Remove player                
                disconnectedPlayer.ConnectionId = null;

                // Update game
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, game.Code);
                await Clients.Groups<IGameClient>(game.Code).UpdateGameRoom(game);
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task CreateLobbyRequest(Configuration config, Player host)
        {
            _logger.LogInformation($"Create lobby request: {Context.ConnectionId}-{Context.UserIdentifier}");
            host.ConnectionId = Context.ConnectionId;
            var game = _gamesService.CreateLobby(config, host);
            _logger.LogDebug(ObjectDumper.Dump(game));

            await Groups.AddToGroupAsync(Context.ConnectionId, game.Code);
            await Clients.Clients<IGameClient>(Context.ConnectionId).CreateGameConfirmation(game);
        }

        public async Task JoinLobbyRequest(string gameCode, Player newPlayer)
        {
            _logger.LogInformation($"Join lobby request: {Context.ConnectionId}-{Context.UserIdentifier}");
            newPlayer.ConnectionId = Context.ConnectionId;
            var game = _gamesService.JoinLobby(gameCode, newPlayer);
            _logger.LogDebug(ObjectDumper.Dump(game));

            if (game != null)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, game.Code);
                await Clients.OthersInGroup(game.Code).UpdateGameRoom(game);
            }

            await Clients.Clients<IGameClient>(Context.ConnectionId).JoinGameConfirmation(game);
        }

        public async Task ChangeTeamRequest(string gameCode, Player player)
        {
            _logger.LogInformation($"Change team request: {Context.ConnectionId}-{Context.UserIdentifier}");
            var game = _gamesService.ChangeTeam(gameCode, player);
            _logger.LogDebug(ObjectDumper.Dump(game));

            await Clients.Groups<IGameClient>(game.Code).UpdateGameRoom(game);
        }

        public async Task ReadyRequest(string gameCode, Player player)
        {
            _logger.LogInformation($"Ready request: {Context.ConnectionId}-{Context.UserIdentifier}");
            var game = _gamesService.Ready(gameCode, player);
            _logger.LogDebug(ObjectDumper.Dump(game));

            await Clients.Groups<IGameClient>(game.Code).UpdateGameRoom(game);
        }

        public async Task NotReadyRequest(string gameCode, Player player)
        {
            _logger.LogInformation($"Not ready request: {Context.ConnectionId}-{Context.UserIdentifier}");
            var game = _gamesService.NotReady(gameCode, player);
            _logger.LogDebug(ObjectDumper.Dump(game));

            await Clients.Groups<IGameClient>(game.Code).UpdateGameRoom(game);
        }

        public async Task ChangeTeamNameRequest(string gameCode, string teamOneName, string teamTwoName)
        {
            _logger.LogInformation($"Change team name request: {Context.ConnectionId}-{Context.UserIdentifier}");
            var game = _gamesService.ChangeTeamName(gameCode, teamOneName, teamTwoName);
            _logger.LogDebug(ObjectDumper.Dump(game));

            await Clients.Groups<IGameClient>(game.Code).UpdateGameRoom(game);
        }

        public async Task StartGameRequest(string gameCode)
        {
            _logger.LogInformation($"Start game request: {Context.ConnectionId}-{Context.UserIdentifier}");
            var game = _gamesService.StartGame(gameCode);
            _logger.LogDebug(ObjectDumper.Dump(game));

            await Clients.Groups<IGameClient>(game.Code).ResetTimer();
            await Clients.Groups<IGameClient>(game.Code).UpdateGameRoom(game);
            await Clients.Groups<IGameClient>(game.Code).OnPlayerTurnWait(game);
        }

        public async Task EndTurnRequest(string gameCode, Game gameToUpdate)
        {
            await Clients.Groups<IGameClient>(gameCode).StopTimer();
            await Clients.Groups<IGameClient>(gameCode).ResetTimer();
            _logger.LogInformation($"End turn request: {Context.ConnectionId}-{Context.UserIdentifier}");
            var game = _gamesService.EndTurn(gameCode, gameToUpdate);
            _logger.LogDebug(ObjectDumper.Dump(game));            
            
            await Clients.Groups<IGameClient>(game.Code).OnPlayerTurnWait(game);            
        }

        public async Task StartTurnRequest(string gameCode)
        {
            _logger.LogInformation($"Start turn request: {Context.ConnectionId}-{Context.UserIdentifier}");
            var game = _gamesService.StartTurn(gameCode);
            _logger.LogDebug(ObjectDumper.Dump(game));

            await Clients.Groups<IGameClient>(game.Code).UpdateGameRoom(game);
            await Clients.Groups<IGameClient>(game.Code).StartTimer();
        }

        public async Task WordFoundRequest(string gameCode, string wordLabel, int scoreTeamOne, int scoreTeamTwo)
        {
            _logger.LogInformation($"Word found request: {Context.ConnectionId}-{Context.UserIdentifier}");
            var game = _gamesService.WordFound(gameCode, scoreTeamOne, scoreTeamTwo, wordLabel);
            _logger.LogDebug(ObjectDumper.Dump(game));

            if (game.CurrentState == GameState.Completed)
            {
                await Clients.Groups<IGameClient>(game.Code).OnGameCompleted(game);
            }
            else if (game.CurrentGame.TurnState == TurnState.WaitingNextRound)
            {
                await Clients.Groups<IGameClient>(game.Code).StopTimer();
                await Clients.Groups<IGameClient>(game.Code).OnPlayerTurnWait(game);
            } 
            else
            {
                await Clients.OthersInGroup(game.Code).UpdateGameRoom(game);
            }            
        }

        public void WordSkippedRequest(string gameCode, string wordLabel)
        {
            _logger.LogInformation($"Word skipped request: {Context.ConnectionId}-{Context.UserIdentifier}");
            var game = _gamesService.WordSkipped(gameCode, wordLabel);
            _logger.LogDebug(ObjectDumper.Dump(game));
        }
    }
}
