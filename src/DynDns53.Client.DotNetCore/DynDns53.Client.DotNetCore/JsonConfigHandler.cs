using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynDns53.CoreLib;

namespace DynDns53.Client.DotNetCore
{
    public class JsonConfigHandler //: IConfigHandler
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
    }
}
