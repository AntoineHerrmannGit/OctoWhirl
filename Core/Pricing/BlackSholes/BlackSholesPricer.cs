using OctoWhirl.Core.Models.Models.Common;
using OctoWhirl.Core.Models.Models.Enums;
using OctoWhirl.Core.Pricing.Models;
using OctoWhirl.Core.Pricing.Models.Interfaces;
using OctoWhirl.Core.Tools.Maths.Functions;
using OctoWhirl.Core.Tools.Technicals.Extensions;

namespace OctoWhirl.Core.Pricing.BlackSholes
{
    public class BlackSholesPricer
    {
        public BlackSholesPricingResponse Price(IBlackSholesOption option)
        {
            double price = PriceValue(option);
            Greek delta = Delta(option);
            Greek gamma = Gamma(option);
            Greek vega = Vega(option);
            Greek theta = Theta(option);
            Greek rho = Rho(option);

            return new BlackSholesPricingResponse
            {
                Option = option,
                Price = price,
                Greeks = new List<Greek> { delta, gamma, vega, theta, rho }
            };
        }

        private static double PriceValue(IBlackSholesOption option)
        {
            double timeToMaturity = option.TimeStamp.GetYearFraction(option.Maturity);
            double discountedStrike = option.Strike * Math.Exp(-option.Rate * timeToMaturity);

            double squaredVolatility = option.Volatility * option.Volatility;
            double normalizedVolatility = option.Volatility * Math.Sqrt(timeToMaturity);

            double d1 = (Math.Log(option.Spot / option.Strike) + (option.Rate + squaredVolatility / 2) * timeToMaturity) / normalizedVolatility;
            double d2 = d1 - normalizedVolatility;

            return option.OptionType switch
            {
                OptionType.Call => Functions.Cerf(d1) * option.Spot - Functions.Cerf(d2) * discountedStrike,
                OptionType.Put => Functions.Cerf(-d2) * discountedStrike - Functions.Cerf(-d1) * option.Spot,
                _ => throw new ArgumentOutOfRangeException(nameof(option.OptionType)),
            };
        }

        private static Greek Delta(IBlackSholesOption option)
        {
            double timeToMaturity = option.TimeStamp.GetYearFraction(option.Maturity);

            double squaredVolatility = option.Volatility * option.Volatility;
            double normalizedVolatility = option.Volatility * Math.Sqrt(timeToMaturity);

            double d1 = (Math.Log(option.Spot / option.Strike) + (option.Rate + squaredVolatility / 2) * timeToMaturity) / normalizedVolatility;

            double value = option.OptionType switch
            {
                OptionType.Call => Functions.Cerf(d1),
                OptionType.Put => -Functions.Cerf(-d1),
                _ => throw new ArgumentOutOfRangeException(nameof(option.OptionType)),
            };

            return new Greek { Value = value, GreekType = GreekEnum.Delta };
        }

        private static Greek Gamma(IBlackSholesOption option)
        {
            double timeToMaturity = option.TimeStamp.GetYearFraction(option.Maturity);

            double squaredVolatility = option.Volatility * option.Volatility;
            double normalizedVolatility = option.Volatility * Math.Sqrt(timeToMaturity);

            double d1 = (Math.Log(option.Spot / option.Strike) + (option.Rate + squaredVolatility / 2) * timeToMaturity) / normalizedVolatility;

            double value = Functions.Normal(d1) / (option.Spot * timeToMaturity);

            return new Greek { Value = value, GreekType = GreekEnum.Gamma };
        }

        private static Greek Vega(IBlackSholesOption option)
        {
            double timeToMaturity = option.TimeStamp.GetYearFraction(option.Maturity);

            double squaredVolatility = option.Volatility * option.Volatility;
            double normalizedVolatility = option.Volatility * Math.Sqrt(timeToMaturity);

            double d1 = (Math.Log(option.Spot / option.Strike) + (option.Rate + squaredVolatility / 2) * timeToMaturity) / normalizedVolatility;

            double value = Functions.Normal(d1) * (option.Spot * timeToMaturity);

            return new Greek { Value = value, GreekType = GreekEnum.Vega };
        }

        private static Greek Theta(IBlackSholesOption option)
        {
            double timeToMaturity = option.TimeStamp.GetYearFraction(option.Maturity);
            double discountedStrike = option.Strike * Math.Exp(-option.Rate * timeToMaturity);

            double squaredVolatility = option.Volatility * option.Volatility;
            double normalizedVolatility = option.Volatility * Math.Sqrt(timeToMaturity);

            double d1 = (Math.Log(option.Spot / option.Strike) + (option.Rate + squaredVolatility / 2) * timeToMaturity) / normalizedVolatility;
            double d2 = d1 - normalizedVolatility;

            double discountedStrikeCoupon = option.Rate * discountedStrike;
            double normalizedSpotRisk = option.Spot * option.Volatility * Functions.Normal(d1) / (2 * timeToMaturity);

            double value = option.OptionType switch
            {
                OptionType.Call => -normalizedSpotRisk - discountedStrikeCoupon * Functions.Cerf(d2),
                OptionType.Put => -normalizedSpotRisk + discountedStrikeCoupon * Functions.Cerf(-d2),
                _ => throw new ArgumentOutOfRangeException(nameof(option.OptionType)),
            };

            return new Greek { Value = value, GreekType = GreekEnum.Theta };
        }

        private static Greek Rho(IBlackSholesOption option)
        {
            double timeToMaturity = option.TimeStamp.GetYearFraction(option.Maturity);
            double discountedStrike = option.Strike * Math.Exp(-option.Rate * timeToMaturity);

            double squaredVolatility = option.Volatility * option.Volatility;
            double normalizedVolatility = option.Volatility * Math.Sqrt(timeToMaturity);

            double d1 = (Math.Log(option.Spot / option.Strike) + (option.Rate + squaredVolatility / 2) * timeToMaturity) / normalizedVolatility;
            double d2 = d1 - normalizedVolatility;

            double discountedStrikeReturn = timeToMaturity * discountedStrike;

            double value = option.OptionType switch
            {
                OptionType.Call => discountedStrikeReturn * Functions.Cerf(d2),
                OptionType.Put => -discountedStrikeReturn * Functions.Cerf(-d2),
                _ => throw new ArgumentOutOfRangeException(nameof(option.OptionType)),
            };

            return new Greek { Value = value, GreekType = GreekEnum.Rho };
        }
    }
}
