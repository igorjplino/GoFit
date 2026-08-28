namespace GoFit.Domain.Authorization;

public static class AppRoles
{
    public const string Admin = "Admin";
    public const string Athlete = "Athlete";

    public static readonly IReadOnlyList<string> All = new[] { Admin, Athlete };
}
