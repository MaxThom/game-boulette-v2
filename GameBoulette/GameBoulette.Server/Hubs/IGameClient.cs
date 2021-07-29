using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameBoulette.Server.Hubs
{
    public interface IGameClient
    {
        Task ReceiveMessage(string user, string message);
    }
}
