import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Role } from '../../shared/models/role';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class RoleService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getRoles() {
    return this.http.get<Role[]>(this.baseUrl + 'role');
  }
}
