using FluentValidation.TestHelper;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Commands;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Validators;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;

namespace GoFit.Api.UnitTests.Validators.WorkoutsTracking;
public class CancelWorkoutTrackingCommandTests
{
    private const string ValidAppUserId = "app-user-1";
    private static readonly Guid AthleteId = Guid.NewGuid();

    private readonly CancelWorkoutTrackingCommandValidator _validator;

    private readonly MockRepository _mockRepository;
    private readonly Mock<IWorkoutTrackingRepository> _workoutTrackingRepositoryMock;
    private readonly Mock<IAthleteRepository> _athleteRepositoryMock;

    public CancelWorkoutTrackingCommandTests()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _workoutTrackingRepositoryMock = _mockRepository.Create<IWorkoutTrackingRepository>();
        _athleteRepositoryMock = _mockRepository.Create<IAthleteRepository>();

        _athleteRepositoryMock
            .Setup(x => x.GetByAppUserIdAsync(ValidAppUserId))
            .ReturnsAsync(new Athlete { Id = AthleteId, Name = "Test Athlete" });

        _validator = new CancelWorkoutTrackingCommandValidator(
            _workoutTrackingRepositoryMock.Object,
            _athleteRepositoryMock.Object);
    }

    [Fact]
    [Trait("WorkoutTracking", "Ownership")]
    public async Task WhenAppUserIdIsEmpty_ShouldFail()
    {
        var command = new CancelWorkoutTrackingCommand(
            WorkoutsTrackingId: Guid.NewGuid(),
            AppUserId: "");

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.AppUserId);
    }

    [Fact]
    [Trait("WorkoutTracking", "Ownership")]
    public async Task WhenNoAthleteLinkedToAppUserId_ShouldFail()
    {
        var command = new CancelWorkoutTrackingCommand(
            WorkoutsTrackingId: Guid.NewGuid(),
            AppUserId: "unknown-app-user");

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.AppUserId);
    }

    [Fact]
    [Trait("WorkoutTracking", "Ownership")]
    public async Task WhenTrackingBelongsToAnotherAthlete_ShouldFail()
    {
        var trackingId = Guid.NewGuid();

        _workoutTrackingRepositoryMock
            .Setup(x => x.GetAsync(trackingId))
            .ReturnsAsync(new WorkoutTracking { Id = trackingId, AthleteId = Guid.NewGuid() });

        var command = new CancelWorkoutTrackingCommand(
            WorkoutsTrackingId: trackingId,
            AppUserId: ValidAppUserId);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.WorkoutsTrackingId);
    }

    [Fact]
    [Trait("WorkoutTracking", "Ownership")]
    public async Task WhenTrackingAlreadyFinished_ShouldFail()
    {
        var trackingId = Guid.NewGuid();

        _workoutTrackingRepositoryMock
            .Setup(x => x.GetAsync(trackingId))
            .ReturnsAsync(new WorkoutTracking { Id = trackingId, AthleteId = AthleteId, EndWorkoutDate = DateTime.UtcNow });

        var command = new CancelWorkoutTrackingCommand(
            WorkoutsTrackingId: trackingId,
            AppUserId: ValidAppUserId);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.WorkoutsTrackingId);
    }

    [Fact]
    [Trait("WorkoutTracking", "Ownership")]
    public async Task WhenTrackingAlreadyCancelled_ShouldFail()
    {
        var trackingId = Guid.NewGuid();

        _workoutTrackingRepositoryMock
            .Setup(x => x.GetAsync(trackingId))
            .ReturnsAsync(new WorkoutTracking { Id = trackingId, AthleteId = AthleteId, CancelledDate = DateTime.UtcNow });

        var command = new CancelWorkoutTrackingCommand(
            WorkoutsTrackingId: trackingId,
            AppUserId: ValidAppUserId);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.WorkoutsTrackingId);
    }

    [Fact]
    [Trait("WorkoutTracking", "Ownership")]
    public async Task WhenTrackingIsOwnedAndInProgress_ShouldNotFail()
    {
        var trackingId = Guid.NewGuid();

        _workoutTrackingRepositoryMock
            .Setup(x => x.GetAsync(trackingId))
            .ReturnsAsync(new WorkoutTracking { Id = trackingId, AthleteId = AthleteId });

        var command = new CancelWorkoutTrackingCommand(
            WorkoutsTrackingId: trackingId,
            AppUserId: ValidAppUserId);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(x => x.WorkoutsTrackingId);
        result.ShouldNotHaveValidationErrorFor(x => x.AppUserId);
    }
}
