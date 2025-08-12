namespace OctoWhirl.Core.Exceptions
{
    public class DeserializationException : Exception
    {
        public DeserializationException() : base("Failed to deserialize JSON response")
        {
        }

        public DeserializationException(string message) : base(message)
        {
        }

        public DeserializationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public DeserializationException(Type expectedType) 
            : base($"Failed to deserialize JSON response to type {expectedType.Name}")
        {
        }

        public DeserializationException(Type expectedType, string jsonContent) 
            : base($"Failed to deserialize JSON response to type {expectedType.Name}. JSON content: {jsonContent}")
        {
        }
    }
}
