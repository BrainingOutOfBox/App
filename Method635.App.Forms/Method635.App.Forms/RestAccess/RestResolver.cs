using Method635.App.Forms.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Method635.App.Forms.RestAccess
{
    public class RestResolver
    {
        private const string URL = "https://sinv-56079.edu.hsr.ch";
        private const int PORT = 40000;
        private const string TIMING_ENDPOINT_DIFF = "/timing/diff";
        private const string TIMING_ENDPOINT_NEW = "/timing/new";
        private const string BRAINSTORMING_ENDPOINT_NEW = "/createBrainstormingFinding";
        


        private HttpResponseMessage PlaceGetCall(string endpoint)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{URL}:{PORT}");

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client.GetAsync(endpoint).Result;
        }

        private HttpResponseMessage PlaceDefaultFindingPostCall(string endpoint)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{URL}:{PORT}");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var finding = new BrainstormingFinding()
            {
                Name = "TestBrainstormingFinding",
                ProblemDescription = "This finding serves as a test or dummy object to check our implementation",
                BaseRoundTime = 5,
                NrOfIdeas = 3
            };

            var content = new StringContent(JsonConvert.SerializeObject(finding), Encoding.UTF8, "application/json");
            Console.WriteLine(JsonConvert.SerializeObject(finding));
            Console.WriteLine(content.Headers);
            return client.PostAsync(endpoint, content).Result;
        }

        public string GetTime()
        {
            // List data response.
            HttpResponseMessage response = PlaceGetCall(TIMING_ENDPOINT_DIFF);  // Blocking call! Program will wait here until a response is received or a timeout occurs.
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
            HttpResponseMessage response = PlaceGetCall(TIMING_ENDPOINT_NEW);// Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Call successfully placed");
            }
            else
            {
                throw new RestEndpointException($"Couldn't place call to {TIMING_ENDPOINT_NEW}.");
            }
        }
        public void StartBrainstorming(string brainstormingTeam = "DemoTeam")
        {
            HttpResponseMessage response = PlaceDefaultFindingPostCall($"{brainstormingTeam}{BRAINSTORMING_ENDPOINT_NEW}");// Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Call successfully placed");  
            }
            else
            {
                throw new RestEndpointException($"Couldn't place call to {TIMING_ENDPOINT_NEW}. Reason: {response.ReasonPhrase}");
            }
        }
    }
}
