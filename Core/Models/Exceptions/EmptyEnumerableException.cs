namespace OctoWhirl.Core.Models.Exceptions
{
    public class EmptyEnumerableException : Exception
    {
        public EmptyEnumerableException() { }

        public EmptyEnumerableException(string message) : base(message) { }
    }
}
