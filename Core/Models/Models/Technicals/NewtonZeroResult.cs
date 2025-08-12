namespace OctoWhirl.Core.Models.Technicals
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. 

{
    public class NewtonZeroResult
    {
        public double Value { get; set; }   
        public int Iterations { get; set; }
        public bool HasConverged { get; set; }
        public double Distance { get; set; }
        public string Error { get; set; }
    }
}
