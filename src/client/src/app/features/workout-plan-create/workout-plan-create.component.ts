import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { MatButton } from '@angular/material/button';
import { MatCard } from '@angular/material/card';
import { WorkoutPlanService } from '../../core/services/workout-plan.service';
import { SnackbarService } from '../../core/services/snackbar.service';
import { TextInputComponent } from '../../shared/components/text-input/text-input.component';
import { WorkoutFormComponent } from '../workout-form/workout-form.component';
import { CreateWorkoutPlanRequest, WorkoutDraft, WorkoutExerciseDraft, WorkoutSet } from '../../shared/models/workout-plan';

@Component({
  selector: 'app-workout-plan-create',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    RouterLink,
    MatCard,
    MatButton,
    TextInputComponent,
    WorkoutFormComponent
  ],
  templateUrl: './workout-plan-create.component.html',
  styleUrl: './workout-plan-create.component.scss'
})
export class WorkoutPlanCreateComponent {
  private fb = inject(FormBuilder);
  private workoutPlanService = inject(WorkoutPlanService);
  private router = inject(Router);
  private snack = inject(SnackbarService);

  submitting = signal(false);
  validationErrors?: string[];
  workoutDrafts: WorkoutDraft[] = [this.blankDraft()];

  planForm = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
    description: ['', [Validators.minLength(3), Validators.maxLength(300)]]
  });

  addWorkout(): void {
    this.workoutDrafts = [...this.workoutDrafts, this.blankDraft()];
  }

  onDraftChange(index: number, draft: WorkoutDraft): void {
    this.workoutDrafts = this.workoutDrafts.map((d, i) => i === index ? draft : d);
  }

  removeWorkout(index: number): void {
    this.workoutDrafts = this.workoutDrafts.filter((_, i) => i !== index);
  }

  get canSubmit(): boolean {
    return this.planForm.valid
      && !this.submitting()
      && this.workoutDrafts.length > 0
      && this.workoutDrafts.every(d => d.name && d.name.trim().length >= 3 && d.exercises.length > 0);
  }

  onSubmit(): void {
    if (!this.canSubmit) {
      return;
    }

    this.submitting.set(true);
    this.validationErrors = undefined;

    const { name, description } = this.planForm.value;

    const payload: CreateWorkoutPlanRequest = {
      title: name!,
      description: description ? description : null,
      workouts: this.workoutDrafts.map((draft, index) => ({
        name: draft.name,
        description: draft.description ? draft.description : null,
        order: index,
        workoutExercises: draft.exercises.map((entry, i) => ({
          exerciseId: entry.exerciseId,
          order: i,
          sets: this.expandSets(entry)
        }))
      }))
    };

    this.workoutPlanService.create(payload).subscribe({
      next: id => {
        this.snack.success('Workout plan created successfully');
        this.router.navigateByUrl(`/workout-plans/${id}`);
      },
      error: errors => {
        this.submitting.set(false);
        this.validationErrors = Array.isArray(errors) ? errors : undefined;
      }
    });
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
