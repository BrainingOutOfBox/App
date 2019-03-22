using Method635.App.Dal.Interfaces;
using Method635.App.Forms.RestAccess;

namespace Method635.App.Dal
{
    public class RestDalService : IDalService
    {
        private readonly IBrainstormingDalService _brainstormingDalService = new BrainstormingFindingRestResolver();
        public IBrainstormingDalService BrainstormingDalService => _brainstormingDalService;
    }
}
