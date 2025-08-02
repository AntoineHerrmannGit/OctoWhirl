namespace OctoWhirl.Exceptions
{
    public class ArgumentNullOrDefaultException : Exception
    {
        public ArgumentNullOrDefaultException() { }

        public ArgumentNullOrDefaultException(string message) : base(message) { }
    }
}
