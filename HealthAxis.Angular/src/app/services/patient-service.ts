import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../config/config';
import { Appointment } from '../dtos/appointment';
import { HealthRecord } from '../dtos/health-record';
import { PatientProfile, PatientRegisterRequest } from '../dtos/patient';

@Injectable({ providedIn: 'root' })
export class PatientService {
  private http = inject(HttpClient);

  getProfile(): Observable<PatientProfile> {
    return this.http.get<PatientProfile>(`${API_BASE_URL}/patient/me`);
  }

  updateProfile(payload: PatientProfile): Observable<PatientProfile> {
    return this.http.put<PatientProfile>(`${API_BASE_URL}/patient/${payload.patientId}`, payload);
  }

  createProfile(payload: PatientRegisterRequest): Observable<any> {
    return this.http.post(`${API_BASE_URL}/patient/complete-profile`, payload);
  }

  register(payload: PatientRegisterRequest): Observable<any> {
    return this.http.post(`${API_BASE_URL}/patient`, payload);
  }

  getAppointments(): Observable<Appointment[]> {
    return this.http.get<Appointment[]>(`${API_BASE_URL}/appointment/my`);
  }

  cancelAppointment(id: number, reason: string): Observable<any> {
    return this.http.delete(`${API_BASE_URL}/appointment/${id}`, { body: { reason } });
  }

  getHealthRecords(): Observable<HealthRecord[]> {
    return this.http.get<HealthRecord[]>(`${API_BASE_URL}/healthrecord/me`);
  }
}
