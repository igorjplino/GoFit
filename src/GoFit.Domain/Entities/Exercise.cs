using GoFit.Domain.Common;

namespace GoFit.Domain.Entities;
public class Exercise : BaseEntity
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}
