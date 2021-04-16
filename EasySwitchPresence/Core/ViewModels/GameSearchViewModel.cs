
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EasySwitchPresence.Models;



namespace EasySwitchPresence.ViewModels
{

    public class GameSearchViewModel : NotifyBase
    {
        /// <summary>
        /// The game to be searched for as inputted by the user
        /// </summary>
        public string GameSearchEntry
        {
            get => _gameSearchEntry;

            set
            {
                _gameSearchEntry = value;
                OnPropertyChanged();
                UpdateSearchEntry();
            }
        }


        public ObservableCollection<string> SearchResults { get; set; } = new ObservableCollection<string>();


        private List<Game> _availableGames;
        private string _gameSearchEntry;


        public GameSearchViewModel(List<Game> availableGames) => _availableGames = availableGames;


        public void ClearSearchEntry() => GameSearchEntry = String.Empty;


        private void UpdateSearchEntry()
        {
            if (!String.IsNullOrEmpty(GameSearchEntry) && _availableGames != null)
            {
                SearchResults.Clear();

                foreach (Game game in _availableGames)
                {
                    if (game.Title.ToLower().Contains(GameSearchEntry.ToLower()))
                    {
                        SearchResults.Add(game.Title);
                    }
                }
            }
        }
    }
    
}
