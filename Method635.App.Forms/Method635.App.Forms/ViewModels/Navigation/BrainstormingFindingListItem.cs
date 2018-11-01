using Method635.App.Forms.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Method635.App.Forms.ViewModels.Navigation
{
    public class BrainstormingFindingListItem
    {
        private BrainstormingFinding _finding;
        public BrainstormingFindingListItem(BrainstormingFinding finding)
        {
            this._finding = finding;
        }

        public string Title { get => _finding.Name; }

    }
}
