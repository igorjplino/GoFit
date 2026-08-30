using MediatR;
using GoFit.Application.Interfaces;
using GoFit.Application.Common;

namespace GoFit.Application.EntitiesActions.WorkoutPlans.Commands;

public record UpdateWorkoutPlanCommand(Guid WorkoutPlanId, string AppUserId, string Title, string? Description)
    : IRequest<Result<Guid?>>
{ }

public class UpdateWorkoutPlanCommandHandler : IRequestHandler<UpdateWorkoutPlanCommand, Result<Guid?>>
{
    private readonly IWorkoutPlanRepository _workoutPlanRepository;
    private readonly IAthleteRepository _athleteRepository;

    public UpdateWorkoutPlanCommandHandler(IWorkoutPlanRepository workoutPlanRepository, IAthleteRepository athleteRepository)
    {
        _workoutPlanRepository = workoutPlanRepository;
        _athleteRepository = athleteRepository;
    }

    public async Task<Result<Guid?>> Handle(UpdateWorkoutPlanCommand request, CancellationToken cancellationToken)
    {
        var athlete = await _athleteRepository.GetByAppUserIdAsync(request.AppUserId);

        if (athlete is null)
            return default;

        var plan = await _workoutPlanRepository.GetAsync(request.WorkoutPlanId);

        if (plan is null || plan.AthleteId != athlete.Id)
            return default;

        await _workoutPlanRepository.UpdateTitleAndDescriptionAsync(plan.Id, request.Title, request.Description);

        return plan.Id;
    }
}
