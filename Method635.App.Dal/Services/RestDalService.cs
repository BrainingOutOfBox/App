using Method635.App.Dal.Interfaces;
using Method635.App.Forms.RestAccess;

namespace Method635.App.Dal
{
    public class RestDalService : IDalService
    {
        public RestDalService(
            IBrainstormingDalService brainstormingService,
            IParticipantDalService participantDalService,
            ITeamDalService teamDalService)
        {
            BrainstormingDalService = brainstormingService;
            ParticipantDalService = participantDalService;
            TeamDalService = teamDalService;
        }

        public IBrainstormingDalService BrainstormingDalService { get; }

        public IParticipantDalService ParticipantDalService { get; }

        public ITeamDalService TeamDalService { get; }
    }
}
