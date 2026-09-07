import { Component, OnInit, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { MatButton } from '@angular/material/button';
import { MatCard, MatCardContent } from '@angular/material/card';
import { AccountService } from '../../../core/services/account.service';
import { WorkoutTrackingService } from '../../../core/services/workout-tracking.service';
import { Permissions } from '../../../core/constants/permissions';

@Component({
  selector: 'app-athlete-home',
  standalone: true,
  imports: [RouterLink, MatButton, MatCard, MatCardContent],
  templateUrl: './athlete-home.component.html',
  styleUrl: './athlete-home.component.scss'
})
export class AthleteHomeComponent implements OnInit {
  accountService = inject(AccountService);
  workoutTrackingService = inject(WorkoutTrackingService);
  permissions = Permissions;

  ngOnInit(): void {
    if (this.accountService.hasPermission(this.permissions.Training.ViewWorkoutTracking)) {
      this.workoutTrackingService.getMine().subscribe();
    }
  }
}
