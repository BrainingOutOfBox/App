using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Method635.App.Forms.ViewModels.Brainstorming
{
	public class NewProblemPageViewModel : BindableBase
	{
        private INavigationService _navigationService;

        public NewProblemPageViewModel(INavigationService navigationService)
        {
            this._navigationService = navigationService;
        }

        public string FindingName { get; set; }
        public int NrOfIdeas { get; set; }
        public int BaseRoundTime { get; set; }
        public string Description { get; set; }

    }
}
