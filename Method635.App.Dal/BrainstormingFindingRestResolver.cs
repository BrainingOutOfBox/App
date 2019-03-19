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
    public class BrainstormingFindingRestResolver : RestResolverBase
    {
        private const string FINDINGS_ENDPOINT = "Finding";
        private const string CREATE_FINDING_ENDPOINT = "createBrainstormingFinding";
        private const string START_FINDING_ENDPOINT = "startBrainstorming";
        private const string TIMING_ENDPOINT_DIFF = "remainingTime";
        private const string GET_FINDINGS_ENDPOINT = "getBrainstormingFindings";
        private const string GET_FINDING_ENDPOINT = "getBrainstormingFinding";
        private const string BRAINSHEET_UPDATE_ENDPOINT = "putBrainsheet";

        // Platform independent logger necessary, thus resolving from xf dependency service.
        private readonly ILogger _logger = DependencyService.Get<ILogManager>().GetLog();

        public TimeSpan GetRemainingTime(string findingId, string teamId)
        {
            try
            {
                _logger.Info("Getting remaining time..");
                HttpResponseMessage response = GetCall($"{FINDINGS_ENDPOINT}/{teamId}/{findingId}/{TIMING_ENDPOINT_DIFF}");

                if (response.IsSuccessStatusCode)
                {
                    var remainingTimeInMs = response.Content.ReadAsAsync<long>().Result;
                    return TimeSpan.FromMilliseconds(remainingTimeInMs);
                }
                else
                {
                    _logger.Info($"{(int)response.StatusCode} ({response.ReasonPhrase})");
                    return TimeSpan.Zero;
                }
            }
            catch (RestEndpointException ex)
            {
                _logger.Error($"Error getting remaining time: {ex}",ex);
            }
            return TimeSpan.Zero;
        }

        public List<BrainstormingFinding> GetAllFindingsForTeam(string teamId)
        {
            try
            {
                _logger.Info($"Getting all brainstorming findings for team {teamId}..");
                var res = GetCall($"{FINDINGS_ENDPOINT}/{teamId}/{GET_FINDINGS_ENDPOINT}");
                if (res.IsSuccessStatusCode)
                {
                    _logger.Info($"Got all Brainstormingfindings finding. Content: {res.Content}");
                    var brainstormingFindings = res.Content.ReadAsAsync<List<BrainstormingFinding>>().Result;
                    _logger.Info("got findings: ");
                    brainstormingFindings.ForEach(finding => _logger.Info(finding.Name));
                    return brainstormingFindings;
                }
            }
            catch (RestEndpointException ex)
            {
                _logger.Error($"Failed to create brainstorming finding: {ex.Message}", ex);
            }
            _logger.Error($"No brainstorming findings found for team {teamId}",null);
            return new List<BrainstormingFinding>();
        }

        public bool UpdateSheet(string findingId, BrainSheet brainSheet)
        {
            try
            {
                _logger.Info("Updating brainsheet..");
                var res = PutCall(brainSheet, $"{FINDINGS_ENDPOINT}/{findingId}/{BRAINSHEET_UPDATE_ENDPOINT}");
                var parsedResponseMessage = res.Content.ReadAsAsync<RestResponseMessage>().Result;
                _logger.Info(parsedResponseMessage.Title);
                _logger.Info(parsedResponseMessage.Text);
                if (res.IsSuccessStatusCode)
                {
                    _logger.Info($"Updated finding. Content: {res.Content}");
                    return true;
                }
            }
            catch(RestEndpointException ex)
            {
                _logger.Error($"There was an error updating the brainsheet: {ex.Message}", ex);
            }
            catch(UnsupportedMediaTypeException ex)
            {
                _logger.Error($"There was an error updating the brainsheet (Unsupported media response): {ex.Message}", ex);
            }
            return false;
        }

        public BrainstormingFinding GetFinding(BrainstormingFinding finding)
        {
            try
            {
                _logger.Info("Getting brainstorming finding..");
                var res = GetCall($"{FINDINGS_ENDPOINT}/{finding.Id}/{GET_FINDING_ENDPOINT}");
                var parsedResponseMessage = res.Content.ReadAsAsync<RestResponseMessage>().Result;
                _logger.Info(parsedResponseMessage.Title);
                _logger.Info(parsedResponseMessage.Text);
                if (res.IsSuccessStatusCode)
                {
                    _logger.Info($"Getting brainstorming finding. Content: {res.Content}");
                    return res.Content.ReadAsAsync<BrainstormingFinding>().Result;
                }
                else
                {
                    _logger.Error($"Couldn't get finding {finding.Id}.");
                }
            }
            catch(RestEndpointException ex)
            {
                _logger.Error($"There was an error getting the finding {finding.Id}");
            }
            return finding;
        }

        public BrainstormingFinding CreateBrainstormingFinding(BrainstormingFinding finding)
        {
            try
            {
                _logger.Info("Creating brainstorming finding..");
                var res = PostCall(finding, $"{FINDINGS_ENDPOINT}/{finding.TeamId}/{CREATE_FINDING_ENDPOINT}");
                if (res.IsSuccessStatusCode)
                {
                    _logger.Info($"Created brainstorming finding. Content: {res.Content}");
                    var parsedResponseMessage = res.Content.ReadAsAsync<RestResponseMessage>().Result;
                    finding.Id = parsedResponseMessage.Text;
                    return finding;
                }
                else
                {
                    _logger.Info("The brainstorming finding couldn't be created.");
                    var parsedResponseMessage = res.Content.ReadAsAsync<RestResponseMessage>().Result;
                    _logger.Error(parsedResponseMessage.Title);
                    _logger.Error(parsedResponseMessage.Text);
                }
            }
            catch (RestEndpointException ex)
            {
                _logger.Error($"Failed to create brainstorming finding: {ex.Message}", ex);
            }
            return finding;
        }
        public bool StartBrainstormingFinding(string findingId)
        {
            try
            {
                _logger.Info("Starting brainstorming finding..");
                var res = GetCall($"{FINDINGS_ENDPOINT}/{findingId}/{START_FINDING_ENDPOINT}");
                var parsedResponseMessage = res.Content.ReadAsAsync<RestResponseMessage>().Result;
                _logger.Info(parsedResponseMessage.Title);
                _logger.Info(parsedResponseMessage.Text);
                if (res.IsSuccessStatusCode)
                {
                    _logger.Info($"Started brainstorming finding. Content: {res.Content}");
                    return true;
                }
                else
                {
                    _logger.Error("The brainstorming finding couldn't be started.");
                }
            }
            catch (RestEndpointException ex)
            {
                _logger.Error($"Failed to create brainstorming finding: {ex.Message}", ex);
            }
            return false;

        }
    }
}
