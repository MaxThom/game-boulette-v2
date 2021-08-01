using GameBoulette.Client.Pages;
using GameBoulette.Server.Services;
using GameBoulette.Shared;
using GameBoulette.Shared.Utilities;

using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameBoulette.Server.Hubs
{
    public class GameHub : Hub<IGameClient>
    {
        public GamesService _gamesService { get; set; }

        public GameHub(GamesService gamesService) : base()
        {
            _gamesService = gamesService;
        }

        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"New user connected: {Context.ConnectionId}-{Context.UserIdentifier}");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine($"User disconnected: {Context.ConnectionId}-{Context.UserIdentifier}");
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
            Console.WriteLine($"Create lobby request: {Context.ConnectionId}-{Context.UserIdentifier}");
            host.ConnectionId = Context.ConnectionId;
            var game = _gamesService.CreateLobby(config, host);
            Console.WriteLine(ObjectDumper.Dump(game));

            await Groups.AddToGroupAsync(Context.ConnectionId, game.Code);
            await Clients.Clients<IGameClient>(Context.ConnectionId).CreateGameConfirmation(game);
        }

        public async Task JoinLobbyRequest(string gameCode, Player newPlayer)
        {
            Console.WriteLine($"Join lobby request: {Context.ConnectionId}-{Context.UserIdentifier}");
            newPlayer.ConnectionId = Context.ConnectionId;
            var game = _gamesService.JoinLobby(gameCode, newPlayer);
            Console.WriteLine(ObjectDumper.Dump(game));

            if (game != null)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, game.Code);
                await Clients.OthersInGroup(game.Code).UpdateGameRoom(game);
            }

            await Clients.Clients<IGameClient>(Context.ConnectionId).JoinGameConfirmation(game);
        }

        public async Task ChangeTeamRequest(string gameCode, Player player)
        {
            Console.WriteLine($"Change team request: {Context.ConnectionId}-{Context.UserIdentifier}");
            var game = _gamesService.ChangeTeam(gameCode, player);
            Console.WriteLine(ObjectDumper.Dump(game));

            await Clients.Groups<IGameClient>(game.Code).UpdateGameRoom(game);
        }

        public async Task ReadyRequest(string gameCode, Player player)
        {

            Console.WriteLine($"Ready request: {Context.ConnectionId}-{Context.UserIdentifier}");
            var game = _gamesService.Ready(gameCode, player);
            Console.WriteLine(ObjectDumper.Dump(game));

            await Clients.Groups<IGameClient>(game.Code).UpdateGameRoom(game);
        }

        public async Task NotReadyRequest(string gameCode, Player player)
        {
            Console.WriteLine($"Not ready request: {Context.ConnectionId}-{Context.UserIdentifier}");
            var game = _gamesService.NotReady(gameCode, player);
            Console.WriteLine(ObjectDumper.Dump(game));

            await Clients.Groups<IGameClient>(game.Code).UpdateGameRoom(game);
        }

        public async Task ChangeTeamNameRequest(string gameCode, string teamOneName, string teamTwoName)
        {
            Console.WriteLine($"Change team request: {Context.ConnectionId}-{Context.UserIdentifier}");
            var game = _gamesService.ChangeTeamName(gameCode, teamOneName, teamTwoName);
            Console.WriteLine(ObjectDumper.Dump(game));

            await Clients.Groups<IGameClient>(game.Code).UpdateGameRoom(game);
        }
    }
}
