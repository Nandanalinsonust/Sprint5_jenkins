import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';

import { CommonModule } from '@angular/common';
import { AuthService } from '../../../services/auth-service';


@Component({
  selector: 'app-main-nav',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink
  ],
  templateUrl: './main-nav.html',
  styleUrl: './main-nav.css'
})
export class MainNavComponent {

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  isLoggedIn(): boolean {

    return this.authService.isLoggedIn();
  }

  isPatient(): boolean {

    return this.authService.isPatient();
  }

  isDoctor(): boolean {

    return this.authService.isDoctor();
  }

  isAdmin(): boolean {

    return this.authService.isAdmin();
  }

  logout(): void {

    this.authService.logout();

    this.router.navigate([
      '/login'
    ]);
  }
}