import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { ActivatedRoute } from '@angular/router';
import { AppointmentService } from '../../../../services/appointment-service';


@Component({
  selector: 'app-book-appointment',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './book-appointment.html',
  styleUrl: './book-appointment.css'
})
export class BookAppointmentComponent implements OnInit {

  doctorId = 0;

  selectedDate = '';

  selectedSlot = '';

  slots: string[] = [];

  bookedSlots: string[] = [];

  message = '';

  readonly defaultSlots = [
    '09:00-09:30',
    '10:00-10:30',
    '11:00-11:30',
    '14:00-14:30',
    '15:00-15:30',
    '16:00-16:30'
  ];

  constructor(
    private route: ActivatedRoute,
    private appointmentService: AppointmentService
  ) {}

  ngOnInit(): void {

    this.route.queryParams.subscribe(
      params => {

        this.doctorId =
          Number(params['doctorId']);
      }
    );
  }

  loadAvailability(): void {

    this.appointmentService
      .getAvailability(
        this.doctorId,
        this.selectedDate
      )
      .subscribe({
        next: data => {

          this.bookedSlots = data;

          this.slots =
            [...this.defaultSlots];
        }
      });
  }

  book(): void {

    this.appointmentService
      .createAppointment({

        doctorId: this.doctorId,

        scheduledDate:
          this.selectedDate,

        timeSlot:
          this.selectedSlot

      })
      .subscribe({
        next: () => {

          this.message =
            'Appointment booked successfully';
        }
      });
  }
}