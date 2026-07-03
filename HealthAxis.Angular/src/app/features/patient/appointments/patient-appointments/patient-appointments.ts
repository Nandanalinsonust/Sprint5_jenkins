import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Appointment } from '../../../../dtos/appointment';
import { PatientService } from '../../../../services/patient-service';


@Component({
  selector: 'app-patient-appointments',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './patient-appointments.html',
  styleUrl: './patient-appointments.css'
})
export class PatientAppointmentsComponent implements OnInit {

  appointments: Appointment[] = [];

  selectedReason = '';

  cancelingId: number | null = null;

  constructor(
    private patientService: PatientService
  ) {}

  ngOnInit(): void {

    this.loadAppointments();
  }

  loadAppointments(): void {

    this.patientService
      .getAppointments()
      .subscribe({
        next: data => {
          this.appointments = data;
        }
      });
  }

  cancelAppointment(
    appointmentId: number
  ): void {

    this.patientService
      .cancelAppointment(
        appointmentId,
        this.selectedReason
      )
      .subscribe({
        next: () => {

          this.cancelingId = null;

          this.selectedReason = '';

          this.loadAppointments();
        }
      });
  }
}