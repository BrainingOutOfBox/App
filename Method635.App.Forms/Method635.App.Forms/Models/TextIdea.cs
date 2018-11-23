using Newtonsoft.Json;

namespace Method635.App.Forms.Models
{
    public class TextIdea : IIdea
    {
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
