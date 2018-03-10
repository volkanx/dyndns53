using System;
using System.Linq;
using DynDns53.CoreLib.Config;
using DynDns53.CoreLib.IPChecker;

namespace DynDns53.Client.DotNetCore
{
    public class ConsoleConfigHandler : IConfigHandler
    {
        private readonly Configuration config;

        public ConsoleConfigHandler(Configuration config)
        {
            this.config = config;
        }

        public DynDns53Config GetConfig()
        {
            return new DynDns53Config()
            {
                Route53AccessKey = config.AccessKey,
                Route53SecretKey = config.SecretKey,
                UpdateInterval = config.UpdateInterval.HasValue ? config.UpdateInterval.Value : 300,
                RunAtSystemStart = false,
                IPChecker = IPCheckerHelper.CreateIPChecker(config.IPChecker),
                DomainList = config.DomainList.ToList()
            };
        }

        public void SaveConfig(DynDns53Config config)
        {
            throw new NotImplementedException();
        }
    }
}
