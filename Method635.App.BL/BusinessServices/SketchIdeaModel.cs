using Method635.App.Models;
using Xamarin.Forms;

namespace Method635.App.BL.BusinessServices
{
    public class SketchIdeaModel : SketchIdea
    {
        private ImageSource _imageSource;
        public ImageSource ImageSource { get=>_imageSource; set=>SetProperty(ref _imageSource, value); }
    }
}
