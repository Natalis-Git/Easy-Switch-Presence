
using System;
using System.IO;
using System.Reflection;
using EasySwitchPresence.Models;
using EasySwitchPresence.Startup;




namespace EasySwitchPresence
{

    public static class AppContext
    {
        /// <summary>
        /// The current version of Easy Switch Presence.
        /// </summary>
        public static readonly string CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        /// <summary>
        /// Path to config file for storing and retrieving the user settings/options of the application
        /// </summary>
        public static readonly string ConfigFilePath = Path.Combine(Path.GetDirectoryName(typeof(App).Assembly.Location), "config.ini");

        /// <summary>
        /// Path to data file containing the currently supported games as well as their respective RPC asset keys
        /// </summary>
        public static readonly string AssetFilePath = Path.Combine(Path.GetDirectoryName(typeof(App).Assembly.Location), "rpcAssets.dat");

        /// <summary>
        /// Path to log.txt file, all errors, warnings, and notifications are sent here
        /// </summary>
        public static readonly string LoggerFilePath = Path.Combine(Path.GetDirectoryName(typeof(App).Assembly.Location), "Log.txt");

        /// <summary>
        /// Path to resources folder containing local assets
        /// </summary>
        public static readonly string ResourcesFolderPath = Path.Combine(Path.GetDirectoryName(typeof(App).Assembly.Location), "Resources\\");

        /// <summary>
        /// Config instance containing Easy Switch Presence user settings
        /// </summary>
        public static readonly Config Settings = new Config(ConfigFilePath);

    }

}
