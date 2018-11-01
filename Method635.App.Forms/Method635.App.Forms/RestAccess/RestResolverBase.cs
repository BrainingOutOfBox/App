using System;
using System.Net.Http;
using System.Net.Http.Headers;

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
    }
}
