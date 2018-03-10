using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynDns53.CoreLib.Config
{
    public class JsonConfigHandler : IConfigHandler
    {
        private readonly string _configName = "config.json";
        
        public DynDns53Config GetConfig()
        {
            var rawJson = File.ReadAllText(_configName);
            var config = Newtonsoft.Json.JsonConvert.DeserializeObject<DynDns53Config>(rawJson);
            return config;
        }

        public void SaveConfig(DynDns53Config config)
        {
            var configAsJson = Newtonsoft.Json.JsonConvert.SerializeObject(config);
            File.WriteAllText(_configName, configAsJson);
        }
    }
}
