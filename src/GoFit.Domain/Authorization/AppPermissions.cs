namespace GoFit.Domain.Authorization;

public static class AppPermissions
{
    public static class Exercises
    {
        public const string View = "exercise:view";
        public const string Create = "exercise:create";
        public const string Edit = "exercise:edit";
        public const string Delete = "exercise:delete";
    }

    public static class Users
    {
        public const string View = "user:view";
        public const string Create = "user:create";
        public const string Edit = "user:edit";
        public const string Disable = "user:disable";
    }

    public static class RoleManagement
    {
        public const string View = "role:view";
        public const string ManageUserRoles = "role:manage-user-roles";
        public const string ManagePermissions = "role:manage-permissions";
    }

    public static class Training
    {
        public const string CreateAthletes = "athlete:create";
        public const string ViewWorkouts = "workout:view";
        public const string CreateWorkouts = "workout:create";
        public const string ViewWorkoutPlans = "workoutplan:view";
        public const string CreateWorkoutPlans = "workoutplan:create";
        public const string ViewWorkoutTracking = "workouttracking:view";
        public const string StartWorkoutTracking = "workouttracking:start";
        public const string EditWorkoutTracking = "workouttracking:edit";
    }

    public static readonly IReadOnlyList<string> All = new[]
    {
        Exercises.View, Exercises.Create, Exercises.Edit, Exercises.Delete,
        Users.View, Users.Create, Users.Edit, Users.Disable,
        RoleManagement.View, RoleManagement.ManageUserRoles, RoleManagement.ManagePermissions,
        Training.CreateAthletes,
        Training.ViewWorkouts, Training.CreateWorkouts,
        Training.ViewWorkoutPlans, Training.CreateWorkoutPlans,
        Training.ViewWorkoutTracking, Training.StartWorkoutTracking, Training.EditWorkoutTracking
    };
}
