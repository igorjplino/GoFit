namespace GoFit.Application.EntitiesActions.Athletes.Dtos;

public record RegisteredAthleteDto
{
    public string DisplayName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
