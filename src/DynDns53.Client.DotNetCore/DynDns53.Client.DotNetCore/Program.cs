using System;
using Fclp;
using DynDns53.CoreLib;
using Amazon.Route53;
using System.Threading.Tasks;
using SimpleInjector;
using DynDns53.CoreLib.IPChecker;
using Amazon;

namespace DynDns53.Client.DotNetCore
{
    class Program
    {
       // static Container container;

        public async Task StartUpdate(Configuration config)
        {
            var ipChecker = IpCheckerHelper.CreateIpChecker(IpCheckers.Custom);

            // var route53Client = new AmazonRoute53Client(config.AccessKey, config.SecretKey, config.Region);

            var route53Client = new AmazonRoute53Client("AKIAJBD2EYUNDLMG7HQA", 
                                                        "1S0dIHu5wIgGivExGUApTGNdVNNbJwMCdItbZXOS", 
                                                        RegionEndpoint.GetBySystemName("eu-west-1"));



            var configHandler = new CommandLineConfigHandler(config);


            var dynDns53 = new DnsUpdater(route53Client);

            while (true)
            {
                Console.WriteLine("Updating DNS...");
                var ipAddress = await ipChecker.GetExternalIpAsync();
               // var config = configHandler.GetConfig();

              //  await dynDns53.UpdateAllAsync(ipAddress, config.DomainList);

                System.Threading.Thread.Sleep(5 * 1000);   

            }


            // Console.ReadLine();

        }

        /*
        static async Task InitializeIOCContainer(Configuration config)
        {
            container = new Container();

            container.Register<IAmazonRoute53, AmazonRoute53Client>();

            container.Verify();
        }
        */

        static async Task Main(string[] args)
        {
            var config = new FluentCommandLineParser<Configuration>();
            config.Setup(arg => arg.AccessKey).As('a', "accessKey");
            config.Setup(arg => arg.AccessKey).As('s', "secretKey");
            config.Setup(arg => arg.AccessKey).As('r', "region");


            var p = new Program();

            Console.WriteLine(config.Object.AccessKey);
            Console.WriteLine(config.Object.SecretKey);
            Console.WriteLine(config.Object.Region);

            // await InitializeIOCContainer(config.Object);

            await p.StartUpdate(config.Object);
        }
    }
}
