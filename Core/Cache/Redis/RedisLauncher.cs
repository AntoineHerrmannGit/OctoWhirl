using Microsoft.Extensions.Logging;
using OctoWhirl.Core.Tools.Technicals.FileManagement;
using System.Diagnostics;

namespace OctoWhirl.Core.Cache.Redis
{
    public class RedisLauncher
    {
        private readonly ILogger<RedisLauncher> _logger;
        private Process _redisProcess;

        public RedisLauncher(ILogger<RedisLauncher> logger)
        {
            _logger = logger;
        }

        #region Public Methods
        public bool StartRedisServer()
        {
            _logger.LogInformation("Starting Redis...");
            var redisDirectory = FileManager.FindDirPath("Redis") 
                ?? throw new FileNotFoundException("Redis");

            var redisPath = Directory.GetFiles(redisDirectory, "redis-server.exe").FirstOrDefault();
            var redisConfig = Directory.GetFiles(redisDirectory, "redis.windows.conf").FirstOrDefault() 
                ?? throw new FileNotFoundException("redis.windows.conf");

            var startInfo = new ProcessStartInfo
            {
                FileName = redisPath,
                Arguments = redisConfig,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            _redisProcess = Process.Start(startInfo);
            _logger.LogInformation("Redis started !");
            return true;
        }

        public bool StopRedisServer()
        {
            _logger.LogInformation($"Stopping Redis...");
            var result = KillProcess(_redisProcess) && Process.GetProcessesByName("redis-server").All(KillProcess);
            _logger.LogInformation("Redis stopped !");

            return result;
        }
        #endregion Public Methods

        #region Private Methods
        private bool KillProcess(Process process)
        {
            process.Kill(true);
            process.WaitForExit();
            return true;
        }
        #endregion Private Methods
    }
}
