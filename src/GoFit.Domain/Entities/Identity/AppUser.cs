using Microsoft.AspNetCore.Identity;

namespace GoFit.Domain.Entities.Identity;
public class AppUser : IdentityUser
{
    public string DisplayName { get; set; }
}
