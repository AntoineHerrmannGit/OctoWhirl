namespace OctoWhirl.GUI.ViewModels.Technical
{
    public interface IStatusService
    {
        string CurrentStatus { get; }
        void SetStatus(string message);
        event EventHandler<string> StatusChanged;
    }
}
