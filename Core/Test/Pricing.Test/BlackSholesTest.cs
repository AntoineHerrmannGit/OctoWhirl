using Microsoft.Extensions.DependencyInjection;
using OctoWhirl.Core.Models.Models.Common;
using OctoWhirl.Core.Models.Models.Enums;
using OctoWhirl.Core.Pricing.BlackSholes;
using OctoWhirl.Core.Pricing.Models;

namespace OctoWhirl.Core.Test.Pricing
{
    [TestClass]
    public sealed class BlackSholesTest
    {
        private IServiceProvider _provider;

        [TestInitialize]
        public void Setup()
        {
            var services = new ServiceCollection();

            // Register Services
            services.AddTransient<BlackSholesPricer>();

            // Build Dependency-Injection
            _provider = services.BuildServiceProvider();
        }

        [TestMethod]
        public void VanillaPricingTest()
        {
            var option = new BlackSholesVanillaOption
            {
                Reference = "VanillaTest",
                Strike = 100,
                Maturity = DateTime.Now.AddYears(1),
                OptionType = OptionType.Call,
                Underlying = "UnderlyingTest",
                Spot = 100,
                TimeStamp = DateTime.Now,
                Rate = 0.04,
                Volatility = 0.25
            };

            var pricer = _provider.GetService<BlackSholesPricer>();
            var pricingResult = pricer.Price(option);

            Assert.IsNotNull(pricingResult);

            var price = pricingResult.Price;
            var delta = pricingResult.Greeks.First(greek => greek.GreekType == GreekEnum.Delta).Value;
            var gamma = pricingResult.Greeks.First(greek => greek.GreekType == GreekEnum.Gamma).Value;
            var theta = pricingResult.Greeks.First(greek => greek.GreekType == GreekEnum.Theta).Value;
            var vega = pricingResult.Greeks.First(greek => greek.GreekType == GreekEnum.Vega).Value;
            var rho = pricingResult.Greeks.First(greek => greek.GreekType == GreekEnum.Rho).Value;

            var expectedPrice = 8.9442725581052258;
            var expectedDelta = 0.57932172757260536;
            var expectedGamma = 0.003830648307635986;
            var expectedTheta = -6.7478263925111941;
            var expectedVega = 38.306483076234016;
            var expectedRho = 48.987900199074843;

            Assert.AreEqual(price, expectedPrice, 1e-10, $"Pricing value failed : got {price} expected {expectedPrice}");
            Assert.AreEqual(delta, expectedDelta, 1e-10, $"Pricing delta failed : got {delta} expected {expectedDelta}");
            Assert.AreEqual(gamma, expectedGamma, 1e-10, $"Pricing gamma failed : got {gamma} expected {expectedGamma}");
            Assert.AreEqual(theta, expectedTheta, 1e-10, $"Pricing theta failed : got {theta} expected {expectedTheta}");
            Assert.AreEqual(vega, expectedVega, 1e-10, $"Pricing vega failed : got {vega} expected {expectedVega}");
            Assert.AreEqual(rho, expectedRho, 1e-10, $"Pricing rho failed : got {rho} expected {expectedRho}");
        }
    }
}
