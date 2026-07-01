import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../config/config';
import { Appointment } from '../dtos/appointment';
import { HealthRecord, CreateHealthRecordRequest } from '../dtos/health-record';

@Injectable({ providedIn: 'root' })
export class DoctorPortalService {
  private http = inject(HttpClient);

  getDoctorAppointments(): Observable<Appointment[]> {
    return this.http.get<Appointment[]>(`${API_BASE_URL}/appointment/doctor`);
  }

  getHealthRecords(): Observable<HealthRecord[]> {
    return this.http.get<HealthRecord[]>(`${API_BASE_URL}/healthrecord/doctor`);
  }

  addHealthRecord(payload: CreateHealthRecordRequest): Observable<any> {
    return this.http.post(`${API_BASE_URL}/healthrecord`, payload);
  }

  updateAppointmentStatus(id: number, status: string, reason?: string): Observable<any> {
    return this.http.put(`${API_BASE_URL}/appointment/status/${id}`, { status, cancellationReason: reason });
  }
}
