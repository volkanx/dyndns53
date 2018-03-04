using Amazon;
using Amazon.Route53;
using DynDns53.Core;
using DynDns53.CoreLib;
using DynDns53.CoreLib.IPChecker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace DynDns53.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            var configHandler = new AppConfigHandler();
            var config = configHandler.GetConfig();
            var ipChecker = IPCheckerHelper.CreateIPChecker(config.IPChecker);
            var route53Client = new AmazonRoute53Client(config.Route53AccessKey, config.Route53SecretKey, RegionEndpoint.EUWest1);
            var dnsUpdater = new DnsUpdater(route53Client);

            HostFactory.Run(x =>
            {
                x.Service<IScheduledTaskService>(s =>
                {
                    s.ConstructUsing(name => new DnsUpdateService(new SchedulerRegistry(new ScheduledTaskWorker(dnsUpdater, ipChecker, config.DomainList))));
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });

                x.RunAsLocalSystem();

                x.SetDisplayName("DynDns53 Service");
                x.SetServiceName("DynDns53");
                x.SetDescription("Updates AWS Route53 records with the current external IP of the system");

                x.StartAutomatically();

                x.EnableServiceRecovery(s =>
                {
                    s.RestartService(1);
                    s.RestartService(2);
                    s.RestartService(5);
                });
            });
        }
    }
}
