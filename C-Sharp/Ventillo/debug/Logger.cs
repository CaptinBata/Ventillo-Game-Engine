using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"INFO: {message}, {data}");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        public void Warn(string message, object data = null)
        {
            if (this.level <= LoggerLevels.WARN)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"WARN: {message}, {data}");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        public void Error(string message, Exception error = null, object data = null)
        {
            if (this.level <= LoggerLevels.ERROR)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ERROR: {message}, {error} - {data}");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
    }
}