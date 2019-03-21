using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;

namespace Method635.App.Dal.Config
{
    public class JsonConfigurationService : IConfigurationService
    {

        public JsonConfigurationService()
        {
            Setup();
        }

        private void Setup()
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(JsonConfigurationService)).Assembly;
            ServerConfig = (ServerConfig)DeserializeFromStream(assembly.GetManifestResourceStream("Method635.App.Dal.Config.JsonDto.backend-config.json"), typeof(ServerConfig));
        }

        private object DeserializeFromStream(Stream stream, Type targetType)
        {
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
