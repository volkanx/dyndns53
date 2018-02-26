using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynDns53.CoreLib;
using Moq;
using Xunit;
using DynDns53.CoreLib.Config;
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

            var mockIpChecker = new Mock<IIpChecker>();
            mockIpChecker.Setup(m => m.GetExternalIpAsync()).ReturnsAsync(currentExternalIp);

            var mockConfigHandler = new Mock<IConfigHandler>();
            mockConfigHandler.Setup(m => m.GetConfig()).Returns(new DynDns53Config()
            {
                DomainList = new List<HostedDomainInfo>()
                {
                    new HostedDomainInfo() { DomainName = subdomain, ZoneId = zoneId }
                }
            });

            var mockAmazonClient = new Mock<IAmazonRoute53>();
            mockAmazonClient.Setup(m =>
                                   m.ListResourceRecordSetsAsync(It.IsAny<ListResourceRecordSetsRequest>(), CancellationToken.None))
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

            var dnsUpdater = new DnsUpdater(mockConfigHandler.Object, mockIpChecker.Object, mockAmazonClient.Object);

            // Act
            await dnsUpdater.UpdateAsync();
            
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

            var mockIpChecker = new Mock<IIpChecker>();
            mockIpChecker.Setup(m => m.GetExternalIpAsync()).ReturnsAsync(currentExternalIp);
            
            var mockConfigHandler = new Mock<IConfigHandler>();
            mockConfigHandler.Setup(m => m.GetConfig()).Returns(new DynDns53Config()
            {
                DomainList = new List<HostedDomainInfo>() 
                {
                    new HostedDomainInfo() { DomainName = subdomain, ZoneId = zoneId }
                }
            });

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

            var dnsUpdater = new DnsUpdater(mockConfigHandler.Object, mockIpChecker.Object, mockAmazonClient.Object);

            // Act
            await dnsUpdater.UpdateAsync();

            // Assert
            mockAmazonClient.Verify(m => m.ChangeResourceRecordSetsAsync(It.IsAny<ChangeResourceRecordSetsRequest>(), CancellationToken.None), Times.Never);
        }

        [Fact]
        public async Task UpdateDns_MultipleDomainsCorrectExistingIp_ShouldUpdateAllSubdomains()
        {
            // Arrange
            string currentExternalIp = "5.6.7.8";
            string currentRecordedIp = "1.2.3.4";
            List<string> subDomainList = new List<string>() { "subdomain1.example.com", "subdomain2.example.com" };
            string zoneId = "ABCDEFGHI";

            var mockIpChecker = new Mock<IIpChecker>();
            mockIpChecker.Setup(m => m.GetExternalIpAsync()).ReturnsAsync(currentExternalIp);

            var mockConfigHandler = new Mock<IConfigHandler>();
            mockConfigHandler.Setup(m => m.GetConfig()).Returns(new DynDns53Config()
            {
                DomainList = subDomainList.Select(s => new HostedDomainInfo() { DomainName = s, ZoneId = zoneId }).ToList()
            });

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

            var dnsUpdater = new DnsUpdater(mockConfigHandler.Object, mockIpChecker.Object, mockAmazonClient.Object);

            // Act
            await dnsUpdater.UpdateAsync();

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
            List<string> subDomainList = new List<string>() { "subdomain1.example.com", "subdomain2.example.com" };
            string zoneId = "ABCDEFGHI";

            var mockIpChecker = new Mock<IIpChecker>();
            mockIpChecker.Setup(m => m.GetExternalIpAsync()).ReturnsAsync(currentExternalIp);

            var mockConfigHandler = new Mock<IConfigHandler>();
            mockConfigHandler.Setup(m => m.GetConfig()).Returns(new DynDns53Config()
            {
                DomainList = subDomainList.Select(s => new HostedDomainInfo() { DomainName = s, ZoneId = zoneId }).ToList()
            });

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

            var dnsUpdater = new DnsUpdater(mockConfigHandler.Object, mockIpChecker.Object, mockAmazonClient.Object);

            // Act
            await dnsUpdater.UpdateAsync();

            // Assert
            mockAmazonClient.Verify(m => m.ChangeResourceRecordSetsAsync(It.Is<ChangeResourceRecordSetsRequest>(c => c.ChangeBatch.Changes.Count == 1), CancellationToken.None), Times.Never);
            mockAmazonClient.Verify(m => m.ChangeResourceRecordSetsAsync(It.Is<ChangeResourceRecordSetsRequest>(c => c.ChangeBatch.Changes.First().ResourceRecordSet.ResourceRecords.First().Value == currentExternalIp), CancellationToken.None), Times.Never);
        }



    }



}
