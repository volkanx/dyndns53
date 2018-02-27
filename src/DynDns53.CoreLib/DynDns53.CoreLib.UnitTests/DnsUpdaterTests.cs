using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynDns53.CoreLib;
using Moq;
using Xunit;
using Amazon.Route53;
using Amazon.Route53.Model;
using System.Threading;
using DynDns53.CoreLib.IPChecker;

namespace DynDns53.UnitTests
{
    public class DnsUpdaterTests
    {
        [Fact]
        public async Task UpdateDns_SingleDomainCorrectNewIp_ShouldUpdateTheDnsWithCorrectIp()
        {
            // Arrange
            string currentExternalIp = "1.2.3.4";
            string currentRecordedIp = "5.6.7.8";
            string subdomain = "test.example.com";
            string zoneId = "ABCD123456";
            var domain = new HostedDomainInfo()
            {
                ZoneId = zoneId,
                DomainName = subdomain
            };

            var mockAmazonClient = new Mock<IAmazonRoute53>();
            mockAmazonClient.Setup(m => m.ListResourceRecordSetsAsync(It.IsAny<ListResourceRecordSetsRequest>(), CancellationToken.None))
                .ReturnsAsync(new ListResourceRecordSetsResponse()
                {
                    ResourceRecordSets = new List<ResourceRecordSet>()
                    {
                        new ResourceRecordSet()
                        {
                            Name = subdomain,
                            ResourceRecords = new List<ResourceRecord>() { new ResourceRecord() { Value = currentRecordedIp } }
                        }
                    }
                });

            var dnsUpdater = new DnsUpdater(mockAmazonClient.Object);

            // Act
            await dnsUpdater.UpdateSingleAsync(currentExternalIp, domain);
            
            // Assert
            mockAmazonClient.Verify(m => m.ChangeResourceRecordSetsAsync(It.Is<ChangeResourceRecordSetsRequest>(c => c.ChangeBatch.Changes.Count == 1), CancellationToken.None), Times.Once);
            mockAmazonClient.Verify(m => m.ChangeResourceRecordSetsAsync(It.Is<ChangeResourceRecordSetsRequest>(c => c.ChangeBatch.Changes.First().ResourceRecordSet.ResourceRecords.First().Value == currentExternalIp), CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task UpdateDns_SingleDomainCorrectExistingIp_ShowuldNotCallAwsClient()
        {
            // Arrange
            string currentExternalIp = "1.2.3.4";
            string currentRecordedIp = "1.2.3.4";
            string subdomain = "test.example.com";
            string zoneId = "ABCD123456";
            var domain = new HostedDomainInfo()
            {
                ZoneId = zoneId,
                DomainName = subdomain
            };

            var mockAmazonClient = new Mock<IAmazonRoute53>();
            mockAmazonClient.Setup(m => m.ListResourceRecordSetsAsync(It.IsAny<ListResourceRecordSetsRequest>(), CancellationToken.None))
                .ReturnsAsync(new ListResourceRecordSetsResponse()
                {
                    ResourceRecordSets = new List<ResourceRecordSet>() 
                    {
                        new ResourceRecordSet()
                        {
                            Name = subdomain,
                            ResourceRecords = new List<ResourceRecord>() { new ResourceRecord() { Value = currentRecordedIp } }
                        }
                    }
                });

            var dnsUpdater = new DnsUpdater(mockAmazonClient.Object);

            // Act
            await dnsUpdater.UpdateSingleAsync(currentExternalIp, domain);

            // Assert
            mockAmazonClient.Verify(m => m.ChangeResourceRecordSetsAsync(It.IsAny<ChangeResourceRecordSetsRequest>(), CancellationToken.None), Times.Never);
        }


        [Fact]
        public async Task UpdateDns_MultipleDomainsCorrectExistingIp_ShouldUpdateAllSubdomains()
        {
            // Arrange
            string currentExternalIp = "5.6.7.8";
            string currentRecordedIp = "1.2.3.4";
            var subDomainList = new List<string>() { "subdomain1.example.com", "subdomain2.example.com" };
            string zoneId = "ABCDEFGHI";
            var domainList = new List<HostedDomainInfo>()
            {
                new HostedDomainInfo() { DomainName = subDomainList[0], ZoneId = zoneId },
                new HostedDomainInfo() { DomainName = subDomainList[1], ZoneId = zoneId }
            };

            var mockAmazonClient = new Mock<IAmazonRoute53>();
            mockAmazonClient.Setup(m => m.ListResourceRecordSetsAsync(It.IsAny<ListResourceRecordSetsRequest>(), CancellationToken.None))
                .ReturnsAsync(new ListResourceRecordSetsResponse()
                {
                    ResourceRecordSets = new List<ResourceRecordSet>() 
                    {
                        new ResourceRecordSet()
                        {
                            Name = subDomainList[0],
                            ResourceRecords = new List<ResourceRecord>() { new ResourceRecord() { Value = currentRecordedIp } }
                        },
                        new ResourceRecordSet()
                        {
                            Name = subDomainList[1],
                            ResourceRecords = new List<ResourceRecord>() { new ResourceRecord() { Value = currentRecordedIp } }
                        }
                    }
                });

            var dnsUpdater = new DnsUpdater(mockAmazonClient.Object);

            // Act
            await dnsUpdater.UpdateAllAsync(currentExternalIp, domainList);

            // Assert
            mockAmazonClient.Verify(m => m.ChangeResourceRecordSetsAsync(It.Is<ChangeResourceRecordSetsRequest>(c => c.ChangeBatch.Changes.Count == 1), CancellationToken.None), Times.Exactly(2));
            mockAmazonClient.Verify(m => m.ChangeResourceRecordSetsAsync(It.Is<ChangeResourceRecordSetsRequest>(c => c.ChangeBatch.Changes.First().ResourceRecordSet.ResourceRecords.First().Value == currentExternalIp), CancellationToken.None), Times.Exactly(2));
        }

        [Fact]
        public async Task UpdateDns_MultipleDomainsCorrectExistingIp_ShowuldNotCallAwsClient()
        {
            // Arrange
            string currentExternalIp = "1.2.3.4";
            string currentRecordedIp = "1.2.3.4";
            var subDomainList = new List<string>() { "subdomain1.example.com", "subdomain2.example.com" };
            string zoneId = "ABCDEFGHI";
            var domainList = new List<HostedDomainInfo>()
            {
                new HostedDomainInfo() { DomainName = subDomainList[0], ZoneId = zoneId },
                new HostedDomainInfo() { DomainName = subDomainList[1], ZoneId = zoneId }
            };

            var mockAmazonClient = new Mock<IAmazonRoute53>();
            mockAmazonClient.Setup(m => m.ListResourceRecordSetsAsync(It.IsAny<ListResourceRecordSetsRequest>(), CancellationToken.None))
                .ReturnsAsync(new ListResourceRecordSetsResponse()
                {
                    ResourceRecordSets = new List<ResourceRecordSet>() 
                    {
                        new ResourceRecordSet()
                        {
                            Name = subDomainList[0],
                            ResourceRecords = new List<ResourceRecord>() { new ResourceRecord() { Value = currentRecordedIp } }
                        },
                        new ResourceRecordSet()
                        {
                            Name = subDomainList[1],
                            ResourceRecords = new List<ResourceRecord>() { new ResourceRecord() { Value = currentRecordedIp } }
                        }
                    }
                });

            var dnsUpdater = new DnsUpdater(mockAmazonClient.Object);

            // Act
            await dnsUpdater.UpdateAllAsync(currentExternalIp, domainList);

            // Assert
            mockAmazonClient.Verify(m => m.ChangeResourceRecordSetsAsync(It.Is<ChangeResourceRecordSetsRequest>(c => c.ChangeBatch.Changes.Count == 1), CancellationToken.None), Times.Never);
            mockAmazonClient.Verify(m => m.ChangeResourceRecordSetsAsync(It.Is<ChangeResourceRecordSetsRequest>(c => c.ChangeBatch.Changes.First().ResourceRecordSet.ResourceRecords.First().Value == currentExternalIp), CancellationToken.None), Times.Never);
        }
    }



}
