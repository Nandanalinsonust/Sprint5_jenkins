import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PatientService } from '../../../services/patient-service';
import { Appointment } from '../../../dtos/appointment';

@Component({
  selector: 'app-patient-appointments',
  imports: [CommonModule, FormsModule],
  templateUrl: './patient-appointments.html',
  styleUrl: './patient-appointments.css',
})
export class PatientAppointmentsComponent implements OnInit {
  patientService = inject(PatientService);
  appointments: Appointment[] = [];
  page = 1;
  pageSize = 5;
  selectedReason = '';
  cancelingId: number | null = null;

  ngOnInit(): void {
    this.patientService.getAppointments().subscribe({ next: data => this.appointments = data });
  }

  get pagedAppointments(): Appointment[] {
    const start = (this.page - 1) * this.pageSize;
    return this.appointments.slice(start, start + this.pageSize);
  }

  totalPages(): number {
    return Math.ceil(this.appointments.length / this.pageSize);
  }

  cancelAppointment(id: number): void {
    this.patientService.cancelAppointment(id, this.selectedReason || 'No reason provided').subscribe({
      next: () => {
        this.appointments = this.appointments.filter(a => a.appointmentId !== id);
        this.selectedReason = '';
        this.cancelingId = null;
      }
    });
  }
}
