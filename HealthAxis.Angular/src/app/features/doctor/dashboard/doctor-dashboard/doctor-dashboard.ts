import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Appointment } from '../../../../dtos/appointment';
import { DoctorService } from '../../../../services/doctor-service';


@Component({
  selector: 'app-doctor-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './doctor-dashboard.html',
  styleUrl: './doctor-dashboard.css'
})
export class DoctorDashboardComponent implements OnInit {

  appointments: Appointment[] = [];

  constructor(
    private doctorService: DoctorService
  ) {}

  ngOnInit(): void {

    this.doctorService
      .getDoctorAppointments()
      .subscribe({
        next: data => {
          this.appointments = data;
        }
      });
  }

  get pendingCount(): number {

    return this.appointments.filter(
      a => a.status === 'Pending'
    ).length;
  }

  get confirmedCount(): number {

    return this.appointments.filter(
      a => a.status === 'Confirmed'
    ).length;
  }

  get completedCount(): number {

    return this.appointments.filter(
      a => a.status === 'Completed'
    ).length;
  }
}