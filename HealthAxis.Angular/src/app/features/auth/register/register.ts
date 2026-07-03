import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  Validators
} from '@angular/forms';

import { Router } from '@angular/router';
import { AuthService } from '../../../services/auth-service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class RegisterComponent {

  registerForm: FormGroup;

  errorMessage = '';

  successMessage = '';

  isLoading = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {

    this.registerForm = this.fb.group(
      {

        email: [
          '',
          [
            Validators.required,
            Validators.email
          ]
        ],

        password: [
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

  register(): void {

    this.errorMessage = '';
    this.successMessage = '';

    if (this.registerForm.invalid) {

      this.registerForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;

    this.authService
      .register(this.registerForm.value)
      .subscribe({

        next: () => {

          this.isLoading = false;

          this.successMessage =
            'Registration successful. Please login.';

          setTimeout(() => {

            this.router.navigate([
              '/login'
            ]);

          }, 1500);
        },

        error: error => {

          this.isLoading = false;

          this.errorMessage =
            this.authService.getErrorMessage(error);
        }
      });
  }

  private passwordMatchValidator(
    control: AbstractControl
  ): ValidationErrors | null {

    const password =
      control.get('password')?.value;

    const confirmPassword =
      control.get('confirmPassword')?.value;

    if (!password || !confirmPassword) {
      return null;
    }

    return password === confirmPassword
      ? null
      : { passwordMismatch: true };
  }

  get email() {
    return this.registerForm.get('email');
  }

  get password() {
    return this.registerForm.get('password');
  }

  get confirmPassword() {
    return this.registerForm.get('confirmPassword');
  }

  get passwordsDoNotMatch(): boolean {

    return !!this.registerForm.errors?.[
      'passwordMismatch'
    ];
  }
}