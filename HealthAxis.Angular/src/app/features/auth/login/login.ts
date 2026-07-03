import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../services/auth-service';
@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './login.html',
  styleUrl: './login.css'

})
export class LoginComponent {

  loginForm: FormGroup;
  errorMessage = '';
  submitted = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {

    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]]
    });
  }

  // ✅ ADD THIS
  get f() {
    return this.loginForm.controls;
  }

  login() {
    this.submitted = true;

    if (this.loginForm.invalid) return;

    this.authService.login(this.loginForm.value)
      .subscribe({
        next: (res: any) => {

          this.authService.setSession(res.data);

          const role = res.data.role;
          const isFirstLogin = res.data.isFirstLogin;

          if (isFirstLogin && role === 'Doctor') {
            this.router.navigate(['/doctor/change-password']);
            return;
          }

          if (isFirstLogin && role === 'Patient') {
            this.router.navigate(['/patient/complete-profile']);
            return;
          }

          if (role === 'Doctor') {
            this.router.navigate(['/doctor/dashboard']);
          } else {
            this.router.navigate(['/patient/dashboard']);
          }
        },

        error: (err) => {
          this.errorMessage =
            this.authService.getErrorMessage(err);
        }
      });
  }
}