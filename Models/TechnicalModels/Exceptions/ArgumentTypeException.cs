namespace Models.TechnicalModels.Exceptions
{
    public class ArgumentTypeException : Exception
    {
        private const string _message = "Wrong argument type";

        public ArgumentTypeException() 
            : base(_message) 
        {
        }

        public ArgumentTypeException(string message) 
            : base($"{_message} : {message}")
        {
        }

        public ArgumentTypeException(string message, Exception exception) 
            : base($"{_message} : {message}", exception)
        {
        }

        public ArgumentTypeException(Type objectType)
            : base($"{_message} : {objectType.Name}")
        {
        }

        public ArgumentTypeException(Type objectType, Type expectedType)
            : base($"{_message} : got {objectType.Name}, expected {expectedType.Name}")
        {
        }
    }
}
