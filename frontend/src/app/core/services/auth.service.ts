import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { LoginRequest, RegisterRequest, AuthResponse } from '../models/auth.model';

const TOKEN_KEY = 'auth_token';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/auth`;

  readonly token = signal<string | null>(localStorage.getItem(TOKEN_KEY));

  login(request: LoginRequest) {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, request).pipe(
      tap((response) => this.setSession(response))
    );
  }

  register(request: RegisterRequest) {
    return this.http.post<AuthResponse>(`${this.apiUrl}/register`, request).pipe(
      tap((response) => this.setSession(response))
    );
  }

  logout() {
    localStorage.removeItem(TOKEN_KEY);
    this.token.set(null);
  }

  isAuthenticated(): boolean {
    return !!this.token();
  }

  private setSession(response: AuthResponse) {
    localStorage.setItem(TOKEN_KEY, response.token);
    this.token.set(response.token);
  }
}
