using GoFit.Application.EntitiesActions.WorkoutsTracking.Commands;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;

namespace GoFit.Api.UnitTests.Handlers.WorkoutsTracking;
public class UpdateWorkoutSetCommandHandlerTests
{
    private static readonly Guid TrackingId = Guid.NewGuid();

    private readonly UpdateWorkoutSetCommandHandler _handler;

    private readonly MockRepository _mockRepository;
    private readonly Mock<IWorkoutSetTrackingRepository> _workoutSetTrackingRepositoryMock;
    private readonly Mock<IWorkoutTrackingRepository> _workoutTrackingRepositoryMock;

    public UpdateWorkoutSetCommandHandlerTests()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _workoutSetTrackingRepositoryMock = _mockRepository.Create<IWorkoutSetTrackingRepository>();
        _workoutTrackingRepositoryMock = _mockRepository.Create<IWorkoutTrackingRepository>();

        _handler = new UpdateWorkoutSetCommandHandler(
            _workoutSetTrackingRepositoryMock.Object,
            _workoutTrackingRepositoryMock.Object);
    }

    [Fact]
    [Trait("WorkoutTracking", "Sets")]
    public async Task WhenSetIsEdited_ShouldUpdateTheSetAtThatPosition()
    {
        var sets = SetupTracking(loggedSets: 3);

        await _handler.Handle(
            new UpdateWorkoutSetCommand(TrackingId, Order: 1, Repetitions: 12, Weight: 22.5f),
            CancellationToken.None);

        _workoutSetTrackingRepositoryMock.Verify(
            x => x.UpdateSetAsync(sets[1].Id, 12, 22.5f),
            Times.Once);
    }

    [Fact]
    [Trait("WorkoutTracking", "Sets")]
    public async Task WhenSetIsEdited_ShouldNotTouchTheOtherSets()
    {
        var sets = SetupTracking(loggedSets: 3);

        await _handler.Handle(
            new UpdateWorkoutSetCommand(TrackingId, Order: 0, Repetitions: 12, Weight: 22.5f),
            CancellationToken.None);

        _workoutSetTrackingRepositoryMock.Verify(
            x => x.UpdateSetAsync(It.IsIn(sets[1].Id, sets[2].Id), It.IsAny<int>(), It.IsAny<float>()),
            Times.Never);
    }

    private List<WorkoutSetTracking> SetupTracking(int loggedSets)
    {
        var sets = Enumerable.Range(0, loggedSets)
            .Select(order => new WorkoutSetTracking { Id = Guid.NewGuid(), Order = order, Repetitions = 10, Weight = 20 })
            .ToList();

        _workoutSetTrackingRepositoryMock
            .Setup(x => x.ListByTrackingIdAsync(TrackingId))
            .ReturnsAsync(sets);

        _workoutTrackingRepositoryMock
            .Setup(x => x.GetWithSetsAsync(TrackingId))
            .ReturnsAsync(new WorkoutTracking
            {
                Id = TrackingId,
                Workout = new Workout { Id = Guid.NewGuid(), Name = "Push day" },
                Sets = sets
            });

        return sets;
    }
}
