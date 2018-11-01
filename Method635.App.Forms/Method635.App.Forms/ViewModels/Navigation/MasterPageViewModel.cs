using Method635.App.Forms.ViewModels.Navigation;
using Method635.App.Forms.Views.Navigation;
using Prism.Mvvm;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Method635.App.Forms.ViewModels
{
    public class MasterPageViewModel : BindableBase
    {
        public ListView ProblemsListView { get { return _problemsListView; } }

        ListView _problemsListView;

        public MasterPageViewModel()
        {
            _problemsListView = new ListView();
            var findingsList = new List<BrainstormingFindingItem>()
            {
                new BrainstormingFindingItem(){
                    BindingContext = new BrainstormingFindingItemViewModel(null)
                    {
                        Title= "TEST BSFINDING"}
                    }
            };
            _problemsListView.ItemsSource = findingsList;
        }
	}
}
