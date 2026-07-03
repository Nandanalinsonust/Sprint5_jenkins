import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { API_BASE_URL } from '../config/config';
import { LoginRequest } from '../dtos/login-request';
import { RegisterRequest } from '../dtos/register-request';
import { AuthResponse } from '../dtos/auth-response';
import { ChangePasswordRequest } from '../dtos/change-password-request';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private http = inject(HttpClient);

  // ================= AUTH =================

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(
      `${API_BASE_URL}/auth/login`,
      request
    );
  }

  register(request: RegisterRequest): Observable<any> {
    return this.http.post(
      `${API_BASE_URL}/auth/register`,
      request
    );
  }

  changePassword(request: ChangePasswordRequest): Observable<any> {
    return this.http.post(
      `${API_BASE_URL}/auth/change-password`,
      request
    );
  }

  // ================= TOKEN =================

  setSession(response: any): void {

    const token =
      response?.data?.token ||
      response?.token;

    if (token) {
      localStorage.setItem('token', token);
    }
  }

  getToken(): string {
    return localStorage.getItem('token') || '';
  }

  isLoggedIn(): boolean {
    return this.getToken() !== '';
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('pendingFirstLoginEmail');
  }

  // ================= FIRST LOGIN =================

  setPendingFirstLoginEmail(email: string): void {
    localStorage.setItem('pendingFirstLoginEmail', email);
  }

  getPendingFirstLoginEmail(): string {
    return localStorage.getItem('pendingFirstLoginEmail') || '';
  }

  clearPendingFirstLoginEmail(): void {
    localStorage.removeItem('pendingFirstLoginEmail');
  }

  // ================= USER INFO =================

  private parseJwt(token: string): any {
    try {
      if (!token) return null;

      const base64 = token.split('.')[1]
        .replace(/-/g, '+')
        .replace(/_/g, '/');

      return JSON.parse(atob(base64));
    } catch {
      return null;
    }
  }

  getUserRoles(): string[] {

    const payload = this.parseJwt(this.getToken());

    const roles =
      payload?.role ||
      payload?.roles ||
      payload?.['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

    if (!roles) return [];

    return Array.isArray(roles) ? roles : [roles];
  }

  getRole(): string {
    return this.getUserRoles()[0] || '';
  }

  isDoctor(): boolean {
    return this.getUserRoles().includes('Doctor');
  }

  isPatient(): boolean {
    return this.getUserRoles().includes('Patient');
  }

  isAdmin(): boolean {
    return this.getUserRoles().includes('Admin');
  }

  // ================= ERROR =================

  getErrorMessage(error: any): string {

    if (error?.error?.message) return error.error.message;

    if (typeof error?.error === 'string') return error.error;

    switch (error?.status) {
      case 400: return 'Bad request.';
      case 401: return 'Invalid credentials.';
      case 403: return 'Access denied.';
      case 404: return 'Not found.';
      case 500: return 'Server error.';
      default: return 'Something went wrong.';
    }
  }
}