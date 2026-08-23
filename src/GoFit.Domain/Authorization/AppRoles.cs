namespace GoFit.Domain.Authorization;

public static class AppRoles
{
    public const string Admin = "Admin";
    public const string Student = "Student";

    public static readonly IReadOnlyList<string> All = new[] { Admin, Student };
}
