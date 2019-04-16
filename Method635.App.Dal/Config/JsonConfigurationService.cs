using Method635.App.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using Xamarin.Forms;

namespace Method635.App.Dal.Config
{
    public class JsonConfigurationService : IConfigurationService
    {
        private const string BackendConfigurationJson = "Method635.App.Dal.Config.backend-config.json";
        private readonly ILogger _logger = DependencyService.Get<ILogManager>().GetLog();

        public JsonConfigurationService()
        {
            Setup();
        }

        private void Setup()
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(JsonConfigurationService)).Assembly;
            ServerConfig = (ServerConfig)DeserializeFromStream(assembly.GetManifestResourceStream(BackendConfigurationJson), typeof(ServerConfig));
            if (ServerConfig == null)
                throw new ArgumentException($"Couldn't read configurationfile {BackendConfigurationJson}.");
        }

        private object DeserializeFromStream(Stream stream, Type targetType)
        {
            if (stream == null)
            {
                _logger.Error("Couldn't parse configuration file, stream was null");
                return null;
            }
            var serializer = new JsonSerializer();

            using (var sr = new StreamReader(stream))
                using (var jsonTextReader = new JsonTextReader(sr))
                {
                    return serializer.Deserialize(jsonTextReader, targetType);
                }

        }

        public IServerConfig ServerConfig { get; set; }
    }
}
