using Method635.App.Forms.Models;
using Method635.App.Forms.RestAccess.ResponseModel;
using Method635.App.Forms.RestAccess.RestExceptions;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Method635.App.Forms.RestAccess
{
    public class TeamRestResolver : RestResolverBase
    {
        private const string TEAM_ENDPOINT = "Team";
        private const string CREATE_ENDPOINT = "createBrainstormingTeam";
        private const string JOIN_ENDPOINT = "joinTeam";
        private const string GET_TEAM = "getBrainstormingTeam";
        private const string GET_MY_TEAMS = "getMyBrainstormingTeams";

        public BrainstormingTeam GetTeamById(string teamId)
        {
            try
            {
                Console.WriteLine($"Getting team {teamId}");
                HttpResponseMessage response = GetCall($"{TEAM_ENDPOINT}/{teamId}/{GET_TEAM}");

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

        internal List<BrainstormingTeam> GetMyBrainstormingTeams(string userName)
        {
            try
            {
                Console.WriteLine($"Getting all teams for {userName}");
                HttpResponseMessage response = GetCall($"{TEAM_ENDPOINT}/{userName}/{GET_MY_TEAMS}");

                if (response.IsSuccessStatusCode)
                {
                    var teams = response.Content.ReadAsAsync<List<BrainstormingTeam>>().Result;
                    Console.WriteLine($"Got {teams.Count} teams for {userName}.");
                    return teams;
                }
                else
                {
                    Console.WriteLine($"Response Code from GetMyTeams unsuccessful: {(int)response.StatusCode} ({response.ReasonPhrase})");
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
            return new List<BrainstormingTeam>();
        }

        internal bool JoinTeam(string teamId, Participant participant)
        {
            try
            {
                Console.WriteLine($"Joining team {teamId}");
                HttpResponseMessage response = PutCall(participant, $"{TEAM_ENDPOINT}/{teamId}/{JOIN_ENDPOINT}");

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

        public Moderator GetModeratorByTeamId(string teamId)
        {
            try
            {
                Console.WriteLine($"Resolving Moderator for team {teamId}");
                HttpResponseMessage response = GetCall($"{TEAM_ENDPOINT}/{teamId}/{GET_TEAM}");

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

        public BrainstormingTeam CreateBrainstormingTeam(BrainstormingTeam brainstormingTeam)
        {
            try
            {
                Console.WriteLine("Creating brainstorming team..");
                var res = PostCall(brainstormingTeam, $"{TEAM_ENDPOINT}/{CREATE_ENDPOINT}");

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
    }
}
