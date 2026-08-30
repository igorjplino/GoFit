import { Component, OnInit, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButton, MatIconButton } from '@angular/material/button';
import { MatCard, MatCardContent } from '@angular/material/card';
import { MatIcon } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { WorkoutPlanService } from '../../core/services/workout-plan.service';
import { WorkoutService } from '../../core/services/workout.service';
import { AccountService } from '../../core/services/account.service';
import { SnackbarService } from '../../core/services/snackbar.service';
import { Permissions } from '../../core/constants/permissions';
import { TextInputComponent } from '../../shared/components/text-input/text-input.component';
import { WorkoutFormComponent } from '../workout-form/workout-form.component';
import { ConfirmDialogComponent } from '../../shared/components/confirm-dialog/confirm-dialog.component';
import { CreateWorkoutRequest, Workout, WorkoutDraft, WorkoutExerciseDraft, WorkoutPlan, WorkoutSet } from '../../shared/models/workout-plan';

@Component({
  selector: 'app-workout-plan-details',
  standalone: true,
  imports: [
    RouterLink,
    ReactiveFormsModule,
    MatCard,
    MatCardContent,
    MatButton,
    MatIconButton,
    MatIcon,
    TextInputComponent,
    WorkoutFormComponent
  ],
  templateUrl: './workout-plan-details.component.html',
  styleUrl: './workout-plan-details.component.scss'
})
export class WorkoutPlanDetailsComponent implements OnInit {
  private workoutPlanService = inject(WorkoutPlanService);
  private workoutService = inject(WorkoutService);
  private activatedRoute = inject(ActivatedRoute);
  private fb = inject(FormBuilder);
  private dialog = inject(MatDialog);
  private snack = inject(SnackbarService);
  accountService = inject(AccountService);
  permissions = Permissions;

  private planId?: string;

  workoutPlan?: WorkoutPlan;

  editingPlan = signal(false);
  savingPlan = signal(false);
  planValidationErrors?: string[];

  planForm = this.fb.group({
    title: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
    description: ['', [Validators.minLength(3), Validators.maxLength(300)]]
  });

  addingWorkout = signal(false);
  savingWorkout = signal(false);
  workoutValidationErrors?: string[];
  newWorkoutDraft: WorkoutDraft = this.blankDraft();

  deletingWorkoutId?: string;

  ngOnInit(): void {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (!id) {
      return;
    }

    this.planId = id;
    this.load();
  }

  exerciseCount(workout: Workout): number {
    return workout.workoutExercises?.length ?? 0;
  }

  // --- Edit plan title/description ---

  startEditPlan(): void {
    if (!this.workoutPlan) {
      return;
    }

    this.planForm.patchValue({
      title: this.workoutPlan.title,
      description: this.workoutPlan.description ?? ''
    });
    this.planValidationErrors = undefined;
    this.editingPlan.set(true);
  }

  cancelEditPlan(): void {
    this.editingPlan.set(false);
    this.planValidationErrors = undefined;
  }

  get canSavePlan(): boolean {
    return this.planForm.valid && !this.savingPlan();
  }

  saveWorkoutPlan(): void {
    if (!this.planId || !this.canSavePlan) {
      return;
    }

    this.savingPlan.set(true);
    this.planValidationErrors = undefined;

    const { title, description } = this.planForm.value;
    const normalizedDescription = description ? description : null;

    this.workoutPlanService.update(this.planId, {
      title: title!,
      description: normalizedDescription
    }).subscribe({
      next: () => {
        if (this.workoutPlan) {
          this.workoutPlan.title = title!;
          this.workoutPlan.description = normalizedDescription;
        }
        this.savingPlan.set(false);
        this.editingPlan.set(false);
        this.snack.success('Workout plan updated successfully');
      },
      error: errors => {
        this.savingPlan.set(false);
        this.planValidationErrors = Array.isArray(errors) ? errors : undefined;
      }
    });
  }

  // --- Add workout ---

  showAddWorkout(): void {
    this.newWorkoutDraft = this.blankDraft();
    this.workoutValidationErrors = undefined;
    this.addingWorkout.set(true);
  }

  cancelAddWorkout(): void {
    this.addingWorkout.set(false);
    this.workoutValidationErrors = undefined;
  }

  onNewWorkoutDraftChange(draft: WorkoutDraft): void {
    this.newWorkoutDraft = draft;
  }

  get canSaveWorkout(): boolean {
    return !this.savingWorkout()
      && !!this.newWorkoutDraft.name
      && this.newWorkoutDraft.name.trim().length >= 3
      && this.newWorkoutDraft.exercises.length > 0;
  }

  saveWorkout(): void {
    if (!this.planId || !this.canSaveWorkout) {
      return;
    }

    this.savingWorkout.set(true);
    this.workoutValidationErrors = undefined;

    const payload: CreateWorkoutRequest = {
      workoutPlanId: this.planId,
      name: this.newWorkoutDraft.name,
      description: this.newWorkoutDraft.description ? this.newWorkoutDraft.description : null,
      workoutExercises: this.newWorkoutDraft.exercises.map((entry, i) => ({
        exerciseId: entry.exerciseId,
        order: i,
        sets: this.expandSets(entry)
      }))
    };

    this.workoutService.create(payload).subscribe({
      next: () => {
        this.savingWorkout.set(false);
        this.addingWorkout.set(false);
        this.snack.success('Workout added successfully');
        this.load();
      },
      error: errors => {
        this.savingWorkout.set(false);
        this.workoutValidationErrors = Array.isArray(errors) ? errors : undefined;
      }
    });
  }

  // --- Delete workout ---

  onDeleteWorkout(event: Event, workout: Workout): void {
    event.stopPropagation();
    event.preventDefault();

    if (!workout.id || this.deletingWorkoutId) {
      return;
    }

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Delete workout',
        message: `Delete "${workout.name}"? This cannot be undone.`,
        confirmLabel: 'Delete'
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (!confirmed || !workout.id) {
        return;
      }

      this.deletingWorkoutId = workout.id;

      this.workoutService.delete(workout.id).subscribe({
        next: () => {
          if (this.workoutPlan) {
            this.workoutPlan.workouts = this.workoutPlan.workouts.filter(w => w.id !== workout.id);
          }
          this.deletingWorkoutId = undefined;
          this.snack.success('Workout deleted');
        },
        error: () => this.deletingWorkoutId = undefined
      });
    });
  }

  private load(): void {
    if (!this.planId) {
      return;
    }

    this.workoutPlanService.getById(this.planId).subscribe({
      next: response => this.workoutPlan = response
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
