import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserSummary } from '@gofit/shared/models/user-summary';
import { environment } from '@gofit/shared/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserManagementService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getUsers() {
    return this.http.get<UserSummary[]>(this.baseUrl + 'user');
  }

  updateUserRole(userId: string, role: string) {
    return this.http.put(this.baseUrl + 'user/' + userId + '/role', { role });
  }
}
