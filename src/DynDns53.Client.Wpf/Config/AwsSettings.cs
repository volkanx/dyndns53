using System;
using System.Configuration;

namespace DynDns53.Core
{
    public class AwsSettings : ConfigurationSection
    {
        private static AwsSettings settings = ConfigurationManager.GetSection("awsSettings") as AwsSettings;
        public static AwsSettings Settings
        {
            get
            {
                return settings;
            }
        }

        [ConfigurationProperty("route53AccessKey", IsRequired = true)]
        public string Route53AccessKey
        {
            get { return (string)this["route53AccessKey"]; }
            set { this["route53AccessKey"] = value; }
        }
        
        [ConfigurationProperty("route53SecretKey", IsRequired = true)]
        public string Route53SecretKey
        {
            get { return (string)this["route53SecretKey"]; }
            set { this["route53SecretKey"] = value; }
        }

        
    }
}

