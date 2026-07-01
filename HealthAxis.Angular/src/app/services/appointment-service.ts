import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../config/config';
import { CreateAppointmentRequest } from '../dtos/appointment';

@Injectable({ providedIn: 'root' })
export class AppointmentService {
  private http = inject(HttpClient);

  createAppointment(payload: CreateAppointmentRequest): Observable<any> {
    return this.http.post(`${API_BASE_URL}/appointment`, payload);
  }

  getAvailability(doctorId: number, date: string): Observable<string[]> {
    return this.http.get<string[]>(`${API_BASE_URL}/doctor/availability/${doctorId}?date=${date}`);
  }

  cancelAppointment(id: number): Observable<any> {
    return this.http.delete(`${API_BASE_URL}/appointment/${id}`);
  }
}