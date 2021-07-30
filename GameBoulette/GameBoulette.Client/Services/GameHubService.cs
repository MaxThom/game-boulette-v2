using GameBoulette.Shared;
using GameBoulette.Shared.Utilities;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameBoulette.Client.Services
{
    public class GameHubService : IAsyncDisposable
    {
        public NavigationManager _navigationManager { get; set; }

        private HubConnection hubConnection;

        private string GameCode;
        private Player You;

        public GameHubService(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        #region Hub

        public bool IsConnected => hubConnection.State == HubConnectionState.Connected;

        public async Task ConnectToGameHub()
        {
            hubConnection = new HubConnectionBuilder()
            .WithUrl(_navigationManager.ToAbsoluteUri("/gamehub"))
            .ConfigureLogging(logging => {
                logging.SetMinimumLevel(LogLevel.Debug);
                //logging.AddConsole();
            })
            .WithAutomaticReconnect()
            .Build();

            hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                var encodedMsg = $"{user}: {message}";
                Console.WriteLine(encodedMsg);
            });

            await hubConnection.StartAsync();
        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (hubConnection is not null)
            {
                await hubConnection.DisposeAsync();
            }
        }

        #endregion Hub

        #region HubCom

        public async Task Send(string userInput, string messageInput) =>
            await hubConnection.SendAsync("SendMessage", userInput, messageInput);

        private async Task JoinLobby()
        {
            await hubConnection.SendAsync("JoinLobby", GameCode, You);
        }

        private async Task CreateLobby()
        {
            await hubConnection.SendAsync("JoinLobby", GameCode, You);
        }

        #endregion HubCom

        public async Task<Tuple<bool, string>> CreateGameConnection(Player you)
        {
            You = you;
            GameCode = GameCodeUtility.GenerateGameCode();
            await ConnectToGameHub();
            if (!IsConnected)
                return new Tuple<bool, string>(false, "Cannot connect to game server. Please try again later.");

            await CreateLobby();

            return new Tuple<bool, string>(IsConnected, "Connected to game lobby !");
        }

        public async Task<Tuple<bool, string>> JoinGameConnection(string gameCode, Player you)
        {
            You = you;
            GameCode = gameCode;
            await ConnectToGameHub();
            if (!IsConnected)
                return new Tuple<bool, string>(false, "Cannot connect to game server. Please try again later.");

            await JoinLobby();

            return new Tuple<bool, string>(IsConnected, "Connected to game lobby !");
        }
    }
}
