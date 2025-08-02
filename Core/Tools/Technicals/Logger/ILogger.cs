namespace OctoWhirl.Core.Tools.Technicals
{
    public interface ILogger
    {
        void Debug(string message);
        void Log(string message);
        void Error(string message);
    }
}