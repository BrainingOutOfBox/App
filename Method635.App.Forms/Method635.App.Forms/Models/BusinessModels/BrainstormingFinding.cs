using Prism.Mvvm;
using System.Collections.Generic;

namespace Method635.App.Forms.BusinessModels
{
    public class BrainstormingFinding : BindableBase
    {
        private string _id;
        public string Id { get=>_id; set=>SetProperty(ref _id, value); }

        private string _teamId;
        public string TeamId { get=>_teamId; set=>SetProperty(ref _teamId, value); }

        private string _name;
        public string Name { get=>_name; set=>SetProperty(ref _name, value); }

        private string _problemDescription;
        public string ProblemDescription { get=>_problemDescription; set=>SetProperty(ref _problemDescription, value); }

        private int _nrOfIdeas;
        public int NrOfIdeas { get=>_nrOfIdeas; set=>SetProperty(ref _nrOfIdeas, value); }

        private int _baseRoundTime;
        public int BaseRoundTime { get=>_baseRoundTime; set=>SetProperty(ref _baseRoundTime, value); }

        private int _currentRound;
        public int CurrentRound { get=>_currentRound; set=>SetProperty(ref _currentRound, value); }

        private List<BrainSheet> _brainSheets;
        public List<BrainSheet> BrainSheets { get=>_brainSheets; set=>SetProperty(ref _brainSheets, value); }
    }
}
