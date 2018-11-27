using Newtonsoft.Json;
using Prism.Mvvm;

namespace Method635.App.Forms.Models
{
    public class TextIdea : BindableBase, IIdea
    {
        private string _description;
        [JsonProperty("description")]
        public string Description { get=>_description;
            set =>SetProperty(ref _description, value); }
    }
}
