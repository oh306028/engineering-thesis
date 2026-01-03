using FluentValidation.TestHelper;
using Thesis.app.Dtos.Account;
using Xunit;

public class AccountLoginModelValidatorTests
{
    private readonly AccountLoginModelValidator _validator;

    public AccountLoginModelValidatorTests()
    {
        _validator = new AccountLoginModelValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Login_Is_Empty()
    {
        var model = new AccountLoginModel
        {
            Login = "",
            Password = "password123"
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Login)
              .WithErrorMessage("Pole jest wymagane");
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Empty()
    {
        var model = new AccountLoginModel
        {
            Login = "test",
            Password = ""
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Password)
              .WithErrorMessage("Pole jest wymagane");
    }

    [Fact]
    public void Should_Not_Have_Errors_When_Model_Is_Valid()
    {
        var model = new AccountLoginModel
        {
            Login = "test",
            Password = "password123"
        };

        var result = _validator.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Should_Have_Error_When_Login_Is_Null_Or_Empty(string login)
    {
        var model = new AccountLoginModel
        {
            Login = login,
            Password = "password123"
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Login);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Should_Have_Error_When_Password_Is_Null_Or_Empty(string password)
    {
        var model = new AccountLoginModel
        {
            Login = "test",
            Password = password
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
}
