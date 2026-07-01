import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DoctorPortalService } from '../../../services/doctor-portal-service';
import { Appointment } from '../../../dtos/appointment';

@Component({
  selector: 'app-doctor-dashboard',
  imports: [CommonModule],
  templateUrl: './doctor-dashboard.html',
  styleUrl: './doctor-dashboard.css',
})
export class DoctorDashboardComponent implements OnInit {
  doctorPortalService = inject(DoctorPortalService);
  appointments: Appointment[] = [];

  ngOnInit(): void {
    this.doctorPortalService.getDoctorAppointments().subscribe({ next: data => this.appointments = data });
  }
}
