using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using GoFit.Api.Controllers;
using GoFit.Application.Interfaces;
using GoFit.Application.WorkoutPlans.Queries;
using GoFit.Application.WorkoutPlans.Dtos;

namespace GoFit.Api.UnitTests.Controllers;

public class TodoItemsControllerTests
{
    private readonly MockRepository _mockRepository;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<ITodoItemRepository> _todoItemRepositoryMock;

    private readonly WorkoutPlanController _controller;

    public TodoItemsControllerTests()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _mediatorMock = _mockRepository.Create<IMediator>();
        _todoItemRepositoryMock = _mockRepository.Create<ITodoItemRepository>();

        _controller = new WorkoutPlanController(_mediatorMock.Object);
    }

    [Fact(Skip = "")]
    public async Task GetById_WhenValueDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var command = new GetWorkoutPlanDtoByIdQuery();

        _mediatorMock
            .Setup(x => x.Send(command, CancellationToken.None))
            .ReturnsAsync(null as WorkoutPlanDto);

        // Act
        var result = await _controller.GetById(Guid.NewGuid());

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<NotFoundResult>();
        result.Value.Should().BeNull();
    }
}
