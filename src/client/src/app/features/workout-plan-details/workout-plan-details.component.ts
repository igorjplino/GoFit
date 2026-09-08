import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { MatButton, MatIconButton } from '@angular/material/button';
import { MatCard, MatCardContent } from '@angular/material/card';
import { MatIcon } from '@angular/material/icon';
import { WorkoutPlanService } from '../../core/services/workout-plan.service';
import { WorkoutTrackingService } from '../../core/services/workout-tracking.service';
import { AccountService } from '../../core/services/account.service';
import { SnackbarService } from '../../core/services/snackbar.service';
import { Permissions } from '../../core/constants/permissions';
import { Workout, WorkoutPlan } from '../../shared/models/workout-plan';

@Component({
  selector: 'app-workout-plan-details',
  standalone: true,
  imports: [
    RouterLink,
    MatCard,
    MatCardContent,
    MatButton,
    MatIconButton,
    MatIcon
  ],
  templateUrl: './workout-plan-details.component.html',
  styleUrl: './workout-plan-details.component.scss'
})
export class WorkoutPlanDetailsComponent implements OnInit {
  private workoutPlanService = inject(WorkoutPlanService);
  private workoutTrackingService = inject(WorkoutTrackingService);
  private activatedRoute = inject(ActivatedRoute);
  private router = inject(Router);
  private snack = inject(SnackbarService);
  accountService = inject(AccountService);
  permissions = Permissions;

  startingWorkoutId?: string;

  private planId?: string;

  workoutPlan?: WorkoutPlan;

  ngOnInit(): void {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (!id) {
      return;
    }

    this.planId = id;
    this.load();
    this.loadActiveWorkout();
  }

  exerciseCount(workout: Workout): number {
    return workout.workoutExercises?.length ?? 0;
  }

  // --- Start workout ---

  isWorkoutActive(workout: Workout): boolean {
    return this.workoutTrackingService.activeWorkout()?.workoutId === workout.id;
  }

  onContinueWorkout(event: Event): void {
    event.stopPropagation();
    event.preventDefault();

    const active = this.workoutTrackingService.activeWorkout();
    if (!active) {
      return;
    }

    this.router.navigateByUrl('/active-workout/' + active.id);
  }

  onStartWorkout(event: Event, workout: Workout): void {
    event.stopPropagation();
    event.preventDefault();

    if (!workout.id || this.startingWorkoutId) {
      return;
    }

    this.startingWorkoutId = workout.id;

    this.workoutTrackingService.getMine().subscribe({
      next: active => {
        if (active) {
          this.startingWorkoutId = undefined;
          this.snack.error('You already have an active workout in progress.');
          this.router.navigateByUrl('/active-workout/' + active.id);
          return;
        }

        this.workoutTrackingService.start({ workoutId: workout.id!, note: null }).subscribe({
          next: newId => {
            this.startingWorkoutId = undefined;
            this.router.navigateByUrl('/active-workout/' + newId);
          },
          error: () => this.startingWorkoutId = undefined
        });
      },
      error: () => this.startingWorkoutId = undefined
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

  private loadActiveWorkout(): void {
    if (!this.accountService.hasPermission(this.permissions.Training.ViewWorkoutTracking)) {
      return;
    }

    this.workoutTrackingService.getMine().subscribe({ error: () => {} });
  }
}
