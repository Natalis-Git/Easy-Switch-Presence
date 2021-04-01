
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using DiscordRPC;
using EasySwitchPresence.Models;
using EasySwitchPresence.ViewModels;
using EasySwitchPresence.Views;

using Forms = System.Windows.Forms; // Alias to prevent naming collisions with System.Windows



namespace EasySwitchPresence.Startup
{
    
    public partial class App : Application
    {
        private Forms.NotifyIcon _trayIcon;
        private MainWindow _window;

        // TODO: Some more exception handling will need to be implemented for file reads (in the event that they are missing)
        private void OnAppStartup(object sender, StartupEventArgs e)
        {
            if (File.Exists(AppContext.LoggerFilePath))
            {
                File.WriteAllText(AppContext.LoggerFilePath, String.Empty);
            }

            this.DispatcherUnhandledException += (sender, e) => {
                UnhandledExceptionDump(e.Exception);
                e.Handled = true;
            };
  
            var presence = new RPCManager(new DiscordRpcClient("819326108196929576"));

            List<Game> supportedGames = LoadRPCAssets();

            var mainViewModel = new MainViewModel();
            mainViewModel.PresenceVM = new PresenceViewModel(presence, supportedGames, this.Dispatcher);
            mainViewModel.GameSearchVM = new GameSearchViewModel(supportedGames);

            mainViewModel.PresenceVM.OnGameSelected = mainViewModel.GameSearchVM.ClearSearchEntry;

            _window = new MainWindow();
            _window.DataContext = mainViewModel;
            _window.CloseToSystemTray = AppContext.Settings.CloseToTray;

            _trayIcon = LoadTrayIcon();
            _trayIcon.Visible = true;
            
            _window.Show();
            
        }


        private void OnAppExit(object sender, ExitEventArgs e)
        {
            AppContext.Settings.Save();

            _trayIcon.Visible = false;
            _trayIcon?.Icon?.Dispose();
            _trayIcon?.Dispose();
        }


        private void OnTrayIconClicked(object sender, Forms.MouseEventArgs e)
        {
            if (e.Button == Forms.MouseButtons.Left)
            {
                _window.Visibility = Visibility.Visible;
                _window.Activate();
            }
        }


        private void OnCloseOptionClicked(object sender, EventArgs e)
        {
            _window.CloseToSystemTray = false;
            _window.Close();
        }


        private Forms.NotifyIcon LoadTrayIcon()
        {
            var icon = new Forms.NotifyIcon();

            icon.Icon = new System.Drawing.Icon(AppContext.ResourcesFolderPath + "spLogoSmall.ico");
            icon.Text = "Switch Presence";

            icon.ContextMenuStrip = new Forms.ContextMenuStrip();
            icon.ContextMenuStrip.Items.Add("Exit", null, OnCloseOptionClicked);
            
            icon.MouseClick += OnTrayIconClicked;

            return icon;
        }


        private List<Game> LoadRPCAssets()
        {
            // The Supported games and their respective asset keys (once decoded) are in a simple
            // key-value pair format which is handled by the Models.Game constructor. 
            string contents = Utility.Decode(File.ReadAllText(AppContext.AssetFilePath));
            string[] temp = contents.Split('\n');

            var gameList = new List<Game>();

            foreach (string str in temp)
            {
                gameList.Add(new Game(str));
            }

            return gameList;
        }


        private void UnhandledExceptionDump(Exception unhandled)
        {
            using StreamWriter stream = new StreamWriter(AppContext.LoggerFilePath, true);
            
            stream.WriteLine($"--- Error: {unhandled.ToString()} ---");
            stream.WriteLine($"{unhandled.Message}");
            stream.WriteLine($"{unhandled.StackTrace}");
            stream.WriteLine("---- End Exception ----");
        }
    }

}
