using FakeDBLibrary;
using FakeProductLibrary;
using Moq;
using NUnit.Framework;
using System;

using PasswordChangeStatus = FakeProductLibrary.DemoClass.PasswordChangeStatus;


namespace FakeProductLibraryTest
{
    [TestFixture]
    public class DemoClassTests_MoqTestExamples
    {
        const string HistoryPassword = "MatchesLast5!";

        [Test]
        // NUnit Parameterized Test Cases, try same test with different inputs and expected results
        [TestCase("GoodPassw0rd!", ExpectedResult = PasswordChangeStatus.Success,
            Description = "Happy path, meets all requirements.")]
        [TestCase("N0Special", ExpectedResult = PasswordChangeStatus.Success,
            Description = "Allowed path, meets all requirements except one (a special character).")]
        [TestCase("n0capital!", ExpectedResult = PasswordChangeStatus.Success,
            Description = "Allowed path, meets all requirements except one (a capitalized character).")]
        [TestCase("NoNumber!", ExpectedResult = PasswordChangeStatus.Success,
            Description = "Allowed path, meets all requirements except one (a number character).")]
        [TestCase("Sh0rt!", ExpectedResult = PasswordChangeStatus.LengthRequirement,
            Description = "Negative test, does not meet length and only length requirement.")]
        [TestCase("no special no number no capital",
            ExpectedResult =
                PasswordChangeStatus.SpecialCharacterRequirement |
                PasswordChangeStatus.NumberRequirement |
                PasswordChangeStatus.UpperCaseLetterRequirement,
            Description = "Negative test, missing more than one of the allowed missing requirements (special, capital, and number).")]
        [TestCase(HistoryPassword, ExpectedResult = PasswordChangeStatus.HistoryRequirement,
            Description = "Negative test, matches a previous password, which is not allowed.")]
        [TestCase("",
            ExpectedResult =
                PasswordChangeStatus.LengthRequirement |
                PasswordChangeStatus.UpperCaseLetterRequirement |
                PasswordChangeStatus.NumberRequirement |
                PasswordChangeStatus.SpecialCharacterRequirement,
            Description = "Boundary test, no/empty password.")]
        public PasswordChangeStatus ChangePasswordTest_WithMoq(string newPassword)
        {
            // Arrange
            Mock<IPanoptoDBDataContext> mockedPanoptoDBDataContext = new Mock<IPanoptoDBDataContext>();
            // Setup order matters when multiple matches possible
            mockedPanoptoDBDataContext.Setup(db => db.PasswordHistoryMatches(It.IsAny<string>(), It.IsAny<int>())).Returns(false);
            mockedPanoptoDBDataContext.Setup(db => db.PasswordHistoryMatches(HistoryPassword, It.IsAny<int>())).Returns(true);
            mockedPanoptoDBDataContext.Setup(db => db.SetPassword(It.IsAny<string>()));
            mockedPanoptoDBDataContext.Setup(db => db.SubmitChanges());
            DemoClass classUnderTest = new DemoClass(); // or new ExampleClass(mockedPanoptoDBDataContext);

            // Act, method under test
            PasswordChangeStatus result = classUnderTest.ChangePassword(mockedPanoptoDBDataContext.Object, newPassword);

            // Assert
            // Expect SetPassword to be called 1 time only when result was a success
            int calledTimesExpected = (result == PasswordChangeStatus.Success) ? 1 : 0;
            mockedPanoptoDBDataContext.Verify(db => db.SubmitChanges(), Times.Exactly(calledTimesExpected));

            // Test case will assert based on ExpectedResult
            return result;
        }

        [Test, Description("Boundary test, null password.")]
        public void ChangePassword_WithMoq_Null_NullReferenceException()
        {
            // You probably wouldn't want your code to throw a NullReferenceException, but here's an example of how to
            //  test for expected exceptions
            Assert.That(() => ChangePasswordTest_WithMoq(null), Throws.TypeOf<NullReferenceException>());
        }
    }
}
