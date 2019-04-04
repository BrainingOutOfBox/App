using System;
using System.Net.Http;
using Method635.App.Models;
using Method635.App.Forms.RestAccess.ResponseModel;
using Method635.App.Forms.RestAccess.RestExceptions;
using Method635.App.Logging;
using Xamarin.Forms;
using Method635.App.Dal.Config;
using Method635.App.Dal.Interfaces;

namespace Method635.App.Forms.RestAccess
{
    public class ParticipantRestResolver : IParticipantDalService
    {
        private readonly ILogger _logger = DependencyService.Get<ILogManager>().GetLog();
        private readonly ParticipantEndpoints _participantConfig;
        private readonly IHttpClientService _clientService;

        public ParticipantRestResolver(IConfigurationService configurationService, IHttpClientService httpClientService)
        {
            _participantConfig = configurationService.ServerConfig.ParticipantEndpoints;
            _clientService = httpClientService;
        }

        public bool CreateParticipant(Participant newParticipant)
        {
            try
            {
                _logger.Info("Calling backend to create participant..");
                var res = _clientService.PostCall(newParticipant, $"{_participantConfig.ParticipantEndpoint}/{_participantConfig.RegisterEndpoint}");
                if (res.IsSuccessStatusCode)
                {
                    _logger.Info($"Created participant. Content: {res.Content}");
                    var parsedResponseMessage = res.Content.ReadAsAsync<RestResponseMessage>().Result;
                    _logger.Info(parsedResponseMessage.Title, parsedResponseMessage.Text);
                    return true;
                }
            }
            catch (RestEndpointException ex)
            {
                _logger.Error($"Failed to create participant: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to create participant: {ex.Message}", ex);
            }
            return false;
        }

        public Participant Login(Participant loginParticipant)
        {
            try
            {
                _logger.Info("Calling backend to login..");
                var res = _clientService.PostCall(loginParticipant, $"{_participantConfig.ParticipantEndpoint}/{_participantConfig.LoginEndpoint}");
                if (res.IsSuccessStatusCode)
                {
                    _logger.Info($"Participant {loginParticipant.UserName} successfully logged in.");
                    return res.Content.ReadAsAsync<RestLoginResponse>().Result?.Participant;
                }
            }
            catch (RestEndpointException ex)
            {
                _logger.Error($"Failed to login: {ex.Message}", ex);
            }
            catch(Exception ex)
            {
                _logger.Error($"Failed to login: {ex.Message}", ex);
            }
            return null; 
        }
    }
}
