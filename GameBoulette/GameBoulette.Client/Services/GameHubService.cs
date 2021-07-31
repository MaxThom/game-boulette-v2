using GameBoulette.Shared;
using GameBoulette.Shared.Utilities;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace GameBoulette.Client.Services
{
    public class GameHubService : IAsyncDisposable
    {
        public NavigationManager _navigationManager { get; set; }
        private HubConnection hubConnection;

        public event EventHandler<GameRoom> OnGameRoomUpdate;


        public Player You { get; set; }
        public GameRoom Game { get; set; }

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

            hubConnection.On<GameRoom>("CreateGameConfirmation", (game) =>
            {
                Console.WriteLine(ObjectDumper.Dump(game));

                Game = game;
                You = GameUtility.FindCorrespondingPlayer(You.Id, Game);

                You.WrittenWords = new List<Word>();
                for (int i = 0; i < Game.Config.NumberOfPaperPerPerson; i++)
                    You.WrittenWords.Add(new Word());
                OnGameRoomUpdate?.Invoke(this, Game);
            });

            hubConnection.On<GameRoom>("JoinGameConfirmation", (game) =>
            {
                Console.WriteLine(ObjectDumper.Dump(game));

                Game = game;
                You = GameUtility.FindCorrespondingPlayer(You.Id, Game);

                You.WrittenWords = new List<Word>();
                for (int i = 0; i < Game.Config.NumberOfPaperPerPerson; i++)
                    You.WrittenWords.Add(new Word());
                OnGameRoomUpdate?.Invoke(this, Game);
            });

            hubConnection.On<GameRoom>("UpdateGameRoom", (game) =>
            {
                Console.WriteLine(ObjectDumper.Dump(game));

                Game = game;
                OnGameRoomUpdate?.Invoke(this, Game);
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

       

        public async Task<Tuple<bool, string>> CreateGameConnection(Configuration config, Player you)
        {
            You = you;
            await ConnectToGameHub();
            if (!IsConnected)
                return new Tuple<bool, string>(false, "Cannot connect to game server. Please try again later.");

            await hubConnection.SendAsync("CreateLobbyRequest", config, You);

            return new Tuple<bool, string>(IsConnected, "Connected to game server. Waiting for lobby !");
        }

        public async Task<Tuple<bool, string>> JoinGameConnection(string gameCode, Player you)
        {
            You = you;
            await ConnectToGameHub();
            if (!IsConnected)
                return new Tuple<bool, string>(false, "Cannot connect to game server. Please try again later.");

            await hubConnection.SendAsync("JoinLobbyRequest", gameCode, You);

            return new Tuple<bool, string>(IsConnected, "Connected to game server. Waiting for lobby !");
        }
    }
}
