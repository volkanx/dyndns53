using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DynDns53.CoreLib.IPChecker
{
    public class CustomIpChecker : IIpChecker
    {
        private readonly string SERVICE_URL = "http://check-ip.herokuapp.com/";

        public async Task<string> GetExternalIpAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(SERVICE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync("");
                string json = await response.Content.ReadAsStringAsync();
                dynamic ip = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                string result = ip.ipAddress;
                return result;
            }
        }
    }
}
