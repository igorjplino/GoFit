using FastEndpoints;
using GoFit.Api.Contracts.Requests;
using GoFit.Api.Contracts.Responses;
using GoFit.Application.WorkoutPlans.Dtos;

namespace GoFit.Api.Contracts.Mappers;

public class WorkoutPlanMapper :
    Mapper<WorkoutPlanRequest, WorkoutPlanResponse, WorkoutPlanDto>
{
    public override WorkoutPlanResponse FromEntity(WorkoutPlanDto e)
    {
        return new WorkoutPlanResponse
        {
            Title = e.Title
        };
    }
}
