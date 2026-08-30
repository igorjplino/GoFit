export type WorkoutSet = {
    warmUp: boolean;
    untilFailure: boolean;
    minRepetitions: number;
    maxRepetitions: number;
    resetTime: number;
    weight: number;
    order: number;
}

export type WorkoutExercise = {
    exerciseId: string;
    exerciseName?: string;
    order: number;
    sets: WorkoutSet[];
}

export type Workout = {
    id?: string;
    workoutPlanId?: string;
    name: string;
    description?: string | null;
    order: number;
    workoutExercises: WorkoutExercise[];
}

export type CreateWorkoutPlanRequest = {
    title: string;
    description?: string | null;
    workouts: Workout[];
}

export type WorkoutPlan = CreateWorkoutPlanRequest & {
    id: string;
}

export type WorkoutExerciseDraft = {
    exerciseId: string;
    exerciseName: string;
    numberOfSets: number;
    weight: number;
    repetitions: number;
    order: number;
}

export type WorkoutDraft = {
    name: string;
    description?: string | null;
    exercises: WorkoutExerciseDraft[];
}

export type UpdateWorkoutRequest = {
    name: string;
    description?: string | null;
    workoutExercises: WorkoutExercise[];
}

export type CreateWorkoutRequest = {
    workoutPlanId: string;
    name: string;
    description?: string | null;
    workoutExercises: WorkoutExercise[];
}

export type UpdateWorkoutPlanRequest = {
    title: string;
    description?: string | null;
}
