import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { API_BASE_URL } from '../config/config';

import {
  PatientProfile,
  CreatePatientProfileRequest,
  UpdatePatientProfileRequest
} from '../dtos/patient';

import { Appointment } from '../dtos/appointment';
import { HealthRecord } from '../dtos/health-record';

@Injectable({
  providedIn: 'root'
})
export class PatientService {

  private http = inject(HttpClient);

  getProfile(): Observable<PatientProfile> {

    return this.http.get<PatientProfile>(
      `${API_BASE_URL}/patient/me`
    );
  }

  getById(
    patientId: number
  ): Observable<PatientProfile> {

    return this.http.get<PatientProfile>(
      `${API_BASE_URL}/patient/${patientId}`
    );
  }

  createProfile(
    payload: CreatePatientProfileRequest
  ): Observable<any> {

    return this.http.post(
      `${API_BASE_URL}/patient/complete-profile`,
      payload
    );
  }

  updateProfile(
    patientId: number,
    payload: UpdatePatientProfileRequest
  ): Observable<any> {

    return this.http.put(
      `${API_BASE_URL}/patient/${patientId}`,
      payload
    );
  }

  getAppointments(): Observable<Appointment[]> {

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

  getHealthRecords(): Observable<HealthRecord[]> {

    return this.http.get<HealthRecord[]>(
      `${API_BASE_URL}/healthrecord/me`
    );
  }
}