namespace OctoWhirl.Core.Models.Technicals
{
    public class NewtonZeroResult
    {
        public double Value { get; set; }   
        public int Iterations { get; set; }
        public bool HasConverged { get; set; }
        public double Distance { get; set; }
        public string Error { get; set; } = string.Empty;
    }
}
