using Method635.App.Forms.Models;
using Method635.App.Forms.RestAccess.ResponseModel;
using Method635.App.Forms.RestAccess.RestExceptions;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Method635.App.Forms.RestAccess
{
    public class BrainstormingFindingRestResolver : RestResolverBase
    {
        private const string FINDINGS_ENDPOINT = "Finding";
        private const string CREATE_FINDING_ENDPOINT = "createBrainstormingFinding";
        private const string TIMING_ENDPOINT_DIFF = "remainingTime";
        private const string GET_FINDINGS_ENDPOINT = "getBrainstormingFindings";

        public string GetRemainingTime(string findingId, string teamId = "525cb90d-b0c9-40ba-a741-f19d1e79fec0")
        {
            try
            {
                Console.WriteLine("Getting remaining time..");
                HttpResponseMessage response = GetCall($"{FINDINGS_ENDPOINT}/{teamId}/{findingId}/{TIMING_ENDPOINT_DIFF}");

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
                Console.WriteLine($"Error getting remaining time: {ex}");
            }
            return string.Empty;
        }

        public List<BrainstormingFinding> GetAllFindingsForTeam(string teamId = "525cb90d-b0c9-40ba-a741-f19d1e79fec0")
        {
            try
            {
                Console.WriteLine($"Getting all brainstorming findings for team {teamId}..");
                var res = GetCall($"{FINDINGS_ENDPOINT}/{teamId}/{GET_FINDINGS_ENDPOINT}");
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Got all Brainstorminginfindgss finding. Content: {res.Content}");
                    var brainstormingFindings = res.Content.ReadAsAsync<List<BrainstormingFinding>>().Result;
                    Console.WriteLine("got findings: ");
                    brainstormingFindings.ForEach(finding => Console.WriteLine(finding.Name));
                    return brainstormingFindings;
                }
            }
            catch (RestEndpointException ex)
            {
                Console.WriteLine($"Failed to create brainstorming finding: {ex.Message}");
            }
            Console.WriteLine($"No brainstorming findings found for team {teamId}");
            return new List<BrainstormingFinding>();
        }

        public BrainstormingFinding CreateBrainstormingFinding(BrainstormingFinding finding, string brainstormingTeamId = "525cb90d-b0c9-40ba-a741-f19d1e79fec0")
        {
            try
            {
                Console.WriteLine("Creating brainstorming finding..");
                var res = PostCall(finding, $"{FINDINGS_ENDPOINT}/{brainstormingTeamId}/{CREATE_FINDING_ENDPOINT}");
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Created brainstorming finding. Content: {res.Content}");
                    var parsedResponseMessage = res.Content.ReadAsAsync<RestResponseMessage>().Result;
                    finding.Id = parsedResponseMessage.Text;
                    return finding;
                }
            }
            catch (RestEndpointException ex)
            {
                Console.WriteLine($"Failed to create brainstorming finding: {ex.Message}");
            }
            return finding;
        }
    }
}
