using System.Collections.Generic;

namespace Method635.App.Models
{
    public class BrainstormingFinding : PropertyChangedBase
    {
        public string Id { get; set; }

        public string TeamId { get; set; }

        public string Name { get; set; }

        public string ProblemDescription { get; set; }

        public int NrOfIdeas { get; set; }

        public int BaseRoundTime { get; set; }
        public string Category { get; set; } = string.Empty;

        private int _currentRound;
        public int CurrentRound { get=>_currentRound; set=>SetProperty(ref _currentRound, value); }

        public List<BrainSheet> BrainSheets { get; set; }
    }
}
