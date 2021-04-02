
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


        private void OnAppStartup(object sender, StartupEventArgs e)
        {
            if (File.Exists(AppContext.LoggerFilePath))
            {
                File.WriteAllText(AppContext.LoggerFilePath, String.Empty);
            }

            DispatcherUnhandledException += (sender, e) => {
                UnhandledExceptionDump(e.Exception);
                e.Handled = true;
            };
  

            RPCManager presence = null;

            try
            {
                presence = new RPCManager(new DiscordRpcClient("819326108196929576"));
            }
            catch (Exception err)
            {
                MessageBox.Show("Discord client error - if this error persists, check for newer app version or report this to developer",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Warning
                );

                UnhandledExceptionDump(err);
                Shutdown();
            }


            List<Game> supportedGames = null;

            try
            {
                supportedGames = LoadRPCAssets();
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Startup error: Missing crucial file(s) - To recover, check/redownload latest release",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Warning
                );

                Shutdown();
            }


            var mainViewModel = new MainViewModel();
            mainViewModel.PresenceVM = new PresenceViewModel(presence, supportedGames, Dispatcher);
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
