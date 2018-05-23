using System;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.Route53;
using DynDns53.CoreLib;
using DynDns53.CoreLib.Config;
using DynDns53.CoreLib.IPChecker;
using log4net;
using Watchdog.Core;

namespace DynDns53.Client.DotNetCore
{
    public class DynDns53ConsoleClient
    {
        private readonly IConfigHandler configHandler;
        private readonly IAmazonRoute53 route53Client;
		private readonly IHeartbeatService heartbeatService;
		private readonly ILog logger;
		private Configuration config;


		public DynDns53ConsoleClient(IConfigHandler configHandler, IAmazonRoute53 route53Client, IHeartbeatService heartbeatService, ILog logger)
        {
            this.configHandler = configHandler;
            this.route53Client = route53Client;
			this.heartbeatService = heartbeatService;
			this.logger = logger;
        }

        public async Task StartApplication(Configuration config)
        {			
            try
            {
				this.config = config;
                await StartUpdate();
            }
            catch (Exception ex)
            {
				logger.Error(ex);
                return;
            }
        }

        public async Task StartUpdate()
        {
            var ipChecker = IPCheckerHelper.CreateIPChecker(this.config.IPChecker);
            var dynDns53 = new DnsUpdater(route53Client);

            while (true)
            {            
                try
				{
					logger.Info("Getting public IP...");
                    var ipAddress = await ipChecker.GetExternalIpAsync();
					logger.Info($"Current public IP: {ipAddress}");

					logger.Info("Updating DNS...");
					await dynDns53.UpdateAllAsync(ipAddress, this.config.DomainList);
					logger.Info($"Update completed. Waiting for {this.config.UpdateInterval} seconds");

					SendHeartbeat();
				}
				catch (Exception ex)
				{
					logger.Error($"Exception: {ex.Message}");
				}
                
				System.Threading.Thread.Sleep(this.config.UpdateInterval.Value * 1000);
            }
        }

		private void SendHeartbeat()
		{
            // Don't use heartbeats if an application id has not been provided
			if (string.IsNullOrWhiteSpace(this.config.HeartbeatApplicationId))
			{
				return;
			}

			logger.Info("Sending heartbeat");

			var heartbeat = new Heartbeat
            {
				ApplicationId = this.config.HeartbeatApplicationId,
				Hostname = this.config.HeartbeatHostname
            };
            
			heartbeatService.SendHeartbeat(heartbeat);

			logger.Info("Heartbeat sent");
		}
    }
}
