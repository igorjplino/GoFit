using FastEndpoints;
using GoFit.Api.Contracts.Requests;
using GoFit.Api.Contracts.Responses;
using GoFit.Application.Exercises.Dtos;

namespace GoFit.Api.Contracts.Mappers;

public class ExerciseMapper :
    Mapper<ExerciseRequest, ExerciseResponse, ExerciseDto>
{
    public override ExerciseResponse FromEntity(ExerciseDto e)
    {
        return new ExerciseResponse
        {
            Name = e.Name,
            Description = e.Description
        };
    }
}
