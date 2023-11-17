using FluentValidation.TestHelper;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Dtos;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Validators;

namespace GoFit.Api.UnitTests.Validators.WorkoutsTracking;
public class WorkoutSetTrackingDtoValidatorTests
{
    private readonly WorkoutSetTrackingDtoValidator _validator;

    public WorkoutSetTrackingDtoValidatorTests()
    {
        _validator = new WorkoutSetTrackingDtoValidator();
    }

    [Fact]
    [Trait("WorkoutSetTrackingDto", "Note")]
    public void WhenIsEqualOrLessThan0Chars_ShouldFail()
    {
        var dto = new WorkoutSetTrackingDto
        {
            Repetitions = 0
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Repetitions);
    }

    [Fact]
    [Trait("WorkoutSetTrackingDto", "Note")]
    public void WhenIsGreaterThan0Chars_ShouldNotFail()
    {
        var dto = new WorkoutSetTrackingDto
        {
            Repetitions = 1
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Repetitions);
    }
}
