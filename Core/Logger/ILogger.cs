namespace OctoWhirl.Core.Logger
{
    public interface ILogger
    {
        void Debug(string message);
        void Log(string message);
        void Error(string message);
    }
}