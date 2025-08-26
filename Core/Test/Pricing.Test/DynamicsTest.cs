using OctoWhirl.Core.Models.Models.Technicals;
using OctoWhirl.Core.Pricing.Dynamics;
using OctoWhirl.Core.Tools.Technicals.Extensions;

namespace OctoWhirl.Core.Test.Pricing
{
    [TestClass]
    public class DynamicsTest
    {
        [TestMethod]
        public async Task BlackSholesDynamicsTest()
        {
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now.AddYears(1);
            ResolutionInterval interval = ResolutionInterval.Day;

            double rate = 0.04;
            double volatility = 0.25;

            IDynamics dynamics = new BlackSholesDynamics(
                startDate: startDate,
                endDate: endDate,
                interval: interval,

                rate: rate,
                volatility: volatility
            );

            var generatedPath = await dynamics.GeneratePath().ConfigureAwait(false);

            int expectedPoints = 260;

            Assert.IsNotNull(generatedPath, "Generation failed : path is null");
            Assert.IsTrue(generatedPath.IsNotEmpty(), "Generation failed : path is empty");
            Assert.IsTrue(generatedPath.Count == expectedPoints, $"Generation failed : path does not contain the right amount of points (got {generatedPath.Count} expected {expectedPoints}).");
        }

        [TestMethod]
        public async Task HestonDynamicsTest()
        {
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now.AddYears(1);
            ResolutionInterval interval = ResolutionInterval.Day;

            double rate = 0.04;
            double averageVolatility = 0.25;
            double volOfVol = 0.3;
            double spotVolCorrelation = 0.85;

            double volatilityReversionStrength = 1;
            double initialVolatility = 0.25;

            IDynamics dynamics = new HestonDynamics(
                startDate: startDate,
                endDate: endDate,
                interval: interval,

                rate: rate,
                averageVolatility: averageVolatility,

                volOfVol: volOfVol,
                spotVolCorrelation: spotVolCorrelation,
                volatilityReversionStrength: volatilityReversionStrength,

                initialVolatility: initialVolatility
            );

            var generatedPath = await dynamics.GeneratePath().ConfigureAwait(false);

            int expectedPoints = 260;

            Assert.IsNotNull(generatedPath, "Generation failed : path is null");
            Assert.IsTrue(generatedPath.IsNotEmpty(), "Generation failed : path is empty");
            Assert.IsTrue(generatedPath.Count == expectedPoints, $"Generation failed : path does not contain the right amount of points (got {generatedPath.Count} expected {expectedPoints}).");
        }
    }
}
