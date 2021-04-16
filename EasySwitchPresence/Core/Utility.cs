
using System;
using System.Text;
using System.Windows.Media.Imaging;
using System.IO;




namespace EasySwitchPresence
{

    /// <summary>
    /// Basic utility class for non-specific helper methods
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Converts text data to Base64 format.
        /// </summary>
        public static string Encode64(string data)
        {
            byte[] encodedBytes = UTF8Encoding.UTF8.GetBytes(data);
            return Convert.ToBase64String(encodedBytes);
        }


        /// <summary>
        /// Converts Base64 back to text data.
        /// </summary>
        public static string Decode64(string data)
        {
            byte[] encodedBytes = Convert.FromBase64String(data);
            return UTF8Encoding.UTF8.GetString(encodedBytes);
        }


        /// <summary>
        /// Writes the provided string to file at provided path
        /// </summary>
        public static void WriteToFile(string path, string contents)
        {
            using var stream = new StreamWriter(path, true);
            stream.WriteLine($"\n{contents}");
        }


        /// <summary>
        /// Reads all content from file at provided path
        /// </summary>
        public static string ReadFromFile(string path)
        {
            using var stream = new StreamReader(path);
            return stream.ReadToEnd();
        }


        /// <summary>
        /// Converts a byte array into a Windows.Media.Imaging.BitmapImage.
        /// </summary>
        public static BitmapImage ConvertToBitmapImage(byte[] bytes)
        {
            using var stream = new MemoryStream(bytes);

            var image = new BitmapImage();

            image.BeginInit();
            image.StreamSource = stream;
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.EndInit();

            if (image.CanFreeze)
            {
                image.Freeze();
            }

            return image;
        }
    }

}
