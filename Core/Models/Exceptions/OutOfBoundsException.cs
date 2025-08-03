namespace OctoWhirl.Exceptions
{
    public class OutOfBoundsException : Exception
    {
        public OutOfBoundsException() { }

        public OutOfBoundsException(string message) : base($"Out of bounds : {message}") { }
    }
}
