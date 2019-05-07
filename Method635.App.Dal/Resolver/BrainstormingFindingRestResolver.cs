using Method635.App.Models;
using Method635.App.Forms.RestAccess.ResponseModel;
using Method635.App.Forms.RestAccess.RestExceptions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Method635.App.Logging;
using Xamarin.Forms;
using Method635.App.Dal.Interfaces;
using Method635.App.Dal.Config;
using Method635.App.Dal.Mapping;
using AutoMapper;
using Method635.App.Dal.Config.JsonDto;

namespace Method635.App.Forms.RestAccess
{
    public class BrainstormingFindingRestResolver : IBrainstormingDalService
    {
        private readonly ILogger _logger = DependencyService.Get<ILogManager>().GetLog();

        private readonly BrainstormingEndpoints _findingsEndpoints;
        private readonly IHttpClientService _clientService;
        private readonly IMapper _brainstormingMapper;

        public BrainstormingFindingRestResolver(IConfigurationService configurationService, IHttpClientService httpClientService, IMapper brainstormingMapper)
        {
            _findingsEndpoints = configurationService.ServerConfig.BrainstormingEndpoints;
            _clientService = httpClientService;
            _brainstormingMapper = brainstormingMapper;
        }

        public TimeSpan GetRemainingTime(string findingId, string teamId)
        {
            try
            {
                _logger.Info("Getting remaining time..");
                HttpResponseMessage response = _clientService.GetCall($"{_findingsEndpoints.FindingsEndpoint}/{findingId}/{_findingsEndpoints.RemainingTimeEndpoint}");

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
            catch (Exception ex)
            {
                _logger.Error("There was an error getting the remaining time", ex);
            }
            return TimeSpan.Zero;
        }

        public List<BrainstormingFinding> GetAllFindings(string teamId)
        {
            try
            {
                _logger.Info($"Getting all brainstorming findings for team {teamId}..");
                var res = _clientService.GetCall($"{_findingsEndpoints.FindingsEndpoint}/{teamId}/{_findingsEndpoints.GetAllEndpoint}");
                if (res.IsSuccessStatusCode)
                {
                    _logger.Info($"Got all Brainstormingfindings finding. Content: {res.Content}");
                    var brainstormingFindingsDto = res.Content.ReadAsAsync<List<BrainstormingFindingDto>>().Result;
                    _logger.Info("got findings: ");
                    brainstormingFindingsDto.ForEach(finding => _logger.Info(finding.Name));
                    return _brainstormingMapper.Map<List<BrainstormingFinding>>(brainstormingFindingsDto);
                }
            }
            catch (RestEndpointException ex)
            {
                _logger.Error($"Failed to get brainstorming findings: {ex.Message}", ex);
            }
            catch(Exception ex)
            {
                _logger.Error($"Couldn't get all findings: {ex.Message}", ex);
            }
            _logger.Error($"No brainstorming findings found for team {teamId}",null);
            return new List<BrainstormingFinding>();
        }

        public bool UpdateSheet(string findingId, BrainSheet brainSheet)
        {
            try
            {
                _logger.Info("Updating brainsheet..");
                var brainsheetDto = _brainstormingMapper.Map<BrainSheetDto>(brainSheet);
                var res = _clientService.PutCall(brainsheetDto, $"{_findingsEndpoints.FindingsEndpoint}/{findingId}/{_findingsEndpoints.UpdateBrainsheetEndpoint}");
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
            catch(Exception ex)
            {
                _logger.Error("There was an error updating the brainsheet", ex);
            }
            return false;
        }

        public BrainstormingFinding GetFinding(string findingId)
        {
            try
            {
                _logger.Info("Getting brainstorming finding..");
                var res = _clientService.GetCall($"{_findingsEndpoints.FindingsEndpoint}/{findingId}/{_findingsEndpoints.GetEndpoint}");
                var parsedResponseMessage = res.Content.ReadAsAsync<RestResponseMessage>().Result;
                _logger.Info(parsedResponseMessage.Title);
                _logger.Info(parsedResponseMessage.Text);
                if (res.IsSuccessStatusCode)
                {
                    _logger.Info($"Getting brainstorming finding. Content: {res.Content}");
                    var findingDto = res.Content.ReadAsAsync<BrainstormingFindingDto>().Result;
                    return _brainstormingMapper.Map<BrainstormingFinding>(findingDto);
                }
                else
                {
                    _logger.Error($"Couldn't get finding {findingId}.");
                }
            }
            catch(RestEndpointException ex)
            {
                _logger.Error($"There was an error getting the finding {findingId}");
            }
            catch (Exception ex)
            {
                _logger.Error("There was an error getting the finding", ex);
            }
            return null;
        }

        public BrainstormingFinding CreateFinding(BrainstormingFinding finding)
        {
            try
            {
                _logger.Info("Creating brainstorming finding..");
                var findingDto = _brainstormingMapper.Map<BrainstormingFindingDto>(finding);
                var res = _clientService.PostCall(findingDto, $"{_findingsEndpoints.FindingsEndpoint}/{findingDto.TeamId}/{_findingsEndpoints.CreateEndpoint}");
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
            catch (Exception ex)
            {
                _logger.Error("There was an error creating the finding", ex);
            }
            return finding;
        }
        public bool StartBrainstormingFinding(string findingId)
        {
            try
            {
                _logger.Info("Starting brainstorming finding..");
                var res = _clientService.GetCall($"{_findingsEndpoints.FindingsEndpoint}/{findingId}/{_findingsEndpoints.StartEndpoint}");
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
            catch (Exception ex)
            {
                _logger.Error("There was an error starting the finding", ex);
            }
            return false;

        }

        public string GetExport(string findingId)
        {
            try
            {
                _logger.Info($"Getting export for finding {findingId}..");
                var res = _clientService.GetCall($"{_findingsEndpoints.FindingsEndpoint}/{findingId}/{_findingsEndpoints.ExportEndpoint}");
                var markdownExport = res.Content.ReadAsStringAsync().Result;
                _logger.Info($"Got markdown export from backend: {Environment.NewLine}{markdownExport}");
                if (res.IsSuccessStatusCode)
                {
                    return markdownExport;
                }
                else
                {
                    _logger.Error($"Couldn't get export for finding {findingId}.");
                }
            }
            catch (RestEndpointException ex)
            {
                _logger.Error($"There was an error getting the export for finding {findingId}", ex);
            }
            catch (Exception ex)
            {
                _logger.Error("There was an error getting the export for finding", ex);
            }
            return string.Empty;

        }
    }
}
