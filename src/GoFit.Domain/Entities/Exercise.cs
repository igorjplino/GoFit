namespace GoFit.Domain.Entities;
public class Exercise
{
    public string? Name { get; set; }
    public IEnumerable<ExerciseSet>? Sets { get; set; }
    public IEnumerable<ExerciseSet>? HistorySets { get; set; }
}
