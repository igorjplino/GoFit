namespace GoFit.Application.Models;

/// <summary>
/// The position a logged set should take after its tracking's sets are resequenced.
/// </summary>
public readonly record struct WorkoutSetPosition(Guid SetId, int Order);
