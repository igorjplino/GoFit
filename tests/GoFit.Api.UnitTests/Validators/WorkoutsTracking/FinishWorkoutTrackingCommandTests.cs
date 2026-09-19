using FluentValidation.TestHelper;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Commands;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Validators;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;

namespace GoFit.Api.UnitTests.Validators.WorkoutsTracking;
public class FinishWorkoutTrackingCommandTests
{
    private const string ValidAppUserId = "app-user-1";
    private static readonly Guid AthleteId = Guid.NewGuid();

    private readonly FinishWorkoutTrackingCommandValidator _validator;

    private readonly MockRepository _mockRepository;
    private readonly Mock<IWorkoutTrackingRepository> _workoutTrackingRepositoryMock;
    private readonly Mock<IAthleteRepository> _athleteRepositoryMock;

    public FinishWorkoutTrackingCommandTests()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _workoutTrackingRepositoryMock = _mockRepository.Create<IWorkoutTrackingRepository>();
        _athleteRepositoryMock = _mockRepository.Create<IAthleteRepository>();

        _athleteRepositoryMock
            .Setup(x => x.GetByAppUserIdAsync(ValidAppUserId))
            .ReturnsAsync(new Athlete { Id = AthleteId, Name = "Test Athlete" });

        _validator = new FinishWorkoutTrackingCommandValidator(
            _workoutTrackingRepositoryMock.Object,
            _athleteRepositoryMock.Object);
    }

    [Fact]
    [Trait("WorkoutTracking", "Ownership")]
    public async Task WhenAppUserIdIsEmpty_ShouldFail()
    {
        var command = new FinishWorkoutTrackingCommand(Guid.NewGuid(), AppUserId: "");

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.AppUserId);
    }

    [Fact]
    [Trait("WorkoutTracking", "Ownership")]
    public async Task WhenTrackingBelongsToAnotherAthlete_ShouldFail()
    {
        var trackingId = SetupTracking(new WorkoutTracking { AthleteId = Guid.NewGuid() });

        var result = await _validator.TestValidateAsync(new FinishWorkoutTrackingCommand(trackingId, ValidAppUserId));

        result.ShouldHaveValidationErrorFor(x => x.WorkoutsTrackingId);
    }

    [Fact]
    [Trait("WorkoutTracking", "Ownership")]
    public async Task WhenTrackingAlreadyFinished_ShouldFail()
    {
        var trackingId = SetupTracking(new WorkoutTracking { AthleteId = AthleteId, EndWorkoutDate = DateTime.UtcNow });

        var result = await _validator.TestValidateAsync(new FinishWorkoutTrackingCommand(trackingId, ValidAppUserId));

        result.ShouldHaveValidationErrorFor(x => x.WorkoutsTrackingId)
            .WithErrorMessage("This workout has already been finished.");
    }

    [Fact]
    [Trait("WorkoutTracking", "Ownership")]
    public async Task WhenTrackingAlreadyCancelled_ShouldFail()
    {
        var trackingId = SetupTracking(new WorkoutTracking { AthleteId = AthleteId, CancelledDate = DateTime.UtcNow });

        var result = await _validator.TestValidateAsync(new FinishWorkoutTrackingCommand(trackingId, ValidAppUserId));

        result.ShouldHaveValidationErrorFor(x => x.WorkoutsTrackingId);
    }

    [Fact]
    [Trait("WorkoutTracking", "Ownership")]
    public async Task WhenTrackingIsOwnedAndInProgress_ShouldNotFail()
    {
        var trackingId = SetupTracking(new WorkoutTracking { AthleteId = AthleteId });

        var result = await _validator.TestValidateAsync(new FinishWorkoutTrackingCommand(trackingId, ValidAppUserId));

        result.ShouldNotHaveAnyValidationErrors();
    }

    private Guid SetupTracking(WorkoutTracking tracking)
    {
        tracking.Id = Guid.NewGuid();

        _workoutTrackingRepositoryMock.Setup(x => x.GetAsync(tracking.Id)).ReturnsAsync(tracking);

        return tracking.Id;
    }
}
