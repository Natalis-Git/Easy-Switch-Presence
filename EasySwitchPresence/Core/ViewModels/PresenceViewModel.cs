﻿
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using EasySwitchPresence.Models;
using EasySwitchPresence.Web;




namespace EasySwitchPresence.ViewModels
{

    public class PresenceViewModel : NotifyPropertyBase
    {
        /// <summary>
        /// The title selected by the user. Acts as a liason for the Presence.CurrentGame property,
        /// which this property will update via Setter.
        /// </summary>
        public string SelectedGame
        {
            get => _selectedGame;

            set
            {
                _selectedGame = value ?? _selectedGame;
                
                OnSelectGame();
                OnPropertyChanged(); 
                
                if (value != null)
                {
                    OnGameSelected?.Invoke();
                }   
            }
        }

        /// <summary>
        /// A local instance of the currently selected game's rich presence thumbnail asset
        /// </summary>
        public BitmapImage LocalSelectedGameAsset
        {
            get => _selectedGameLocalAsset;

            private set
            {
                _selectedGameLocalAsset = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Local rich presence details (the title of the currently selected game)
        /// <summary>
        public string LocalPresenceDetails
        {
            get => _localPresenceDetails;

            private set
            {
                _localPresenceDetails = value ?? Presence.DetailsDefault;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Local timestamp of currently displayed presence, in string format for binding
        /// </summary>
        public string LocalPresenceTimestamp
        {
            get => _presenceTimestamp;

            private set
            {
                _presenceTimestamp = value;
                OnPropertyChanged();
            }
        }

        public RPCManager Presence { get; }
        public List<Game> Games { get; }

        public RelayCommand ExecuteEnablePresence { get; private set; }
        public RelayCommand ExecuteDisablePresence { get; private set; }

        /// <summary>
        /// Custom delegate fired when a new game is selected. Intended to be attached to game search instance only, 
        /// hence the use of a custom delegate as opposed to a proper event handler.
        /// </summary>
        public delegate void GameSelected();
        public GameSelected OnGameSelected;


        private string _selectedGame;
        private BitmapImage _selectedGameLocalAsset;
        private string _localPresenceDetails;
        private string _presenceTimestamp;

        // Counter to track local presence elapsed time
        private DispatchCounter _counter;

        // Reference to UI Dispatcher to allow for invocation on UI thread
        private Dispatcher _dispatcher;
    
        // Stopwatch for tracking time seperately from Presence and DispatchTimer
        private Stopwatch _stopwatch = new Stopwatch(); 


        public PresenceViewModel(RPCManager presence, List<Game> games, Dispatcher dispatcher)
        {
            Presence = presence;
            Games = games;

            LocalSelectedGameAsset = new BitmapImage(
                new Uri(AppContext.ResourcesFolderPath + RPCManager.DefaultAssetKey + ".jpg")
            );

            _dispatcher = dispatcher;

            _counter = new DispatchCounter(TimeSpan.FromSeconds(1), _dispatcher);
            _counter.OnTickProxy = OnCounterSecondElapsed;

            Presence.ConnectionChanged += OnConnectionChanged;

            AppContext.Settings.PropertyChanged += OnSettingChanged;
            
            ExecuteEnablePresence = new RelayCommand(OnEnablePresence, CanEnablePresence);
            ExecuteDisablePresence = new RelayCommand(OnDisablePresence, CanDisablePresence);

            if (AppContext.Settings.KeepSelectedGame == true)
            {
                SelectedGame = AppContext.Settings.LastSelectedGame;
            }
        }


        private void OnEnablePresence()
        {
            Presence.Enabled = true;

            _counter.Start();
            
            if (AppContext.Settings.DisableAfterSetTime == true)
            {
                _stopwatch.Start();
            }

            ExecuteDisablePresence.RaiseCanExecuteChanged();
            ExecuteEnablePresence.RaiseCanExecuteChanged();
        }

        
        private bool CanEnablePresence() => !Presence.Enabled && Presence.IsReady;


        private void OnDisablePresence()
        {
            Presence.Enabled = false;

            _counter.Stop();

            if (AppContext.Settings.ShowElapsedTime == true && Presence.CurrentGame != null)
            {
                LocalPresenceTimestamp = "00:00 elapsed";
            }
            else
            {
                LocalPresenceTimestamp = String.Empty;
            }

            _stopwatch.Stop();
            _stopwatch.Reset();
            
            ExecuteEnablePresence.RaiseCanExecuteChanged();
            ExecuteDisablePresence.RaiseCanExecuteChanged();
        }


        private bool CanDisablePresence() => Presence.Enabled && Presence.IsReady;


        // This method subscribes to an event called within RPCManager's internal discord instance
        // which is ran on a seperate thread, hence the usage of the UI thread Dispatcher here.
        private void OnConnectionChanged(object sender, EventArgs e)
        {
            _dispatcher.Invoke(() => {
                ExecuteDisablePresence.RaiseCanExecuteChanged();
                ExecuteEnablePresence.RaiseCanExecuteChanged();
            });
        }


        private async void OnSelectGame()
        {
            Presence.CurrentGame = SelectedGame != null ? Games.Find(game => game.Title == SelectedGame) : null;
            LocalPresenceDetails = Presence.CurrentGame?.Title;
            
            if (Presence.CurrentGame == null)
            {
                LocalSelectedGameAsset = new BitmapImage(
                    new Uri(AppContext.ResourcesFolderPath + RPCManager.DefaultAssetKey + ".jpg")
                );
            }
            else
            {
                var asset = await AppClient.GetAssetAsync(Presence.CurrentGame.AssetKey);       
                LocalSelectedGameAsset = Utility.ConvertToBitmapImage(asset);

                LocalPresenceTimestamp = AppContext.Settings.ShowElapsedTime ? "00:00 elapsed" : String.Empty;

                if (Presence.Enabled == true)
                {
                    _counter.Restart();
                }
            }

            AppContext.Settings.LastSelectedGame = Presence.CurrentGame?.Title;
        }

        
        // NOTE: Despite invoking this method on the UI thread, there seems to still be an issue with the rate at which the
        // bound UI timestamp element updates (usually skipping seconds, but staying in sync with time). In the future this
        // should probably be looked into, but for now it is a minor problem that goes away after a couple dozen seconds.
        private void OnCounterSecondElapsed(object sender, EventArgs e)
        {
            if (AppContext.Settings.ShowElapsedTime == true && Presence.CurrentGame != null)
            {
                if (_counter.TimeElapsed.Hours < 1)
                {
                    LocalPresenceTimestamp = $"{_counter.TimeElapsed.ToString("mm\\:ss")} elapsed";
                }
                else
                {
                    LocalPresenceTimestamp = $"{_counter.TimeElapsed.ToString("hh\\:mm\\:ss")} elapsed";
                }
            }

            if (AppContext.Settings.DisableAfterSetTime == true)
            {
                if (_stopwatch.Elapsed.Hours >= AppContext.Settings.TimeToDisable)
                {
                    OnDisablePresence();
                }
            }
        }


        private void OnSettingChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ShowElapsedTime")
            {
                Presence.Update();

                if (AppContext.Settings.ShowElapsedTime == true)
                {
                    LocalPresenceTimestamp = Presence.CurrentGame != null ? "00:00 elapsed" : String.Empty;
                    
                    if (Presence.Enabled == true)
                    {
                        _counter.Restart();
                    } 
                }
                else
                {
                    LocalPresenceTimestamp = String.Empty;
                }
            }

            if (e.PropertyName == "DisableAfterSetTime")
            {
                if (AppContext.Settings.DisableAfterSetTime == true && Presence.Enabled == true)
                {
                    _stopwatch.Start();
                }
                else if (AppContext.Settings.DisableAfterSetTime == false && _stopwatch.IsRunning == true)
                {
                    _stopwatch.Stop();
                    _stopwatch.Reset();
                }

            }
        }
        
    }

}
