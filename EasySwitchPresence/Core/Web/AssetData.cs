﻿
using System;




namespace EasySwitchPresence.Web
{
    
    /// <summary>
    /// Representative custom type for retrieving and storing assets via web client. 
    /// An "asset" is just Discord's name for the thumbnail of a game instance to be displayed by Rich Presence.
    /// </summary>
    public struct AssetData
    {
        public string id { get; set; }
        public int type { get; set; }
        public string name {get; set; }
    }

}
