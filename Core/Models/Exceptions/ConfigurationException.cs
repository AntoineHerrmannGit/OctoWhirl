namespace OctoWhirl.Core.Models.Exceptions
{
    public class ConfigurationException : Exception
    {
        public ConfigurationException() { }

        public ConfigurationException(string message) : base($"Invalid Configuration: {message}") { }
    }
}
