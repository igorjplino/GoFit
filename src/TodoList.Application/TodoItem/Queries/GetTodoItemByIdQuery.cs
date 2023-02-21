using MediatR;

namespace TodoList.Application.TodoItem.Queries;

public record GetTodoItemByIdQuery : IRequest<TempResponse>
{
    public Guid Id { get; set; }
}

public class GetTodoItemByIdQueryHandler : IRequestHandler<GetTodoItemByIdQuery, TempResponse>
{
    public Task<TempResponse> Handle(GetTodoItemByIdQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new TempResponse { Temp = "Temporary response" });
    }
}
