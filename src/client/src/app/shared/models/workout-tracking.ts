import { Workout } from './workout-plan';

export type WorkoutSetTracking = {
    repetitions: number;
    weight: number;
    order: number;
}

export type WorkoutTracking = {
    id: string;
    workoutId: string;
    workout: Workout;
    startWorkoutDate: string;
    endWorkoutDate?: string | null;
    cancelledDate?: string | null;
    note?: string | null;
    sets: WorkoutSetTracking[];
}

export type StartWorkoutTrackingRequest = {
    workoutId: string;
    note?: string | null;
    sets: WorkoutSetTracking[];
}

export type UpdateWorkoutTrackingRequest = {
    workoutsTrackingId: string;
    startWorkoutDate: string;
    endWorkoutDate?: string | null;
    note?: string | null;
    sets: WorkoutSetTracking[];
}
