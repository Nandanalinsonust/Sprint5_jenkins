import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../services/auth-service';

@Component({
  selector: 'app-first-login',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './first-login.html',
  styleUrl: './first-login.css',
})
export class FirstLoginComponent {
  form: FormGroup;
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);
  message = '';

  constructor() {
    this.form = this.fb.group({
      oldPassword: ['', Validators.required],
      newPassword: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required],
    });
  }

  submit() {
    if (this.form.invalid) {
      this.message = 'Please enter a valid password.';
      return;
    }

    if (this.form.value.newPassword !== this.form.value.confirmPassword) {
      this.message = 'New passwords do not match.';
      return;
    }

    const email = this.authService.getPendingFirstLoginEmail();
    if (!email) {
      this.message = 'Please login again to continue.';
      setTimeout(() => this.router.navigate(['/login']), 700);
      return;
    }

    const newPassword = this.form.value.newPassword;
    this.authService.changePassword({ email, oldPassword: this.form.value.oldPassword, newPassword }).subscribe({
      next: () => {
        this.authService.clearPendingFirstLoginEmail();
        this.message = 'Password updated successfully. Please login again with your new password.';
        setTimeout(() => this.router.navigate(['/login']), 1200);
      },
      error: () => {
        this.message = 'Unable to update password.';
      }
    });
  }
}
