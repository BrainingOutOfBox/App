using Method635.App.Models;
using Method635.App.Forms.RestAccess.ResponseModel;
using Method635.App.Forms.RestAccess.RestExceptions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Method635.App.Logging;
using Xamarin.Forms;

namespace Method635.App.Forms.RestAccess
{
    public class TeamRestResolver : RestResolverBase
    {
        private const string TEAM_ENDPOINT = "Team";
        private const string CREATE_ENDPOINT = "createBrainstormingTeam";
        private const string JOIN_ENDPOINT = "joinTeam";
        private const string GET_TEAM = "getBrainstormingTeam";
        private const string GET_MY_TEAMS = "getMyBrainstormingTeams";

        // Platform independent logger necessary, thus resolving from xf dependency service.
        private readonly ILogger _logger = DependencyService.Get<ILogManager>().GetLog();
        
        public BrainstormingTeam GetTeamById(string teamId)
        {
            try
            {
                _logger.Info($"Getting team {teamId}");
                HttpResponseMessage response = GetCall($"{TEAM_ENDPOINT}/{teamId}/{GET_TEAM}");

                if (response.IsSuccessStatusCode)
                {
                    var team = response.Content.ReadAsAsync<BrainstormingTeam>().Result;
                    _logger.Info($"Got team {team.Name}");
                    return team;
                }
                else
                {
                    _logger.Error($"Response Code from GetTeam unsuccessful: {(int)response.StatusCode} ({response.ReasonPhrase})");
                }
            }
            catch (RestEndpointException ex)
            {
                _logger.Error($"Error getting Team: {ex}", ex);
            }
            catch (UnsupportedMediaTypeException ex)
            {
                _logger.Error($"Error getting Team (unsupported media type in response): {ex}", ex);
            }
            return null;
        }

        public List<BrainstormingTeam> GetMyBrainstormingTeams(string userName)
        {
            try
            {
                _logger.Info($"Getting all teams for {userName}");
                HttpResponseMessage response = GetCall($"{TEAM_ENDPOINT}/{userName}/{GET_MY_TEAMS}");

                if (response.IsSuccessStatusCode)
                {
                    var teams = response.Content.ReadAsAsync<List<BrainstormingTeam>>().Result;
                    _logger.Info($"Got {teams.Count} teams for {userName}.");
                    return teams;
                }
                else
                {
                    _logger.Error($"Response Code from GetMyTeams unsuccessful: {(int)response.StatusCode} ({response.ReasonPhrase})");
                }
            }
            catch (RestEndpointException ex)
            {
                _logger.Error($"Error getting Team: {ex}", ex);
            }
            catch (UnsupportedMediaTypeException ex)
            {
                _logger.Error($"Error getting Team (unsupported media type in response): {ex}", ex);
            }
            return new List<BrainstormingTeam>();
        }

        public bool JoinTeam(string teamId, Participant participant)
        {
            try
            {
                _logger.Info($"Joining team {teamId}");
                HttpResponseMessage response = PutCall(participant, $"{TEAM_ENDPOINT}/{teamId}/{JOIN_ENDPOINT}");

                if (response.IsSuccessStatusCode)
                {
                    _logger.Info($"Successfully joined team {teamId}");
                    return true;
                }
                else
                {
                    _logger.Error($"Response Code from JoinTeam unsuccessful: {(int)response.StatusCode} ({response.ReasonPhrase})");
                }
            }
            catch (RestEndpointException ex)
            {
                _logger.Error($"Error joining Team: {ex}", ex);
            }
            catch (UnsupportedMediaTypeException ex)
            {
                _logger.Info($"Error joining Team (unsupported media type in response): {ex}", ex);
            }
            return false;
        }

        public Moderator GetModeratorByTeamId(string teamId)
        {
            try
            {
                _logger.Info($"Resolving Moderator for team {teamId}");
                HttpResponseMessage response = GetCall($"{TEAM_ENDPOINT}/{teamId}/{GET_TEAM}");

                if (response.IsSuccessStatusCode)
                {
                    var team = response.Content.ReadAsAsync<BrainstormingTeam>().Result;
                    _logger.Info($"Got team {team.Name}");
                    return team.Moderator ?? null;
                }
                else
                {
                    _logger.Error($"Response Code from GetTeam unsuccessful: {(int)response.StatusCode} ({response.ReasonPhrase})");
                }
            }
            catch (RestEndpointException ex)
            {
                _logger.Error($"Error getting Team: {ex}", ex);
            }
            catch(UnsupportedMediaTypeException ex)
            {
                _logger.Error($"Error getting Team (unsupported media type in response): {ex}", ex);
            }
            return null;
        }

        public BrainstormingTeam CreateBrainstormingTeam(BrainstormingTeam brainstormingTeam)
        {
            try
            {
                _logger.Info("Creating brainstorming team..");
                var res = PostCall(brainstormingTeam, $"{TEAM_ENDPOINT}/{CREATE_ENDPOINT}");

                if (res.IsSuccessStatusCode)
                {
                    _logger.Info($"Created brainstorming team. Content: {res.Content}");
                    var parsedResponseMessage = res.Content.ReadAsAsync<RestResponseMessage>().Result;
                    brainstormingTeam.Id = parsedResponseMessage.Text;
                    return brainstormingTeam;
                }
                else
                {
                    _logger.Error($"Couldn't parse the response id of the team {brainstormingTeam.Name}. API returned: '{res.ReasonPhrase}'");
                }

            }
            catch (RestEndpointException ex)
            {
                _logger.Error($"Failed to create brainstorming finding: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to create brainstorming finding: {ex.Message}", ex);
            }
            return brainstormingTeam;
        }
    }
}
