
using System;
using System.IO;
using IniParser;
using IniParser.Model;




namespace EasySwitchPresence.Models
{
    
    /// <summary>
    /// Very basic class for containing Easy Switch Presence settings
    /// <summary>
    public class Config : NotifyPropertyBase
    {
        /// <summary>
        /// Whether or not Rich Presence will show time elapsed for a displayed game
        /// </summary>
        public bool ShowElapsedTime
        {
            get => _showElapsedTime;

            set
            {
                _showElapsedTime = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Whether or not the application will keep the currently selected game when exiting.
        /// </summary>
        public bool KeepSelectedGame
        {
            get => _keepSelectedGame;

            set
            {
                _keepSelectedGame = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Whether or not the application will hide to tray instead of exiting when the user closes
        /// </summary>
        public bool CloseToTray
        {
            get => _closeToTray;

            set
            {
                _closeToTray = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Whether or not the active Rich Presence will disable after an amount of time set by user
        /// </summary>
        public bool DisableAfterSetTime
        {
            get => _disableAfterSetTime;

            set
            {
                _disableAfterSetTime = value;
                OnPropertyChanged();
            }
        }

        public bool DisableAfterOneHour
        {
            get => _disableAfterOneHour;

            set
            {
                _disableAfterOneHour = value;
                TimeToDisable = value ? 1 : TimeToDisable;

                if (value == true)
                {
                    UpdateDisableTimeValues();
                }

                OnPropertyChanged();
            }
        }

        public bool DisableAfterTwoHours
        {
            get => _disableAfterTwoHours;

            set
            {
                _disableAfterTwoHours = value;
                TimeToDisable = value ? 2 : TimeToDisable;

                if (value == true)
                {
                    UpdateDisableTimeValues();
                }

                OnPropertyChanged();
            }
        }

        public bool DisableAfterFourHours
        {
            get => _disableAfterFourHours;

            set
            {
                _disableAfterFourHours = value;
                TimeToDisable = value ? 4 : TimeToDisable;

                if (value == true)
                {
                    UpdateDisableTimeValues();
                }

                OnPropertyChanged();
            }
        }

        public bool DisableAfterEightHours
        {
            get => _disableAfterEightHours;

            set
            {
                _disableAfterEightHours = value;
                TimeToDisable = value ? 8 : TimeToDisable;

                if (value == true)
                {
                    UpdateDisableTimeValues();
                }

                OnPropertyChanged();
            }
        }
 
        /// <summary>
        /// The set number of hours for the automatic disable setting
        /// </summary>
        public int TimeToDisable { get; set; }

        /// <summary>
        /// The last selected game to be saved if KeepSelectedGame is enabled
        /// </summary>
        public string LastSelectedGame { get; set; }


        private bool _showElapsedTime;
        private bool _keepSelectedGame;
        private bool _closeToTray;
        private bool _disableAfterSetTime;

        private bool _disableAfterOneHour;
        private bool _disableAfterTwoHours;
        private bool _disableAfterFourHours;
        private bool _disableAfterEightHours;

        private readonly string _path;


        public Config(string path)
        {
            _path = path;

            if (!File.Exists(_path))
            {
                GenerateNewConfig();
            }

            IniData data = new FileIniDataParser().ReadFile(_path);

            ShowElapsedTime     = Boolean.Parse(data["Settings"]["ShowElapsedTime"]);
            KeepSelectedGame    = Boolean.Parse(data["Settings"]["KeepSelectedGame"]);
            CloseToTray         = Boolean.Parse(data["Settings"]["CloseToTray"]);
            DisableAfterSetTime = Boolean.Parse(data["Settings"]["DisableAfterSetTime"]);
            

            if (KeepSelectedGame == true)
            {
                LastSelectedGame = data["Other"]["LastSelectedGame"];
            }

            TimeToDisable = Int32.Parse(data["Other"]["TimeToDisable"]);
            
            switch(TimeToDisable)
            {
                case 1: DisableAfterOneHour     = true; break;
                case 2: DisableAfterTwoHours    = true; break;
                case 4: DisableAfterFourHours   = true; break;
                case 8: DisableAfterEightHours  = true; break; 
                         
                default: DisableAfterOneHour    = true; break;
            }
        }


        public void Save()
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(_path);

            data["Settings"]["ShowElapsedTime"]     = ShowElapsedTime.ToString();
            data["Settings"]["KeepSelectedGame"]    = KeepSelectedGame.ToString();
            data["Settings"]["CloseToTray"]         = CloseToTray.ToString();
            data["Settings"]["DisableAfterSetTime"] = DisableAfterSetTime.ToString();

            data["Other"]["LastSelectedGame"]   = LastSelectedGame;
            data["Other"]["TimeToDisable"]      = TimeToDisable.ToString();

            parser.WriteFile("config.ini", data);
        }


        // Manages mutual exclusivity between time settings. This is done here manually due to binding issues.
        private void UpdateDisableTimeValues()
        {
            switch (TimeToDisable)
            {
                case 1:
                    DisableAfterTwoHours = false;
                    DisableAfterFourHours = false;
                    DisableAfterEightHours = false;
                    break;
                case 2:
                    DisableAfterOneHour = false;
                    DisableAfterFourHours = false;
                    DisableAfterEightHours = false;
                    break;
                case 4:
                    DisableAfterOneHour = false;
                    DisableAfterTwoHours = false;
                    DisableAfterEightHours = false;
                    break;
                case 8:
                    DisableAfterOneHour = false;
                    DisableAfterTwoHours = false;
                    DisableAfterFourHours = false;
                    break;
            }
        }


        private void GenerateNewConfig()
        {
            var contents = new string[] {
                "[Settings]",
                "ShowElapsedTime = True",
                "KeepSelectedGame = False",
                "CloseToTray = False",
                "DisableAfterSetTime = False",
                "\n",
                "[Other]",
                "LastSelectedGame = ",
                "TimeToDisable = 1"
            };

            File.WriteAllLines(_path, contents);        
        }
        
    }

}
