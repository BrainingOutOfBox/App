using System.Globalization;

namespace Method635.App.Forms.Resources
{
    public interface ILocalize
    {
        CultureInfo GetCurrentCultureInfo();
        void SetLocale(CultureInfo ci);
    }
}