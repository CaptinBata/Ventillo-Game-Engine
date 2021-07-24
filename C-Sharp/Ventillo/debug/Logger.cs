using System;

namespace Ventillo.Debug
{
    enum LoggerLevels
    {
        DEBUG,
        INFO,
        WARN,
        ERROR
    }
    class Logger
    {
        LoggerLevels level;
        Logger(LoggerLevels level)
        {
            this.level = level;
        }

        public void Debug(string message, object data)
        {
            if (this.level <= LoggerLevels.DEBUG)
            {
                Console.WriteLine($"DEBUG: {message} {data}");
            }
        }

        public void Info(string message, object data)
        {
            if (this.level <= LoggerLevels.INFO)
            {
                Console.WriteLine($"INFO: {message} {data}");
            }
        }

        public void Warn(string message, object data)
        {
            if (this.level <= LoggerLevels.WARN)
            {
                Console.WriteLine($"WARN: {message} {data}");
            }
        }

        public void Error(string message, object data)
        {
            if (this.level <= LoggerLevels.ERROR)
            {
                Console.WriteLine($"ERROR: {message} {data}");
            }
        }
    }
}