using Amazon.Route53;
using Amazon.Route53.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynDns53.CoreLib
{
    public interface IDnsUpdater
    {
        Task UpdateAllAsync(string ipAddress, IEnumerable<HostedDomainInfo> domainList);
        Task UpdateSingleAsync(string ipAddress, HostedDomainInfo domain);
    }

    public class DnsUpdater : IDnsUpdater
    {
        private readonly IAmazonRoute53 amazonClient;

        public DnsUpdater(IAmazonRoute53 amazonClient)
        {
            this.amazonClient = amazonClient;
        }

        public async Task UpdateAllAsync(string ipAddress, IEnumerable<HostedDomainInfo> domainList)
        {
            foreach (var domain in domainList)
            {
                await UpdateSingleAsync(ipAddress, domain);
            }
        }

        public async Task UpdateSingleAsync(string ipAddress, HostedDomainInfo domain)
        {
            var currentExternalIp = ipAddress;

            // Ensure the domain name doesn't end with a dot. This is how they are returned from AWS API but they will be trimmed
            // as well so that they match
            var domainName = domain.DomainName;
            domainName = domainName.TrimEnd(".".ToCharArray());

            var zoneId = domain.ZoneId;

            var listResourceRecordSetsResponse = await amazonClient.ListResourceRecordSetsAsync(new ListResourceRecordSetsRequest() { HostedZoneId = zoneId });
            if (listResourceRecordSetsResponse.ResourceRecordSets.Count == 0)
            {
                throw new InvalidOperationException($"Could not find any ResourceRecordSets for zone: {zoneId}");
            }

            var resourceRecordSet = listResourceRecordSetsResponse.ResourceRecordSets.FirstOrDefault(recordset => recordset.Name.TrimEnd(".".ToCharArray()) == domainName);
            if (resourceRecordSet == null)
            {
                throw new InvalidOperationException($"Could not find any resource record set for domain: {domainName}");
            }

            var resourceRecord = resourceRecordSet.ResourceRecords.FirstOrDefault();
            if (resourceRecord == null)
            {
                throw new InvalidOperationException($"Could not find any resouce record in the set");
            }

            if (resourceRecord.Value != currentExternalIp)
            {
                resourceRecord.Value = currentExternalIp;

                await amazonClient.ChangeResourceRecordSetsAsync(new ChangeResourceRecordSetsRequest()
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
