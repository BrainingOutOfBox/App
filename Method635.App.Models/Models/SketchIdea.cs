using Method635.App.Models.Models;
using System.IO;
using Xamarin.Forms;

namespace Method635.App.Models
{
    public class SketchIdea : PictureIdea
    {
        public SketchIdea()
        {
            ImageSource = ImageSource.FromFile("download_image.png");
        }
        public Stream ImageStream { get; set; }
    }
}
