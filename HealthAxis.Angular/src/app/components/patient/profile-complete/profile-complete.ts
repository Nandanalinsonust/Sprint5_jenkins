import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../services/auth-service';
import { PatientService } from '../../../services/patient-service';
import { PatientProfile } from '../../../dtos/patient';

@Component({
  selector: 'app-profile-complete',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './profile-complete.html',
  styleUrl: './profile-complete.css',
})
export class ProfileCompleteComponent implements OnInit {
  profileForm!: FormGroup;
  patientService = inject(PatientService);
  authService = inject(AuthService);
  fb = inject(FormBuilder);
  router = inject(Router);
  profile: PatientProfile | null = null;
  isNew = false;
  message = '';

  ngOnInit(): void {
    this.profileForm = this.fb.group({
      fullName: ['', [Validators.required, Validators.minLength(2)]],
      dateOfBirth: ['', Validators.required],
      gender: ['Male', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', Validators.required],
      insuranceID: ['']
    });

    const currentEmail = this.authService.getCurrentUserEmail();
    if (currentEmail) {
      this.profileForm.get('email')?.setValue(currentEmail);
    }

    this.patientService.getProfile().subscribe({
      next: data => {
        if (data && data.patientId) {
          this.profile = data;
          this.profileForm.patchValue(data);
          this.isNew = false;
        } else {
          this.isNew = true;
        }
      },
      error: () => {
        this.isNew = true;
      }
    });
  }

  save(): void {
    if (this.profileForm.invalid) {
      this.message = 'Please complete all fields correctly.';
      return;
    }

    const payload = {
      ...this.profileForm.value,
      dateOfBirth: this.profileForm.value.dateOfBirth
    };

    if (this.profile) {
      payload.patientId = this.profile.patientId;
      this.patientService.updateProfile(payload).subscribe({
        next: () => {
          this.message = 'Profile updated successfully. Redirecting to dashboard...';
          setTimeout(() => this.router.navigate(['/patient/dashboard']), 800);
        },
        error: err => {
          this.message = err?.error?.message || err?.error || 'Unable to save profile.';
        }
      });
      return;
    }

    this.patientService.createProfile(payload).subscribe({
      next: () => {
        this.message = 'Profile saved successfully. Redirecting to dashboard...';
        setTimeout(() => this.router.navigate(['/patient/dashboard']), 800);
      },
      error: err => {
        this.message = err?.error?.message || err?.error || 'Unable to save profile.';
      }
    });
  }
}
