using System;
namespace DynDns53.CoreLib.IPChecker
{
    public enum IPChecker
    {
        AWS,
        DynDns,
        Custom
    }

    public class IPCheckerHelper
    {
        public static IIPCheckerStrategy CreateIPChecker(IPChecker ipChecker)
        {
            switch(ipChecker)
            {
                case IPChecker.AWS: return new AwsIPCheckerStrategy();
                case IPChecker.DynDns: return new DynDnsIPCheckerStrategy();
                case IPChecker.Custom: return new CustomIPCheckerStrategy();
                default: throw new ArgumentException("Unknown IP Checker");
            }
        }
    }
}
