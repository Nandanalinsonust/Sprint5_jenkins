import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { API_BASE_URL } from '../config/config';

import { Doctor, UpdateDoctorRequest } from '../dtos/doctor';
import { Appointment } from '../dtos/appointment';

import {
  HealthRecord,
  CreateHealthRecordRequest
} from '../dtos/health-record';

@Injectable({
  providedIn: 'root'
})
export class DoctorService {

  private http = inject(HttpClient);

  getAll(): Observable<Doctor[]> {

    return this.http.get<Doctor[]>(
      `${API_BASE_URL}/doctor`
    );
  }

  getById(
    doctorId: number
  ): Observable<Doctor> {

    return this.http.get<Doctor>(
      `${API_BASE_URL}/doctor/${doctorId}`
    );
  }

  getProfile(): Observable<Doctor> {

    return this.http.get<Doctor>(
      `${API_BASE_URL}/doctor/profile`
    );
  }

  updateProfile(
    doctorId: number,
    payload: UpdateDoctorRequest
  ): Observable<any> {

    return this.http.put(
      `${API_BASE_URL}/doctor/${doctorId}`,
      payload
    );
  }

  getBySpecialisation(
    specialisation: string
  ): Observable<Doctor[]> {

    return this.http.get<Doctor[]>(
      `${API_BASE_URL}/doctor/specialisation/${specialisation}`
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

  getDoctorAppointments(): Observable<Appointment[]> {

    return this.http.get<Appointment[]>(
      `${API_BASE_URL}/appointment/doctor`
    );
  }

  updateAppointmentStatus(
    appointmentId: number,
    status: string,
    cancellationReason?: string
  ): Observable<any> {

    return this.http.put(
      `${API_BASE_URL}/appointment/status/${appointmentId}`,
      {
        status,
        cancellationReason
      }
    );
  }

  getHealthRecords(): Observable<HealthRecord[]> {

    return this.http.get<HealthRecord[]>(
      `${API_BASE_URL}/healthrecord/doctor`
    );
  }

  getPatientHealthRecords(
    patientId: number
  ): Observable<HealthRecord[]> {

    return this.http.get<HealthRecord[]>(
      `${API_BASE_URL}/healthrecord/patient/${patientId}`
    );
  }

  addHealthRecord(
    payload: CreateHealthRecordRequest
  ): Observable<any> {

    return this.http.post(
      `${API_BASE_URL}/healthrecord`,
      payload
    );
  }

  existsForAppointment(
    appointmentId: number
  ): Observable<boolean> {

    return this.http.get<boolean>(
      `${API_BASE_URL}/healthrecord/exists/${appointmentId}`
    );
  }

  deleteHealthRecord(
    healthRecordId: number
  ): Observable<any> {

    return this.http.delete(
      `${API_BASE_URL}/healthrecord/${healthRecordId}`
    );
  }

  changePassword(payload: {
  currentPassword: string;
  newPassword: string;
}): Observable<any> {
  return this.http.put(
    `${API_BASE_URL}/doctor/change-password`,
    payload
  );
}
}