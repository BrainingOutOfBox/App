using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Method635.App.Forms.Models;
using Method635.App.Forms.RestAccess.ResponseModel;
using Newtonsoft.Json;

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
                var res = CreateParticipantCall(newParticipant);
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
            return false;
        }

        private HttpResponseMessage CreateParticipantCall(Participant newParticipant)
        {
            using (var client = RestClient())
            {
                var participantJson = JsonConvert.SerializeObject(newParticipant);
                var content = new StringContent(participantJson, Encoding.UTF8, "application/json");
                Console.WriteLine(participantJson);
                Console.WriteLine(content.Headers);
                return client.PostAsync($"{PARTICIPANT_ENDPOINT}/{REGISTER_ENDPOINT}", content).Result;
            }
        }
    }
}
