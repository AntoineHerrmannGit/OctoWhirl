namespace MathModels.Optimizers
{
    public class GradientResult
    {
        public double Value { get; set; }
        public double DerivativeResidue { get; set; }
        public string? Error { get; set; }
        public int Iterations { get; set; }
        public bool HasConverged => Error is null;
    }
}
