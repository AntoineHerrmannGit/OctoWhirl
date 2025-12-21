namespace Models.TechnicalModels.Exceptions
{
    public class WrongArgumentException : Exception
    {
        public WrongArgumentException()
            : base()
        {
        }

        public WrongArgumentException(string message) 
            : base(message) 
        { 
        }

        public WrongArgumentException(string message, Exception innerException) 
            : base(message, innerException) 
        {
        }
    }
}
