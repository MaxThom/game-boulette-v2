using GameBoulette.Shared;
using GameBoulette.Shared.Utilities;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
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
        public LanguageManagerService _languageManager { get; set; }
        public IWebAssemblyHostEnvironment _hostEnv { get; set;  }
        private HubConnection hubConnection;

        public event EventHandler<GameRoom> OnGameRoomUpdate;
        public event EventHandler<GameRoom> OnPlayerTurnWaitEvent;
        public event EventHandler<GameRoom> OnGameCompletedEvent;
        public event EventHandler OnStartTimerEvent;
        public event EventHandler OnStopTimerEvent;
        public event EventHandler OnResetTimerEvent;         

        public Player You { get; set; }
        public bool? IsTeamOne { get; set; }
        public GameRoom Game { get; set; }

        public GameHubService(NavigationManager navigationManager, LanguageManagerService languageManager, IWebAssemblyHostEnvironment hostEnv)
        {
            _navigationManager = navigationManager;
            _languageManager = languageManager;
            _hostEnv = hostEnv;
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

            hubConnection.Closed += HubConnection_Closed;

            hubConnection.On<GameRoom>("CreateGameConfirmation", (game) =>
            {
                Log("CreateGameConfirmation");
                Log(ObjectDumper.Dump(game));

                Game = game;
                (You, IsTeamOne) = GameUtility.FindCorrespondingPlayer(You.Id, Game);

                You.WrittenWords = new List<Word>();
                for (int i = 0; i < Game.Config.NumberOfPaperPerPerson; i++)
                    You.WrittenWords.Add(new Word());
                OnGameRoomUpdate?.Invoke(this, Game);
            });

            hubConnection.On<GameRoom>("JoinGameConfirmation", (game) =>
            {
                Log("JoinGameConfirmation");
                Log(ObjectDumper.Dump(game));

                Game = game;
                if (game != null)
                {
                    (You, IsTeamOne) = GameUtility.FindCorrespondingPlayer(You.Id, Game);

                    You.WrittenWords = new List<Word>();
                    for (int i = 0; i < Game.Config.NumberOfPaperPerPerson; i++)
                        You.WrittenWords.Add(new Word());
                }
                OnGameRoomUpdate?.Invoke(this, Game);
            });
            
            hubConnection.On<GameRoom>("UpdateGameRoom", (game) =>
            {
                Log("UpdateGameRoom");

                Game = game;
                OnGameRoomUpdate?.Invoke(this, Game);
            });

            hubConnection.On<GameRoom>("OnGameCompleted", (game) =>
            {
                Log("OnGameCompleted");

                Game = game;
                OnGameCompletedEvent?.Invoke(this, Game);
            });

            hubConnection.On<GameRoom>("OnPlayerTurnWait", (game) =>
            {
                Log("OnPlayerTurnWait");

                Game = game;
                OnPlayerTurnWaitEvent?.Invoke(this, Game);
            });

            hubConnection.On("StartTimer", () =>
            {
                Log("Timer start");
                OnStartTimerEvent?.Invoke(this, null);
            });

            hubConnection.On("StopTimer", () =>
            {
                Log("Timer stop");
                OnStopTimerEvent?.Invoke(this, null);
            });

            hubConnection.On("ResetTimer", () =>
            {
                Log("Timer reset");
                OnResetTimerEvent?.Invoke(this, null);
            });

            await hubConnection.StartAsync();
        }

        private Task HubConnection_Closed(Exception arg)
        {
            var gameCode = Game.Code;
            You = null;
            Game = null;
            
            OnGameRoomUpdate?.Invoke(this, Game);
            return Task.FromResult(0);
        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (hubConnection is not null)
            {
                await hubConnection.DisposeAsync();
            }
        }

        public async Task DisconnectFromGameHub()
        {
            if (hubConnection is not null && IsConnected)
            {
                You = null;
                Game = null;
                await hubConnection.DisposeAsync();
            }
        }

       #endregion Hub

        public async Task<Tuple<bool, string>> CreateGameConnection(Configuration config, Player you)
        {
            You = you;
            await ConnectToGameHub();
            if (!IsConnected)
                return new Tuple<bool, string>(false, $"{_languageManager.CurrentLanguage.IndexPage.SnackNotConnectedLong}");

            await hubConnection.SendAsync("CreateLobbyRequest", config, You);

            return new Tuple<bool, string>(IsConnected, $"{_languageManager.CurrentLanguage.IndexPage.SnackConnectedLong}");
        }

        public async Task<Tuple<bool, string>> JoinGameConnection(string gameCode, Player you)
        {
            You = you;
            await ConnectToGameHub();
            if (!IsConnected)
                return new Tuple<bool, string>(false, $"{_languageManager.CurrentLanguage.IndexPage.SnackNotConnectedLong}");

            await hubConnection.SendAsync("JoinLobbyRequest", gameCode, You);

            return new Tuple<bool, string>(IsConnected, $"{_languageManager.CurrentLanguage.IndexPage.SnackConnectedLong}");
        }

        public async Task ChangeTeamRequest()
        {
            await hubConnection.SendAsync("ChangeTeamRequest", Game.Code, You);
        }

        public async Task ReadyRequest()
        {
            You.IsReady = true;
            await hubConnection.SendAsync("ReadyRequest", Game.Code, You);
        }

        public async Task NotReadyRequest()
        {
            You.IsReady = false;
            await hubConnection.SendAsync("NotReadyRequest", Game.Code, You);
        }

        public async Task ChangeTeamNameRequest()
        {
            await hubConnection.SendAsync("ChangeTeamNameRequest", Game.Code, Game.TeamOne.Name, Game.TeamTwo.Name);
        }

        public async Task StartGameRequest()
        {
            await hubConnection.SendAsync("StartGameRequest", Game.Code);
        }

        public async Task StartTurnRequest()
        {
            await hubConnection.SendAsync("StartTurnRequest", Game.Code);
        }
        public async Task WordFoundRequest(int scoreTeamOne, int scoreTeamTwo, string wordLabel)
        {
            await hubConnection.SendAsync("WordFoundRequest", Game.Code, wordLabel, scoreTeamOne, scoreTeamTwo);
        }

        public async Task WordSkippedRequest(string wordLabel)
        {
            await hubConnection.SendAsync("WordSkippedRequest", Game.Code, wordLabel);
        }

        public async Task EndTurnRequest()
        {
            await hubConnection.SendAsync("EndTurnRequest", Game.Code, Game.CurrentGame);
        }

        private void Log(string msg)
        {
            if (_hostEnv.IsDevelopment())
                Console.WriteLine(msg);
        }
    }
}
