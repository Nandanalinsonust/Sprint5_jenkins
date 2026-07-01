import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { PatientService } from '../../../services/patient-service';
import { Appointment } from '../../../dtos/appointment';
import { HealthRecord } from '../../../dtos/health-record';
import { PatientProfile } from '../../../dtos/patient';

@Component({
  selector: 'app-patient-dashboard',
  imports: [CommonModule, RouterLink],
  templateUrl: './patient-dashboard.html',
  styleUrl: './patient-dashboard.css',
})
export class PatientDashboard implements OnInit {
  patientService = inject(PatientService);
  router = inject(Router);
  profile: PatientProfile | null = null;
  appointments: Appointment[] = [];
  records: HealthRecord[] = [];

  ngOnInit(): void {
    this.patientService.getProfile().subscribe({
      next: data => this.profile = data,
      error: () => this.router.navigate(['/patient/complete-profile'])
    });

    this.patientService.getAppointments().subscribe({ next: data => this.appointments = data });
    this.patientService.getHealthRecords().subscribe({ next: data => this.records = data });
  }
}
