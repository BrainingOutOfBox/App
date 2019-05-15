namespace Method635.App.Logging
{
    public interface ILogger
    {
        void Error(string message, params object[] args);
        void Warn(string message, params object[] args);
        void Info(string message, params object[] args);
        void Debug(string message, params object[] args);
    }
}
