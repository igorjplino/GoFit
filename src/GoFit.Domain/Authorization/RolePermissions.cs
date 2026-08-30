namespace GoFit.Domain.Authorization;

/// <summary>
/// Single source of truth for which permissions each role grants.
/// Adding a new role only requires a constant in <see cref="AppRoles"/> and an entry here -
/// no authorization logic elsewhere needs to change.
/// </summary>
public static class RolePermissions
{
    private static readonly IReadOnlyDictionary<string, string[]> Map = new Dictionary<string, string[]>
    {
        [AppRoles.Admin] = AppPermissions.All.ToArray(),

        [AppRoles.Athlete] = new[]
        {
            AppPermissions.Exercises.View,
            AppPermissions.Training.ViewWorkouts,
            AppPermissions.Training.CreateWorkouts,
            AppPermissions.Training.EditWorkouts,
            AppPermissions.Training.ViewWorkoutPlans,
            AppPermissions.Training.CreateWorkoutPlans,
            AppPermissions.Training.EditWorkoutPlans,
            AppPermissions.Training.ViewWorkoutTracking,
            AppPermissions.Training.StartWorkoutTracking,
            AppPermissions.Training.EditWorkoutTracking
        }
    };

    public static IReadOnlyList<string> For(string role)
    {
        return Map.TryGetValue(role, out var permissions) ? permissions : Array.Empty<string>();
    }
}
