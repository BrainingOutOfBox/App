using Method635.App.Forms.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Method635.App.Forms.ViewModels.Navigation
{
    public class BrainstormingFindingListItem
    {
        public readonly BrainstormingFinding Finding;
        public BrainstormingFindingListItem(BrainstormingFinding finding)
        {
            this.Finding = finding;
        }

        public string Title { get => Finding.Name; }
        
    }
}
