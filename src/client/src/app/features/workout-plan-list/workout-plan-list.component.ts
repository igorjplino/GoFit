import { Component, OnInit, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { MatButton } from '@angular/material/button';
import { MatCard, MatCardContent } from '@angular/material/card';
import { WorkoutPlanService } from '../../core/services/workout-plan.service';
import { WorkoutPlan } from '../../shared/models/workout-plan';
import { AccountService } from '../../core/services/account.service';
import { Permissions } from '../../core/constants/permissions';

@Component({
  selector: 'app-workout-plan-list',
  standalone: true,
  imports: [RouterLink, MatButton, MatCard, MatCardContent],
  templateUrl: './workout-plan-list.component.html',
  styleUrl: './workout-plan-list.component.scss'
})
export class WorkoutPlanListComponent implements OnInit {
  private workoutPlanService = inject(WorkoutPlanService);
  accountService = inject(AccountService);
  permissions = Permissions;

  workoutPlans: WorkoutPlan[] = [];

  ngOnInit(): void {
    this.workoutPlanService.listMine().subscribe({
      next: response => this.workoutPlans = response
    });
  }

  exerciseCount(plan: WorkoutPlan): number {
    return plan.workouts?.[0]?.workoutExercises?.length ?? 0;
  }
}
