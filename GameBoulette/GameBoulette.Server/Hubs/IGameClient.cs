using GameBoulette.Shared;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameBoulette.Server.Hubs
{
    public interface IGameClient
    {
        Task CreateGameConfirmation(GameRoom game);

        Task JoinGameConfirmation(GameRoom game);

        Task UpdateGameRoom(GameRoom game);

        Task OnPlayerTurnWait(GameRoom game);

        Task OnGameCompleted(GameRoom game);

        Task StartTimer();

        Task StopTimer();

        Task ResetTimer();
    }
}
