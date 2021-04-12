﻿
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;


// TODO: Exception handling still needs to be implemented here.


namespace EasySwitchPresence.Web
{

    /// <summary>
    /// Represents application web client for making any necessary server side requests
    /// </summary>
    public static class AppClient
    {
        /// <summary>
        /// Key to default Switch logo asset for use by large or small icon in RPC
        /// </summary>
        public static string DefaultAssetKey { get; }


        private static readonly HttpClient _client = new HttpClient();

        private static readonly string _assetDataUri = "https://discord.com/api/v8/oauth2/applications/819326108196929576/assets";
        private static readonly string _assetStorageUri = "https://cdn.discordapp.com/app-assets/819326108196929576";

        private static List<Asset> _assets = null;


        /// <summary>
        /// Retrieves presence assets from discord. Should be called once during startup.
        /// </summary>
        public static async void LoadAssetDataAsync()
        {
            HttpResponseMessage response = await _client.GetAsync(_assetDataUri);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            _assets = JsonSerializer.Deserialize<List<Asset>>(content);
        }


        /// <summary>
        /// Retrieves asset from discord.
        /// </summary>
        public static async Task<byte[]> GetAssetAsync(string key)
        {
            var asset = _assets.Find(asset => asset.name == key);

            HttpResponseMessage response = await _client.GetAsync(_assetDataUri + '/' + asset.id + ".png");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsByteArrayAsync();
        }
    }

}
