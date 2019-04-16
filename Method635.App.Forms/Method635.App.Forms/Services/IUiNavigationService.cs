using Prism.Navigation;
using System.Threading.Tasks;

namespace Method635.App.Forms.Services
{
    public interface IUiNavigationService
    {
        Task NavigateToMainPage();
        Task NavigateToLogin();
        Task NavigateToRegister();
        Task NavigateToTeamsTab();
        Task NavigateToCreateTeam();
        Task NavigateToInviteTeam();
        Task NavigateToJoinTeam();
        Task NavigateToBrainstormingListTab();
        Task NavigateToBrainstormingTab();
        Task NavigateToCreateBrainstorming();
        Task NavigateToStartBrainstorming();
        Task NavigateToInsertSpecial(INavigationParameters parameters);
        Task NavigateToInsertSketch(INavigationParameters parameters);
    }
}
