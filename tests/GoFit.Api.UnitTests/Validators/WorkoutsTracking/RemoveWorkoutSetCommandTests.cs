using FluentValidation.TestHelper;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Commands;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Validators;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;

namespace GoFit.Api.UnitTests.Validators.WorkoutsTracking;
public class RemoveWorkoutSetCommandTests
{
    private const string ValidAppUserId = "app-user-1";
    private static readonly Guid AthleteId = Guid.NewGuid();

    private readonly RemoveWorkoutSetCommandValidator _validator;

    private readonly MockRepository _mockRepository;
    private readonly Mock<IWorkoutTrackingRepository> _workoutTrackingRepositoryMock;
    private readonly Mock<IAthleteRepository> _athleteRepositoryMock;

    public RemoveWorkoutSetCommandTests()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _workoutTrackingRepositoryMock = _mockRepository.Create<IWorkoutTrackingRepository>();
        _athleteRepositoryMock = _mockRepository.Create<IAthleteRepository>();

        _athleteRepositoryMock
            .Setup(x => x.GetByAppUserIdAsync(ValidAppUserId))
            .ReturnsAsync(new Athlete { Id = AthleteId, Name = "Test Athlete" });

        _validator = new RemoveWorkoutSetCommandValidator(
            _workoutTrackingRepositoryMock.Object,
            _athleteRepositoryMock.Object);
    }

    [Fact]
    [Trait("WorkoutTracking", "Sets")]
    public async Task WhenNoSetHasTheOrder_ShouldFail()
    {
        var tracking = SetupTracking(loggedSets: 1);

        var command = new RemoveWorkoutSetCommand(tracking.Id, Order: 3, AppUserId: ValidAppUserId);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Order);
    }

    [Fact]
    [Trait("WorkoutTracking", "Ownership")]
    public async Task WhenTrackingAlreadyFinished_ShouldFail()
    {
        var tracking = SetupTracking(loggedSets: 1, endWorkoutDate: DateTime.UtcNow);

        var command = new RemoveWorkoutSetCommand(tracking.Id, Order: 0, AppUserId: ValidAppUserId);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.WorkoutsTrackingId);
    }

    [Fact]
    [Trait("WorkoutTracking", "Sets")]
    public async Task WhenSetExistsAndTrackingIsInProgress_ShouldNotFail()
    {
        var tracking = SetupTracking(loggedSets: 3);

        var command = new RemoveWorkoutSetCommand(tracking.Id, Order: 1, AppUserId: ValidAppUserId);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    private WorkoutTracking SetupTracking(int loggedSets, DateTime? endWorkoutDate = null)
    {
        var tracking = new WorkoutTracking
        {
            Id = Guid.NewGuid(),
            AthleteId = AthleteId,
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
