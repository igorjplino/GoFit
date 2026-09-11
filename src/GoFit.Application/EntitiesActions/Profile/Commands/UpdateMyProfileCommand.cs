using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.Profile.Dtos;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GoFit.Application.EntitiesActions.Profile.Commands;

public record UpdateMyProfileCommand(string Name, string AppUserId = "") : IRequest<Result<ProfileDto>>
{ }

public class UpdateMyProfileCommandHandler : IRequestHandler<UpdateMyProfileCommand, Result<ProfileDto>>
{
    private readonly IAthleteRepository _athleteRepository;
    private readonly UserManager<AppUser> _userManager;

    public UpdateMyProfileCommandHandler(
        IAthleteRepository athleteRepository,
        UserManager<AppUser> userManager)
    {
        _athleteRepository = athleteRepository;
        _userManager = userManager;
    }

    public async Task<Result<ProfileDto>> Handle(UpdateMyProfileCommand request, CancellationToken cancellationToken)
    {
        AppUser? user = await _userManager.FindByIdAsync(request.AppUserId);

        if (user is null)
        {
            return default;
        }

        var name = request.Name.Trim();

        // The name is stored in GoFitDb (Athlete) and IdentityDb (AppUser). They are separate
        // databases with no shared transaction, so the athlete row is written first: its Name column
        // carries the stricter constraints, and the read path sources the name from AppUser. If the
        // identity write below fails, nothing the user can see has changed and a retry reconciles both.
        var athlete = await _athleteRepository.GetByAppUserIdAsync(request.AppUserId);

        if (athlete is not null)
        {
            // Load-then-mutate: the context is NoTracking and UpdateAsync writes every column,
            // so a freshly constructed entity would wipe AppUserId, Email and Type.
            athlete.Name = name;
            athlete.DisplayName = name;

            await _athleteRepository.UpdateAsync(athlete);
        }

        user.DisplayName = name;
        IdentityResult identityResult = await _userManager.UpdateAsync(user);

        if (!identityResult.Succeeded)
        {
            return new InvalidOperationException(
                string.Join(" ", identityResult.Errors.Select(error => error.Description)));
        }

        return new ProfileDto
        {
            Name = name,
            Email = user.Email ?? string.Empty
        };
    }
}
