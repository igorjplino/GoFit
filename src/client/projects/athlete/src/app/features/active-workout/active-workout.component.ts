import { Component, OnInit, computed, effect, inject, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButton, MatIconButton } from '@angular/material/button';
import { MatCard, MatCardContent } from '@angular/material/card';
import { MatIcon } from '@angular/material/icon';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { MatDialog } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { WorkoutTrackingService } from '../../core/services/workout-tracking.service';
import { WorkoutService } from '../../core/services/workout.service';
import { SnackbarService } from '@gofit/shared/services/snackbar.service';
import { ConfirmDialogComponent } from '@gofit/shared/components/confirm-dialog/confirm-dialog.component';
import { Workout } from '@gofit/shared/models/workout-plan';
import { WorkoutTracking } from '@gofit/shared/models/workout-tracking';

type SetRow = {
  exerciseIndex: number;
  exerciseName: string;
  setNumber: number;
  globalIndex: number;
  plannedWeight: number | null;
  plannedReps: number;
  status: 'completed' | 'next' | 'upcoming';
  actualWeight?: number;
  actualReps?: number;
};

type ExerciseGroup = {
  exerciseIndex: number;
  exerciseName: string;
  rows: SetRow[];
};

@Component({
  selector: 'app-active-workout',
  standalone: true,
  imports: [
    RouterLink,
    ReactiveFormsModule,
    MatCard,
    MatCardContent,
    MatButton,
    MatIconButton,
    MatIcon,
    MatFormField,
    MatLabel,
    MatInput
  ],
  templateUrl: './active-workout.component.html',
  styleUrl: './active-workout.component.scss'
})
export class ActiveWorkoutComponent implements OnInit {
  private activatedRoute = inject(ActivatedRoute);
  private router = inject(Router);
  private workoutTrackingService = inject(WorkoutTrackingService);
  private workoutService = inject(WorkoutService);
  private dialog = inject(MatDialog);
  private snack = inject(SnackbarService);
  private fb = inject(FormBuilder);

  private trackingId?: string;

  tracking = signal<WorkoutTracking | null>(null);
  plannedWorkout = signal<Workout | null>(null);

  // Single in-flight guard for every set mutation (complete/edit/remove). Sets are addressed by position, so a
  // request sent before an earlier one shifted the positions could otherwise hit the wrong set.
  savingSet = signal(false);
  finishing = signal(false);
  cancelling = signal(false);
  setValidationErrors = signal<string[] | undefined>(undefined);

  // globalIndex of the completed set being edited, which is also its index into tracking().sets and its order on the server.
  editingIndex = signal<number | null>(null);

  logSetForm = this.fb.group({
    weight: [0, [Validators.required, Validators.min(0)]],
    repetitions: [0, [Validators.required, Validators.min(1)]]
  });

  editSetForm = this.fb.group({
    weight: [0, [Validators.required, Validators.min(0)]],
    repetitions: [0, [Validators.required, Validators.min(1)]]
  });

  private flatPlannedSets = computed(() => this.flattenPlannedSets(this.plannedWorkout()));

  totalSets = computed(() => this.flatPlannedSets().length);
  completedSets = computed(() => this.tracking()?.sets.length ?? 0);

  rows = computed<SetRow[]>(() => {
    const flat = this.flatPlannedSets();
    const track = this.tracking();
    const completedCount = track?.sets.length ?? 0;

    return flat.map((set, index) => {
      if (index < completedCount) {
        const actual = track!.sets[index];
        return { ...set, globalIndex: index, status: 'completed' as const, actualWeight: actual.weight, actualReps: actual.repetitions };
      }
      if (index === completedCount) {
        return { ...set, globalIndex: index, status: 'next' as const };
      }
      return { ...set, globalIndex: index, status: 'upcoming' as const };
    });
  });

  exerciseGroups = computed<ExerciseGroup[]>(() => {
    const groups: ExerciseGroup[] = [];

    for (const row of this.rows()) {
      const lastGroup = groups[groups.length - 1];
      if (!lastGroup || lastGroup.exerciseIndex !== row.exerciseIndex) {
        groups.push({ exerciseIndex: row.exerciseIndex, exerciseName: row.exerciseName, rows: [row] });
      } else {
        lastGroup.rows.push(row);
      }
    }

    return groups;
  });

  nextSet = computed(() => this.rows().find(row => row.status === 'next'));

  statusLabel = computed(() => {
    const track = this.tracking();
    if (!track) {
      return '';
    }
    if (track.cancelledDate) {
      return 'Cancelled';
    }
    if (track.endWorkoutDate) {
      return 'Finished';
    }
    return 'In Progress';
  });

  isActive = computed(() => this.statusLabel() === 'In Progress');

  constructor() {
    effect(() => {
      const next = this.nextSet();
      if (next) {
        this.logSetForm.patchValue({ weight: next.plannedWeight ?? 0, repetitions: next.plannedReps }, { emitEvent: false });
      }
    });
  }

  ngOnInit(): void {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (!id) {
      return;
    }

    this.trackingId = id;
    this.load();
  }

  logSet(): void {
    const track = this.tracking();
    if (!track || this.logSetForm.invalid || this.savingSet()) {
      return;
    }

    const { weight, repetitions } = this.logSetForm.value;

    this.mutateSets(this.workoutTrackingService.logSet(track.id, { weight: weight!, repetitions: repetitions! }));
  }

  startEdit(row: SetRow): void {
    if (!this.isActive() || this.savingSet()) {
      return;
    }

    this.setValidationErrors.set(undefined);
    this.editSetForm.setValue({ weight: row.actualWeight ?? 0, repetitions: row.actualReps ?? 0 });
    this.editingIndex.set(row.globalIndex);
  }

  cancelEdit(): void {
    this.editingIndex.set(null);
    this.setValidationErrors.set(undefined);
  }

  saveEdit(): void {
    const track = this.tracking();
    const index = this.editingIndex();
    if (!track || index === null || this.editSetForm.invalid || this.savingSet()) {
      return;
    }

    const { weight, repetitions } = this.editSetForm.value;

    this.mutateSets(
      this.workoutTrackingService.updateSet(track.id, index, { weight: weight!, repetitions: repetitions! }),
      () => this.editingIndex.set(null));
  }

  removeSet(row: SetRow): void {
    const track = this.tracking();
    if (!track || this.savingSet()) {
      return;
    }

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Remove set?',
        message: `Set ${row.setNumber} of ${row.exerciseName} will be removed from this workout.`,
        confirmLabel: 'Remove'
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (!confirmed) {
        return;
      }

      this.mutateSets(
        this.workoutTrackingService.removeSet(track.id, row.globalIndex),
        () => this.editingIndex.set(null));
    });
  }

  finish(): void {
    const track = this.tracking();
    if (!track || this.finishing()) {
      return;
    }

    const incomplete = this.totalSets() - this.completedSets();
    if (incomplete > 0) {
      const dialogRef = this.dialog.open(ConfirmDialogComponent, {
        data: {
          title: 'Finish workout?',
          message: `You still have ${incomplete} set${incomplete === 1 ? '' : 's'} remaining. Finish anyway?`,
          confirmLabel: 'Finish'
        }
      });

      dialogRef.afterClosed().subscribe(confirmed => {
        if (confirmed) {
          this.doFinish(track);
        }
      });
      return;
    }

    this.doFinish(track);
  }

  cancel(): void {
    const track = this.tracking();
    if (!track || this.cancelling()) {
      return;
    }

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Cancel workout?',
        message: 'Your current progress will be cancelled.',
        confirmLabel: 'Cancel workout',
        cancelLabel: 'Keep workout'
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (!confirmed) {
        return;
      }

      this.cancelling.set(true);

      this.workoutTrackingService.cancel(track.id).subscribe({
        next: () => {
          this.cancelling.set(false);
          this.workoutTrackingService.clearActive();
          this.snack.success('Workout cancelled');
          this.router.navigateByUrl('/');
        },
        error: () => this.cancelling.set(false)
      });
    });
  }

  private doFinish(track: WorkoutTracking): void {
    this.finishing.set(true);

    this.workoutTrackingService.finish(track.id).subscribe({
      next: () => {
        this.finishing.set(false);
        this.workoutTrackingService.clearActive();
        this.snack.success('Workout finished');
        this.router.navigateByUrl('/');
      },
      error: () => this.finishing.set(false)
    });
  }

  // Every set mutation funnels through here. The server numbers the sets, so its updated tracking replaces the local one.
  private mutateSets(request: Observable<WorkoutTracking>, onSuccess?: () => void): void {
    this.savingSet.set(true);
    this.setValidationErrors.set(undefined);

    request.subscribe({
      next: tracking => {
        this.tracking.set(tracking);
        this.savingSet.set(false);
        onSuccess?.();
      },
      error: errors => {
        this.savingSet.set(false);
        this.setValidationErrors.set(Array.isArray(errors) ? errors : undefined);
      }
    });
  }

  private load(): void {
    if (!this.trackingId) {
      return;
    }

    this.workoutTrackingService.getById(this.trackingId).subscribe({
      next: tracking => {
        this.tracking.set(tracking);

        this.workoutService.getById(tracking.workoutId).subscribe({
          next: workout => this.plannedWorkout.set(workout)
        });
      }
    });
  }

  private flattenPlannedSets(workout: Workout | null) {
    if (!workout) {
      return [];
    }

    const exercises = [...workout.workoutExercises].sort((a, b) => a.order - b.order);
    const flat: Omit<SetRow, 'globalIndex' | 'status' | 'actualWeight' | 'actualReps'>[] = [];

    exercises.forEach((exercise, exerciseIndex) => {
      const sets = [...exercise.sets].sort((a, b) => a.order - b.order);
      sets.forEach((set, i) => {
        flat.push({
          exerciseIndex,
          exerciseName: exercise.exerciseName ?? 'Exercise',
          setNumber: i + 1,
          plannedWeight: set.weight,
          plannedReps: set.minRepetitions
        });
      });
    });

    return flat;
  }
}
