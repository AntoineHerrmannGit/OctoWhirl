using MathModels.Objects.Series;
using OctoWhirl.Core.Tools.Maths.Generators.Interfaces;

namespace OctoWhirl.Core.Tools.Maths.Generators
{
    public class TimeSerieGenerator : ISimpleGenerator<TimeSerie<double>>
    {
        private readonly ISimpleGenerator<double> _generator;
        private readonly TimeSpan _interval;
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;

        public TimeSerieGenerator(ISimpleGenerator<double> generator, DateTime? startDate = null, DateTime? endDate = null, TimeSpan? interval = null)
        {
            _generator = generator;
            _startDate = startDate.HasValue ? startDate.Value : DateTime.Now;
            _endDate = endDate ?? _startDate.AddYears(1);
            _interval = interval ?? TimeSpan.FromDays(1);
        }

        public TimeSerie<double> GetNext()
        {
            var result = new TimeSerie<double>();
            var intervalAsMinutes = _interval.TotalMinutes;
            for (var date = _startDate; date <= _endDate; date = date.AddMinutes(intervalAsMinutes))
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
