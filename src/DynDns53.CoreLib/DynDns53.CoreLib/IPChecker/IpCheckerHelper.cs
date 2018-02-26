using System;
namespace DynDns53.CoreLib.IPChecker
{
    public enum IpCheckers
    {
        AWS,
        DynDns,
        Custom
    }

    public class IpCheckerHelper
    {
        public static IIpChecker CreateIpChecker(IpCheckers ipChecker)
        {
            switch(ipChecker)
            {
                case IpCheckers.AWS: return new AwsIpChecker();
                case IpCheckers.DynDns: return new DynDnsIPChecker();
                case IpCheckers.Custom: return new CustomIpChecker();
                default: throw new ArgumentException("Unknown IP Checker");
            }
        }
    }
}
