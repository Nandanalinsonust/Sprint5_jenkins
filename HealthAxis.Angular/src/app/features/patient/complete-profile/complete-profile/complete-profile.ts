import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import { Router } from '@angular/router';
import { PatientService } from '../../../../services/patient-service';

@Component({
  selector: 'app-complete-profile',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './complete-profile.html',
  styleUrl: './complete-profile.css'
})
export class CompleteProfileComponent {

  profileForm: FormGroup;

  errorMessage = '';

  isLoading = false;

  constructor(
    private fb: FormBuilder,
    private patientService: PatientService,
    private router: Router
  ) {

    this.profileForm = this.fb.group({

      fullName: [
        '',
        [
          Validators.required,
          Validators.minLength(2)
        ]
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
        '',
        [
          Validators.required,
          Validators.email
        ]
      ],

      phoneNumber: [
        '',
        [
          Validators.required,
          Validators.pattern(/^[0-9]{10}$/)
        ]
      ],

      insuranceID: ['']
    });
  }

  save(): void {

  this.errorMessage = '';

  if (this.profileForm.invalid) {

    this.profileForm.markAllAsTouched();
    return;
  }

  this.isLoading = true;

  this.patientService
    .createProfile(this.profileForm.value)
    .subscribe({

      next: () => {

        this.isLoading = false;

        alert('Profile completed successfully.');

        this.router.navigate([
          '/patient/dashboard'
        ]);
      },

      error: error => {

        this.isLoading = false;

        this.errorMessage =
          error.error?.message ??
          'Unable to save profile.';
      }
    });
}
}