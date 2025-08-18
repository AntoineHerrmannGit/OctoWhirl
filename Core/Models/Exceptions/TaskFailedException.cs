namespace OctoWhirl.Core.Models.Exceptions
{
    public class TaskFailedException : Exception
    {
        public TaskFailedException() { }

        public TaskFailedException(string message) : base($"Task failed with message : {message}") { }

        public TaskFailedException(Exception exception) : base($"Task failed with exception : {exception.GetType().Name} : {exception.Message}") { }
    }
}
