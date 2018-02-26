using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DynDns53.CoreLib.IPChecker
{
    public class DynDnsIPChecker : IIpChecker
    {
        private readonly string DYNDNS_URL = "http://checkip.dyndns.org/";

        public async Task<string> GetExternalIpAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(DYNDNS_URL);
                var response = await client.GetAsync("");
                var rawString = await response.Content.ReadAsStringAsync();
                return ExtractIPAddress(rawString);
            }
        }

        private string ExtractIPAddress(string fullText)
        {
            string regExPattern = @"\b(?:[0-9]{1,3}\.){3}[0-9]{1,3}\b";

            var regex = new Regex(regExPattern);
            var match = regex.Match(fullText);
            if (match != null && match.Success)
            {
                return match.Value;
            }

            throw new ArgumentException("Provided text does not contain an IP address");
        }
    }
}
