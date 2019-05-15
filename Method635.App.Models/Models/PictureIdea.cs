using Xamarin.Forms;

namespace Method635.App.Models.Models
{
    public class PictureIdea : Idea
    {
        public PictureIdea()
        {
            ImageSource = ImageSource.FromFile("download_image.png");
        }
        public string PictureId { get; set; }

        private ImageSource _imageSource;
        public ImageSource ImageSource { get => _imageSource; set => SetProperty(ref _imageSource, value); }
    }
}
