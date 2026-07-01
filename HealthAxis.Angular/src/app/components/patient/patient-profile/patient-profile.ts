import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { PatientService } from '../../../services/patient-service';
import { PatientProfile } from '../../../dtos/patient';

@Component({
  selector: 'app-patient-profile',
  imports: [ReactiveFormsModule],
  templateUrl: './patient-profile.html',
  styleUrl: './patient-profile.css',
})
export class PatientProfileComponent implements OnInit {
  profileForm!: FormGroup;
  patientService = inject(PatientService);
  fb = inject(FormBuilder);
  router = inject(Router);
  profile: PatientProfile | null = null;

  ngOnInit(): void {
    this.profileForm = this.fb.group({
      patientId: [0],
      fullName: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      gender: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', Validators.required],
      insuranceID: [''],
      isActive: [true]
    });

    this.patientService.getProfile().subscribe({
      next: data => {
        this.profile = data;
        this.profileForm.patchValue(data);
      },
      error: () => this.router.navigate(['/patient/complete-profile'])
    });
  }

  saveProfile(): void {
    if (this.profileForm.valid) {
      this.patientService.updateProfile(this.profileForm.value).subscribe({
        next: data => {
          this.profile = data;
        }
      });
    }
  }
}
