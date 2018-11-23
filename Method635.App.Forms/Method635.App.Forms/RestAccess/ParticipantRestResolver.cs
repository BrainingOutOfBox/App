using System;
using System.Net.Http;
using AutoMapper;
using Method635.App.Forms.BusinessModels;
using Method635.App.Forms.Dto;
using Method635.App.Forms.RestAccess.ResponseModel;
using Method635.App.Forms.RestAccess.RestExceptions;

namespace Method635.App.Forms.RestAccess
{
    public class ParticipantRestResolver : RestResolverBase
    {
        private const string PARTICIPANT_ENDPOINT = "Participant";
        private const string REGISTER_ENDPOINT = "register";
        private const string LOGIN_ENDPOINT = "login";

        internal bool CreateParticipant(Participant newParticipant)
        {
            try
            {
                Console.WriteLine("Calling backend to create participant..");
                var participantDto = Mapper.Map<ParticipantDto>(newParticipant);
                
                var res = PostCall(participantDto, $"{PARTICIPANT_ENDPOINT}/{REGISTER_ENDPOINT}");
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Created participant. Content: {res.Content}");
                    var parsedResponseMessage = res.Content.ReadAsAsync<RestResponseMessage>().Result;
                    Console.WriteLine(parsedResponseMessage.Title, parsedResponseMessage.Text);
                    return true;
                }
            }
            catch (RestEndpointException ex)
            {
                Console.WriteLine($"Failed to create participant: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to create participant: {ex.Message}");
            }
            return false;
        }

        internal RestLoginResponse Login(Participant loginParticipant)
        {
            try
            {
                Console.WriteLine("Calling backend to login..");

                var loginParticipantDto = Mapper.Map<ParticipantDto>(loginParticipant);
                var res = PostCall(loginParticipantDto, $"{PARTICIPANT_ENDPOINT}/{LOGIN_ENDPOINT}");

                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Participant {loginParticipant.UserName} successfully logged in.");
                    var responseDto = res.Content.ReadAsAsync<RestLoginResponseDto>().Result;
                    return Mapper.Map<RestLoginResponse>(responseDto);
                }
            }
            catch (RestEndpointException ex)
            {
                Console.WriteLine($"Failed to login: {ex.Message}");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Failed to login: {ex.Message}");
            }
            return null; 
        }
    }
}
