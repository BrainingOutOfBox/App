﻿using System.IO;

namespace Method635.App.Models.Models
{
    public class SketchIdea : Idea
    {
        public string PictureId { get; set; }
        public Stream ImageStream { get; set; }
    }
}
