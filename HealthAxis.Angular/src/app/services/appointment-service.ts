import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { API_BASE_URL } from '../config/config';

import {
  Appointment,
  CreateAppointmentRequest
} from '../dtos/appointment';

@Injectable({
  providedIn: 'root'
})
export class AppointmentService {

  private http = inject(HttpClient);

  createAppointment(
    request: CreateAppointmentRequest
  ): Observable<any> {

    return this.http.post(
      `${API_BASE_URL}/appointment`,
      request
    );
  }

  getAvailability(
    doctorId: number,
    date: string
  ): Observable<string[]> {

    return this.http.get<string[]>(
      `${API_BASE_URL}/doctor/availability/${doctorId}?date=${date}`
    );
  }

  getMyAppointments(): Observable<Appointment[]> {

    return this.http.get<Appointment[]>(
      `${API_BASE_URL}/appointment/my`
    );
  }

  cancelAppointment(
    appointmentId: number,
    reason: string
  ): Observable<any> {

    return this.http.delete(
      `${API_BASE_URL}/appointment/${appointmentId}`,
      {
        body: {
          reason
        }
      }
    );
  }
}