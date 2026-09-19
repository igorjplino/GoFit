import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { tap } from 'rxjs';
import { environment } from '@gofit/shared/environments/environment';
import { StartWorkoutTrackingRequest, WorkoutSetRequest, WorkoutTracking, WorkoutTrackingSummary } from '@gofit/shared/models/workout-tracking';

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

  // Set changes are answered with the updated tracking: the server owns set order, so its copy replaces the local one.
  logSet(id: string, payload: WorkoutSetRequest) {
    return this.http.post<WorkoutTracking>(this.baseUrl + 'workouttracking/' + id + '/sets', payload).pipe(
      tap(tracking => this.activeWorkout.set(tracking))
    );
  }

  updateSet(id: string, order: number, payload: WorkoutSetRequest) {
    return this.http.put<WorkoutTracking>(this.baseUrl + 'workouttracking/' + id + '/sets/' + order, payload).pipe(
      tap(tracking => this.activeWorkout.set(tracking))
    );
  }

  removeSet(id: string, order: number) {
    return this.http.delete<WorkoutTracking>(this.baseUrl + 'workouttracking/' + id + '/sets/' + order).pipe(
      tap(tracking => this.activeWorkout.set(tracking))
    );
  }

  finish(id: string) {
    return this.http.put<string>(this.baseUrl + 'workouttracking/' + id + '/finish', {});
  }

  cancel(id: string) {
    return this.http.put<string>(this.baseUrl + 'workouttracking/' + id + '/cancel', {});
  }

  clearActive() {
    this.activeWorkout.set(null);
  }
}
