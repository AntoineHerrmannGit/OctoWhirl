namespace MathModels.Optimizers
{
    public class NewtonResult
    {
        public double Value { get; set; }
        public double Residue { get; set; }
        public string? Error { get; set; }
        public int Iterations { get; set; }
        public bool HasConverged => Error is null;
    }
}
