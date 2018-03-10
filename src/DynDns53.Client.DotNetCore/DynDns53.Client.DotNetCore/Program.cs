using System;
using Amazon.Route53;
using System.Threading.Tasks;
using SimpleInjector;
using Amazon;
using CommandLine;
using Newtonsoft.Json;
using System.Linq;
using CommandLine.Text;
using DynDns53.CoreLib.Config;

namespace DynDns53.Client.DotNetCore
{
    class Program
    {
        static Container container;

        private void InitializeIOCContainer(Configuration config)
        {
            container = new Container();

            container.Register<IAmazonRoute53>(() =>
            { 
                return new AmazonRoute53Client(config.AccessKey, config.SecretKey, RegionEndpoint.GetBySystemName("us-east-1")); 
            }, Lifestyle.Singleton);

            container.Register<IConfigHandler, JsonConfigHandler>();

            container.Verify();
        }

        private Configuration ParseArguments(string[] args)
        {
            var parser = new Parser(with => with.EnableDashDash = true);
            var parsedArgs = parser.ParseArguments<Configuration>(args);
            return parsedArgs.MapResult(c => c,
                                        errors =>
                                        {
                                            var errorMessage = $"Parsing of command line arguments has failed: {JsonConvert.SerializeObject(errors)}";
                                            throw new FormatException(errorMessage);
                                        });
        }

        private void PrintUsage()
        {
            Console.WriteLine("Usage: ");
            Console.WriteLine("dotnet DynDns53.Client.DotNetCore.dll --AccessKey ACCESS_KEY --SecretKey SECRET_KEY --Domains zoneId1:domain1 zoneId2:domain2 [--Interval 300] [--IPChecker Custom]");
            Console.WriteLine("AccessKey: Mandatory. AWS IAM Account Access Key with Route53 access");
            Console.WriteLine("SecretKey: Mandatory. AWS IAM Account Secret Key with Route53 access");
            Console.WriteLine("Domains: Mandatory. Domains to update the IP address. Format: zoneId1:domain1 zoneId2:domain2");
            Console.WriteLine("Interval: Optional. Time to interval to run the updater. Default is 5 minutes (300 seconds)");
            Console.WriteLine("IPChecker: Optional. The service to use to get public IP. Default is Custom. Can be AWS, DynDns or Custom");
        }

        static async Task Main(string[] args)
        {
            var p = new Program();

            if (args.Length == 0)
            {
                p.PrintUsage();
                return;
            }

            Configuration config;
            try
            {
                config = p.ParseArguments(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                p.PrintUsage();
                return;
            }

            p.InitializeIOCContainer(config);

            var configHandler = container.GetInstance<IConfigHandler>();
            var route53Client = container.GetInstance<IAmazonRoute53>();
            var client = new DynDns53ConsoleClient(configHandler, route53Client);
            await client.StartApplication(config);
        }
    }
}
