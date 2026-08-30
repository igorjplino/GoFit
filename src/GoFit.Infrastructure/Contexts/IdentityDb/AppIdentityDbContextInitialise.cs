using GoFit.Application.EntitiesActions.Athletes.Commands;
using GoFit.Application.Interfaces;
using GoFit.Domain.Authorization;
using GoFit.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GoFit.Infrastructure.Contexts.IdentityDb;

public static class AppIdentityDbContextInitialise
{
    public static async Task ApplyMigrationAsync(this AppIdentityDbContext context)
    {
        await context.Database.MigrateAsync();
    }

    public static async Task EnsureRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        await RenameLegacyStudentRoleAsync(roleManager);

        foreach (var role in AppRoles.All)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    /// <summary>
    /// The role used to be called "Student" before it was renamed to "Athlete". Renaming it in
    /// place (instead of letting EnsureRolesAsync create a fresh "Athlete" role) preserves
    /// existing users' role assignments rather than orphaning them under the old name.
    /// </summary>
    private static async Task RenameLegacyStudentRoleAsync(RoleManager<IdentityRole> roleManager)
    {
        var legacyRole = await roleManager.FindByNameAsync("Student");
        if (legacyRole is null || await roleManager.RoleExistsAsync(AppRoles.Athlete))
        {
            return;
        }

        legacyRole.Name = AppRoles.Athlete;
        await roleManager.UpdateNormalizedRoleNameAsync(legacyRole);
        await roleManager.UpdateAsync(legacyRole);
    }

    /// <summary>
    /// Guarantees at least one Admin account exists, so the system can never be left without
    /// an administrator. Only runs if no user currently holds the Admin role.
    /// </summary>
    public static async Task EnsureAdminAsync(
        UserManager<AppUser> userManager,
        IConfiguration configuration,
        ILogger logger)
    {
        var existingAdmins = await userManager.GetUsersInRoleAsync(AppRoles.Admin);
        if (existingAdmins.Count > 0)
        {
            return;
        }

        var email = configuration["Identity:AdminSeed:Email"];
        var password = configuration["Identity:AdminSeed:Password"];

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            logger.LogWarning(
                "No Admin user exists and Identity:AdminSeed:Email/Password are not configured - " +
                "the application has no administrator. Set these in appsettings.Local.json to seed one.");
            return;
        }

        var user = await userManager.FindByEmailAsync(email);

        if (user is null)
        {
            user = new AppUser
            {
                DisplayName = "Admin",
                UserName = email,
                Email = email
            };

            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                logger.LogError(
                    "Failed to seed default Admin user: {Errors}",
                    string.Join(", ", result.Errors.Select(e => e.Description)));
                return;
            }
        }

        await userManager.AddToRoleAsync(user, AppRoles.Admin);
    }

    /// <summary>
    /// Users created before roles existed have no role assigned, which would leave them with
    /// zero permissions. Defaults them to Athlete - the same role new self-registered users get -
    /// rather than silently locking them out.
    /// </summary>
    public static async Task BackfillMissingRolesAsync(UserManager<AppUser> userManager, ILogger logger)
    {
        var users = await userManager.Users.ToListAsync();

        foreach (var user in users)
        {
            var roles = await userManager.GetRolesAsync(user);
            if (roles.Count == 0)
            {
                await userManager.AddToRoleAsync(user, AppRoles.Athlete);
                logger.LogWarning("User {Email} had no role - defaulted to {Role}", user.Email, AppRoles.Athlete);
            }
        }
    }

    /// <summary>
    /// Users created before the Athlete/AppUser link existed have no corresponding Athlete row,
    /// which would leave them unable to own any workout data. Creates the missing Athlete via the
    /// same CreateAthleteCommand used at registration, so it goes through the normal validation
    /// pipeline rather than writing to the repository directly.
    /// </summary>
    public static async Task BackfillMissingAthletesAsync(
        UserManager<AppUser> userManager,
        IMediator mediator,
        IAthleteRepository athleteRepository,
        ILogger logger)
    {
        var users = await userManager.Users.ToListAsync();

        foreach (var user in users)
        {
            var existingAthlete = await athleteRepository.GetByAppUserIdAsync(user.Id);

            if (existingAthlete is not null)
            {
                continue;
            }

            await mediator.Send(new CreateAthleteCommand(user.Id, user.DisplayName, user.Email));
            logger.LogWarning("User {Email} had no linked Athlete - created one", user.Email);
        }
    }
}
