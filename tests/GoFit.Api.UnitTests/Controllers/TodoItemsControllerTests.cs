using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using GoFit.Api.Controllers;
using GoFit.Application.Interfaces;
using GoFit.Application.TodoItems.Commands.Create;
using GoFit.Application.TodoItems.Dtos;
using GoFit.Application.TodoItems.Queries;

namespace GoFit.Api.UnitTests.Controllers;

public class TodoItemsControllerTests
{
    private readonly MockRepository _mockRepository;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<ITodoItemRepository> _todoItemRepositoryMock;

    private readonly TodoItemsController _controller;

    public TodoItemsControllerTests()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _mediatorMock = _mockRepository.Create<IMediator>();
        _todoItemRepositoryMock = _mockRepository.Create<ITodoItemRepository>();

        _controller = new TodoItemsController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetById_WhenValueDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var command = new GetTodoItemByIdQuery();

        _mediatorMock
            .Setup(x => x.Send(command, CancellationToken.None))
            .ReturnsAsync(null as TodoItemDto);

        // Act
        var result = await _controller.GetById(Guid.NewGuid());

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<NotFoundResult>();
        result.Value.Should().BeNull();
    }
}
