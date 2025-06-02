namespace OctoWhirl.Core.Exceptions
{
    public class MissingSectionException : ConfigurationException
    {
        public MissingSectionException() { }

        public MissingSectionException(string message) : base($"Missing Section: {message}") { }
    }
}
