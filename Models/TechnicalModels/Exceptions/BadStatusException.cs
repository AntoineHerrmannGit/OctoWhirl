namespace Models.TechnicalModels.Exceptions
{
    public class BadStatusException : Exception
    {
        public BadStatusException() { }

        public BadStatusException(string message) : base(message) { }
    }
}
