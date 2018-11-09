using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Method635.App.Forms.Models
{
    public class Idea
    {
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
