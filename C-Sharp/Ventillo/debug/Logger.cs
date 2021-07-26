using System;

namespace Ventillo.Debug
{
    public enum LoggerLevels
    {
        DEBUG,
        INFO,
        WARN,
        ERROR
    }
    public class Logger
    {
        LoggerLevels level;
        public Logger(LoggerLevels level)
        {
            this.level = level;
        }

        public void Debug(string message, object data = null)
        {
            if (this.level <= LoggerLevels.DEBUG)
            {
                Console.WriteLine($"DEBUG: {message}, {data}");
            }
        }

        public void Info(string message, object data = null)
        {
            if (this.level <= LoggerLevels.INFO)
            {
                Console.WriteLine($"INFO: {message}, {data}");
            }
        }

        public void Warn(string message, object data = null)
        {
            if (this.level <= LoggerLevels.WARN)
            {
                Console.WriteLine($"WARN: {message}, {data}");
            }
        }

        public void Error(string message, Exception error = null, object data = null)
        {
            if (this.level <= LoggerLevels.ERROR)
            {
                Console.WriteLine($"ERROR: {message}, {error} - {data}");
            }
        }
    }
}