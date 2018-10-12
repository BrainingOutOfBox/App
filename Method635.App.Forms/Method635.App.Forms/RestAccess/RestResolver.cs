using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Method635.App.Forms.RestAccess
{
    public class RestResolver
    {
        private const string URL = "http://sinv-56079.edu.hsr.ch";
        private const int PORT = 40000;
        private const string TIMING_ENDPOINT_DIFF = "/timing/diff";
        private const string TIMING_ENDPOINT_NEW = "/timing/new";
        


        private HttpResponseMessage PlaceCall(string endpoint)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{URL}:{PORT}");

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client.GetAsync(endpoint).Result;
        }

        public string GetTime()
        {
            // List data response.
            HttpResponseMessage response = PlaceCall(TIMING_ENDPOINT_DIFF);  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (response.IsSuccessStatusCode)
            {
                var remainingTimeInMs = response.Content.ReadAsAsync<long>().Result; 
                var timeRemaining = TimeSpan.FromMilliseconds(remainingTimeInMs);
                return ($"{timeRemaining.Minutes:D2}m:{timeRemaining.Seconds:D2}s");
            }
            else
            {
                Console.WriteLine($"{(int)response.StatusCode} ({response.ReasonPhrase})");
                return string.Empty;
            }
        }
        public void StartTimer()
        {
            HttpResponseMessage response = PlaceCall(TIMING_ENDPOINT_NEW);// Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Call successfully placed");
            }
            else
            {
                throw new RestEndpointException($"Couldn't place call to {TIMING_ENDPOINT_NEW}.");
            }
        }
    }
}
