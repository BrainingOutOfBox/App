using Newtonsoft.Json;

namespace Method635.App.Dal.Mapping.DTO
{
    public class SketchIdeaDto : IdeaDto
    {
        [JsonProperty("pictureId")]
        public string PictureId { get; set; }
    }
}
