using Method635.App.Models;
using System;
using System.Collections.Generic;

namespace Method635.App.Dal.Interfaces
{
    public interface IBrainstormingDalService
    {
        BrainstormingFinding CreateFinding(BrainstormingFinding finding);
        BrainstormingFinding GetFinding(string findingId);
        List<BrainstormingFinding> GetAllFindings(string teamId);
        bool UpdateSheet(string findingId, BrainSheet brainSheet);
        TimeSpan GetRemainingTime(string findingId, string teamId);
        bool StartBrainstormingFinding(string findingId);
        string GetExport(string findingId);
    }
}
