import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { CreateWorkoutRequest, UpdateWorkoutRequest, Workout } from '../../shared/models/workout-plan';

@Injectable({
  providedIn: 'root'
})
export class WorkoutService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getById(id: string) {
    return this.http.get<Workout>(this.baseUrl + 'workout/' + id);
  }

  update(id: string, payload: UpdateWorkoutRequest) {
    return this.http.put<void>(this.baseUrl + 'workout/' + id, payload);
  }

  create(payload: CreateWorkoutRequest) {
    return this.http.post<string>(this.baseUrl + 'workout', payload);
  }

  delete(id: string) {
    return this.http.delete<void>(this.baseUrl + 'workout/' + id);
  }
}
