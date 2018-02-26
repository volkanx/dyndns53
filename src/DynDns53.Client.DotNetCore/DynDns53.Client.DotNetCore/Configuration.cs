using System;
namespace DynDns53.Client.DotNetCore
{
    public class Configuration
    {
        // AWS Route53 settings - Keys to access route53 and the 
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string Region { get; set; }

        // Interval to run DNS updater. In seconds. Default is 5 minutes
        public int UpdateInterval { get; set; } = 300;

        // Choose to save the supplied config locally so that they will be
        // used on consecutive runs if no values are supplied
        public bool SaveConfig { get; set; } = true;

    }
}
