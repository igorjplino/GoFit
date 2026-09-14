import { Injectable, signal } from '@angular/core';
import { environment } from '@gofit/shared/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { User } from '../models/user';
import { Profile, UpdateProfileRequest } from '../models/profile';
import { map, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  currentUser = signal<User | null>(null);

  constructor(private http: HttpClient) { }

  login(values: any) {
    return this.http.post<User>(this.baseUrl + 'account/login', values);
  }

  register(values: any) {
    return this.http.post(this.baseUrl + 'account/register', values);
  }

  getUserInfo() {
    return this.http.get<User>(this.baseUrl + 'account/user-info', { withCredentials: true }).pipe(
      map(user => {
        this.currentUser.set(user); 
        return user;
      })
    )
  }

  getAuthState() {
    return this.http.get<{ isAuthenticated: boolean }>(this.baseUrl + 'account/auth-status');
  }

  logout() {
    return this.http.post(this.baseUrl + 'account/logout', {}).pipe(
      tap(() => this.currentUser.set(null))
    );
  }

  getProfile() {
    return this.http.get<Profile>(this.baseUrl + 'profile/me');
  }

  updateProfile(payload: UpdateProfileRequest) {
    return this.http.put<Profile>(this.baseUrl + 'profile/me', payload).pipe(
      tap(profile => this.setDisplayName(profile.name))
    );
  }

  setDisplayName(displayName: string) {
    this.currentUser.update(user => user ? { ...user, displayName } : user);
  }

  hasPermission(permission: string): boolean {
    return this.currentUser()?.permissions?.includes(permission) ?? false;
  }

  hasAnyPermission(...permissions: string[]): boolean {
    return permissions.some(permission => this.hasPermission(permission));
  }
}
