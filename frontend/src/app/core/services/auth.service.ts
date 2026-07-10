import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { LoginRequest, RegisterRequest, AuthResponse } from '../models/auth.model';
import {
  CreateProviderRequest,
  ProviderAuthResponse,
  ProviderLoginRequest
} from '../models/provider.model';

const TOKEN_KEY = 'auth_token';
const ROLE_KEY = 'auth_role';
const PROVIDER_ID_KEY = 'auth_provider_id';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private http = inject(HttpClient);
  private apiUrl = environment.apiUrl;

  readonly token = signal<string | null>(localStorage.getItem(TOKEN_KEY));
  readonly role = signal<string | null>(localStorage.getItem(ROLE_KEY));
  readonly providerId = signal<string | null>(localStorage.getItem(PROVIDER_ID_KEY));

  login(request: LoginRequest) {
    return this.http.post<AuthResponse>(`${this.apiUrl}/auth/login`, request).pipe(
      tap((response) => this.setSession(response.token, response.role))
    );
  }

  register(request: RegisterRequest) {
    return this.http.post<AuthResponse>(`${this.apiUrl}/auth/register`, request).pipe(
      tap((response) => this.setSession(response.token, response.role))
    );
  }

  loginProvider(request: ProviderLoginRequest) {
    return this.http.post<ProviderAuthResponse>(`${this.apiUrl}/providers/login`, request).pipe(
      tap((response) => this.setSession(response.token, response.role, response.providerId))
    );
  }

  registerProvider(request: CreateProviderRequest) {
    return this.http.post<ProviderAuthResponse>(`${this.apiUrl}/providers`, request).pipe(
      tap((response) => this.setSession(response.token, response.role, response.providerId))
    );
  }

  logout() {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(ROLE_KEY);
    localStorage.removeItem(PROVIDER_ID_KEY);
    this.token.set(null);
    this.role.set(null);
    this.providerId.set(null);
  }

  isAuthenticated(): boolean {
    return !!this.token();
  }

  isProvider(): boolean {
    return this.role() === 'Provider';
  }

  private setSession(token: string, role: string, providerId: string | null = null) {
    localStorage.setItem(TOKEN_KEY, token);
    localStorage.setItem(ROLE_KEY, role);
    this.token.set(token);
    this.role.set(role);

    if (providerId) {
      localStorage.setItem(PROVIDER_ID_KEY, providerId);
    } else {
      localStorage.removeItem(PROVIDER_ID_KEY);
    }
    this.providerId.set(providerId);
  }
}