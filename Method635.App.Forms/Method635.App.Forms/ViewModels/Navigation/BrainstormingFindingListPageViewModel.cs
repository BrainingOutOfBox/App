﻿using Method635.App.BL.Context;
using Method635.App.Dal.Interfaces;
using Method635.App.Forms.Services;
using Method635.App.Forms.ViewModels.Navigation;
using Method635.App.Logging;
using Method635.App.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Method635.App.Forms.ViewModels
{
    public class BrainstormingFindingListPageViewModel : BindableBase, INavigatedAware, IDestructible
    {
        private readonly IUiNavigationService _navigationService;
        private readonly IBrainstormingDalService _brainstormingDalService;
        private readonly BrainstormingContext _brainstormingContext;
        private readonly ILogger _logger;

        public BrainstormingFindingListPageViewModel(IUiNavigationService navigationService,
            IDalService iDalService,
            BrainstormingContext brainstormingContext,
            ILogger logger)
        {
            _navigationService = navigationService;
            _brainstormingDalService = iDalService.BrainstormingDalService;
            _brainstormingContext = brainstormingContext;
            _logger = logger;

            FillFindingListItems();

            _brainstormingContext.PropertyChanged += Context_PropertyChanged;

            SelectFindingCommand = new DelegateCommand(SelectFinding);
            CreateFindingCommand = new DelegateCommand(async ()=> await CreateBrainstormingFinding());
        }

        private void Context_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(_brainstormingContext.CurrentFinding) || !(sender is BrainstormingContext changedContext))
                return;
            var idx = FindingList.FindIndex(f => f.Finding.Id.Equals(changedContext.CurrentFinding.Id));
            if (idx < 0 || idx > FindingList.Count - 1)
                return;

            FindingList[idx] = new BrainstormingFindingListItem(changedContext.CurrentFinding);
        }

        private async Task<bool> RefreshFindingList()
        {
            await Task.Run(() => FillFindingListItems());
            return true;
        }

        private async Task CreateBrainstormingFinding()
        {
            await _navigationService.NavigateToCreateBrainstorming();
        }

        private void FillFindingListItems()
        {
            var findingItems = _brainstormingDalService.GetAllFindings(_brainstormingContext.CurrentBrainstormingTeam?.Id);
            HasFindings = findingItems.Any();
            // Encapsulate findings from model into findings to be displayed in listview
            FindingList = findingItems.Select(finding => new BrainstormingFindingListItem(finding)).ToList();
        }

        private void SelectFinding()
        {
            _brainstormingContext.CurrentFinding = SelectedFinding.Finding;
            _navigationService.NavigateToBrainstormingTab(new NavigationParameters());
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            FillFindingListItems();
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            //
        }

        public void Destroy()
        {
            _brainstormingContext.PropertyChanged -= Context_PropertyChanged;
        }

        public BrainstormingFindingListItem SelectedFinding { get; set; }


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
        public string Title { get; set; }

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
            set
            {
                SetProperty(ref _hasFindings, value);
                HasNoFindings = !value;
            }
        }
        private bool _hasNoFindings;
        public bool HasNoFindings
        {
            get => _hasNoFindings;
            set => SetProperty(ref _hasNoFindings, value);
        }
    }
}
