using OctoWhirl.Core.Models.Models.Technicals;
using OctoWhirl.Core.Tools.Maths.Generators.Interfaces;
using OctoWhirl.Core.Tools.Technicals.Extensions;

namespace OctoWhirl.Core.Tools.Maths.Generators
{
    public class TimeSerieGenerator : ISimpleGenerator<TimeSerie<double>>
    {
        private readonly ISimpleGenerator<double> _generator;
        private readonly ResolutionInterval _interval;
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;

        public TimeSerieGenerator(ISimpleGenerator<double> generator, DateTime? startDate = null, DateTime? endDate = null, ResolutionInterval interval = ResolutionInterval.Day)
        {
            _generator = generator;
            _startDate = startDate.HasValue ? startDate.Value : DateTime.Now;
            _endDate = endDate ?? _startDate.AddYears(1);
            _interval = interval;
        }

        public TimeSerie<double> GetNext()
        {
            var result = new TimeSerie<double>();
            var intervalAsSeconds = _interval.AsTimeSpan().TotalSeconds;
            for (var date = _startDate; date <= _endDate; date = date.AddSeconds(intervalAsSeconds))
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                    result[date] = _generator.GetNext();

            return result;
        }

        public void Reset()
        {
            _generator.Reset();
        }
    }
}
