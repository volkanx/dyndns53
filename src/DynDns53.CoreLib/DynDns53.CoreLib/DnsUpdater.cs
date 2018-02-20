using Amazon.Route53;
using Amazon.Route53.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynDns53.CoreLib
{
    public interface IDnsUpdater
    {
        Task UpdateAsync();
    }

    public class DnsUpdater : IDnsUpdater
    {
        private readonly IConfigHandler _configHandler;
        private readonly IIpChecker _ipChecker;
        private readonly IAmazonRoute53 _amazonClient;

        public DnsUpdater(IConfigHandler configHandler, IIpChecker ipchecker, IAmazonRoute53 amazonClient)
        {
            _configHandler = configHandler;
            _ipChecker = ipchecker;
            _amazonClient = amazonClient;
        }

        public async Task UpdateAsync()
        {
            var config = _configHandler.GetConfig();
            string currentExternalIp = _ipChecker.GetExternalIp();

            foreach (var domain in config.DomainList)
            {
                string subdomain = domain.DomainName;
                string zoneId = domain.ZoneId;

                var listResourceRecordSetsResponse = await _amazonClient.ListResourceRecordSetsAsync(new ListResourceRecordSetsRequest() { HostedZoneId = zoneId });
                var resourceRecordSet = listResourceRecordSetsResponse.ResourceRecordSets.First(recordset => recordset.Name == subdomain);
                var resourceRecord = resourceRecordSet.ResourceRecords.First();

                if (resourceRecord.Value != currentExternalIp)
                {
                    resourceRecord.Value = currentExternalIp;

                    await _amazonClient.ChangeResourceRecordSetsAsync(new ChangeResourceRecordSetsRequest()
                    {
                        HostedZoneId = zoneId,
                        ChangeBatch = new ChangeBatch()
                        {
                            Changes = new List<Change>() 
                            { 
                                new Change(ChangeAction.UPSERT, resourceRecordSet)
                            }
                        }
                    });
                }
            }
        }
    }
}
