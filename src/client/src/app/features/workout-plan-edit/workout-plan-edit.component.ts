import { Component, OnInit, inject, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { forkJoin } from 'rxjs';
import { MatButton } from '@angular/material/button';
import { MatCard } from '@angular/material/card';
import { WorkoutPlanService } from '../../core/services/workout-plan.service';
import { WorkoutService } from '../../core/services/workout.service';
import { SnackbarService } from '../../core/services/snackbar.service';
import { TextInputComponent } from '../../shared/components/text-input/text-input.component';
import { WorkoutFormComponent } from '../workout-form/workout-form.component';
import {
  CreateWorkoutRequest,
  UpdateWorkoutRequest,
  Workout,
  WorkoutDraft,
  WorkoutExercise,
  WorkoutExerciseDraft,
  WorkoutPlan,
  WorkoutSet
} from '../../shared/models/workout-plan';

@Component({
  selector: 'app-workout-plan-edit',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    RouterLink,
    MatCard,
    MatButton,
    TextInputComponent,
    WorkoutFormComponent
  ],
  templateUrl: './workout-plan-edit.component.html',
  styleUrl: './workout-plan-edit.component.scss'
})
export class WorkoutPlanEditComponent implements OnInit {
  private fb = inject(FormBuilder);
  private workoutPlanService = inject(WorkoutPlanService);
  private workoutService = inject(WorkoutService);
  private activatedRoute = inject(ActivatedRoute);
  private router = inject(Router);
  private snack = inject(SnackbarService);

  private planId?: string;
  private workoutIds: (string | undefined)[] = [];
  private removedWorkoutIds: string[] = [];

  plan?: WorkoutPlan;
  submitting = signal(false);
  validationErrors?: string[];
  workoutDrafts: WorkoutDraft[] = [];

  planForm = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
    description: ['', [Validators.minLength(3), Validators.maxLength(300)]]
  });

  ngOnInit(): void {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (!id) {
      return;
    }

    this.planId = id;

    this.workoutPlanService.getById(id).subscribe({
      next: plan => {
        this.plan = plan;
        this.planForm.patchValue({
          name: plan.title,
          description: plan.description ?? ''
        });
        this.workoutDrafts = plan.workouts.map(workout => this.toDraft(workout));
        this.workoutIds = plan.workouts.map(workout => workout.id);
      }
    });
  }

  addWorkout(): void {
    this.workoutDrafts = [...this.workoutDrafts, this.blankDraft()];
    this.workoutIds = [...this.workoutIds, undefined];
  }

  onDraftChange(index: number, draft: WorkoutDraft): void {
    this.workoutDrafts = this.workoutDrafts.map((d, i) => i === index ? draft : d);
  }

  removeWorkout(index: number): void {
    const removedId = this.workoutIds[index];
    if (removedId) {
      this.removedWorkoutIds = [...this.removedWorkoutIds, removedId];
    }
    this.workoutDrafts = this.workoutDrafts.filter((_, i) => i !== index);
    this.workoutIds = this.workoutIds.filter((_, i) => i !== index);
  }

  get canSubmit(): boolean {
    return this.planForm.valid
      && !this.submitting()
      && this.workoutDrafts.length > 0
      && this.workoutDrafts.every(d => d.name && d.name.trim().length >= 3 && d.exercises.length > 0);
  }

  onSubmit(): void {
    if (!this.planId || !this.canSubmit) {
      return;
    }

    this.submitting.set(true);
    this.validationErrors = undefined;

    const { name, description } = this.planForm.value;
    const normalizedDescription = description ? description : null;

    const planUpdate$ = this.workoutPlanService.update(this.planId, {
      title: name!,
      description: normalizedDescription
    });

    const workoutRequests$ = this.workoutDrafts.map((draft, index) => {
      const workoutExercises = draft.exercises.map((entry, i) => ({
        exerciseId: entry.exerciseId,
        order: i,
        sets: this.expandSets(entry)
      }));
      const existingId = this.workoutIds[index];

      if (existingId) {
        const payload: UpdateWorkoutRequest = {
          name: draft.name,
          description: draft.description ? draft.description : null,
          workoutExercises
        };
        return this.workoutService.update(existingId, payload);
      }

      const payload: CreateWorkoutRequest = {
        workoutPlanId: this.planId!,
        name: draft.name,
        description: draft.description ? draft.description : null,
        workoutExercises
      };
      return this.workoutService.create(payload);
    });

    const deleteRequests$ = this.removedWorkoutIds.map(id => this.workoutService.delete(id));

    forkJoin([planUpdate$, ...workoutRequests$, ...deleteRequests$]).subscribe({
      next: () => {
        this.snack.success('Workout plan updated successfully');
        this.router.navigateByUrl(`/workout-plans/${this.planId}`);
      },
      error: errors => {
        this.submitting.set(false);
        this.validationErrors = Array.isArray(errors) ? errors : undefined;
      }
    });
  }

  private toDraft(workout: Workout): WorkoutDraft {
    return {
      name: workout.name,
      description: workout.description,
      exercises: workout.workoutExercises.map(exercise => this.toExerciseDraft(exercise))
    };
  }

  private toExerciseDraft(exercise: WorkoutExercise): WorkoutExerciseDraft {
    return {
      exerciseId: exercise.exerciseId,
      exerciseName: exercise.exerciseName ?? '',
      numberOfSets: exercise.sets.length,
      weight: exercise.sets[0]?.weight ?? 0,
      repetitions: exercise.sets[0]?.maxRepetitions ?? 0,
      order: exercise.order
    };
  }

  private blankDraft(): WorkoutDraft {
    return { name: '', description: null, exercises: [] };
  }

  private expandSets(entry: WorkoutExerciseDraft): WorkoutSet[] {
    return Array.from({ length: entry.numberOfSets }, (_, i) => ({
      warmUp: false,
      untilFailure: false,
      minRepetitions: entry.repetitions,
      maxRepetitions: entry.repetitions,
      resetTime: 60,
      weight: entry.weight,
      order: i
    }));
  }
}
