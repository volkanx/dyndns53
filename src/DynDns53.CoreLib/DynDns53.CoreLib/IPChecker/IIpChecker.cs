using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynDns53.CoreLib
{
    public interface IIpChecker
    {
        string GetExternalIp();
        Task<string> GetExternalIpAsync();
    }
}
