using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

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

        public GameHubService(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        public bool IsConnected => hubConnection.State == HubConnectionState.Connected;

        public async Task ConnectToGameHub()
        {
            hubConnection = new HubConnectionBuilder()
            .WithUrl(_navigationManager.ToAbsoluteUri("/gamehub"))
            .WithAutomaticReconnect()
            .Build();

            hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                var encodedMsg = $"{user}: {message}";
                Console.WriteLine(encodedMsg);
            });

            await hubConnection.StartAsync();
        }

        public async Task Send(string userInput, string messageInput) =>
            await hubConnection.SendAsync("SendMessage", userInput, messageInput);

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (hubConnection is not null)
            {
                await hubConnection.DisposeAsync();
            }
        }
    }
}
