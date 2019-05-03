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
            IFileDalService fileDalService,
            IPatternDalService patternDalService)
        {
            BrainstormingDalService = brainstormingService;
            ParticipantDalService = participantDalService;
            TeamDalService = teamDalService;
            FileDalService = fileDalService;
            PatternDalService = patternDalService;
        }

        public IBrainstormingDalService BrainstormingDalService { get; }

        public IParticipantDalService ParticipantDalService { get; }

        public ITeamDalService TeamDalService { get; }

        public IFileDalService FileDalService { get; }

        public IPatternDalService PatternDalService { get; }
    }
}
