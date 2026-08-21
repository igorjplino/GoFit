import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Pagination } from '../../shared/models/pagination';
import { Exercise } from '../../shared/models/exercise';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ExerciseService {
  baseUrl = environment.apiUrl;
  
  constructor(private http: HttpClient) {}

  getExercise(id: string) {
    return this.http.get<Exercise>(this.baseUrl + 'exercise/' + id);
  }

  getExercises() {
    return this.http.get<Pagination<Exercise>>(this.baseUrl + 'exercise');
  }
}
