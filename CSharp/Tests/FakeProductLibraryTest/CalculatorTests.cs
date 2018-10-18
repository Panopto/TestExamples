using FakeProductLibrary;
using NUnit.Framework;
using System;

namespace FakeProductLibraryTest
{
    [TestFixture]
    public class CalculatorTests
    {
        [Test]
        [TestCase(2, 3, ExpectedResult = 8)]
        [TestCase(-2, 3, ExpectedResult = -8)]
        [TestCase(-3, 4, ExpectedResult = 81)]
        [TestCase(2, 1, ExpectedResult = 2)]
        [TestCase(2, 0, ExpectedResult = 1)]
        [TestCase(1, 5, ExpectedResult = 1)]
        [TestCase(2, -3, ExpectedResult = 0.125)]
        public int PowerTest(int x, int y)
        {
            // Arrage
            Calculator classUnderTest = new Calculator();

            // Act
            return classUnderTest.Power(x, y);

            // Assert, will occur based on ExpectedResult
        }

        [Test]
        public void Power_MaxIntTo2_OverflowException()
        {
            // Arrage
            Calculator classUnderTest = new Calculator();

            // Act and Assert
            Assert.That(() => classUnderTest.Power(int.MaxValue, 2), Throws.TypeOf<OverflowException>());
        }
    }
}
