using System.IO;
using Xamarin.Forms;

namespace Method635.App.Models
{
    public class SketchIdea : Idea
    {
        public SketchIdea()
        {
            ImageSource = ImageSource.FromFile("download_image.png");
        }
        public string PictureId { get; set; }
        public Stream ImageStream { get; set; }
        private ImageSource _imageSource;
        public ImageSource ImageSource { get => _imageSource; set=>SetProperty(ref _imageSource, value); }
    }
}
