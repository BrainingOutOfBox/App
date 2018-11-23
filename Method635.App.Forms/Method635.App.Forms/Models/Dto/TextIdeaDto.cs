using Newtonsoft.Json;

namespace Method635.App.Forms.Dto
{
    public class TextIdeaDto : IIdea
    {
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
