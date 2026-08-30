import { HttpClient, HttpParams } from '@angular/common/http';
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

  getExercises(params?: { pageNumber?: number; pageSize?: number; name?: string }) {
    let httpParams = new HttpParams();

    if (params?.pageNumber !== undefined) {
      httpParams = httpParams.set('pageNumber', params.pageNumber);
    }
    if (params?.pageSize !== undefined) {
      httpParams = httpParams.set('pageSize', params.pageSize);
    }
    if (params?.name) {
      httpParams = httpParams.set('name', params.name);
    }

    return this.http.get<Pagination<Exercise>>(this.baseUrl + 'exercise', { params: httpParams });
  }

  createExercise(exercise: { name: string; description: string | null }) {
    return this.http.post<string>(this.baseUrl + 'exercise', exercise);
  }

  deleteExercise(id: string) {
    return this.http.delete(this.baseUrl + 'exercise/' + id);
  }
}
