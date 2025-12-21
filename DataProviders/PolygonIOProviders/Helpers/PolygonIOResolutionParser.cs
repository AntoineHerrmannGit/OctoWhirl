using Models.Models.Enums;
using System.Text.RegularExpressions;

namespace PolygonIOProviders.Helpers
{
    internal static class PolygonIOResolutionParser
    {
        public static string GetInterval(this ResolutionInterval interval)
        {
            var regex = "^([a-zA-Z]*)([0-9]*)";
            var timelaps = Regex.Matches(interval.ToString(), regex).FirstOrDefault()?.Value.ToLower();
            if (timelaps is null)
                throw new ArgumentException($"Failed parse \"{interval}\" as polygon interval regex.");
            return timelaps;
        }

        public static int GetAmplitude(this ResolutionInterval interval)
        {
            var regex = "^([a-zA-Z]*)([0-9]*)";
            var timelaps = Regex.Matches(interval.ToString(), regex).LastOrDefault()?.Value;
            if (timelaps is null)
                throw new ArgumentException($"Failed parse \"{interval}\" as polygon amplitude regex.");
            return int.TryParse(timelaps, out var amplitude) ? amplitude : 1;
        }
    }
}
