using Prism.Mvvm;
using System.Collections.Generic;

namespace Method635.App.Forms.BusinessModels
{
    public class BrainstormingTeam : BindableBase
    {
        private string _id;
        public string Id { get => _id; set => SetProperty(ref _id, value); }

        private string _name;
        public string Name { get => _name; set => SetProperty(ref _name, value); }

        private string _purpose;
        public string Purpose { get => _purpose; set => SetProperty(ref _purpose, value); }

        private int _nrOfParticipants;
        public int NrOfParticipants { get => _nrOfParticipants; set => SetProperty(ref _nrOfParticipants, value); }

        private int _currentNrOfParticipants;
        public int CurrentNrOfParticipants { get => _currentNrOfParticipants; set => SetProperty(ref _currentNrOfParticipants, value); }

        private List<Participant> _participants;
        public List<Participant> Participants { get => _participants; set => SetProperty(ref _participants, value); }

        private Moderator _moderator;
        public Moderator Moderator { get => _moderator; set => SetProperty(ref _moderator, value); }
    }
}

