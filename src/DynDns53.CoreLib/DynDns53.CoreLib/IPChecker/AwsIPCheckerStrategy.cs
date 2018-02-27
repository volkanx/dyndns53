using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DynDns53.CoreLib.IPChecker
{
    public class AwsIPCheckerStrategy : IIPCheckerStrategy
    {
        private readonly string AWS_URL = "http://checkip.amazonaws.com";

        public async Task<string> GetExternalIpAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AWS_URL);
                string result = await client.GetStringAsync("");
                return result.TrimEnd('\n');
            }
        }
    }
}
