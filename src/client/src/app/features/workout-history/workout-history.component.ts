import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatCard, MatCardContent } from '@angular/material/card';
import { WorkoutTrackingService } from '../../core/services/workout-tracking.service';
import { WorkoutTrackingSummary } from '../../shared/models/workout-tracking';

type HistoryFilter = 'all' | 'completed' | 'cancelled';

@Component({
  selector: 'app-workout-history',
  standalone: true,
  imports: [DatePipe, RouterLink, MatButtonToggleModule, MatCard, MatCardContent],
  templateUrl: './workout-history.component.html',
  styleUrl: './workout-history.component.scss'
})
export class WorkoutHistoryComponent implements OnInit {
  private workoutTrackingService = inject(WorkoutTrackingService);

  trackings = signal<WorkoutTrackingSummary[]>([]);
  loaded = signal(false);
  filter = signal<HistoryFilter>('all');

  completedCount = computed(() => this.trackings().filter(t => !this.isCancelled(t)).length);
  cancelledCount = computed(() => this.trackings().filter(t => this.isCancelled(t)).length);

  // Filtering client-side: the endpoint already returns the athlete's whole history,
  // so switching tabs costs no extra request.
  visible = computed(() => {
    const all = this.trackings();

    switch (this.filter()) {
      case 'completed':
        return all.filter(t => !this.isCancelled(t));
      case 'cancelled':
        return all.filter(t => this.isCancelled(t));
      default:
        return all;
    }
  });

  ngOnInit(): void {
    this.workoutTrackingService.listHistory().subscribe({
      next: response => {
        this.trackings.set(response);
        this.loaded.set(true);
      }
    });
  }

  isCancelled(tracking: WorkoutTrackingSummary): boolean {
    return !!tracking.cancelledDate;
  }

  statusLabel(tracking: WorkoutTrackingSummary): string {
    return this.isCancelled(tracking) ? 'Cancelled' : 'Completed';
  }

  durationLabel(tracking: WorkoutTrackingSummary): string {
    if (!tracking.endWorkoutDate) {
      return '';
    }

    const start = new Date(tracking.startWorkoutDate).getTime();
    const end = new Date(tracking.endWorkoutDate).getTime();
    const minutes = Math.max(0, Math.round((end - start) / 60000));

    if (minutes < 60) {
      return `${minutes} min`;
    }

    return `${Math.floor(minutes / 60)}h ${minutes % 60}min`;
  }

  setsLabel(tracking: WorkoutTrackingSummary): string {
    return `${tracking.setCount} ${tracking.setCount === 1 ? 'set' : 'sets'}`;
  }
}
