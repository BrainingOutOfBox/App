using System;
using System.Collections.Generic;
using System.Text;

namespace Method635.App.Dal.Config
{
    class ConfigurationService : IConfigurationService
    {
        private string _configPath;

        public ConfigurationService(string configPath = null)
        {
            _configPath = configPath;
            Setup();
        }

        private void Setup()
        {
            if (string.IsNullOrEmpty(_configPath))
            {
                //TODO get path to backend-config.json and jsonserialize it
            }
        }

        public IServerConfig ServerConfig { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
