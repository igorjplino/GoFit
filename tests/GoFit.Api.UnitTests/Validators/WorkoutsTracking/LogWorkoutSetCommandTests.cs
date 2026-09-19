using FluentValidation.TestHelper;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Commands;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Validators;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;

namespace GoFit.Api.UnitTests.Validators.WorkoutsTracking;
public class LogWorkoutSetCommandTests
{
    private const string ValidAppUserId = "app-user-1";
    private const int PlannedSets = 2;
    private static readonly Guid AthleteId = Guid.NewGuid();
    private static readonly Guid WorkoutId = Guid.NewGuid();

    private readonly LogWorkoutSetCommandValidator _validator;

    private readonly MockRepository _mockRepository;
    private readonly Mock<IWorkoutRepository> _workoutRepositoryMock;
    private readonly Mock<IWorkoutTrackingRepository> _workoutTrackingRepositoryMock;
    private readonly Mock<IAthleteRepository> _athleteRepositoryMock;

    public LogWorkoutSetCommandTests()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _workoutRepositoryMock = _mockRepository.Create<IWorkoutRepository>();
        _workoutTrackingRepositoryMock = _mockRepository.Create<IWorkoutTrackingRepository>();
        _athleteRepositoryMock = _mockRepository.Create<IAthleteRepository>();

        _athleteRepositoryMock
            .Setup(x => x.GetByAppUserIdAsync(ValidAppUserId))
            .ReturnsAsync(new Athlete { Id = AthleteId, Name = "Test Athlete" });

        _workoutRepositoryMock
            .Setup(x => x.GetWithDetailsAsync(WorkoutId))
            .ReturnsAsync(new Workout
            {
                Id = WorkoutId,
                WorkoutExercises = [new WorkoutExercise { Sets = Enumerable.Range(0, PlannedSets).Select(order => new WorkoutSet { Order = order }).ToList() }]
            });

        _validator = new LogWorkoutSetCommandValidator(
            _workoutRepositoryMock.Object,
            _workoutTrackingRepositoryMock.Object,
            _athleteRepositoryMock.Object);
    }

    [Fact]
    [Trait("WorkoutTracking", "Sets")]
    public async Task WhenRepetitionsIsZero_ShouldFail()
    {
        var tracking = SetupTracking(loggedSets: 0);

        var command = new LogWorkoutSetCommand(tracking.Id, Repetitions: 0, Weight: 20, AppUserId: ValidAppUserId);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Repetitions);
    }

    [Fact]
    [Trait("WorkoutTracking", "Sets")]
    public async Task WhenWeightIsNegative_ShouldFail()
    {
        var tracking = SetupTracking(loggedSets: 0);

        var command = new LogWorkoutSetCommand(tracking.Id, Repetitions: 10, Weight: -1, AppUserId: ValidAppUserId);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Weight);
    }

    [Fact]
    [Trait("WorkoutTracking", "Ownership")]
    public async Task WhenTrackingBelongsToAnotherAthlete_ShouldFail()
    {
        var tracking = SetupTracking(loggedSets: 0, athleteId: Guid.NewGuid());

        var command = new LogWorkoutSetCommand(tracking.Id, Repetitions: 10, Weight: 20, AppUserId: ValidAppUserId);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.WorkoutsTrackingId);
    }

    [Fact]
    [Trait("WorkoutTracking", "Ownership")]
    public async Task WhenTrackingAlreadyFinished_ShouldFail()
    {
        var tracking = SetupTracking(loggedSets: 0, endWorkoutDate: DateTime.UtcNow);

        var command = new LogWorkoutSetCommand(tracking.Id, Repetitions: 10, Weight: 20, AppUserId: ValidAppUserId);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.WorkoutsTrackingId);
    }

    [Fact]
    [Trait("WorkoutTracking", "Sets")]
    public async Task WhenAllPlannedSetsAreLogged_ShouldFail()
    {
        var tracking = SetupTracking(loggedSets: PlannedSets);

        var command = new LogWorkoutSetCommand(tracking.Id, Repetitions: 10, Weight: 20, AppUserId: ValidAppUserId);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.WorkoutsTrackingId)
            .WithErrorMessage("All planned sets have already been logged.");
    }

    [Fact]
    [Trait("WorkoutTracking", "Sets")]
    public async Task WhenPlannedSetsRemain_ShouldNotFail()
    {
        var tracking = SetupTracking(loggedSets: PlannedSets - 1);

        var command = new LogWorkoutSetCommand(tracking.Id, Repetitions: 10, Weight: 0, AppUserId: ValidAppUserId);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    private WorkoutTracking SetupTracking(int loggedSets, Guid? athleteId = null, DateTime? endWorkoutDate = null)
    {
        var tracking = new WorkoutTracking
        {
            Id = Guid.NewGuid(),
            AthleteId = athleteId ?? AthleteId,
            WorkoutId = WorkoutId,
            EndWorkoutDate = endWorkoutDate,
            Sets = Enumerable.Range(0, loggedSets)
                .Select(order => new WorkoutSetTracking { Order = order, Repetitions = 10, Weight = 20 })
                .ToList()
        };

        _workoutTrackingRepositoryMock.Setup(x => x.GetAsync(tracking.Id)).ReturnsAsync(tracking);
        _workoutTrackingRepositoryMock.Setup(x => x.GetWithSetsAsync(tracking.Id)).ReturnsAsync(tracking);

        return tracking;
    }
}
