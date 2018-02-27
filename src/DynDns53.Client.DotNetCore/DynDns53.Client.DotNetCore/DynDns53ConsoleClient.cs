using System;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.Route53;
using DynDns53.CoreLib;
using DynDns53.CoreLib.IPChecker;

namespace DynDns53.Client.DotNetCore
{
    public class DynDns53ConsoleClient
    {
        private readonly IConfigHandler configHandler;
        private readonly IAmazonRoute53 route53Client;

        public DynDns53ConsoleClient(IConfigHandler configHandler, IAmazonRoute53 route53Client)
        {
            this.configHandler = configHandler;
            this.route53Client = route53Client;
        }

        public async Task StartApplication(Configuration config)
        {
            try
            {
                configHandler.VerifyConfig(config);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return;
            }

            await StartUpdate(config);
        }

        public async Task StartUpdate(Configuration config)
        {
            var ipChecker = IPCheckerHelper.CreateIPChecker(config.IPChecker);
            var dynDns53 = new DnsUpdater(route53Client);

            while (true)
            {
                Console.WriteLine("Getting public IP...");
                var ipAddress = await ipChecker.GetExternalIpAsync();
                Console.WriteLine($"Current public IP: {ipAddress}");

                Console.WriteLine("Updating DNS...");
                await dynDns53.UpdateAllAsync(ipAddress, config.DomainList);
                Console.WriteLine($"Update completed. Waiting for {config.UpdateInterval} seconds");

                System.Threading.Thread.Sleep(config.UpdateInterval.Value * 1000);
            }
        }
    }
}
