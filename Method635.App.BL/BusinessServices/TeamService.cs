using System.Collections.Generic;
using Method635.App.BL.Context;
using Method635.App.BL.Interfaces;
using Method635.App.Dal.Interfaces;
using Method635.App.Logging;
using Method635.App.Models;
using Xamarin.Forms;

namespace Method635.App.BL.BusinessServices
{
    public class TeamService : ITeamService
    {
        private readonly ITeamDalService _teamDalService;
        private readonly BrainstormingContext _context;
        private readonly ILogger _logger = DependencyService.Get<ILogManager>().GetLog();

        public TeamService(IDalService dalService, BrainstormingContext context)
        {
            _teamDalService = dalService.TeamDalService;
            _context = context;
        }

        public BrainstormingTeam AddTeam(BrainstormingTeam newTeam)
        {
            var team = _teamDalService.CreateBrainstormingTeam(newTeam);
            if (string.IsNullOrEmpty(team.Id))
            {
                _logger.Error("Team id from backend was null.");
            }
            return team;
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
