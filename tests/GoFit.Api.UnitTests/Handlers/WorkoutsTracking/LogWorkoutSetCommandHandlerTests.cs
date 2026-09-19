using GoFit.Application.EntitiesActions.WorkoutsTracking.Commands;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Dtos;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;

namespace GoFit.Api.UnitTests.Handlers.WorkoutsTracking;
public class LogWorkoutSetCommandHandlerTests
{
    private static readonly Guid TrackingId = Guid.NewGuid();

    private readonly LogWorkoutSetCommandHandler _handler;

    private readonly MockRepository _mockRepository;
    private readonly Mock<IWorkoutSetTrackingRepository> _workoutSetTrackingRepositoryMock;
    private readonly Mock<IWorkoutTrackingRepository> _workoutTrackingRepositoryMock;

    public LogWorkoutSetCommandHandlerTests()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _workoutSetTrackingRepositoryMock = _mockRepository.Create<IWorkoutSetTrackingRepository>();
        _workoutTrackingRepositoryMock = _mockRepository.Create<IWorkoutTrackingRepository>();

        _handler = new LogWorkoutSetCommandHandler(
            _workoutSetTrackingRepositoryMock.Object,
            _workoutTrackingRepositoryMock.Object);
    }

    [Fact]
    [Trait("WorkoutTracking", "Sets")]
    public async Task WhenSetIsLogged_ShouldTakeThePositionAfterTheLastSet()
    {
        SetupTracking(loggedSets: 2);

        await _handler.Handle(new LogWorkoutSetCommand(TrackingId, Repetitions: 8, Weight: 42.5f), CancellationToken.None);

        _workoutSetTrackingRepositoryMock.Verify(
            x => x.CreateAsync(It.Is<WorkoutSetTracking>(set =>
                set.WorkoutTrackingId == TrackingId
                && set.Order == 2
                && set.Repetitions == 8
                && set.Weight == 42.5f)),
            Times.Once);
    }

    [Fact]
    [Trait("WorkoutTracking", "Sets")]
    public async Task WhenFirstSetIsLogged_ShouldTakePositionZero()
    {
        SetupTracking(loggedSets: 0);

        await _handler.Handle(new LogWorkoutSetCommand(TrackingId, Repetitions: 5, Weight: 0), CancellationToken.None);

        _workoutSetTrackingRepositoryMock.Verify(
            x => x.CreateAsync(It.Is<WorkoutSetTracking>(set => set.Order == 0)),
            Times.Once);
    }

    [Fact]
    [Trait("WorkoutTracking", "Sets")]
    public async Task WhenSetIsLogged_ShouldReturnTheStoredTracking()
    {
        SetupTracking(loggedSets: 1);

        var result = await _handler.Handle(new LogWorkoutSetCommand(TrackingId, Repetitions: 8, Weight: 20), CancellationToken.None);

        WorkoutTrackingDto dto = result.Match(succ => succ, fail => throw fail);

        Assert.Equal(TrackingId, dto.Id);
        Assert.Equal(new[] { 0 }, dto.Sets.Select(o => o.Order));
    }

    // The response is read back after the write, so the mocked re-read is what the handler maps.
    private void SetupTracking(int loggedSets)
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
    }
}
