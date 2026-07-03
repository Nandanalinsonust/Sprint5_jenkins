import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { PatientProfile } from '../../../../dtos/patient';
import { Appointment } from '../../../../dtos/appointment';
import { HealthRecord } from '../../../../dtos/health-record';
import { PatientService } from '../../../../services/patient-service';


@Component({
  selector: 'app-patient-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink
  ],
  templateUrl: './patient-dashboard.html',
  styleUrl: './patient-dashboard.css'
})
export class PatientDashboardComponent implements OnInit {

  profile: PatientProfile | null = null;

  appointments: Appointment[] = [];

  records: HealthRecord[] = [];

  loading = true;

  constructor(
    private patientService: PatientService
  ) {}

  ngOnInit(): void {

    this.patientService
      .getProfile()
      .subscribe({
        next: data => {
          this.profile = data;
        }
      });

    this.patientService
      .getAppointments()
      .subscribe({
        next: data => {
          this.appointments = data;
        }
      });

    this.patientService
      .getHealthRecords()
      .subscribe({
        next: data => {

          this.records = data;

          this.loading = false;
        }
      });
  }

  get upcomingAppointments(): number {

    return this.appointments.filter(
      x =>
        x.status === 'Pending' ||
        x.status === 'Confirmed'
    ).length;
  }

  get completedAppointments(): number {

    return this.appointments.filter(
      x =>
        x.status === 'Completed'
    ).length;
  }
}