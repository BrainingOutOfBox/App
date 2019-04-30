using Method635.App.Dal.Config;
using Method635.App.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Method635.App.Forms.RestAccess
{
    public class RestClientService : IHttpClientService
    {
        private static readonly ILogger _logger = DependencyService.Get<ILogManager>().GetLog();
        private readonly Server _serverConfig;

        public RestClientService(IConfigurationService configurationService)
        {
            _serverConfig = configurationService.ServerConfig.Server;
        }

        private HttpClient RestClient()
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri($"{_serverConfig.HostName}:{_serverConfig.Port}")
            };

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _logger.Debug($"Initialized Rest Client using {_serverConfig.HostName}:{_serverConfig.Port}");
            return client;
        }

        public HttpResponseMessage GetCall(string endpoint)
        {
            _logger.Debug($"HTTP GET call to {endpoint}");
            using (var client = RestClient())
            {
                return client.GetAsync(endpoint).Result;
            }
        }
        public HttpResponseMessage PutCall(object parameter, string endpoint)
        {
            _logger.Debug($"HTTP PUT call to {endpoint}");
            using (var client = RestClient())
            {
                var jsonObject = JsonConvert.SerializeObject(parameter);
                var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                return client.PutAsync(endpoint, content).Result;
            }
        }
        public HttpResponseMessage PostCall(object parameter, string endpoint)
        {
            _logger.Debug($"HTTP POST call to {endpoint}");
            if(parameter is Stream s)
            {
                return PostStream(s, endpoint);
            }
            using (var client = RestClient())
            {
                var jsonObject = JsonConvert.SerializeObject(parameter);
                var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                return client.PostAsync(endpoint, content).Result;
            }
        }

        public HttpResponseMessage PostStream(Stream stream, string endpoint)
        {
            using (var httpClient = RestClient())
            {
                using (var content = new MultipartFormDataContent())
                {

                    var streamContent = new StreamContent(stream);
                    streamContent.Headers.Add("Content-Type", "image/png");
                    streamContent.Headers.Add("Content-Disposition", "form-data; name=\"name\"; filename=\"image.png\"");

                    content.Add(streamContent);
                    return httpClient.PostAsync(endpoint, content).Result;
                }
            }
        }
    }
}
