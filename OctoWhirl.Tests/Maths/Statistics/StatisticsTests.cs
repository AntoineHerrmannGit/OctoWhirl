using NUnit.Framework;
using OctoWhirl.Core.Models.Exceptions;
using System;
using System.Collections.Generic;

namespace OctoWhirl.Tests.Maths.Statistics
{
    [TestFixture]
    public class StatisticsTests
    {
        [Test]
        public void Correlation_PerfectPositive_ReturnsOne()
        {
            // Arrange
            var series1 = new List<double> { 1, 2, 3, 4, 5 };
            var series2 = new List<double> { 2, 4, 6, 8, 10 };

            // Act
            double result = OctoWhirl.Maths.Statistics.Statistics.Correlation(series1, series2);

            // Assert
            Assert.That(result, Is.EqualTo(1.0).Within(1e-10), "Perfect positive correlation should return 1.0");
        }

        [Test]
        public void Correlation_PerfectNegative_ReturnsMinusOne()
        {
            // Arrange
            var series1 = new List<double> { 1, 2, 3, 4, 5 };
            var series2 = new List<double> { 5, 4, 3, 2, 1 };

            // Act
            double result = OctoWhirl.Maths.Statistics.Statistics.Correlation(series1, series2);

            // Assert
            Assert.That(result, Is.EqualTo(-1.0).Within(1e-10), "Perfect negative correlation should return -1.0");
        }

        [Test]
        public void Correlation_ConstantValues_ThrowsDivideByZero()
        {
            // Arrange
            var series1 = new List<double> { 5, 5, 5, 5, 5 };
            var series2 = new List<double> { 3, 3, 3, 3, 3 };

            // Act & Assert
            Assert.Throws<DivideByZeroException>(() => 
                OctoWhirl.Maths.Statistics.Statistics.Correlation(series1, series2), 
                "Constant series should throw DivideByZeroException");
        }

        [Test]
        public void Mean_ValidSeries_ReturnsCorrectMean()
        {
            // Arrange
            var series = new List<double> { 1, 2, 3, 4, 5 };
            double expected = 3.0;

            // Act
            double result = OctoWhirl.Maths.Statistics.Statistics.Mean(series);

            // Assert
            Assert.That(result, Is.EqualTo(expected).Within(1e-10), "Mean of 1,2,3,4,5 should be 3.0");
        }

        [Test]
        public void Variance_ValidSeries_ReturnsCorrectVariance()
        {
            // Arrange
            var series = new List<double> { 1, 2, 3, 4, 5 };
            double expected = 2.0;

            // Act
            double result = OctoWhirl.Maths.Statistics.Statistics.Variance(series);

            // Assert
            Assert.That(result, Is.EqualTo(expected).Within(1e-10), "Variance of 1,2,3,4,5 should be 2.0");
        }
    }
}
