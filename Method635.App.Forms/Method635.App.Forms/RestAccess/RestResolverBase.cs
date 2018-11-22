using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Method635.App.Forms.RestAccess
{
    public abstract class RestResolverBase
    {
        private const string URL = "https://sinv-56079.edu.hsr.ch";
        private const int PORT = 40000;

        protected static HttpClient RestClient()
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri($"{URL}:{PORT}")
            };
            
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        protected static HttpResponseMessage GetCall(string endpoint)
        {
            using (var client = RestClient())
            {
                return client.GetAsync(endpoint).Result;
            }
        }
        protected static HttpResponseMessage PutCall(object parameter, string endpoint)
        {
            using (var client = RestClient())
            {
                var jsonObject = JsonConvert.SerializeObject(parameter);
                var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                return client.PutAsync(endpoint, content).Result;
            }
        }
        protected static HttpResponseMessage PostCall(object parameter, string endpoint)
        {
            using (var client = RestClient())
            {
                var jsonObject = JsonConvert.SerializeObject(parameter);
                var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                return client.PostAsync(endpoint, content).Result;
            }
        }
    }
}
