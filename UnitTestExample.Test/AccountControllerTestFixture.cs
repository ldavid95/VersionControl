using System;

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
    }
}
