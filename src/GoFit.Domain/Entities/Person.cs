using GoFit.Domain.Common;
using GoFit.Domain.Enums;

namespace GoFit.Domain.Entities;
public class Person : BaseEntity
{
    public string? Name { get; set; }
    public string? DisplayName { get; set; }
    public string? Email { get; set; }
    public PersonType Type { get; set; }
}
