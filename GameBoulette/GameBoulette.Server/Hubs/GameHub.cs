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
    public class GameHub: Hub<IGameClient>
    {
        public GamesService _gamesService { get; set; }

        public GameHub(GamesService gamesService) : base()
        {
            _gamesService = gamesService;
        }

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task CreateLobbyRequest(Configuration config, Player host)
        {
            var game = _gamesService.CreateLobby(config, host);
            Console.WriteLine(ObjectDumper.Dump(game));

            await Groups.AddToGroupAsync(Context.ConnectionId, game.Code);
            await Clients.Clients<IGameClient>(Context.ConnectionId).CreateGameConfirmation(game);

            //  await _hub.Clients.Group(sensorUnit.Key).SendAsync("ReceiveSensorData", data);
        }

        public async Task JoinLobbyRequest(string gameCode, Player newPlayer)
{
            var game = _gamesService.JoinLobby(gameCode, newPlayer);
            Console.WriteLine(ObjectDumper.Dump(game));

            if (game != null)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, game.Code);
                await Clients.Groups<IGameClient>(game.Code).UpdateGameRoom(game);
            }

            await Clients.Clients<IGameClient>(Context.ConnectionId).JoinGameConfirmation(game);            
        }
    }
}
