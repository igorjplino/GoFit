import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { StartWorkoutTrackingRequest, UpdateWorkoutTrackingRequest, WorkoutTracking, WorkoutTrackingSummary } from '../../shared/models/workout-tracking';

@Injectable({
  providedIn: 'root'
})
export class WorkoutTrackingService {
  baseUrl = environment.apiUrl;
  activeWorkout = signal<WorkoutTracking | null>(null);

  constructor(private http: HttpClient) {}

  getMine() {
    return this.http.get<WorkoutTracking | null>(this.baseUrl + 'workouttracking/mine').pipe(
      tap(tracking => this.activeWorkout.set(tracking))
    );
  }

  getById(id: string) {
    return this.http.get<WorkoutTracking>(this.baseUrl + 'workouttracking/' + id).pipe(
      tap(tracking => this.activeWorkout.set(tracking))
    );
  }

  // No tap into activeWorkout: history is finished work, unrelated to the active-workout signal.
  listHistory() {
    return this.http.get<WorkoutTrackingSummary[]>(this.baseUrl + 'workouttracking/mine/history');
  }

  start(payload: StartWorkoutTrackingRequest) {
    return this.http.post<string>(this.baseUrl + 'workouttracking', payload);
  }

  update(id: string, payload: UpdateWorkoutTrackingRequest) {
    return this.http.put<UpdateWorkoutTrackingRequest>(this.baseUrl + 'workouttracking/' + id, payload);
  }

  cancel(id: string) {
    return this.http.put<string>(this.baseUrl + 'workouttracking/' + id + '/cancel', {});
  }

  clearActive() {
    this.activeWorkout.set(null);
  }
}
