import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { DoctorService } from '../../../../services/doctor-service';


@Component({
  selector: 'app-doctor-profile',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './doctor-profile.html',
  styleUrl: './doctor-profile.css'
})
export class DoctorProfileComponent implements OnInit {

  form!: FormGroup;

  message = '';

  constructor(
    private fb: FormBuilder,
    private doctorService: DoctorService
  ) {}

  ngOnInit(): void {

    this.form = this.fb.group({

      fullName: [
        '',
        Validators.required
      ],


      specialisation: [''],

      yearsOfExperience: [0],

      consultationFee: [0]
    });

    this.loadProfile();
  }

  loadProfile(): void {

    this.doctorService
      .getProfile()
      .subscribe({

        next: doctor => {

          this.form.patchValue(
            doctor
          );
        }
      });
  }

  save(): void {

    if (this.form.invalid) {
      return;
    }

    this.doctorService
      .getProfile()
      .subscribe({

        next: doctor => {

          this.doctorService
            .updateProfile(
              doctor.doctorId,
              this.form.value
            )
            .subscribe({

              next: () => {

                this.message =
                  'Profile updated successfully';
              }
            });
        }
      });
  }
}