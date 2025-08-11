using NUnit.Framework;
using OctoWhirl.Core.Extensions;
using System.Collections.Generic;

namespace OctoWhirl.Tests.Extensions
{
    [TestFixture]
    public class EnumerableExtensionsTests
    {
        [Test]
        public void ExtensionMethods_DelegateCorrectly_SampleTest()
        {
            // Arrange
            var series1 = new List<double> { 1, 2, 3, 4, 5 };
            var series2 = new List<double> { 2, 4, 6, 8, 10 };

            // Act & Assert
            Assert.DoesNotThrow(() => series1.Mean(), "Mean extension should delegate successfully");
            Assert.DoesNotThrow(() => series1.Variance(), "Variance extension should delegate successfully");
            Assert.DoesNotThrow(() => series1.StdDev(), "StdDev extension should delegate successfully");
            Assert.DoesNotThrow(() => series1.Correlation(series2), "Correlation extension should delegate successfully");
            Assert.DoesNotThrow(() => series1.Correlation(series2, 0.1), "Correlation with lambda should delegate successfully");
            Assert.DoesNotThrow(() => series1.Covariance(series2), "Covariance extension should delegate successfully");
            Assert.DoesNotThrow(() => series1.Moment(3), "Moment extension should delegate successfully");
            Assert.DoesNotThrow(() => series1.Skew(), "Skew extension should delegate successfully");
            Assert.DoesNotThrow(() => series1.Kurtosis(), "Kurtosis extension should delegate successfully");
            
            double mean = series1.Mean();
            Assert.That(mean, Is.EqualTo(3.0).Within(1e-10), "Delegation should return correct results");
        }
    }
}
