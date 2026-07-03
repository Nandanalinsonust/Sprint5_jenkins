import { Component } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
  AbstractControl,
  ValidationErrors
} from '@angular/forms';

import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

import { AuthService } from '../../../services/auth-service';

@Component({
  selector: 'app-first-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './first-login.html',
  styleUrl: './first-login.css'
})
export class FirstLoginComponent {

  form: FormGroup;

  message = '';

  isLoading = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {

    this.form = this.fb.group(
      {
        currentPassword: [
          '',
          Validators.required
        ],

        newPassword: [
          '',
          [
            Validators.required,
            Validators.minLength(8),
            Validators.pattern(
              /^(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).+$/
            )
          ]
        ],

        confirmPassword: [
          '',
          Validators.required
        ]
      },
      {
        validators: this.passwordMatchValidator
      }
    );
  }

  changePassword(): void {

    this.message = '';

    if (this.form.invalid) {

      this.form.markAllAsTouched();
      return;
    }

    const email =
      this.authService.getPendingFirstLoginEmail();

    if (!email) {

      this.message =
        'Login session expired. Please login again.';

      this.router.navigate(['/login']);
      return;
    }

    this.isLoading = true;

    this.authService.changePassword({


      currentPassword:
        this.form.value.currentPassword,

      newPassword:
        this.form.value.newPassword,

      confirmPassword:
        this.form.value.confirmPassword

    }).subscribe({

      next: () => {

        this.isLoading = false;

        this.authService.clearPendingFirstLoginEmail();

        alert(
          'Password changed successfully. Please login again.'
        );

        this.router.navigate(['/login']);
      },

      error: error => {

        this.isLoading = false;

        this.message =
          this.authService.getErrorMessage(error);
      }
    });
  }

  private passwordMatchValidator(
    control: AbstractControl
  ): ValidationErrors | null {

    const password =
      control.get('newPassword')?.value;

    const confirmPassword =
      control.get('confirmPassword')?.value;

    if (!password || !confirmPassword) {
      return null;
    }

    return password === confirmPassword
      ? null
      : { passwordMismatch: true };
  }
}