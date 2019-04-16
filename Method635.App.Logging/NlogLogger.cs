using System;
using NLog;

namespace Method635.App.Logging
{
    public class NLogLogger : ILogger
    {
        private readonly Logger _logger;

        public NLogLogger()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }
        public NLogLogger(Logger logger)
        {
            _logger = logger;
        }

        public void Debug(string message, params object[] args)
        {
            _logger.Debug(message, args);
        }

        public void Error(string message, params object[] args)
        {
            _logger.Error(message, args);
        }

        public void Info(string message, params object[] args)
        {
            _logger.Info(message, args);
        }

        public void Warn(string message, params object[] args)
        {
            _logger.Warn(message, args);
        }
    }
}
