using Method635.App.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xamarin.Forms;

namespace Method635.App.Forms.RestAccess
{
    public abstract class RestResolverBase
    {
        private const string URL = "https://sinv-56079.edu.hsr.ch";
        //private const string URL = "172.96.";
        private const int PORT = 40000;

        // Platform independent logger necessary, thus resolving from xf dependency service.
        private static readonly ILogger _logger = DependencyService.Get<ILogManager>().GetLog();

        protected static HttpClient RestClient()
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri($"{URL}:{PORT}")
            };
            
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _logger.Debug($"Initialized Rest Client using {URL}:{PORT}");
            return client;
        }

        protected static HttpResponseMessage GetCall(string endpoint)
        {
            _logger.Debug($"HTTP GET call to {endpoint}");
            using (var client = RestClient())
            {
                return client.GetAsync(endpoint).Result;
            }
        }
        protected static HttpResponseMessage PutCall(object parameter, string endpoint)
        {
            _logger.Debug($"HTTP PUT call to {endpoint}");
            using (var client = RestClient())
            {
                var jsonObject = JsonConvert.SerializeObject(parameter);
                var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                return client.PutAsync(endpoint, content).Result;
            }
        }
        protected static HttpResponseMessage PostCall(object parameter, string endpoint)
        {
            _logger.Debug($"HTTP POST call to {endpoint}");
            using (var client = RestClient())
            {
                var jsonObject = JsonConvert.SerializeObject(parameter);
                var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                return client.PostAsync(endpoint, content).Result;
            }
        }
    }
}
