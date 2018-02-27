using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynDns53.CoreLib;

namespace DynDns53.Client.DotNetCore
{
    public interface IConfigHandler
    {
        Configuration GetConfig();
        void SaveConfig(Configuration config);
        void VerifyConfig(Configuration updatedConfig);
    }

    public class JsonConfigHandler : IConfigHandler
    {
        private readonly string configName = "config.json";

        public Configuration GetConfig()
        {
            var rawJson = File.ReadAllText(configName);
            var config = Newtonsoft.Json.JsonConvert.DeserializeObject<Configuration>(rawJson);
            return config;
        }

        public void SaveConfig(Configuration config)
        {
            var configAsJson = Newtonsoft.Json.JsonConvert.SerializeObject(config);
            File.WriteAllText(configName, configAsJson);
        }

        public void VerifyConfig(Configuration updatedConfig)
        {
            if (string.IsNullOrWhiteSpace(updatedConfig.AccessKey))
            {
                throw new ArgumentException("AccessKey is not supplied");
            }

            if (string.IsNullOrWhiteSpace(updatedConfig.SecretKey))
            {
                throw new ArgumentException("SecretKey is not supplied");
            }

            if (updatedConfig.RawDomainList == null || !updatedConfig.RawDomainList.Any())
            {
                throw new ArgumentException("At least 1 domain must be supplied");
            }
        }
    }
}
