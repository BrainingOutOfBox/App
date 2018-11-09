using Method635.App.Forms.Models;
using System;
using System.Net.Http;

namespace Method635.App.Forms.RestAccess
{
    public class TeamRestResolver : RestResolverBase
    {
        private const string TEAM_ENDPOINT = "Team";
        public Moderator GetModeratorByTeamId(string teamId)
        {
            try
            {
                Console.WriteLine($"Resolving Moderator for team {teamId}");
                HttpResponseMessage response = GetTeamCall(teamId);

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

        private HttpResponseMessage GetTeamCall(string teamId)
        {
            using (var client = RestClient())
            {
                var response = client.GetAsync($"{TEAM_ENDPOINT}/{teamId}").Result;
                return response;
            }
        }
    }
}
