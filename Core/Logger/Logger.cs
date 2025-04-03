using System;
using System.IO;

namespace OctoWhirl.Core.Logger
{
    public class Logger : ILogger
    {
        private string _logFile;
        private LogLevel _level;

        public Logger(string logFile = null, LogLevel level = LogLevel.Info)
        {
            _level = level;

            _logFile = logFile ?? Path.Combine(GetRootPath(), "log.log");
            if (!File.Exist(_logFile))
                File.Create(_logFile);
        }

        #region ILogger Methods
        public void Debug(string message)
        {
            Write($"[Info] : {DateTime.Now} : {message}");
        }

        public void Log(string message)
        {
            if (_level >= LogLevel.Info)
                Write($"[Info] : {DateTime.Now} : {message}");
        }

        public void Error(string message)
        {
            if (_level >= LogLevel.Error)
                Write($"[Info] : {DateTime.Now} : {message}");
        }
        #endregion ILogger Methods
        
        #region Private Methods
        private void Write(string message)
        {
            lock
            {
                using (StreamWriter outputFile = new StreamWriter(_logFile, true))
                {
                    outputFile.WriteLine(message);
                }
            }
        }

        private string GetRootPath()
        {
            var currentDir = Directory.GetCurrentDirectory()
            var paths = currentDir.Split('\\');
            var rootDir = paths[..paths.IndexOf("OctoWhirl")]

            var root = string.Join("\\", rootDir);
            return root;
        }
        #endregion Private Methods
    }
}