
using System;




namespace EasySwitchPresence.ViewModels
{
    
    /// <summary>
    /// Master viewmodel to be set as data context main view. Set upon startup
    /// <summary>
    public class MainViewModel
    {
        public PresenceViewModel PresenceVM { get; set; }
        public GameSearchViewModel GameSearchVM { get; set; }
    }

}
