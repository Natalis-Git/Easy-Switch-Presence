
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
        /// Converts a byte array into a Windows.Media.Imaging.BitmapImage.
        /// </summary>
        public static BitmapImage ConvertToBitmapImage(byte[] bytes)
        {
            using var stream = new MemoryStream(bytes);

            var image = new BitmapImage();

            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();

            if (image.CanFreeze)
            {
                image.Freeze();
            }

            return image;
        }
    }

}
