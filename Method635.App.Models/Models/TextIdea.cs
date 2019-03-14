using Newtonsoft.Json;

namespace Method635.App.Models
{
    public class TextIdea : PropertyChangedBase, IIdea
    {
        private string _description;
        [JsonProperty("description")]
        public string Description { get=>_description;
            set =>SetProperty(ref _description, value); }
    }
}
