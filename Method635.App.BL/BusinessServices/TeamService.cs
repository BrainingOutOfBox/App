using System.Collections.Generic;
using Method635.App.BL.Context;
using Method635.App.BL.Interfaces;
using Method635.App.Dal.Interfaces;
using Method635.App.Models;

namespace Method635.App.BL.BusinessServices
{
    public class TeamService : ITeamService
    {
        private readonly ITeamDalService _teamDalService;
        private BrainstormingContext _context;

        public TeamService(IDalService dalService, BrainstormingContext context)
        {
            _teamDalService = dalService.TeamDalService;
            _context = context;
        }

        public BrainstormingTeam AddTeam(BrainstormingTeam newTeam)
        {
            return _teamDalService.CreateBrainstormingTeam(newTeam);
        }

        public BrainstormingTeam GetCurrentTeam()
        {
            return _teamDalService.GetTeamById(_context.CurrentBrainstormingTeam.Id);
        }

        public BrainstormingTeam GetTeam(string teamId)
        {
            return _teamDalService.GetTeamById(teamId);
        }

        public List<BrainstormingTeam> GetTeamsByUserName(string userName)
        {
            return _teamDalService.GetMyBrainstormingTeams(userName);
        }

        public bool JoinTeam(string teamId, Participant participant)
        {
            return _teamDalService.JoinTeam(teamId, participant);
        }
    }
}
