using Method635.App.Forms.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Method635.App.Forms.ViewModels.Navigation
{
    public class BrainstormingFindingItemViewModel
    {
        private BrainstormingFinding _finding;
        public BrainstormingFindingItemViewModel(BrainstormingFinding finding)
        {
        }

        private string _title;
        public string Title { get => _title; set => _title = value; }
    }
}
