using Method635.App.Models;
using System.Collections.Generic;

namespace Method635.App.BL.Interfaces
{
    public interface ITeamService
    {
        BrainstormingTeam GetCurrentTeam();
        bool JoinTeam(string teamId, Participant participant);
        BrainstormingTeam GetTeam(string teamId);
        List<BrainstormingTeam> GetTeamsByUserName(string userName);
        BrainstormingTeam AddTeam(BrainstormingTeam newTeam);
    }
}
