namespace GoFit.Application.EntitiesActions.Roles.Dtos;
public class RoleDto
{
    public string Name { get; set; } = string.Empty;
    public string[] Permissions { get; set; } = Array.Empty<string>();
}
