namespace Method635.App.Models
{
    public abstract class Idea : PropertyChangedBase
    {
        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }
    }
}
