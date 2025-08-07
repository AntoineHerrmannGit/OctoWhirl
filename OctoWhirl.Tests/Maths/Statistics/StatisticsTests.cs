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
        public void Correlation_BothConstantValues_ReturnsOne()
        {
            // Arrange
            var series1 = new List<double> { 5, 5, 5, 5, 5 };
            var series2 = new List<double> { 3, 3, 3, 3, 3 };

            // Act
            double result = OctoWhirl.Maths.Statistics.Statistics.Correlation(series1, series2);

            // Assert
            Assert.That(result, Is.EqualTo(1.0).Within(1e-10),
                "Both flat series should return 1.0 (Dirac delta distributions)");
        }

        [Test]
        public void Correlation_OneConstantOneVariable_ReturnsZero()
        {
            // Arrange
            var series1 = new List<double> { 5, 5, 5, 5, 5 };
            var series2 = new List<double> { 1, 2, 3, 4, 5 };

            // Act
            double result = OctoWhirl.Maths.Statistics.Statistics.Correlation(series1, series2);

            // Assert
            Assert.That(result, Is.EqualTo(0.0).Within(1e-10),
                "One flat series should return 0.0 (total decorrelation)");
        }

        [Test]
        public void Correlation_WithLambdaAdjustment_AdjustsCorrectly()
        {
            // Lambda = 0 (pas d'ajustement)
            var s1 = new List<double> { 1, 2, 3, 4, 5 };
            var s2 = new List<double> { 2, 4, 6, 8, 10 };
            double resultZero = OctoWhirl.Maths.Statistics.Statistics.Correlation(s1, s2, 0);
            Assert.That(resultZero, Is.EqualTo(1.0).Within(1e-10), "Lambda=0 should not adjust correlation");
        }

        [Test]
        public void Correlation_LambdaPositive_IncreasesCorrelation()
        {
            var s1 = new List<double> { 1, 2, 3, 4, 5 };
            var s2 = new List<double> { 2, 4, 6, 8, 9 }; // corr < 1
            double corrBrut = OctoWhirl.Maths.Statistics.Statistics.Correlation(s1, s2, 0);
            double corrAdjPos = OctoWhirl.Maths.Statistics.Statistics.Correlation(s1, s2, 0.5);
            Assert.That(corrAdjPos, Is.GreaterThan(corrBrut), "Lambda>0 should increase correlation towards +1");
            Assert.That(corrAdjPos, Is.LessThanOrEqualTo(1.0), "Correlation should not exceed 1.0");
        }

        [Test]
        public void Correlation_LambdaNegative_DecreasesCorrelation()
        {
            var s1 = new List<double> { 1, 2, 3, 4, 5 };
            var s2 = new List<double> { 2, 4, 6, 8, 9 }; // corr > -1
            double corrBrut = OctoWhirl.Maths.Statistics.Statistics.Correlation(s1, s2, 0);
            double corrAdjNeg = OctoWhirl.Maths.Statistics.Statistics.Correlation(s1, s2, -0.5);
            Assert.That(corrAdjNeg, Is.LessThan(corrBrut), "Lambda<0 should decrease correlation towards -1");
            Assert.That(corrAdjNeg, Is.GreaterThanOrEqualTo(-1.0), "Correlation should not be less than -1.0");
        }

        [Test]
        public void Correlation_LambdaPositive_PerfectCorrelation_RemainsOne()
        {
            var s1 = new List<double> { 1, 2, 3, 4, 5 };
            var s2 = new List<double> { 2, 4, 6, 8, 10 };
            double corrAdjPerfPos = OctoWhirl.Maths.Statistics.Statistics.Correlation(s1, s2, 0.5);
            Assert.That(corrAdjPerfPos, Is.EqualTo(1.0).Within(1e-10), "Perfect correlation stays at 1 even with lambda>0");
        }

        [Test]
        public void Correlation_LambdaNegative_PerfectNegativeCorrelation_RemainsMinusOne()
        {
            var s1 = new List<double> { 1, 2, 3, 4, 5 };
            var s2 = new List<double> { 5, 4, 3, 2, 1 };
            double corrAdjPerfNeg = OctoWhirl.Maths.Statistics.Statistics.Correlation(s1, s2, -0.5);
            Assert.That(corrAdjPerfNeg, Is.EqualTo(-1.0).Within(1e-10), "Perfect negative correlation stays at -1 even with lambda<0");
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