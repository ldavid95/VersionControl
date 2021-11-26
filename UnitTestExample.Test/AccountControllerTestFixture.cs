using System;
using namespace NUnit.Framework;

public class AccountControllerTestFixture
{
    [
                Test,
                TestCase("abcd1234", false),
                TestCase("irf@uni-corvinus", false),
                TestCase("irf.uni-corvinus.hu", false),
                TestCase("irf@uni-corvinus.hu", true)
    ]
    public void TestValidateEmail(string email, bool expectedResult)
    {
        // Arrange
        var accountController = new AccountController();

        // Act
        var actualResult = accountController.ValidateEmail(email);

        // Assert
        Assert.AreEqual(expectedResult, actualResult);
    }
    public bool ValidatePassword(string password)
    {
        var LowerCase = new Regex(@"[a-z]+");
        var UpperCase = new Regex(@"[A-Z]+");
        var Number = new Regex(@"[0-9]+");
        var EightLong = new Regex(@".{8,}+");
        return LowerCase.IsMatch(password) && UpperCase.IsMatch(password) && Number.IsMatch(password) && EightLong.IsMatch(password);
    }
}