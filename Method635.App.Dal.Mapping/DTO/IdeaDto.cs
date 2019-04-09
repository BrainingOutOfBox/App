using Newtonsoft.Json;

namespace Method635.App.Dal.Mapping.DTO
{
    [JsonConverter(typeof(IdeaConverter))]
    public abstract class IdeaDto
    {
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
