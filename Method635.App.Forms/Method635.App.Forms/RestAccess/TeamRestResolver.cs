using Method635.App.Forms.Models;
using Method635.App.Forms.RestAccess.ResponseModel;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Method635.App.Forms.RestAccess
{
    public class TeamRestResolver : RestResolverBase
    {
        private const string TEAM_ENDPOINT = "Team";
        private const string CREATE_ENDPOINT = "createBrainstormingTeam";
        private const string JOIN_ENDPOINT = "joinTeam";
        private const string GET_TEAM = "getBrainstormingTeam";

        public BrainstormingTeam GetTeamById(string teamId)
        {
            try
            {
                Console.WriteLine($"Getting team {teamId}");
                HttpResponseMessage response = GetTeamCall(teamId).Result;

                if (response.IsSuccessStatusCode)
                {
                    var team = response.Content.ReadAsAsync<BrainstormingTeam>().Result;
                    Console.WriteLine($"Got team {team.Name}");
                    return team;
                }
                else
                {
                    Console.WriteLine($"Response Code from GetTeam unsuccessful: {(int)response.StatusCode} ({response.ReasonPhrase})");
                }
            }
            catch (RestEndpointException ex)
            {
                Console.WriteLine($"Error getting Team: {ex}");
            }
            catch (UnsupportedMediaTypeException ex)
            {
                Console.WriteLine($"Error getting Team (unsupported media type in response): {ex}");
            }
            return null;
        }

        internal bool JoinTeam(string teamId, Participant participant)
        {
            try
            {
                Console.WriteLine($"Joining team {teamId}");
                HttpResponseMessage response = JoinTeamCall(teamId, participant);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Successfully joined team {teamId}");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Response Code from JoinTeam unsuccessful: {(int)response.StatusCode} ({response.ReasonPhrase})");
                    
                }
            }
            catch (RestEndpointException ex)
            {
                Console.WriteLine($"Error joining Team: {ex}");
            }
            catch (UnsupportedMediaTypeException ex)
            {
                Console.WriteLine($"Error joining Team (unsupported media type in response): {ex}");
            }
            return false;
        }

        private HttpResponseMessage JoinTeamCall(string teamId, Participant participant)
        {
            using (var client = RestClient())
            {
                var participantJson = JsonConvert.SerializeObject(participant);
                var content = new StringContent(participantJson, Encoding.UTF8, "application/json");
                return client.PutAsync($"{TEAM_ENDPOINT}/{teamId}/{JOIN_ENDPOINT}", content).Result;
            }
        }

        public Moderator GetModeratorByTeamId(string teamId)
        {
            try
            {
                Console.WriteLine($"Resolving Moderator for team {teamId}");
                HttpResponseMessage response = GetTeamCall(teamId).Result;

                if (response.IsSuccessStatusCode)
                {
                    var team = response.Content.ReadAsAsync<BrainstormingTeam>().Result;
                    Console.WriteLine($"Got team {team.Name}");
                    return team.Moderator ?? null;
                }
                else
                {
                    Console.WriteLine($"Response Code from GetTeam unsuccessful: {(int)response.StatusCode} ({response.ReasonPhrase})");
                }
            }
            catch (RestEndpointException ex)
            {
                Console.WriteLine($"Error getting Team: {ex}");
            }
            catch(UnsupportedMediaTypeException ex)
            {
                Console.WriteLine($"Error getting Team (unsupported media type in response): {ex}");
            }
            return null;
        }

        private async Task<HttpResponseMessage> GetTeamCall(string teamId)
        {
            using (var client = RestClient())
            {
                return await client.GetAsync($"{TEAM_ENDPOINT}/{teamId}/{GET_TEAM}");
            }
        }

        public BrainstormingTeam CreateBrainstormingTeam(BrainstormingTeam brainstormingTeam)
        {
            try
            {
                Console.WriteLine("Creating brainstorming team..");
                var res = CreateBrainstormingTeamCall(brainstormingTeam);
                
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Created brainstorming team. Content: {res.Content}");
                    var parsedResponseMessage = res.Content.ReadAsAsync<RestResponseMessage>().Result;
                    brainstormingTeam.Id = parsedResponseMessage.Text;
                    return brainstormingTeam;
                }
                else
                {
                    Console.WriteLine($"Couldn't parse the response id of the team {brainstormingTeam.Name}. API returned: '{res.ReasonPhrase}'");
                }

            }
            catch (RestEndpointException ex)
            {
                Console.WriteLine($"Failed to create brainstorming finding: {ex.Message}");
            }
            return brainstormingTeam;
        }

        private HttpResponseMessage CreateBrainstormingTeamCall(BrainstormingTeam brainstormingTeam)
        {
            brainstormingTeam.Moderator = new Moderator() { FirstName = "Lolo", LastName = "Langfuss", UserName = "LLFF", Password = "pwllff" };
            using (var client = RestClient())
            {
                var teamJson = JsonConvert.SerializeObject(brainstormingTeam);
                var content = new StringContent(teamJson, Encoding.UTF8, "application/json");
                return client.PostAsync($"{TEAM_ENDPOINT}/{CREATE_ENDPOINT}", content).Result;
            }
        }
    }
}
