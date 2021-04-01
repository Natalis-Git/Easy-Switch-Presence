﻿
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using DiscordRPC;
using DiscordRPC.Logging;




namespace EasySwitchPresence.Models
{
    /// <summary>
    /// Rich Presence instance manager; Essentially a wrapper for DiscordRpcClient
    /// </summary>
    public class RPCManager : INotifyPropertyChanged
    {      
        /// <summary>
        /// Gets or sets the current game to be displayed by Rich Presence. Refreshes automatically.
        /// null value sets default presence.
        /// </summary>
        public Game CurrentGame
        {
            get => _currentGame;

            set
            {
                _currentGame = value;
                GameIsSelected = _currentGame == null ? false : true;
                Update();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Whether or not there is a game currently set to display to Rich Presence
        /// </summary>
        public bool GameIsSelected
        {
            get => _gameIsSelected;

            private set
            {
                _gameIsSelected = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets whether Rich Presence is currently enabled and displaying to Discord. Presence updates accordingly.
        /// </summary> 
        public bool Enabled
        {
            get => _enabled;

            set 
            {
                _enabled = value;
                ConnectionStatus = _enabled ? "Enabled" : "Disabled";
                Update();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Current connection status for Discord client
        /// </summary>
        public string ConnectionStatus
        {
            get => _connectionStatus;

            private set
            {
                _connectionStatus = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets whether or not the Discord client is connected and ready for use
        /// </summary>
        public bool IsReady { get; private set; }

        /// <summary>
        /// The default presence details for when no game is selected
        /// </summary>
        public string DetailsDefault { get; set; } = "Menus - Idle";

        /// <summary>
        /// The default rich presence asset key for when no game is selected
        /// </summary>
        public string AssetKeyDefault { get; set; } = "spa_000";


        /// <summary>
        /// Fires whenever connection to discord is established or lost
        /// </summary>
        public event EventHandler ConnectionChanged;

        public event PropertyChangedEventHandler PropertyChanged;


        private Game _currentGame;
        private bool _gameIsSelected;
        private bool _enabled;
        private string _connectionStatus;

        ////////// Core Discord client //////////
        private readonly DiscordRpcClient _client;


        public RPCManager(DiscordRpcClient client)
        {
            _client = client;

            _client.Logger = new FileLogger(AppContext.LoggerFilePath) { Level = LogLevel.Warning };

            _client.OnReady += OnReady;
            _client.OnPresenceUpdate += OnPresenceUpdate;
            _client.OnConnectionFailed += OnConnectionFailed;
            _client.OnError += OnError;

            ConnectionStatus = "Connecting to Discord...";
            GameIsSelected = false;

            _client.Initialize();      
        }


        public void Update()
        {
            if (Enabled == false)
            {
                _client.ClearPresence();
                return;
            }

            if (CurrentGame == null)
            {
                _client.SetPresence(new RichPresence() {
                    Details = DetailsDefault,
                    Assets = new Assets() {
                        LargeImageKey = AssetKeyDefault,
                        LargeImageText = "Idle"
                    }
               });

               return;
            }

            _client.SetPresence(new RichPresence() {
                Details = CurrentGame.Title,
                Timestamps = AppContext.Settings.ShowElapsedTime ? Timestamps.Now : null,
                Assets = new Assets() {
                    LargeImageKey = CurrentGame.AssetKey,
                    LargeImageText = $"Playing {CurrentGame.Title}",
                    SmallImageKey = AssetKeyDefault,
                    SmallImageText = "Nintendo Switch"
                }
            });
        }


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        protected virtual void OnConnectionChanged()
        {
            ConnectionChanged?.Invoke(this, EventArgs.Empty);
        }


        private void OnReady(object sender, DiscordRPC.Message.ReadyMessage msg)
        {
            IsReady = true;
            Enabled = false;
            OnConnectionChanged();

            using StreamWriter stream = new StreamWriter(AppContext.LoggerFilePath, true);
            stream.WriteLine($"User ready - {msg.User.Username}");
        }


        private void OnPresenceUpdate(object sender, DiscordRPC.Message.PresenceMessage msg)
        {
            using StreamWriter stream = new StreamWriter(AppContext.LoggerFilePath, true);
            stream.WriteLine($"Recieved Presence Update - {msg.Presence.Details}");
        }


        private void OnConnectionFailed(object sender, DiscordRPC.Message.ConnectionFailedMessage msg)
        {
            IsReady = false;
            ConnectionStatus = "Unable to connect to Discord. Retrying...";
            OnConnectionChanged();

            using StreamWriter stream = new StreamWriter(AppContext.LoggerFilePath, true);
            stream.WriteLine("ERROR - Failed to connect to Discord");
        }


        private void OnError(object sender, DiscordRPC.Message.ErrorMessage msg)
        {
            using StreamWriter stream = new StreamWriter(AppContext.LoggerFilePath, true);
            stream.WriteLine($"ERROR - {msg.Message}");
        }
    }

}
