using Android.Content;
using Method635.App.BL.Interfaces;
using Method635.App.Forms.Droid.Clipboard;
using Xamarin.Forms;

[assembly: Dependency(typeof(ClipboardService))]
namespace Method635.App.Forms.Droid.Clipboard
{
    public class ClipboardService : IClipboardService
    {
        public void CopyToClipboard(string content)
        {
            var clipboardManager = (ClipboardManager)Xamarin.Forms.Forms.Context.GetSystemService(Context.ClipboardService);
            ClipData clip = ClipData.NewPlainText("Android Clipboard", content);
            clipboardManager.PrimaryClip = clip;
        }
    }
}