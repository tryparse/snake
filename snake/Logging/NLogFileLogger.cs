using SnakeGame.Shared.Logging;
using SnakeGame.Shared.Settings.Interfaces;

namespace snake.Logging
{
    internal class NLogFileLogger : ILogger
    {
        private readonly NLog.ILogger _logger;
        private readonly IGameSettings _configuration;

        public NLogFileLogger(IGameSettings configuration)
        {
            _logger = NLog.LogManager.GetLogger("f");
            _configuration = configuration;
        }

        public void Debug(string message)
        {
            if (_configuration.IsLoggingEnabled)
            {
                _logger.Debug(message);
            }
        }
    }
}
