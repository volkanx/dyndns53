using DynDns53.CoreLib;
using DynDns53.CoreLib.IPChecker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynDns53.Core
{
    public class DynDns53Config
    {
        public string ClientId { get; set; }
        public int UpdateInterval { get; set; }
        public List<HostedDomainInfo> DomainList { get; set; }
        public string Route53AccessKey { get; set; }
        public string Route53SecretKey { get; set; }
        public bool RunAtSystemStart { get; set; }
        public IPChecker IPChecker { get; set; }

        public DynDns53Config()
        {
            DomainList = new List<HostedDomainInfo>();
        }
    }
}
