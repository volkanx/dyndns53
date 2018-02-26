using System;
using System.Collections.Generic;
using DynDns53.CoreLib;
using DynDns53.CoreLib.Config;
             
namespace DynDns53.Client.DotNetCore
{
    public class CommandLineConfigHandler //: IConfigHandler
    {
        private readonly Configuration config;

        public CommandLineConfigHandler(Configuration config)
        {
            this.config = config;
        }

        /*
        public DynDns53Config GetConfig()
        {
            return new DynDns53Config()
            {
                //Route53AccessKey = config.AccessKey,
                // Route53SecretKey = config.SecretKey,
                // UpdateInterval = config.UpdateInterval,

                DomainList = new List<HostedDomainInfo>()
                {
                    new HostedDomainInfo()
                    {
                        ZoneId = "Z5E5IXDGLAKX6",
                        DomainName = "test1.vlkn.me"
                    },
                    new HostedDomainInfo()
                    {
                        ZoneId = "Z5E5IXDGLAKX6",
                        DomainName = "test2.vlkn.me"
                    }
                }

            };
        }
        */

        /*
        public void SaveConfig(DynDns53Config config)
        {
            throw new NotImplementedException();
        }
        */
    }
}
