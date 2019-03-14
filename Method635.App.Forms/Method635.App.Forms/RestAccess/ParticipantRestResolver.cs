using System;
using System.Net.Http;
using Method635.App.Models;
using Method635.App.Forms.RestAccess.ResponseModel;
using Method635.App.Forms.RestAccess.RestExceptions;
using Method635.App.Logging;
using Xamarin.Forms;

namespace Method635.App.Forms.RestAccess
{
    public class ParticipantRestResolver : RestResolverBase
    {
        private const string PARTICIPANT_ENDPOINT = "Participant";
        private const string REGISTER_ENDPOINT = "register";
        private const string LOGIN_ENDPOINT = "login";

        // Platform independent logger necessary, thus resolving from xf dependency service.
        private readonly ILogger _logger = DependencyService.Get<ILogManager>().GetLog();

        internal bool CreateParticipant(Participant newParticipant)
        {
            try
            {
                _logger.Info("Calling backend to create participant..");
                var res = PostCall(newParticipant, $"{PARTICIPANT_ENDPOINT}/{REGISTER_ENDPOINT}");
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

        internal RestLoginResponse Login(Participant loginParticipant)
        {
            try
            {
                _logger.Info("Calling backend to login..");
                var res = PostCall(loginParticipant, $"{PARTICIPANT_ENDPOINT}/{LOGIN_ENDPOINT}");
                if (res.IsSuccessStatusCode)
                {
                    _logger.Info($"Participant {loginParticipant.UserName} successfully logged in.");
                    return res.Content.ReadAsAsync<RestLoginResponse>().Result;
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
