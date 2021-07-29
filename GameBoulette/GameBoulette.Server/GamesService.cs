using GameBoulette.Server.Hubs;

using Microsoft.AspNetCore.SignalR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameBoulette.Server
{
    public class GamesService
    {
        private readonly IHubContext<GameHub, IGameClient> _gameHub;

        public GamesService(IHubContext<GameHub, IGameClient> hubContext)
        {
            _gameHub = hubContext;
        }

        public async Task SendMessage(string user, string message)
        {
            await _gameHub.Clients.All.ReceiveMessage(user, message);
        }
    }
}
