using GoFit.Application.EntitiesActions.WorkoutsTracking.Commands;
using GoFit.Application.Interfaces;
using GoFit.Application.Models;
using GoFit.Domain.Entities;

namespace GoFit.Api.UnitTests.Handlers.WorkoutsTracking;
public class RemoveWorkoutSetCommandHandlerTests
{
    private static readonly Guid TrackingId = Guid.NewGuid();

    private readonly RemoveWorkoutSetCommandHandler _handler;

    private readonly MockRepository _mockRepository;
    private readonly Mock<IWorkoutSetTrackingRepository> _workoutSetTrackingRepositoryMock;
    private readonly Mock<IWorkoutTrackingRepository> _workoutTrackingRepositoryMock;

    public RemoveWorkoutSetCommandHandlerTests()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _workoutSetTrackingRepositoryMock = _mockRepository.Create<IWorkoutSetTrackingRepository>();
        _workoutTrackingRepositoryMock = _mockRepository.Create<IWorkoutTrackingRepository>();

        _handler = new RemoveWorkoutSetCommandHandler(
            _workoutSetTrackingRepositoryMock.Object,
            _workoutTrackingRepositoryMock.Object);
    }

    [Fact]
    [Trait("WorkoutTracking", "Sets")]
    public async Task WhenMiddleSetIsRemoved_ShouldMoveOnlyTheSetsAfterItDown()
    {
        var sets = SetupTracking(loggedSets: 3);

        await _handler.Handle(new RemoveWorkoutSetCommand(TrackingId, Order: 1), CancellationToken.None);

        var expected = new[] { new WorkoutSetPosition(sets[2].Id, 1) };

        _workoutSetTrackingRepositoryMock.Verify(
            x => x.RemoveAndResequenceAsync(
                sets[1].Id,
                It.Is<IReadOnlyCollection<WorkoutSetPosition>>(positions => positions.SequenceEqual(expected))),
            Times.Once);
    }

    [Fact]
    [Trait("WorkoutTracking", "Sets")]
    public async Task WhenFirstSetIsRemoved_ShouldMoveEveryLaterSetDown()
    {
        var sets = SetupTracking(loggedSets: 3);

        await _handler.Handle(new RemoveWorkoutSetCommand(TrackingId, Order: 0), CancellationToken.None);

        var expected = new[]
        {
            new WorkoutSetPosition(sets[1].Id, 0),
            new WorkoutSetPosition(sets[2].Id, 1)
        };

        _workoutSetTrackingRepositoryMock.Verify(
            x => x.RemoveAndResequenceAsync(
                sets[0].Id,
                It.Is<IReadOnlyCollection<WorkoutSetPosition>>(positions => positions.SequenceEqual(expected))),
            Times.Once);
    }

    [Fact]
    [Trait("WorkoutTracking", "Sets")]
    public async Task WhenLastSetIsRemoved_ShouldNotMoveAnySet()
    {
        var sets = SetupTracking(loggedSets: 3);

        await _handler.Handle(new RemoveWorkoutSetCommand(TrackingId, Order: 2), CancellationToken.None);

        _workoutSetTrackingRepositoryMock.Verify(
            x => x.RemoveAndResequenceAsync(
                sets[2].Id,
                It.Is<IReadOnlyCollection<WorkoutSetPosition>>(positions => positions.Count == 0)),
            Times.Once);
    }

    private List<WorkoutSetTracking> SetupTracking(int loggedSets)
    {
        var sets = Enumerable.Range(0, loggedSets)
            .Select(order => new WorkoutSetTracking { Id = Guid.NewGuid(), Order = order, Repetitions = 10 + order, Weight = 20 })
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
