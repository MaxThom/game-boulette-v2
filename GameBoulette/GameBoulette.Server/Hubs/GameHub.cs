using Microsoft.AspNetCore.SignalR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameBoulette.Server.Hubs
{
    public class GameHub: Hub
    {
        public async Task SendMessage(string user, string message)
        {
            Console.WriteLine($"user: {user}, message: {message}");
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
