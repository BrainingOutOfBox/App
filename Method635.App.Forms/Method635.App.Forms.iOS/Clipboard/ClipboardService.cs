using Method635.App.BL.Interfaces;
using Method635.App.Forms.iOS.Clipboard;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(ClipboardService))]
namespace Method635.App.Forms.iOS.Clipboard
{
    public class ClipboardService : IClipboardService
    {
        public void CopyToClipboard(string content)
        {
            UIPasteboard clipboard = UIPasteboard.General;
            clipboard.String = content;
        }
    }
}