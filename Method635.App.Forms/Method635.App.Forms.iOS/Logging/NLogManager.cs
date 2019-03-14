using System;
using System.IO;
using System.Runtime.CompilerServices;
using Method635.App.Forms.iOS.Logging;
using Method635.App.Logging;
using NLog;
using NLog.Config;
using NLog.Targets;

[assembly: Xamarin.Forms.Dependency(typeof(NLogManager))]
namespace Method635.App.Forms.iOS.Logging
{
    class NLogManager : ILogManager
    {
        public NLogManager()
        {
            var config = new LoggingConfiguration();

            var consoleTarget = new ConsoleTarget();
            config.AddTarget("console", consoleTarget);

            var consoleRule = new LoggingRule("*", LogLevel.Trace, consoleTarget);
            config.LoggingRules.Add(consoleRule);

            var fileTarget = new FileTarget();
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            fileTarget.FileName = Path.Combine(folder, "Log.txt");
            config.AddTarget("file", fileTarget);

            var fileRule = new LoggingRule("*", LogLevel.Warn, fileTarget);
            config.LoggingRules.Add(fileRule);

            LogManager.Configuration = config;
        }

        public Method635.App.Logging.ILogger GetLog([CallerFilePath] string callerFilePath = "")
        {
            var fileName = callerFilePath;

            if (fileName.Contains("/"))
            {
                fileName = fileName.Substring(fileName.LastIndexOf("/", StringComparison.CurrentCultureIgnoreCase) + 1);
            }

            var logger = LogManager.GetLogger(fileName);
            return new NLogLogger(logger);
        }
    }
}