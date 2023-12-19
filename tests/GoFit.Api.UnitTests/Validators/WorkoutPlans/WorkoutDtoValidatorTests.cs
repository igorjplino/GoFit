using FluentValidation.TestHelper;
using GoFit.Application.EntitiesActions.WorkoutPlans.Dtos;
using GoFit.Application.EntitiesActions.WorkoutPlans.Validators;
using GoFit.Application.Interfaces;

namespace GoFit.Api.UnitTests.Validators.WorkoutPlans;

public class WorkoutDtoValidatorTests
{
    private readonly WorkoutDtoValidator _validator;

    private readonly MockRepository _mockRepository;
    private readonly Mock<IExerciseRepository> _exerciseRepositoryMock;

    public WorkoutDtoValidatorTests()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _exerciseRepositoryMock = _mockRepository.Create<IExerciseRepository>();

        _validator = new WorkoutDtoValidator(_exerciseRepositoryMock.Object);
    }

    [Fact]
    [Trait("WorkoutDto", "Name")]
    public async Task WhenNameIsNull_ShouldFail()
    {
        var dto = new WorkoutDto
        {
            Name = null
        };

        var result = await _validator.TestValidateAsync(dto);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    [Trait("WorkoutDto", "Name")]
    public async Task WhenNameIsEmpty_ShouldFail()
    {
        var dto = new WorkoutDto
        {
            Name = string.Empty
        };

        var result = await _validator.TestValidateAsync(dto);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    [Trait("WorkoutDto", "Name")]
    public async Task WhenNameHasLessThan3Chars_ShouldFail()
    {
        var dto = new WorkoutDto
        {
            Name = "ab"
        };

        var result = await _validator.TestValidateAsync(dto);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    [Trait("WorkoutDto", "Name")]
    public async Task WhenNameHasMoreThan100Chars_ShouldFail()
    {
        var dto = new WorkoutDto
        {
            Name = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa."
        };

        var result = await _validator.TestValidateAsync(dto);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    [Trait("WorkoutDto", "Name")]
    public async Task WhenNameisCorrect_ShouldNotFail()
    {
        var dto = new WorkoutDto
        {
            Name = "Barbell"
        };

        var result = await _validator.TestValidateAsync(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    [Trait("WorkoutDto", "Description")]
    public async Task WhenDescriptionIsNull_ShouldFail()
    {
        var dto = new WorkoutDto
        {
            Description = null
        };

        var result = await _validator.TestValidateAsync(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    [Trait("WorkoutDto", "Description")]
    public async Task WhenDescriptionIsEmpty_ShouldFail()
    {
        var dto = new WorkoutDto
        {
            Description = string.Empty
        };

        var result = await _validator.TestValidateAsync(dto);

        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    [Trait("WorkoutDto", "Description")]
    public async Task WhenDescriptionHasLessThan3Chars_ShouldFail()
    {
        var dto = new WorkoutDto
        {
            Description = "ab"
        };

        var result = await _validator.TestValidateAsync(dto);

        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    [Trait("WorkoutDto", "Description")]
    public async Task WhenDescriptionHasMoreThan300Chars_ShouldFail()
    {
        var dto = new WorkoutDto
        {
            Description = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo."
        };

        var result = await _validator.TestValidateAsync(dto);

        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    [Trait("WorkoutDto", "Description")]
    public async Task WhenDescriptionIsCorrect_ShouldNotFail()
    {
        var dto = new WorkoutDto
        {
            Description = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem."
        };

        var result = await _validator.TestValidateAsync(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }
}
