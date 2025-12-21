namespace MathModels.Exceptions
{
    public class NotEnoughPointsExceptions : Exception
    {
        public NotEnoughPointsExceptions(int minNbOfPoints)
            : base($"Expected at least {minNbOfPoints} points.")
        {
        }

        public NotEnoughPointsExceptions(int minNbOfPoints, string message)
            : base($"Expected at least {minNbOfPoints} points ({message}).")
        {
        }

        public NotEnoughPointsExceptions(int minNbOfPoints, string message, Exception? innerExceptions)
            : base($"Expected at least {minNbOfPoints} points ({message}).", innerExceptions)
        {
        }
    }
}
