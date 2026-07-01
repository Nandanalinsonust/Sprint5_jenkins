import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AppointmentService } from '../../../services/appointment-service';
import { DoctorService } from '../../../services/doctor-service';
import { PatientService } from '../../../services/patient-service';
import { Doctor } from '../../../dtos/doctor';
import { Appointment } from '../../../dtos/appointment';

@Component({
  selector: 'app-book-appointment',
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './book-appointment.html',
  styleUrl: './book-appointment.css',
})
export class BookAppointmentComponent implements OnInit {
  doctorService = inject(DoctorService);
  appointmentService = inject(AppointmentService);
  patientService = inject(PatientService);
  router = inject(Router);
  route = inject(ActivatedRoute);
  doctors: Doctor[] = [];
  specialisations = ['Cardiologist', 'Dermatologist', 'Neurologist', 'Pediatrician', 'Psychiatrist'];
  selectedSpecialisation = '';
  selectedDoctorId = 0;
  selectedDate = '';
  slots: string[] = ['09:00-09:30','10:00-10:30','11:00-11:30','12:00-12:30','14:00-14:30','15:00-15:30','16:00-16:30'];
  bookedSlots: string[] = [];
  selectedSlot = '';
  message = '';
  patientAppointments: Appointment[] = [];

  ngOnInit(): void {
    this.doctorService.getAll().subscribe({ next: data => this.doctors = data });
    this.patientService.getAppointments().subscribe({ next: data => this.patientAppointments = data });
    this.route.queryParams.subscribe(params => {
      if (params['doctorId']) {
        this.selectedDoctorId = +params['doctorId'];
      }
      if (params['specialization']) {
        this.selectedSpecialisation = params['specialization'];
        this.onSpecialisationChange();
      }
    });
  }

  onSpecialisationChange(): void {
    this.selectedDoctorId = 0;
    this.selectedDate = '';
    this.selectedSlot = '';
    if (this.selectedSpecialisation) {
      this.doctorService.getBySpecialisation(this.selectedSpecialisation).subscribe({ next: data => this.doctors = data });
    } else {
      this.doctorService.getAll().subscribe({ next: data => this.doctors = data });
    }
  }

  loadAvailability(): void {
    if (!this.selectedDoctorId || !this.selectedDate) {
      this.message = 'Select a doctor and date to load available slots.';
      return;
    }
    const date = new Date(this.selectedDate);
    if (date < new Date(new Date().toISOString().split('T')[0])) {
      this.message = 'Cannot book a date in the past.';
      return;
    }
    this.appointmentService.getAvailability(this.selectedDoctorId, this.selectedDate).subscribe({
      next: (data: string[]) => {
        this.bookedSlots = data;
        this.message = '';
      },
      error: () => {
        this.message = 'Unable to load availability.';
      }
    });
  }

  isSlotBooked(slot: string): boolean {
    return this.bookedSlots.includes(slot);
  }

  book(): void {
    if (!this.selectedSpecialisation || !this.selectedDoctorId || !this.selectedDate || !this.selectedSlot) {
      this.message = 'Complete all booking fields before submitting.';
      return;
    }

    const selectedDate = new Date(this.selectedDate);
    const today = new Date(new Date().toISOString().split('T')[0]);
    if (selectedDate < today) {
      this.message = 'Cannot book in the past.';
      return;
    }

    const sameDayConflict = this.patientAppointments.some(a =>
      a.scheduledDate === this.selectedDate && a.doctorId === this.selectedDoctorId
    );
    if (sameDayConflict) {
      this.message = 'You cannot book the same doctor on the same day twice.';
      return;
    }

    if (this.isSlotBooked(this.selectedSlot)) {
      this.message = 'Slot already booked. Choose another slot.';
      return;
    }

    this.appointmentService.createAppointment({ doctorId: this.selectedDoctorId, scheduledDate: this.selectedDate, timeSlot: this.selectedSlot }).subscribe({
      next: () => {
        this.message = 'Appointment booked successfully.';
        this.patientService.getAppointments().subscribe({ next: data => this.patientAppointments = data });
      },
      error: err => {
        this.message = err?.error?.message || 'Unable to book appointment.';
      }
    });
  }
}
