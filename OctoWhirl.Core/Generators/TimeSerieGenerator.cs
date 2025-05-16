using OctoWhirl.Core.Extensions;
using OctoWhirl.Core.Models.Technicals;

namespace OctoWhirl.Core.Generators
{
    public class TimeSerieGenerator : IGenerator<TimeSerie<double>>
    {
        private readonly IGenerator<double> _generator;
        private readonly ResolutionInterval _interval;
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;

        public TimeSerieGenerator(IGenerator<double> generator, DateTime? startDate = null, DateTime? endDate = null, ResolutionInterval interval = ResolutionInterval.Day)
        {
            _generator = generator;
            _startDate = startDate.HasValue ? startDate.Value : DateTime.Now;
            _endDate = endDate ?? _startDate.AddYears(1);
            _interval = interval;
        }

        public TimeSerie<double> GetNext()
        {
            var result = new TimeSerie<double>();
            var intervalAsMinutes = _interval.AsTimeSpan().TotalMinutes;
            for (var date = _startDate; date <= _endDate; date = date.AddSeconds(intervalAsMinutes))
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
