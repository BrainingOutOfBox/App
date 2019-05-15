using Method635.App.Logging;
using Method635.App.Forms.ViewModels;
using Method635.App.Forms.ViewModels.Account;
using Method635.App.Forms.ViewModels.Brainstorming;
using Method635.App.Forms.ViewModels.Team;
using Method635.App.Forms.Views;
using Method635.App.Forms.Views.Account;
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
using Method635.App.Forms.Services;
using Method635.App.Dal.Config;
using Method635.App.BL;
using Method635.App.Dal.Interfaces;
using Method635.App.Dal;
using Method635.App.Forms.RestAccess;
using Method635.App.BL.Interfaces;
using Method635.App.BL.BusinessServices;
using Method635.App.BL.Context;
using Method635.App.Models.Models;
using AutoMapper;
using Method635.App.Dal.Mapping.Mappers;
using System;
using Method635.App.Forms.Views.Brainstorming.SpecialContent;
using Method635.App.Dal.Resolver;
using Method635.App.Forms.Views.Brainstorming.SpecialContent.PatternIdea;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Method635.App.Forms
{
    public partial class App : PrismApplication
    {
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }
        private readonly ILogger _logger = DependencyService.Get<ILogManager>().GetLog();

        protected override async void OnInitialized()
        {
            try
            {
                InitializeComponent();
                await NavigationService.NavigateAsync("NavigationPage/LoginPage");
            }
            catch (Exception ex)
            {
                _logger.Error("Fatal exception, the world has come to an end :(", ex);
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterSingleton<IUiNavigationService, UiNavigationService>();

            RegisterDalServices(containerRegistry);

            RegisterMapping(containerRegistry);

            RegisterPlatformServices(containerRegistry);
            RegisterBusinessInstances(containerRegistry);
            RegisterNavigationPages(containerRegistry);
        }

        private static void RegisterDalServices(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IConfigurationService, JsonConfigurationService>();

            containerRegistry.RegisterSingleton<IHttpClientService, RestClientService>();
            containerRegistry.Register<IDalService, RestDalService>();
            containerRegistry.Register<IBrainstormingDalService, BrainstormingFindingRestResolver>();
            containerRegistry.Register<IParticipantDalService, ParticipantRestResolver>();
            containerRegistry.Register<ITeamDalService, TeamRestResolver>();
            containerRegistry.Register<IFileDalService, FileRestResolver>();
            containerRegistry.Register<IPatternDalService, PatternRestResolver>();
        }

        private static void RegisterMapping(IContainerRegistry containerRegistry)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BrainstormingMappingProfile());
                cfg.AddProfile(new ParticipantMappingProfile());
                cfg.AddProfile(new TeamMappingProfile());
            });
            config.AssertConfigurationIsValid();

            containerRegistry.RegisterInstance(typeof(IMapper), config.CreateMapper());
        }

        private static void RegisterBusinessInstances(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IBrainstormingService, BrainstormingService>();
            containerRegistry.Register<IParticipantService, ParticipantService>();
            containerRegistry.Register<ITeamService, TeamService>();

            containerRegistry.RegisterSingleton<BrainstormingContext>();
            containerRegistry.RegisterSingleton<BrainstormingModel>();
        }

        private static void RegisterNavigationPages(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();

            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<CreateAccountPage, CreateAccountPageViewModel>();

            containerRegistry.RegisterForNavigation<BrainstormingPage, BrainstormingPageViewModel>();
            containerRegistry.RegisterForNavigation<NewBrainstormingPage, NewBrainstormingPageViewModel>();
            containerRegistry.RegisterForNavigation<BrainstormingFindingListPage, BrainstormingFindingListPageViewModel>();

            containerRegistry.RegisterForNavigation<TeamPage, TeamPageViewModel>();
            containerRegistry.RegisterForNavigation<NewTeamPage, NewTeamPageViewModel>();
            containerRegistry.RegisterForNavigation<InviteTeamPage, InviteTeamPageViewModel>();
            containerRegistry.RegisterForNavigation<JoinTeamPage, JoinTeamPageViewModel>();
            containerRegistry.RegisterForNavigation<InsertSpecialPage, InsertSpecialPageViewModel>();
            containerRegistry.RegisterForNavigation<SketchPage>();
            containerRegistry.RegisterForNavigation<PatternPage, PatternPageViewModel>();
        }

        private static void RegisterPlatformServices(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ILogger, NLogLogger>();

            var toastMessenger = DependencyService.Get<IToastMessageService>();
            containerRegistry.RegisterInstance(typeof(IToastMessageService), toastMessenger);
            var clipboardService = DependencyService.Get<IClipboardService>();
            containerRegistry.RegisterInstance(typeof(IClipboardService), clipboardService);
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
