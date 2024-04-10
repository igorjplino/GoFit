using FluentValidation.TestHelper;
using GoFit.Application.EntitiesActions.Exercises.Commands;
using GoFit.Application.EntitiesActions.WorkoutPlans.Commands;
using GoFit.Application.EntitiesActions.WorkoutPlans.Dtos;
using GoFit.Application.EntitiesActions.WorkoutPlans.Validators;
using GoFit.Application.Interfaces;

namespace GoFit.Api.UnitTests.Validators.WorkoutPlans;

public class CreateWorkoutPlanCommandValidatorTests
{
    private readonly CreateWorkoutPlanCommandValidator _validator;

    private readonly MockRepository _mockRepository;
    private readonly Mock<IExerciseRepository> _exerciseRepositoryMock;
    private readonly Mock<IAthleteRepository> _athleteRepositoryMock;

    public CreateWorkoutPlanCommandValidatorTests()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _exerciseRepositoryMock = _mockRepository.Create<IExerciseRepository>();
        _athleteRepositoryMock = _mockRepository.Create<IAthleteRepository>();

        _validator = new CreateWorkoutPlanCommandValidator(_exerciseRepositoryMock.Object, _athleteRepositoryMock.Object);
    }

    [Fact]
    [Trait("WorkoutPlan", "Title")]
    public async Task WhenTitleIsNull_ShouldFail()
    {
        var command = new CreateWorkoutPlanCommand(
            AthleteId: default,
            Title: null,
            Description: null,
            Workouts: Enumerable.Empty<WorkoutDto>());

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    [Trait("WorkoutPlan", "Title")]
    public async Task WhenTitleIsEmpty_ShouldFail()
    {
        var command = new CreateWorkoutPlanCommand(
            AthleteId: default,
            Title: string.Empty,
            Description: null,
            Workouts: Enumerable.Empty<WorkoutDto>());

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    [Trait("WorkoutPlan", "Title")]
    public async Task WhenTitleHasLessThan3Chars_ShouldFail()
    {
        var command = new CreateWorkoutPlanCommand(
            AthleteId: default,
            Title: "ab",
            Description: null,
            Workouts: Enumerable.Empty<WorkoutDto>());

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    [Trait("WorkoutPlan", "Title")]
    public async Task WhenTitleHasMoreThan100Chars_ShouldFail()
    {
        var command = new CreateWorkoutPlanCommand(
            AthleteId: default,
            Title: "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa.",
            Description: null,
            Workouts: Enumerable.Empty<WorkoutDto>());

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    [Trait("WorkoutPlan", "Title")]
    public async Task WhenTitleIsCorrect_ShouldNotFail()
    {
        var command = new CreateWorkoutPlanCommand(
            AthleteId: default,
            Title: "Barbell",
            Description: null,
            Workouts: Enumerable.Empty<WorkoutDto>());

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    [Trait("WorkoutPlan", "Description")]
    public async Task WhenDescriptionIsNull_ShouldFail()
    {
        var command = new CreateWorkoutPlanCommand(
            AthleteId: default,
            Title: string.Empty,
            Description: null,
            Workouts: Enumerable.Empty<WorkoutDto>());

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    [Trait("WorkoutPlan", "Description")]
    public async Task WhenDescriptionIsEmpty_ShouldFail()
    {
        var command = new CreateWorkoutPlanCommand(
            AthleteId: default,
            Title: string.Empty,
            Description: string.Empty,
            Workouts: Enumerable.Empty<WorkoutDto>());

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    [Trait("WorkoutPlan", "Description")]
    public async Task WhenDescriptionHasLessThan3Chars_ShouldFail()
    {
        var command = new CreateWorkoutPlanCommand(
            AthleteId: default,
            Title: string.Empty,
            Description: "ab",
            Workouts: Enumerable.Empty<WorkoutDto>());

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    [Trait("WorkoutPlan", "Description")]
    public async Task WhenDescriptionHasMoreThan300Chars_ShouldFail()
    {
        var command = new CreateWorkoutPlanCommand(
            AthleteId: default,
            Title: string.Empty,
            Description: "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo.",
            Workouts: Enumerable.Empty<WorkoutDto>());

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    [Trait("WorkoutPlan", "Description")]
    public async Task WhenDescriptionIsCorrect_ShouldNotFail()
    {
        var command = new CreateWorkoutPlanCommand(
            AthleteId: default,
            Title: string.Empty,
            Description: "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem.",
            Workouts: Enumerable.Empty<WorkoutDto>());

        var result = await _validator.TestValidateAsync(command);
        
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    [Trait("WorkoutPlan", "Workouts")]
    public async Task WhenWorkoutsIsNull_ShouldFail()
    {
        var command = new CreateWorkoutPlanCommand(
            AthleteId: default,
            Title: string.Empty,
            Description: null,
            Workouts: null);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Workouts);
    }

    [Fact]
    [Trait("WorkoutPlan", "Workouts")]
    public async Task WhenWorkoutsIsEmpty_ShouldFail()
    {
        var command = new CreateWorkoutPlanCommand(
            AthleteId: default,
            Title: string.Empty,
            Description: null,
            Workouts: Enumerable.Empty<WorkoutDto>());

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Workouts);
    }

    [Fact]
    [Trait("WorkoutPlan", "Workouts")]
    public async Task WhenWorkoutsHasNullItem_ShouldFail()
    {
        var command = new CreateWorkoutPlanCommand(
            AthleteId: default,
            Title: string.Empty,
            Description: null,
            Workouts: new List<WorkoutDto> { null });

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Workouts);
    }
}
