import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { AuthResponse, AuthUser } from '../models/models';

@Injectable({ providedIn: 'root' })
export class Auth {
  private readonly apiUrl = 'http://localhost:5149/api/auth';

  constructor(private http: HttpClient) {}

  login(data: { email: string; password: string }): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, data).pipe(
      tap(response => {
        localStorage.setItem('contractflow_token', response.token);
        localStorage.setItem('contractflow_user', JSON.stringify({
          userId: response.userId,
          name: response.name,
          email: response.email,
          role: response.role
        }));
      })
    );
  }

  logout(): void {
    localStorage.removeItem('contractflow_token');
    localStorage.removeItem('contractflow_user');
  }

  getToken(): string | null { return localStorage.getItem('contractflow_token'); }
  isAuthenticated(): boolean { return !!this.getToken(); }

  getUser(): AuthUser | null {
    const raw = localStorage.getItem('contractflow_user');
    if (!raw) return null;
    try { return JSON.parse(raw) as AuthUser; } catch { return null; }
  }
}
