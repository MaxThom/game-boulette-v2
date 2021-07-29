﻿using Microsoft.AspNetCore.SignalR;

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

        public async Task SendMessage(string user, string message)
        {
            Console.WriteLine($"user: {user}, message: {message}");
            await _gamesService.SendMessage(user, message);
            //await Clients.All.ReceiveMessage(user, message);
        }
    }
}
