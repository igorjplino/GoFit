using FluentValidation.TestHelper;
using GoFit.Application.EntitiesActions.Athletes.Commands;
using GoFit.Application.EntitiesActions.Athletes.Validators;
using GoFit.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace GoFit.Api.UnitTests.Validators.Athletes;
public class RegisterAthleteCommandValidatorTests
{
    private const string TakenEmail = "taken@gofit.test";
    private const string FreeEmail = "new-athlete@gofit.test";

    private readonly RegisterAthleteCommandValidator _validator;

    private readonly Mock<UserManager<AppUser>> _userManagerMock;

    public RegisterAthleteCommandValidatorTests()
    {
        _userManagerMock = new Mock<UserManager<AppUser>>(
            Mock.Of<IUserStore<AppUser>>(), null!, null!, null!, null!, null!, null!, null!, null!);

        _userManagerMock
            .Setup(x => x.FindByEmailAsync(TakenEmail))
            .ReturnsAsync(new AppUser { Email = TakenEmail, DisplayName = "Existing Athlete" });

        _validator = new RegisterAthleteCommandValidator(_userManagerMock.Object);
    }

    [Fact]
    [Trait("RegisterAthlete", "Name")]
    public async Task WhenNameIsEmpty_ShouldFail()
    {
        var result = await _validator.TestValidateAsync(new RegisterAthleteCommand("", FreeEmail, "Pa$$w0rd"));

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    [Trait("RegisterAthlete", "Name")]
    public async Task WhenNameIsLongerThan200Chars_ShouldFail()
    {
        var result = await _validator.TestValidateAsync(new RegisterAthleteCommand(new string('a', 201), FreeEmail, "Pa$$w0rd"));

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    [Trait("RegisterAthlete", "Email")]
    public async Task WhenEmailIsInvalid_ShouldFailWithoutLookingUpTheAccount()
    {
        var result = await _validator.TestValidateAsync(new RegisterAthleteCommand("Athlete", "not-an-email", "Pa$$w0rd"));

        result.ShouldHaveValidationErrorFor(x => x.Email);
        _userManagerMock.Verify(x => x.FindByEmailAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    [Trait("RegisterAthlete", "Email")]
    public async Task WhenEmailIsAlreadyTaken_ShouldFail()
    {
        var result = await _validator.TestValidateAsync(new RegisterAthleteCommand("Athlete", TakenEmail, "Pa$$w0rd"));

        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Email is already taken");
    }

    [Fact]
    [Trait("RegisterAthlete", "Password")]
    public async Task WhenPasswordIsEmpty_ShouldFail()
    {
        var result = await _validator.TestValidateAsync(new RegisterAthleteCommand("Athlete", FreeEmail, ""));

        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    [Trait("RegisterAthlete", "Email")]
    public async Task WhenAllFieldsAreValid_ShouldNotFail()
    {
        var result = await _validator.TestValidateAsync(new RegisterAthleteCommand("Athlete", FreeEmail, "Pa$$w0rd"));

        result.ShouldNotHaveAnyValidationErrors();
    }
}
