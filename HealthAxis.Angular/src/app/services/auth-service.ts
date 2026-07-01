import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginRequest } from '../dtos/login-request';
import { RegisterRequest } from '../dtos/register-request';
import { API_BASE_URL } from '../config/config';
import { AuthResponse } from '../dtos/auth-response';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private http: HttpClient) {}

  login(loginRequest: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${API_BASE_URL}/auth/login`, loginRequest);
  }

  register(registerRequest: RegisterRequest): Observable<any> {
    return this.http.post(`${API_BASE_URL}/auth/register`, registerRequest);
  }

  changePassword(payload: { email: string; oldPassword: string; newPassword: string }): Observable<any> {
    return this.http.post(`${API_BASE_URL}/auth/change-password`, payload);
  }

  setPendingFirstLoginEmail(email: string) {
    localStorage.setItem('pendingFirstLoginEmail', email);
  }

  getPendingFirstLoginEmail(): string {
    return localStorage.getItem('pendingFirstLoginEmail') || '';
  }

  clearPendingFirstLoginEmail() {
    localStorage.removeItem('pendingFirstLoginEmail');
  }

  setSession(authResponse: AuthResponse) {
    localStorage.setItem('token', authResponse.data.token);
  }

  getToken() {
    return localStorage.getItem('token');
  }

  isLoggedIn() {
    return localStorage.getItem('token') != null;
  }

  getUserRolesFromToken(token: string) {
    const payload = this.parseJwt(token);
    if (!payload) return [];
    const roleClaims = payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || payload.role || payload.roles;
    return Array.isArray(roleClaims) ? roleClaims : roleClaims ? [roleClaims] : [];
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('accessToken');
  }

  private parseJwt(token: string): any | null {
    try {
      if (!token) return null;
      const parts = token.split('.');
      if (parts.length !== 3) return null;
      const base64Url = parts[1];
      const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
      return JSON.parse(atob(base64));
    } catch (e) {
      return null;
    }
  }

  getUserRoles() {
    const token = this.getToken();
    if (!token) return [];
    const payload = this.parseJwt(token);
    if (!payload) return [];
    const roleClaims = payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || payload.role || payload.roles;
    return Array.isArray(roleClaims) ? roleClaims : roleClaims ? [roleClaims] : [];
  }

  getCurrentUserEmail() {
    const token = this.getToken();
    if (!token) return '';
    const payload = this.parseJwt(token);
    return payload?.email || payload?.['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] || '';
  }
}
