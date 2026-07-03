import { Component, OnInit } from '@angular/core';

import {
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
  Validators
} from '@angular/forms';

import { CommonModule } from '@angular/common';
import { PatientService } from '../../../../services/patient-service';
import { AuthService } from '../../../../services/auth-service';


@Component({
  selector: 'app-patient-profile',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './patient-profile.html',
  styleUrl: './patient-profile.css'
})
export class PatientProfileComponent implements OnInit {

  profileForm!: FormGroup;

  message = '';

  constructor(
    private fb: FormBuilder,
    private patientService: PatientService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {

    this.profileForm =
      this.fb.group({

        FullName: [
          '',
          Validators.required
        ],

        dateOfBirth: [
          '',
          Validators.required
        ],

        gender: [
          '',
          Validators.required
        ],

        email: [
          {
            value: '',
            disabled: true
          }
        ],

        phoneNumber: [
          '',
          Validators.required
        ],

        insuranceID: ['']
      });

    this.loadProfile();
  }

  loadProfile(): void {

    this.patientService
      .getProfile()
      .subscribe({

        next: profile => {

          this.profileForm.patchValue({
            FullName: profile.fullName,
            dateOfBirth: profile.dateOfBirth,
            gender: profile.gender,
            email: profile.email,
            phoneNumber: profile.phoneNumber,
            insuranceID: profile.insuranceID
          });
        }
      });
  }

  saveProfile(): void {

    if (this.profileForm.invalid) {

      this.profileForm.markAllAsTouched();

      return;
    }

    this.patientService
      .getProfile()
      .subscribe({

        next: profile => {

          this.patientService
            .updateProfile(
              profile.patientId,
              this.profileForm.value
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