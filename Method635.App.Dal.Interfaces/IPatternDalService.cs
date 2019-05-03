using Method635.App.Models;
using System.Collections.Generic;

namespace Method635.App.Dal.Interfaces
{
    public interface IPatternDalService
    {
        List<PatternIdea> GetAllPatterns();
    }
}
