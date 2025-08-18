using Microsoft.Extensions.Configuration;
using OctoWhirl.Core.Models.Exceptions;

namespace OctoWhirl.Core.Tools.Technicals.Extensions
{
    public static class IConfigurationExtension
    {
        public static T GetForSure<T>(this IConfiguration configuration, string section)
        {
            return configuration.GetRequiredSection(section).Get<T>() ?? throw new MissingSectionException();
        }
    }
}
