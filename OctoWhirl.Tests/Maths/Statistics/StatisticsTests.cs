using NUnit.Framework;
using OctoWhirl.Core.Maths;
using OctoWhirl.Core.Models.Exceptions;
using System;
using System.Collections.Generic;

namespace OctoWhirl.Tests.Maths
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
            double result = Statistics.Correlation(series1, series2);

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
            double result = Statistics.Correlation(series1, series2);

            // Assert
            Assert.That(result, Is.EqualTo(-1.0).Within(1e-10), "Perfect negative correlation should return -1.0");
        }

        [Test]
        public void Correlation_WithLambdaPositive_IncreasesCorrelation()
        {
            var s1 = new List<double> { 1, 2, 3, 4, 5 };
            var s2 = new List<double> { 2, 4, 6, 8, 9 }; // corr < 1
            double corrBrut = Statistics.Correlation(s1, s2, 0);
            double corrAdjPos = Statistics.Correlation(s1, s2, 0.5);
            Assert.That(corrAdjPos, Is.GreaterThan(corrBrut), "Lambda>0 should increase correlation towards +1");
            Assert.That(corrAdjPos, Is.LessThanOrEqualTo(1.0), "Correlation should not exceed 1.0");
        }

        [Test]
        public void Mean_ValidSeries_ReturnsCorrectMean()
        {
            // Arrange
            var series = new List<double> { 1, 2, 3, 4, 5 };
            double expected = 3.0;

            // Act
            double result = Statistics.Mean(series);

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
            double result = Statistics.Variance(series);

            // Assert
            Assert.That(result, Is.EqualTo(expected).Within(1e-10), "Variance of 1,2,3,4,5 should be 2.0");
        }

        [Test]
        public void StdDev_ValidSeries_ReturnsCorrectStdDev()
        {
            // Arrange
            var series = new List<double> { 1, 2, 3, 4, 5 };
            double expected = Math.Sqrt(2.0);

            // Act
            double result = Statistics.StdDev(series);

            // Assert
            Assert.That(result, Is.EqualTo(expected).Within(1e-10), "StdDev of 1,2,3,4,5 should be sqrt(2.0)");
        }

        [Test]
        public void Covariance_ValidSeries_ReturnsCorrectCovariance()
        {
            // Arrange
            var series1 = new List<double> { 1, 2, 3, 4, 5 };
            var series2 = new List<double> { 2, 4, 6, 8, 10 };
            double expected = 4.0; // Covariance of perfectly correlated series

            // Act
            double result = Statistics.Covariance(series1, series2);

            // Assert
            Assert.That(result, Is.EqualTo(expected).Within(1e-10), "Covariance should be 4.0");
        }

        [Test]
        public void Correlation_EmptySeries_ThrowsArgumentException()
        {
            // Arrange
            var empty = new List<double>();
            var series = new List<double> { 1, 2, 3 };

            // Act & Assert
            Assert.Throws<EmptyEnumerableException>(() => Statistics.Correlation(empty, series));
            Assert.Throws<EmptyEnumerableException>(() => Statistics.Correlation(series, empty));
        }

        [Test]
        public void Variance_EmptySeries_ThrowsArgumentException()
        {
            // Arrange
            var empty = new List<double>();

            // Act & Assert
            Assert.Throws<EmptyEnumerableException>(() => Statistics.Variance(empty));
        }

        [Test]
        public void Moment_FirstOrder_ReturnsMean()
        {
            // Arrange
            var series = new List<double> { 1, 2, 3, 4, 5 };
            double expected = 3.0;

            // Act
            double result = Statistics.Moment(series, 1);

            // Assert
            Assert.That(result, Is.EqualTo(expected).Within(1e-10), "First moment should equal mean");
        }

        [Test]
        public void Moment_SecondOrder_ReturnsVariance()
        {
            // Arrange
            var series = new List<double> { 1, 2, 3, 4, 5 };
            double expected = 2.0;

            // Act
            double result = Statistics.Moment(series, 2);

            // Assert
            Assert.That(result, Is.EqualTo(expected).Within(1e-10), "Second moment should equal variance");
        }

        [Test]
        public void Moment_OrderZero_ThrowsArgumentException()
        {
            // Arrange
            var series = new List<double> { 1, 2, 3 };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => Statistics.Moment(series, 0));
        }

        [Test]
        public void Moment_EmptySeries_ThrowsArgumentException()
        {
            // Arrange
            var empty = new List<double>();

            // Act & Assert
            Assert.Throws<EmptyEnumerableException>(() => Statistics.Moment(empty, 3));
        }

        [Test]
        public void Skew_SymmetricSeries_ReturnsZero()
        {
            // Arrange
            var symmetricSeries = new List<double> { -2, -1, 0, 1, 2 };

            // Act
            double result = Statistics.Skew(symmetricSeries);

            // Assert
            Assert.That(Math.Abs(result), Is.LessThan(1e-10), "Perfectly symmetric series should have skewness near zero");
        }

        [Test]
        public void Skew_EmptySeries_ThrowsArgumentException()
        {
            // Arrange
            var empty = new List<double>();

            // Act & Assert
            Assert.Throws<EmptyEnumerableException>(() => Statistics.Skew(empty));
        }

        [Test]
        public void Kurtosis_NormalSeries_ReturnsExpectedValue()
        {
            // Arrange
            var series = new List<double> { 1, 2, 3, 4, 5 };

            // Act
            double result = Statistics.Kurtosis(series);

            // Assert
            Assert.That(result, Is.Not.NaN, "Kurtosis should return a valid number");
            Assert.That(result, Is.GreaterThan(-10), "Kurtosis should be reasonable");
            Assert.That(result, Is.LessThan(10), "Kurtosis should be reasonable");
        }

        [Test]
        public void Kurtosis_EmptySeries_ThrowsArgumentException()
        {
            // Arrange
            var empty = new List<double>();

            // Act & Assert
            Assert.Throws<EmptyEnumerableException>(() => Statistics.Kurtosis(empty));
        }
    }
}
