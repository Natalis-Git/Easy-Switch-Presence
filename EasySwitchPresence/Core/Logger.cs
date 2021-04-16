
using System;
using System.IO;




namespace EasySwitchPresence
{
    
    /// <summary>
    /// Very simple logging class for writing to application log file.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Write a custom message to log file.
        /// </summary>
        public static void LogMessage(string msg)
        {
            using var stream = new StreamWriter(AppContext.LoggerFilePath, true);

            stream.WriteLine($"\n{msg}");
        }


        /// <summary>
        /// Write basic exception info to log file with a custom message preceding it.
        /// </summary>
        public static void LogException(string msg, Exception err)
        {
            using var stream = new StreamWriter(AppContext.LoggerFilePath, true);

            stream.WriteLine($"\n{msg}");
            stream.WriteLine($"{err.ToString()}");
            stream.WriteLine($"{err.Message}");
        }


        /// <summary>
        /// Write basic exception info to log file.
        /// </summary>
        public static void LogException(Exception err)
        {
            using var stream = new StreamWriter(AppContext.LoggerFilePath, true);

            stream.WriteLine($"{err.ToString()}");
            stream.WriteLine($"{err.Message}");
        }


        /// <summary>
        /// For dumping an unhandled exception; Writes all crucial data from the exception to log file
        /// </summary>
        public static void UnhandledExceptionDump(Exception unhandled)
        {
            using var stream = new StreamWriter(AppContext.LoggerFilePath, true);
            
            stream.WriteLine("\n\n---- Unhandled Exception ----");
            stream.WriteLine($"{unhandled.ToString()}");
            stream.WriteLine($"{unhandled.Message}");
            stream.WriteLine($"{unhandled.StackTrace}");
            stream.WriteLine("---- End Exception ----");
        }
    }

}
