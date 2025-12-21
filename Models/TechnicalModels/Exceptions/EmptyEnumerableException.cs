namespace Models.TechnicalModels.Exceptions
{
    public class EmptyEnumerableException : Exception
    {
        public EmptyEnumerableException() { }

        public EmptyEnumerableException(string message) : base($"Empty enumerable : {message}") { }
    }
}
