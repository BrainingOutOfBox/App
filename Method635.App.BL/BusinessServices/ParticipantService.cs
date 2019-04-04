using Method635.App.BL.Interfaces;
using Method635.App.Dal.Interfaces;
using Method635.App.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Method635.App.BL.BusinessServices
{
    public class ParticipantService : IParticipantService
    {
        private readonly IParticipantDalService _participantDalService;

        public ParticipantService(IDalService dalService)
        {
            _participantDalService = dalService.ParticipantDalService;
        }
        public Participant Login(Participant participant)
        {
            return _participantDalService.Login(participant);
        }

        public bool Register(Participant participant)
        {
            return _participantDalService.CreateParticipant(participant);
        }
    }
}
