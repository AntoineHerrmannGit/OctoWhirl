namespace OctoWhirl.TechnicalServices.Strategy.KPI
{
    public class StrategyRunKPIReport
    {
        public TimeSpan InitializationTime { get; set; }
        public TimeSpan RumTime { get; set; }
        public TimeSpan CloseTime { get; set; }
        public int NbOfEvents { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
