import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth-service';
import { PatientService } from '../../services/patient-service';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login implements OnInit {
  loginForm!: FormGroup;
  fb: FormBuilder = inject(FormBuilder);
  private authService: AuthService = inject(AuthService);
  private patientService = inject(PatientService);
  private router = inject(Router);

  errorMessage = '';

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      email: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  handleLogin(): void {
    const loginPayload = this.loginForm.value;
    this.authService.login(loginPayload).subscribe({
      next: (response) => {
        this.authService.clearPendingFirstLoginEmail();
    const token = response.data.token;
    const roles = this.authService.getUserRolesFromToken(token);

    if (roles.includes('Admin')) {
      const redirectUrl = `https://localhost:7110/bridge.html?accessToken=${encodeURIComponent(token)}`;
      window.location.href = redirectUrl;
      return;
    }

    this.authService.setSession(response);
        if (roles.includes('Doctor')) {
          this.router.navigate(['/doctor/dashboard']);
          return;
        }

        this.patientService.getProfile().subscribe({
          next: () => this.router.navigate(['/patient/dashboard']),
          error: () => this.router.navigate(['/patient/complete-profile'])
        });
      },
      error: (err) => {
        if (err.status === 401 && err.error?.message === 'FirstLogin') {
          this.authService.setPendingFirstLoginEmail(loginPayload.email);
          this.router.navigate(['/doctor/first-login']);
          return;
        }
        this.errorMessage = 'Invalid Login Credentials';
      }
    });
  }
}
