using FastEndpoints;
using GoFit.Api.Contracts.Mappers;
using GoFit.Api.Contracts.Requests;
using GoFit.Api.Contracts.Responses;
using GoFit.Application.Exercises.Dtos;
using GoFit.Application.Exercises.Queries;
using MediatR;

namespace GoFit.Api.Endpoints;

public class ExerciseEndpoint :
    Endpoint<ExerciseRequest, ExerciseResponse, ExerciseMapper>
{
    public IMediator Mediator { get; init; }

    public override void Configure()
    {
        Verbs(Http.GET);
        Get("Exercise/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ExerciseRequest req, CancellationToken ct)
    {
        ExerciseDto? result = await Mediator.Send(new GetExerciseDtoByIdQuery { Id = req.Id });

        var response = Map.FromEntity(result);

        await SendAsync(response, cancellation: ct);
    }
}
