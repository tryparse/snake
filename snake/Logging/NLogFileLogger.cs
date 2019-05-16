using snake.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake.Logging
{
    class NLogFileLogger : ILogger
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
