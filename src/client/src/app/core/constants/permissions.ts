export const Permissions = {
    Exercises: {
        View: 'exercise:view',
        Create: 'exercise:create',
        Edit: 'exercise:edit',
        Delete: 'exercise:delete'
    },
    Users: {
        View: 'user:view',
        Create: 'user:create',
        Edit: 'user:edit',
        Disable: 'user:disable'
    },
    RoleManagement: {
        View: 'role:view',
        ManageUserRoles: 'role:manage-user-roles',
        ManagePermissions: 'role:manage-permissions'
    },
    Training: {
        ViewWorkouts: 'workout:view',
        CreateWorkouts: 'workout:create',
        EditWorkouts: 'workout:edit',
        ViewWorkoutPlans: 'workoutplan:view',
        CreateWorkoutPlans: 'workoutplan:create',
        EditWorkoutPlans: 'workoutplan:edit',
        ViewWorkoutTracking: 'workouttracking:view',
        StartWorkoutTracking: 'workouttracking:start',
        EditWorkoutTracking: 'workouttracking:edit'
    }
} as const;

export type PermissionCatalogEntry = {
    category: string;
    key: string;
    label: string;
}

export const PermissionCatalog: PermissionCatalogEntry[] = [
    { category: 'Exercises', key: Permissions.Exercises.View, label: 'View exercises' },
    { category: 'Exercises', key: Permissions.Exercises.Create, label: 'Create exercises' },
    { category: 'Exercises', key: Permissions.Exercises.Edit, label: 'Edit exercises' },
    { category: 'Exercises', key: Permissions.Exercises.Delete, label: 'Delete exercises' },

    { category: 'Users', key: Permissions.Users.View, label: 'View users' },
    { category: 'Users', key: Permissions.Users.Create, label: 'Create users' },
    { category: 'Users', key: Permissions.Users.Edit, label: 'Edit users' },
    { category: 'Users', key: Permissions.Users.Disable, label: 'Disable users' },

    { category: 'Roles & Permissions', key: Permissions.RoleManagement.View, label: 'View roles & permissions' },
    { category: 'Roles & Permissions', key: Permissions.RoleManagement.ManageUserRoles, label: 'Manage user roles' },
    { category: 'Roles & Permissions', key: Permissions.RoleManagement.ManagePermissions, label: 'Manage role permissions' },

    { category: 'Training', key: Permissions.Training.ViewWorkouts, label: 'View workouts' },
    { category: 'Training', key: Permissions.Training.CreateWorkouts, label: 'Create workouts' },
    { category: 'Training', key: Permissions.Training.EditWorkouts, label: 'Edit workouts' },
    { category: 'Training', key: Permissions.Training.ViewWorkoutPlans, label: 'View workout plans' },
    { category: 'Training', key: Permissions.Training.CreateWorkoutPlans, label: 'Create workout plans' },
    { category: 'Training', key: Permissions.Training.EditWorkoutPlans, label: 'Edit workout plans' },
    { category: 'Training', key: Permissions.Training.ViewWorkoutTracking, label: 'View workout tracking' },
    { category: 'Training', key: Permissions.Training.StartWorkoutTracking, label: 'Start workout tracking' },
    { category: 'Training', key: Permissions.Training.EditWorkoutTracking, label: 'Update workout tracking' }
];
