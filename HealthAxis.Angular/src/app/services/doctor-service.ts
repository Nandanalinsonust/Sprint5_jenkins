import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { API_BASE_URL } from '../config/config';
import { Doctor } from '../dtos/doctor';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class DoctorService {
  private http = inject(HttpClient);

  getAll(): Observable<Doctor[]> {
    return this.http.get<Doctor[]>(`${API_BASE_URL}/doctor`);
  }

  getBySpecialisation(specialisation: string): Observable<Doctor[]> {
    return this.http.get<Doctor[]>(`${API_BASE_URL}/doctor/specialisation/${specialisation}`);
  }

  getAvailability(doctorId: number, date: string): Observable<string[]> {
    return this.http.get<string[]>(`${API_BASE_URL}/doctor/availability/${doctorId}?date=${date}`);
  }
}