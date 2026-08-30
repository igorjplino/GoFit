import { Component, OnInit, inject, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatCard } from '@angular/material/card';
import { WorkoutService } from '../../core/services/workout.service';
import { SnackbarService } from '../../core/services/snackbar.service';
import { TextInputComponent } from '../../shared/components/text-input/text-input.component';
import { WorkoutExercisesFormComponent } from '../../shared/components/workout-exercises-form/workout-exercises-form.component';
import { UpdateWorkoutRequest, Workout, WorkoutExercise, WorkoutExerciseDraft, WorkoutSet } from '../../shared/models/workout-plan';

@Component({
  selector: 'app-workout-edit',
  standalone: true,
  imports: [RouterLink, ReactiveFormsModule, MatButton, MatCard, TextInputComponent, WorkoutExercisesFormComponent],
  templateUrl: './workout-edit.component.html',
  styleUrl: './workout-edit.component.scss'
})
export class WorkoutEditComponent implements OnInit {
  private workoutService = inject(WorkoutService);
  private activatedRoute = inject(ActivatedRoute);
  private router = inject(Router);
  private snack = inject(SnackbarService);
  private fb = inject(FormBuilder);

  private workoutId?: string;

  workout?: Workout;
  entries: WorkoutExerciseDraft[] = [];
  submitting = signal(false);
  validationErrors?: string[];

  form = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
    description: ['', [Validators.minLength(3), Validators.maxLength(300)]]
  });

  ngOnInit(): void {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (!id) {
      return;
    }

    this.workoutId = id;

    this.workoutService.getById(id).subscribe({
      next: response => {
        this.workout = response;
        this.entries = response.workoutExercises.map(exercise => this.toDraft(exercise));
        this.form.patchValue({
          name: response.name,
          description: response.description ?? ''
        });
      }
    });
  }

  onEntriesChange(entries: WorkoutExerciseDraft[]): void {
    this.entries = entries;
  }

  get canSave(): boolean {
    return this.form.valid && this.entries.length > 0 && !this.submitting();
  }

  onSave(): void {
    if (!this.workoutId || this.submitting() || this.form.invalid) {
      return;
    }

    if (this.entries.length === 0) {
      this.snack.error('Add at least one exercise before saving.');
      return;
    }

    this.submitting.set(true);
    this.validationErrors = undefined;

    const { name, description } = this.form.value;

    const payload: UpdateWorkoutRequest = {
      name: name!,
      description: description ? description : null,
      workoutExercises: this.entries.map((entry, index) => ({
        exerciseId: entry.exerciseId,
        order: index,
        sets: this.expandSets(entry)
      }))
    };

    this.workoutService.update(this.workoutId, payload).subscribe({
      next: () => {
        this.snack.success('Workout updated successfully');
        this.router.navigateByUrl(`/workout-plans/${this.workout?.workoutPlanId}`);
      },
      error: errors => {
        this.submitting.set(false);
        this.validationErrors = Array.isArray(errors) ? errors : undefined;
      }
    });
  }

  private toDraft(exercise: WorkoutExercise): WorkoutExerciseDraft {
    return {
      exerciseId: exercise.exerciseId,
      exerciseName: exercise.exerciseName ?? '',
      numberOfSets: exercise.sets.length,
      weight: exercise.sets[0]?.weight ?? 0,
      repetitions: exercise.sets[0]?.maxRepetitions ?? 0,
      order: exercise.order
    };
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
