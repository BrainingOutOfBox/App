using Method635.App.Models;
using System.Collections.Generic;

namespace Method635.App.Dal.Interfaces
{
    public interface ITeamDalService
    {
        BrainstormingTeam GetTeamById(string teamId);
        List<BrainstormingTeam> GetMyBrainstormingTeams(string userName);
        bool JoinTeam(string teamId, Participant participant);
        Moderator GetModeratorByTeamId(string teamId);
        BrainstormingTeam CreateBrainstormingTeam(BrainstormingTeam brainstormingTeam);
    }
}
