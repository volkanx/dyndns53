using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine;
using DynDns53.CoreLib;
using DynDns53.CoreLib.IPChecker;

namespace DynDns53.Client.DotNetCore
{
    public class Configuration
    {
        [Option('a', "AccessKey", Required = true, HelpText = "AWS IAM Account Access Key with Route53 access" )]
        public string AccessKey { get; set; }

        [Option('s', "SecretKey", Required = true, HelpText = "AWS IAM Account Secret Key with Route53 access")]
        public string SecretKey { get; set; }

        [Option('i', "Interval", Required = false, Default = 300, HelpText = "Time to interval to run the updater")]
        public int? UpdateInterval { get; set; }

        [Option('d', "Domains", Required = true, HelpText = "Domains to update the IP address. Format: zoneId1:domain1 zoneId2:domain2")]
        public IEnumerable<string> RawDomainList { get; set; }

        [Option('c', "IPChecker", Required = false, Default = IPChecker.Custom, HelpText = "The IP Checking service to be used. Options: AWS, DynDns, Custom")]
        public IPChecker IPChecker { get; set; }
        
        public IEnumerable<HostedDomainInfo> DomainList 
        { 
            get
            {
                return RawDomainList.Select(x => 
                {
                    var tokens = x.Split(':');
                    return new HostedDomainInfo()
                    {
                        ZoneId = tokens[0],
                        DomainName = tokens[1]
                    };
                });
            }
        }

		[Option('n', "HeartbeatHostname", Required = false, HelpText = "Hostname to use in heartbeat messages")]
		public string HeartbeatHostname { get; set; }

		[Option('h', "HeartbeatApplicationId", Required = false, HelpText = "ApplicationId to use in heartbeat messages")]
        public string HeartbeatApplicationId { get; set; }

        [Option('u', "HeartbeatServiceUrl", Required = false, HelpText = "URL to post heartbeat messages")]
        public string HeartbeatServiceUrl { get; set; }

        [Option('k', "HeartbeatServiceApiKey", Required = false, HelpText = "API Key to authenticate to heartbeat service")]
        public string HeartbeatServiceApiKey { get; set; }
    }
}
