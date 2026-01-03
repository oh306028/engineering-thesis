using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Thesis.app.Dtos.Account;
using Thesis.data;
using Thesis.data.Data;
using Xunit;

public static class DbContextHelper
{
    public static AppDbContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        return new AppDbContext(options);
    }
}

public class AccountRegisterModelValidatorTests
{
    private AccountRegisterModel CreateValidModel()
        => new()
        {
            ParentFirstName = "Jan",
            ParentLastName = "Kowalski",
            StudentFirstName = "Adam",
            StudentLastName = "Kowalski",
            Email = "test@test.pl",
            Login = "login123",
            Password = "password",
            ConfirmPassword = "password",
            StudentDateOfBirth = DateTime.Now.AddYears(-10)
        };

    [Fact]
    public void Should_Have_Error_When_Email_Already_Exists()
    {
        var context = DbContextHelper.CreateContext(nameof(Should_Have_Error_When_Email_Already_Exists));

        context.Users.Add(new User { Email = "test@test.pl" });
        context.SaveChanges();

        var validator = new AccountRegisterModelValidator(context);
        var model = CreateValidModel();

        var result = validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Email)
              .WithErrorMessage("Istnieje już taki email");
    }

    [Fact]
    public void Should_Have_Error_When_Login_Already_Exists()
    {
        var context = DbContextHelper.CreateContext(nameof(Should_Have_Error_When_Login_Already_Exists));

        context.Users.Add(new User { Login = "login123" });
        context.SaveChanges();

        var validator = new AccountRegisterModelValidator(context);
        var model = CreateValidModel();

        var result = validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Login)
              .WithErrorMessage("Istnieje już taki login");
    }

    [Fact]
    public void Should_Have_Error_When_Passwords_Do_Not_Match()
    {
        var context = DbContextHelper.CreateContext(nameof(Should_Have_Error_When_Passwords_Do_Not_Match));
        var validator = new AccountRegisterModelValidator(context);

        var model = CreateValidModel();
        model.ConfirmPassword = "different";

        var result = validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Password)
              .WithErrorMessage("Hasła muszą się zgadzać");
    }

    [Fact]
    public void Should_Not_Have_Errors_When_Model_Is_Valid()
    {
        var context = DbContextHelper.CreateContext(nameof(Should_Not_Have_Errors_When_Model_Is_Valid));
        var validator = new AccountRegisterModelValidator(context);
        var model = CreateValidModel();

        var result = validator.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
