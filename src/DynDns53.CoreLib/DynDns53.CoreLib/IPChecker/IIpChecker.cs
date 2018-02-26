using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynDns53.CoreLib.IPChecker
{
    public interface IIpChecker
    {
        Task<string> GetExternalIpAsync();
    }
}
