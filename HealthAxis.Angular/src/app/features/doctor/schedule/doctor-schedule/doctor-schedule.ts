import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Appointment } from '../../../../dtos/appointment';
import { DoctorService } from '../../../../services/doctor-service';


@Component({
  selector: 'app-doctor-schedule',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './doctor-schedule.html',
  styleUrl: './doctor-schedule.css'
})
export class DoctorScheduleComponent implements OnInit {

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
}