import { Component, OnInit } from '@angular/core';

import { CommonModule } from '@angular/common';

import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { HealthRecord } from '../../../../dtos/health-record';
import { DoctorService } from '../../../../services/doctor-service';


@Component({
  selector: 'app-doctor-records',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './doctor-records.html',
  styleUrl: './doctor-records.css'
})
export class DoctorRecordsComponent implements OnInit {

  form!: FormGroup;

  records: HealthRecord[] = [];

  message = '';

  constructor(
    private fb: FormBuilder,
    private doctorService: DoctorService
  ) {}

  ngOnInit(): void {

    this.form = this.fb.group({

      patientId: [
        '',
        Validators.required
      ],

      doctorId: [
        ''
      ],

      appointmentId: [
        '',
        Validators.required
      ],

      diagnosis: [
        '',
        Validators.required
      ],

      prescription: [
        '',
        Validators.required
      ],

      notes: ['']
    });

    this.loadRecords();
  }

  loadRecords(): void {

    this.doctorService
      .getHealthRecords()
      .subscribe({
        next: data => {
          this.records = data;
        }
      });
  }

  saveRecord(): void {

    if (this.form.invalid) {
      return;
    }

    const appointmentId =
      Number(
        this.form.value.appointmentId
      );

    this.doctorService
      .existsForAppointment(
        appointmentId
      )
      .subscribe({

        next: exists => {

          if (exists) {

            this.message =
              'Health record already exists.';

            return;
          }

          this.doctorService
            .addHealthRecord(
              this.form.value
            )
            .subscribe({

              next: () => {

                this.message =
                  'Record added successfully';

                this.form.reset();

                this.loadRecords();
              }
            });
        }
      });
  }
}