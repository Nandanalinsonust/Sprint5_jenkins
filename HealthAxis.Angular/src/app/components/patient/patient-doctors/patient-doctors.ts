import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DoctorService } from '../../../services/doctor-service';
import { Doctor } from '../../../dtos/doctor';
import { AppointmentService } from '../../../services/appointment-service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-patient-doctors',
  imports: [CommonModule, FormsModule],
  templateUrl: './patient-doctors.html',
  styleUrl: './patient-doctors.css',
})
export class PatientDoctorsComponent implements OnInit {
  doctorService = inject(DoctorService);
  appointmentService = inject(AppointmentService);
  router = inject(Router);
  doctors: Doctor[] = [];
  filteredDoctors: Doctor[] = [];
  selectedSpecialisation = '';
  searchTerm = '';
  selectedDoctorId = 0;
  selectedDate = '';
  selectedSlot = '';
  message = '';

  ngOnInit(): void {
    this.doctorService.getAll().subscribe({ next: data => { this.doctors = data; this.filteredDoctors = data; } });
  }

  filterDoctors(): void {
    this.filteredDoctors = this.doctors.filter(doctor => {
      const matchSpecialisation = this.selectedSpecialisation ? doctor.specialisation === this.selectedSpecialisation : true;
      const matchSearch = !this.searchTerm || doctor.fullName.toLowerCase().includes(this.searchTerm.toLowerCase());
      return matchSpecialisation && matchSearch;
    });
  }

  bookAppointment(): void {
    if (!this.selectedDoctorId || !this.selectedDate || !this.selectedSlot) {
      this.message = 'Please choose doctor, date and slot.';
      return;
    }

    this.appointmentService.createAppointment({ doctorId: this.selectedDoctorId, scheduledDate: this.selectedDate, timeSlot: this.selectedSlot }).subscribe({
      next: () => {
        this.message = 'Appointment booked successfully.';
        this.router.navigate(['/patient/appointments']);
      },
      error: err => this.message = err?.error?.message || 'Unable to book appointment.'
    });
  }

  quickBook(doctorId: number): void {
    this.router.navigate(['/patient/book-appointment'], { queryParams: { doctorId, specialization: this.selectedSpecialisation } });
  }
}
