# TestExamples
A repository with demo-able example tests, unit tests using NUnit and Moq, and other samples.

## Contents
The solution and CSharp\Tests contains a Unit Test demo project (FakeProductLibraryTest). This Unit Test project tests the FakeProductLibrary project (which has a dependency FakeDBLibrary).

### Basic Unit Test Examples (Calculator)
In the FakeProductLibraryTest project you can find a CalculatorTests class which contains the most basic example of a Unit Test in action that tests the FakeProductLibrary.Calculator.Power(base, exponent) method.

### Demo Unit Test Examples (FakeProductLibrary which depends on a fake PanoptoDBDataContext)
FakeProductLibraryTest also contains a DemoClassTests_UnitTestExamples class which provides a more complex unit test which requires mocking/stubbing. This class shows how stubbing and mocking works in its most raw form, without using any mocking frameworks.

DemoClassTests_MoqTestExamples is the same set of tests showing how to acheive the same mocking/stubbing concept described with a mocking framework called Moq.
