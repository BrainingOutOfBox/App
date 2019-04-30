using Newtonsoft.Json;

namespace Method635.App.Dal.Config.JsonDto
{
    public class FileEndpoints
    {
        [JsonProperty("file")]
        public string FilesEndpoint { get; set; }
        [JsonProperty("download")]
        public string DownloadEndpoint { get; set; }
        [JsonProperty("upload")]
        public string UploadEndpoint { get; set; }
    }
}
