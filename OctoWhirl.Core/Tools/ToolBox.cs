namespace OctoWhirl.Core.Tools
{
    public static class ToolBox
    {
        public static double GetDaysInAYear(DateTime? now = null)
        {
            DateTime date = now ?? DateTime.Now;
            return (date.AddYears(1) - date).TotalDays;
        }


        public static double GetMinutesInAYear(DateTime? now = null)
        {
            DateTime date = now ?? DateTime.Now;
            return (date.AddYears(1) - date).TotalMinutes;
        }
    }
}
