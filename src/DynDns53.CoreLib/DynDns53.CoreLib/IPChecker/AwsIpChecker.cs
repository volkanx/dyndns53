using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DynDns53.CoreLib
{
    public class AwsIpChecker : IIpChecker
    {
        private readonly string AWS_URL = "http://checkip.amazonaws.com";
        
        public string GetExternalIp()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AWS_URL);
                string result = client.GetStringAsync("").Result;
                return result.TrimEnd('\n');
            }
        }

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
