import { Component } from '@angular/core';

import {
  Router,
  RouterLink,
  RouterOutlet
} from '@angular/router';

import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth-service';


@Component({
  selector: 'app-patient-layout',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    RouterOutlet
  ],
  templateUrl: './patient-layout.html',
  styleUrl: './patient-layout.css'
})
export class PatientLayoutComponent {

  sidebarOpen = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  toggleSidebar(): void {

    this.sidebarOpen =
      !this.sidebarOpen;
  }

  logout(): void {

    this.authService.logout();

    this.router.navigate([
      '/login'
    ]);
  }
}