
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;




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
        private const string _projectUri = "https://raw.githubusercontent.com/Natalis-Git/Easy-Switch-Presence/main/EasySwitchPresence/EasySwitchPresence.csproj";


        /// <summary>
        /// Requests initial startup data for app. Should only be called once. To ensure data is properly loaded before
        /// dependencies need it, this method is fully synchronous and will block the calling thread while making requests.
        /// </summary>
        public static void Startup()
        {
            try 
            {
                LoadAssetList();
            }
            catch (HttpRequestException err)
            {
                Logger.LogException("AppClient HTTP request error - failed to retrieve asset list", err);
            }
            catch (IOException err)
            {
                Logger.LogException("AppClient IO error", err);
            }

            try
            {
                LoadAssetData();
            }
            catch (HttpRequestException err)
            {
                Logger.LogException("AppClient HTTP request error - failed to retrieve asset data", err);
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


        /// <summary>
        /// Retrieves the latest available version number from remote repository
        /// </summary>
        public static async Task<string> GetVersionAsync()
        {
            HttpResponseMessage response = await _client.GetAsync(_projectUri);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var element = "<Version>";

            var version = content.Substring(content.IndexOf(element) + element.Length, /* Version number length */ 5);

            return String.Concat(version, ".0");
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
