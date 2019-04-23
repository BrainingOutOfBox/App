using Method635.App.Dal.Interfaces;
using Method635.App.Forms.RestAccess;

namespace Method635.App.Dal
{
    public class RestDalService : IDalService
    {
        public RestDalService(
            IBrainstormingDalService brainstormingService,
            IParticipantDalService participantDalService,
            ITeamDalService teamDalService,
            IFileDalService fileDalService)
        {
            BrainstormingDalService = brainstormingService;
            ParticipantDalService = participantDalService;
            TeamDalService = teamDalService;
            FileDalService = fileDalService;
        }

        public IBrainstormingDalService BrainstormingDalService { get; }

        public IParticipantDalService ParticipantDalService { get; }

        public ITeamDalService TeamDalService { get; }

        public IFileDalService FileDalService { get; }
    }
}
