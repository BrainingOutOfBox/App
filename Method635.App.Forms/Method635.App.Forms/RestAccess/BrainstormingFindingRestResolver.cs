using Method635.App.Forms.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace Method635.App.Forms.RestAccess
{
    public class BrainstormingFindingRestResolver : RestResolverBase
    {
        private const string CREATE_FINDING_ENDPOINT = "createBrainstormingFinding";
        private const string TIMING_ENDPOINT_DIFF = "525cb90d-b0c9-40ba-a741-f19d1e79fec0/43a7608e-1862-482b-93a0-dc48c8efc631/remainingTime";

        public string GetRemainingTime(string teamId = null, string findingId = null)
        {
            try
            {
                Console.WriteLine("Getting remaining time..");
                HttpResponseMessage response = GetRemainingTimeCall();

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
            catch (RestEndpointException ex)
            {
                Console.WriteLine("Error getting remaining time: ", ex);
            }
            return string.Empty;
        }

        private static HttpResponseMessage GetRemainingTimeCall()
        {
            var client = RestClient();

            var response = client.GetAsync(TIMING_ENDPOINT_DIFF).Result;
            return response;
        }

        private HttpResponseMessage CreateBrainstormingFindingCall(string teamId)
        {
            HttpClient client = RestClient();
            var finding = new BrainstormingFinding()
            {
                Name = "TestBrainstormingFinding",
                ProblemDescription = "This finding serves as a test or dummy object to check our implementation",
                BaseRoundTime = 5,
                NrOfIdeas = 3
            };
            var findingJson = JsonConvert.SerializeObject(finding);
            var content = new StringContent(findingJson,Encoding.UTF8, "application/json");
            Console.WriteLine(findingJson);
            Console.WriteLine(content.Headers);
            return client.PostAsync($"{teamId}/{CREATE_FINDING_ENDPOINT}", content).Result;
        }

        public string CreateBrainstormingFinding(string brainstormingTeamId = "525cb90d-b0c9-40ba-a741-f19d1e79fec0")
        {
            try
            {
                Console.WriteLine("Creating brainstorming finding..");
                var res = CreateBrainstormingFindingCall(brainstormingTeamId);
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("Created brainstorming finding. Content: ", res.Content);
                    return res.Content.ToString();
                }
            }
            catch (RestEndpointException ex)
            {
                Console.WriteLine("Failed to create brainstorming finding: ", ex.Message);
            }
            return string.Empty;
        }
    }
}
