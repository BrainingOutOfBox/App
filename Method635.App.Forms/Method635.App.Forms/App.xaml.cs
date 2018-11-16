using Method635.App.Forms.Context;
using Method635.App.Forms.ViewModels;
using Method635.App.Forms.ViewModels.Brainstorming;
using Method635.App.Forms.ViewModels.Team;
using Method635.App.Forms.Views.Brainstorming;
using Method635.App.Forms.Views.Navigation;
using Method635.App.Forms.Views.Team;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Method635.App.Forms
{
    public partial class App : PrismApplication
    {
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<BrainstormingPage, BrainstormingPageViewModel>();
            containerRegistry.RegisterForNavigation<StartBrainstormingPage, StartBrainstormingPageViewModel>();
            containerRegistry.RegisterForNavigation<NewBrainstormingPage, NewBrainstormingPageViewModel>();
            containerRegistry.RegisterForNavigation<BrainstormingFindingListPage, BrainstormingFindingListPageViewModel>();
            containerRegistry.RegisterForNavigation<TeamPage, TeamPageViewModel>();
            containerRegistry.RegisterForNavigation<NewTeamPage, NewTeamPageViewModel>();
            containerRegistry.RegisterForNavigation<InviteTeamPage, InviteTeamPageViewModel>();
            containerRegistry.RegisterForNavigation<JoinTeamPage, JoinTeamPageViewModel>();


            containerRegistry.RegisterSingleton<BrainstormingContext>();
        }

        protected override void OnStart()
        {
            AppCenter.Start("android=99ec95ab-f8c7-42ab-91b4-46af3874744f;" +
                     "uwp={Your UWP App secret here};" +
                     "ios=dca97776-7990-4b5a-9674-ecc97cfcb787;",
                     typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
