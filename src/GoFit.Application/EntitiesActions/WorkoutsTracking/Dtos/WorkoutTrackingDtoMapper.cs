using GoFit.Domain.Entities;
using WorkoutDto = GoFit.Application.EntitiesActions.Workouts.Dtos.WorkoutDto;

namespace GoFit.Application.EntitiesActions.WorkoutsTracking.Dtos;

internal static class WorkoutTrackingDtoMapper
{
    public static WorkoutTrackingDto ToDto(WorkoutTracking workoutTracking)
        => new()
        {
            Id = workoutTracking.Id,
            WorkoutId = workoutTracking.WorkoutId,
            Workout = new WorkoutDto
            {
                Id = workoutTracking.Workout.Id,
                WorkoutPlanId = workoutTracking.Workout.WorkoutPlanId,
                Name = workoutTracking.Workout.Name,
                Description = workoutTracking.Workout.Description,
                Order = workoutTracking.Workout.Order
            },
            StartWorkoutDate = workoutTracking.StartWorkoutDate,
            EndWorkoutDate = workoutTracking.EndWorkoutDate,
            CancelledDate = workoutTracking.CancelledDate,
            Note = workoutTracking.Note,
            // Ordered explicitly: the client matches the n-th logged set to the n-th planned set by
            // position, and UpdateWorkoutTrackingAsync reinserts every row with a fresh key on each save.
            Sets = workoutTracking.Sets.OrderBy(o => o.Order).Select(o => new WorkoutSetTrackingDto
            {
                Order = o.Order,
                Repetitions = o.Repetitions,
                Weight = o.Weight
            }).ToList()
        };

    public static WorkoutTrackingSummaryDto ToSummaryDto(WorkoutTracking workoutTracking)
        => new()
        {
            Id = workoutTracking.Id,
            WorkoutId = workoutTracking.WorkoutId,
            WorkoutName = workoutTracking.Workout.Name,
            StartWorkoutDate = workoutTracking.StartWorkoutDate,
            EndWorkoutDate = workoutTracking.EndWorkoutDate,
            CancelledDate = workoutTracking.CancelledDate,
            Note = workoutTracking.Note,
            SetCount = workoutTracking.Sets.Count
        };
}
