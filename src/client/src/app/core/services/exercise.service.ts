import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Pagination } from '../../shared/models/pagination';
import { Exercise } from '../../shared/models/exercise';

@Injectable({
  providedIn: 'root'
})
export class ExerciseService {
  baseUrl = 'http://localhost:5001/api/';
  
  constructor(private http: HttpClient) {}

  getExercises() {
    return this.http.get<Pagination<Exercise>>(this.baseUrl + 'exercise');
  }
}
