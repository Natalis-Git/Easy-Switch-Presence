﻿
using System;




namespace EasySwitchPresence.Models
{

    /// <Summary>
    /// Represents a game supported by EasySwitchPresence that can be set to display.
    /// </Summary>
    public class Game
    {
        public string Title { get; }
        public string AssetKey { get; }


        /// <summary>
        /// Ctor; To be provided with a formatted line from the list of supported games decoded from rpcAssets.dat
        /// </summary>
        public Game(string line)
        {
            string[] temp = line.Split("=");
            
            Title = temp[0].Trim('\"');
            AssetKey = temp[1].Trim('\"');
        }
    }

}
