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
        Task NavigateToBrainstormingTab(INavigationParameters parameters);
        Task NavigateToCreateBrainstorming();
        Task NavigateToStartBrainstorming();
        Task NavigateToInsertSpecial();
        Task NavigateToInsertSketch();
        Task NavigateToInsertPattern();
    }
}
