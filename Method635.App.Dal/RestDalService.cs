using Method635.App.Dal.Interfaces;
using Method635.App.Forms.RestAccess;

namespace Method635.App.Dal
{
    public class RestDalService : IDalService
    {
        public RestDalService(IBrainstormingDalService brainstormingService)
        {
            BrainstormingDalService = brainstormingService;
        }
        public IBrainstormingDalService BrainstormingDalService { get; }

    }
}
