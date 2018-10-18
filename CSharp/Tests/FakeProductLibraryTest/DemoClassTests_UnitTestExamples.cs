using FakeDBLibrary;
using FakeProductLibrary;
using NUnit.Framework;
using System;

using PasswordChangeStatus = FakeProductLibrary.DemoClass.PasswordChangeStatus;


namespace FakeProductLibraryTest
{
    [TestFixture]
    public class DemoClassTests_UnitTestExamples
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
        public PasswordChangeStatus ChangePasswordTest(string newPassword)
        {
            // Arrange
            MockPanoptoDBDataContext mockedPanoptoDBDataContext = new MockPanoptoDBDataContext();
            DemoClass classUnderTest = new DemoClass(); // or new ExampleClass(mockedPanoptoDBDataContext);

            // Act, method under test
            PasswordChangeStatus result = classUnderTest.ChangePassword(mockedPanoptoDBDataContext, newPassword);

            // Assert
            // Expect SetPassword to be called 1 time only when result was a success
            int calledTimesExpected = (result == PasswordChangeStatus.Success) ? 1 : 0;
            Assert.That(mockedPanoptoDBDataContext.SubmitChanges_WasCalled, Is.EqualTo(calledTimesExpected),
                "SetPassword method was called {0} times!", mockedPanoptoDBDataContext.SubmitChanges_WasCalled);

            // Test case will assert based on ExpectedResult
            return result;
        }

        [Test, Description("Boundary test, null password.")]
        public void ChangePassword_Null_NullReferenceException()
        {
            // You probably wouldn't want your code to throw a NullReferenceException, but here's an example of how to
            //  test for expected exceptions
            Assert.That(() => ChangePasswordTest(null), Throws.TypeOf<NullReferenceException>());
        }

        private class MockPanoptoDBDataContext : IPanoptoDBDataContext
        {
            public int SubmitChanges_WasCalled = 0;

            #region IDisposable Support
            public void Dispose()
            {
                // dispose
            }
            #endregion

            public bool PasswordHistoryMatches(string passwordToCheck, int passwordHistoryMax)
            {
                // Method stub
                bool returnValue = false;
                // if password from password history test, mock a return value of true
                if (passwordToCheck == HistoryPassword)
                {
                    returnValue = true;
                }

                return returnValue;
            }

            public void SetPassword(string newPassword)
            {
                // Empty method stub
            }

            public void SubmitChanges()
            {
                // Empty method stub

                // Allow validating the mocked object's method was called
                SubmitChanges_WasCalled += 1;
            }
        }
    }
}
