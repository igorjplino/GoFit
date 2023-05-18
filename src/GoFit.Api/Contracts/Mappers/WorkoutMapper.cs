using FastEndpoints;
using GoFit.Api.Contracts.Requests;
using GoFit.Api.Contracts.Responses;
using GoFit.Application.Workouts.Dtos;
using GoFit.Domain.Entities;

namespace GoFit.Api.Contracts.Mappers;

public class WorkoutMapper :
    Mapper<WorkoutRequest, WorkoutResponse, WorkoutDto>
{
    public override WorkoutResponse FromEntity(WorkoutDto e)
    {
        return new WorkoutResponse
        {
            Name= e.Name,
            Description = e.Description
        };
    }
}
