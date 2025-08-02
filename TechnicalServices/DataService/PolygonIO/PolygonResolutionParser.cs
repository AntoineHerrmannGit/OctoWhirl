using OctoWhirl.Core.Extensions;
using OctoWhirl.Core.Models.Technicals;
using System.Text.RegularExpressions;
using Technicals.Extensions;

namespace OctoWhirl.TechnicalServices.DataService.PolygonIO
{
    public static class PolygonResolutionParser
    {
        public static string GetInterval(this ResolutionInterval interval)
        {
            var regex = "^([a-zA-Z]*)([0-9]*)";
            var timelaps = Regex.Matches(interval.ToString(), regex).FirstOrDefault()?.Value.ToLower();
            if (timelaps.IsNull())
                throw new ArgumentException($"Failed parse \"{interval}\" as polygon interval regex.");
            return timelaps;
        }

        public static int GetAmplitude(this ResolutionInterval interval)
        {
            var regex = "^([a-zA-Z]*)([0-9]*)";
            var timelaps = Regex.Matches(interval.ToString(), regex).LastOrDefault()?.Value;
            if (timelaps.IsNull())
                throw new ArgumentException($"Failed parse \"{interval}\" as polygon amplitude regex.");
            return int.TryParse(timelaps, out var amplitude) ? amplitude : 1;
        }
    }
}
