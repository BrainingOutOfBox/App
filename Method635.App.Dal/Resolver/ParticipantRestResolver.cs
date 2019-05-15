using System;
using System.Net.Http;
using Method635.App.Models;
using Method635.App.Forms.RestAccess.ResponseModel;
using Method635.App.Forms.RestAccess.RestExceptions;
using Method635.App.Logging;
using Xamarin.Forms;
using Method635.App.Dal.Config;
using Method635.App.Dal.Interfaces;
using AutoMapper;
using Method635.App.Dal.Mapping;
using Method635.App.Dal.Config.JsonDto;

namespace Method635.App.Forms.RestAccess
{
    public class ParticipantRestResolver : IParticipantDalService
    {
        private readonly ILogger _logger;
        private readonly ParticipantEndpoints _participantConfig;
        private readonly IHttpClientService _clientService;
        private readonly IMapper _participantMapper;

        public ParticipantRestResolver(ILogger logger, IConfigurationService configurationService, IHttpClientService httpClientService, IMapper mapper)
        {
            _logger = logger;
            _participantConfig = configurationService.ServerConfig.ParticipantEndpoints;
            _clientService = httpClientService;
            _participantMapper = mapper;
        }

        public bool CreateParticipant(Participant newParticipant)
        {
            try
            {
                _logger.Info("Calling backend to create participant..");
                var participantDto = _participantMapper.Map<ParticipantDto>(newParticipant);
                var res = _clientService.PostCall(participantDto, $"{_participantConfig.ParticipantEndpoint}/{_participantConfig.RegisterEndpoint}");
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
                var participantDto = _participantMapper.Map<ParticipantDto>(loginParticipant);
                var res = _clientService.PostCall(participantDto, $"{_participantConfig.ParticipantEndpoint}/{_participantConfig.LoginEndpoint}");
                if (res.IsSuccessStatusCode)
                {
                    _logger.Info($"Participant {loginParticipant.UserName} successfully logged in.");
                    var participantDtoRes = res.Content.ReadAsAsync<ParticipantDto>().Result;
                    if (participantDtoRes == null)
                    {
                        _logger.Error("Result from client was null");
                        return null;
                    }
                    return _participantMapper.Map<Participant>(participantDtoRes);
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
