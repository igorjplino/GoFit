import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { CreateWorkoutPlanRequest, UpdateWorkoutPlanRequest, WorkoutPlan } from '../../shared/models/workout-plan';

@Injectable({
  providedIn: 'root'
})
export class WorkoutPlanService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  create(payload: CreateWorkoutPlanRequest) {
    return this.http.post<string>(this.baseUrl + 'workoutplan', payload);
  }

  getById(id: string) {
    return this.http.get<WorkoutPlan>(this.baseUrl + 'workoutplan/' + id);
  }

  listMine() {
    return this.http.get<WorkoutPlan[]>(this.baseUrl + 'workoutplan/mine');
  }

  update(id: string, payload: UpdateWorkoutPlanRequest) {
    return this.http.put<string>(this.baseUrl + 'workoutplan/' + id, payload);
  }
}
