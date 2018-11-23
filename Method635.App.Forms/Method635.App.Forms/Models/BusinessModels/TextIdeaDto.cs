using Prism.Mvvm;

namespace Method635.App.Forms.BusinessModels
{
    public class TextIdea : BindableBase, IIdea
    {
        private string _description;
        public string Description { get => _description; set => SetProperty(ref _description, value); }
    }
}
