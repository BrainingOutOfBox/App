using Newtonsoft.Json;

namespace Method635.App.Dal.Mapping.DTO
{
    public class PatternIdeaDto : IdeaDto
    {
        [JsonProperty("category")]
        public string Category { get; set; }
        [JsonProperty("problem")]
        public string Problem { get; set; }
        [JsonProperty("solution")]
        public string Solution { get; set; }
        [JsonProperty("pictureId")]
        public string PictureId { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
