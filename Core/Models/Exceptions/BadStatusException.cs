namespace OctoWhirl.Core.Models.Exceptions
{
    public class BadStatusException : Exception
    {
        public BadStatusException() { }

        public BadStatusException(string message) : base(message) { }
    }
}
