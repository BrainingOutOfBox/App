using Method635.App.Forms.Context;
using Method635.App.Forms.PrismEvents;
using Method635.App.Forms.RestAccess;
using Method635.App.Forms.ViewModels.Navigation;
using Prism;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Method635.App.Forms.ViewModels
{
    public class BrainstormingFindingListPageViewModel : BindableBase, INavigatedAware
    {
        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;
        private readonly BrainstormingContext _brainstormingContext;

        public BrainstormingFindingListPageViewModel(INavigationService navigationService,
            IEventAggregator eventAggregator,
            BrainstormingContext brainstormingContext)
        {
            this._navigationService = navigationService;
            this._eventAggregator = eventAggregator;
            this._brainstormingContext = brainstormingContext;

            FillFindingListItems();

            this.SelectFindingCommand = new DelegateCommand(SelectFinding);
            this.CreateFindingCommand = new DelegateCommand(CreateBrainstormingFinding);
        }

        private async Task<bool> RefreshFindingList()
        {
            FillFindingListItems();
            return true;
        }

        private async void CreateBrainstormingFinding()
        {
            await this._navigationService.NavigateAsync("NewBrainstormingPage");
        }

        private void FillFindingListItems()
        {
            var findingItems = new BrainstormingFindingRestResolver().GetAllFindingsForTeam(_brainstormingContext.CurrentBrainstormingTeam?.Id);
            HasFindings = findingItems.Count > 0;
            // Encapsulate findings from model into findings to be displayed in listview
            FindingList = findingItems.Select(finding => new BrainstormingFindingListItem(finding)).ToList();
        }

        public DelegateCommand SelectFindingCommand { get; set; }
        public DelegateCommand CreateFindingCommand { get; }
        public DelegateCommand SwipeLeftGestureCommand { get; }
        public DelegateCommand SwipeRightGestureCommand { get; }
        public ICommand RefreshFindingListCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;

                    await RefreshFindingList();

                    IsRefreshing = false;
                });
            }
        }
        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }
        private void SelectFinding()
        {
            _brainstormingContext.CurrentFinding = SelectedFinding.Finding;
            this._eventAggregator.GetEvent<RenderBrainstormingEvent>().Publish();
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            FillFindingListItems();
        }

        public BrainstormingFindingListItem SelectedFinding { get; set; }

        public string Title => "Brainstorming Findings";

        private List<BrainstormingFindingListItem> _findingList;
        public List<BrainstormingFindingListItem> FindingList
        {
            get
            {
                return _findingList;
            }
            set
            {
                SetProperty(ref _findingList, value);
            }
        }
        private bool _hasFindings;
        public bool HasFindings
        {
            get => _hasFindings;
            set => SetProperty(ref _hasFindings, value);
        }
        public bool HasNoFindings => !HasFindings;
        
    }
}
