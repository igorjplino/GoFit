using FluentValidation.TestHelper;
using GoFit.Application.EntitiesActions.Profile.Commands;
using GoFit.Application.EntitiesActions.Profile.Validators;

namespace GoFit.Api.UnitTests.Validators.Profile;

public class UpdateMyProfileCommandValidatorTests
{
    private const string ValidAppUserId = "8f1c9d2e-4b7a-4f31-9c65-0d4b2a6e7f10";

    private readonly UpdateMyProfileCommandValidator _validator;

    public UpdateMyProfileCommandValidatorTests()
    {
        _validator = new UpdateMyProfileCommandValidator();
    }

    [Fact]
    [Trait("Profile", "Name")]
    public async Task WhenNameIsNull_ShouldFail()
    {
        var command = new UpdateMyProfileCommand(Name: null!, AppUserId: ValidAppUserId);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    [Trait("Profile", "Name")]
    public async Task WhenNameIsEmpty_ShouldFail()
    {
        var command = new UpdateMyProfileCommand(Name: string.Empty, AppUserId: ValidAppUserId);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    [Trait("Profile", "Name")]
    public async Task WhenNameHasLessThan3Chars_ShouldFail()
    {
        var command = new UpdateMyProfileCommand(Name: "ab", AppUserId: ValidAppUserId);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    [Trait("Profile", "Name")]
    public async Task WhenNameHasMoreThan200Chars_ShouldFail()
    {
        var command = new UpdateMyProfileCommand(Name: new string('a', 201), AppUserId: ValidAppUserId);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    [Trait("Profile", "Name")]
    public async Task WhenNameHasExactly200Chars_ShouldNotFail()
    {
        var command = new UpdateMyProfileCommand(Name: new string('a', 200), AppUserId: ValidAppUserId);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    [Trait("Profile", "Name")]
    public async Task WhenNameIsCorrect_ShouldNotFail()
    {
        var command = new UpdateMyProfileCommand(Name: "Igor Lino", AppUserId: ValidAppUserId);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    [Trait("Profile", "Ownership")]
    public async Task WhenAppUserIdIsEmpty_ShouldFail()
    {
        var command = new UpdateMyProfileCommand(Name: "Igor Lino", AppUserId: string.Empty);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.AppUserId);
    }

    [Fact]
    [Trait("Profile", "Ownership")]
    public async Task WhenAppUserIdIsProvided_ShouldNotFail()
    {
        var command = new UpdateMyProfileCommand(Name: "Igor Lino", AppUserId: ValidAppUserId);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(x => x.AppUserId);
    }
}
