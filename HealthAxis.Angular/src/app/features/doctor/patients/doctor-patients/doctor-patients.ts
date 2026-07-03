import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Appointment } from '../../../../dtos/appointment';
import { DoctorService } from '../../../../services/doctor-service';


@Component({
  selector: 'app-doctor-patients',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './doctor-patients.html',
  styleUrl: './doctor-patients.css'
})
export class DoctorPatientsComponent implements OnInit {

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

  get uniquePatients() {

    return this.appointments.filter(
      (appointment, index, self) =>
        index === self.findIndex(
          patient =>
            patient.patientId ===
            appointment.patientId
        )
    );
  }
}