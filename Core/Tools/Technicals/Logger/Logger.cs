namespace OctoWhirl.Core.Tools.Technicals.Logger
{
    public class Logger : ILogger
    {
        private StreamWriter _writer;

        private string _logFile;
        private LogLevel _level;

        public Logger(string logFile = null, LogLevel level = LogLevel.Info)
        {
            _level = level;

            _logFile = logFile ?? Path.Combine(GetRootPath(), "log.log");
            if (!File.Exists(_logFile))
                File.Create(_logFile);
            
            _writer = new StreamWriter(_logFile, true);
        }

        #region ILogger Methods
        public void Debug(string message)
        {
            Write($"[DEBUG] : {DateTime.Now} : {message}");
        }

        public void Log(string message)
        {
            if (_level >= LogLevel.Info)
                Write($"[INFO] : {DateTime.Now} : {message}");
        }

        public void Error(string message)
        {
            if (_level >= LogLevel.Error)
                Write($"[ERROR] : {DateTime.Now} : {message}");
        }
        #endregion ILogger Methods
        
        #region Private Methods
        private void Write(string message)
        {
            lock(_writer)
            {
                _writer.WriteLine(message);
            }
        }

        private string GetRootPath()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var paths = currentDir.Split('\\').ToList();
            var rootDir = paths.Take(paths.IndexOf("OctoWhirl"));

            var root = string.Join("\\", rootDir);
            return root;
        }
        #endregion Private Methods
    }
}