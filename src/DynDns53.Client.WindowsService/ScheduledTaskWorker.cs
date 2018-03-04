using DynDns53.Core;
using DynDns53.CoreLib;
using DynDns53.CoreLib.IPChecker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynDns53.Service
{
    public class ScheduledTaskWorker
    {
        private readonly IDnsUpdater dnsUpdater;
        private readonly IIPCheckerStrategy ipChecker;
        private readonly IEnumerable<HostedDomainInfo> domainList;

        public ScheduledTaskWorker(IDnsUpdater dnsUpdater, IIPCheckerStrategy ipChecker, IEnumerable<HostedDomainInfo> domainList)
        {
            this.dnsUpdater = dnsUpdater;
            this.ipChecker = ipChecker;
            this.domainList = domainList;
        }

        public async Task RunAsync()
        {
            Console.WriteLine("Running DNS updates..." + DateTime.Now.ToString());

            var ipAddress = await ipChecker.GetExternalIpAsync();
            await dnsUpdater.UpdateAllAsync(ipAddress, domainList);

            Console.WriteLine("Finished updating at " + DateTime.Now.ToString());
        }
    }
}
