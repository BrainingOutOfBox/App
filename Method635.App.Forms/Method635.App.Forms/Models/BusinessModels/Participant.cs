using Prism.Mvvm;

namespace Method635.App.Forms.BusinessModels
{
    public class Participant : BindableBase
    {
        private string _userName;
        public string UserName { get => _userName; set => SetProperty(ref _userName, value); }

        private string _password;
        public string Password { get => _password; set => SetProperty(ref _password, value); }

        private string _firstName;
        public string FirstName { get => _firstName; set => SetProperty(ref _firstName, value); }

        private string _lastName;
        public string LastName { get => _lastName; set => SetProperty(ref _lastName, value); }
    }
}
