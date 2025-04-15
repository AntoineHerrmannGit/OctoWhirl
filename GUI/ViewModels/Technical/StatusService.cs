namespace OctoWhirl.GUI.ViewModels.Technical
{
    public class StatusService : IStatusService
    {
        private string _currentStatus;
        public string CurrentStatus => _currentStatus;

        public event EventHandler<string> StatusChanged;

        public void SetStatus(string message)
        {
            _currentStatus = message;
            StatusChanged?.Invoke(this, message);
        }
    }
}
