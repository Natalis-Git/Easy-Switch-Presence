
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;


// TODO: Exception handling still needs to be implemented here as well.


namespace EasySwitchPresence.Web
{

    /// <summary>
    /// Represents application web client for making any necessary server side requests. Wraps System.Net.Http.HttpClient
    /// </summary>
    public static class AppClient
    {
        private static readonly HttpClient _client = new HttpClient();
        private static List<AssetData> _assets = null;

        private const string _gameListUri = "https://raw.githubusercontent.com/Natalis-Git/Easy-Switch-Presence/main/EasySwitchPresence/rpcAssets.dat";
        private const string _assetDataUri = "https://discord.com/api/v8/oauth2/applications/819326108196929576/assets";
        private const string _assetStorageUri = "https://cdn.discordapp.com/app-assets/819326108196929576/";


        /// <summary>
        /// Requests initial startup data for AppClient such as assets, version, etc. Should only be called once.
        /// To ensure data is properly loaded before dependencies need it, this method is fully synchronous and will
        /// block the calling thread while making requests.
        /// </summary>
        public static void Startup()
        {
            try 
            {
                LoadAssetList();
            }
            catch (HttpRequestException err)
            {
                Utility.WriteToFile(AppContext.LoggerFilePath, "AppClient HTTP request error - failed to retrieve asset list");
                Utility.WriteToFile(AppContext.LoggerFilePath, $"{err.Message}");
            }
            catch (IOException err)
            {
                Utility.WriteToFile(AppContext.LoggerFilePath, $"\nAppClient IO error - {err.Message}");
            }

            try
            {
                LoadAssetData();
            }
            catch (HttpRequestException err)
            {
                Utility.WriteToFile(AppContext.LoggerFilePath, "\nAppClient HTTP request error - failed to retrieve asset data");
                Utility.WriteToFile(AppContext.LoggerFilePath, $"{err.Message}");
            }
        }


        /// <summary>
        /// Requests raw asset from discord using the provided key.
        /// </summary>
        public static async Task<byte[]> GetAssetAsync(string key)
        {
            var asset = _assets.Find(asset => asset.name == key);

            HttpResponseMessage response = await _client.GetAsync(_assetStorageUri + asset.id + ".png");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsByteArrayAsync();
        }


        private static void LoadAssetList()
        {
            var requestTask = Task.Run(() => _client.GetAsync(_gameListUri));    
            requestTask.Wait();

            HttpResponseMessage response = requestTask.Result;
            response.EnsureSuccessStatusCode();

            var readTask = Task.Run(() => response.Content.ReadAsStringAsync());
            readTask.Wait();

            string content = readTask.Result;
            File.WriteAllText(AppContext.AssetFilePath, content);
        }


        private static void LoadAssetData()
        {
            var requestTask = Task.Run(() => _client.GetAsync(_assetDataUri));
            requestTask.Wait();

            HttpResponseMessage response = requestTask.Result;
            response.EnsureSuccessStatusCode();

            var readTask = Task.Run(() => response.Content.ReadAsStringAsync());
            readTask.Wait();

            string content = readTask.Result;

            _assets = JsonSerializer.Deserialize<List<AssetData>>(content);
        }
    }

}
