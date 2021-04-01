
using System;
using System.Text;




namespace EasySwitchPresence
{

    /// <summary>
    /// Basic utility class for non-specific helper methods
    /// </summary>
    /// <remarks>
    /// The encode() and decode() methods are used for converting the list of available games and their respective keys
    /// into a Base64 text format. This is a horrendous way to "encrypt" data, but it keeps your average user from
    /// fiddling with the list, which is the goal
    /// </remarks>
    public static class Utility
    {
        /// <summary>
        /// Converts text data to Base64 format.
        /// </summary>
        public static string Encode(string data)
        {
            byte[] encodedBytes = UTF8Encoding.UTF8.GetBytes(data);
            return Convert.ToBase64String(encodedBytes);
        }


        /// <summary>
        /// Converts Base64 back to text data
        /// </summary>
        public static string Decode(string data)
        {
            byte[] encodedBytes = Convert.FromBase64String(data);
            return UTF8Encoding.UTF8.GetString(encodedBytes);
        }
    }

}
