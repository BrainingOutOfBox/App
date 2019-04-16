using System.Runtime.CompilerServices;

namespace Method635.App.Logging
{
    public interface ILogManager
    {
        ILogger GetLog([CallerFilePath]string callerFilePath = "");
    }
}
