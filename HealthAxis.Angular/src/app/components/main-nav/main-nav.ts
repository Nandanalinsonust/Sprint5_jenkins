import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth-service';

@Component({
  selector: 'app-main-nav',
  imports: [RouterLink],
  templateUrl: './main-nav.html',
  styleUrl: './main-nav.css',
})
export class MainNav {
  constructor(private authService: AuthService, private router: Router) {}

  isLoggedIn() {
    return this.authService.isLoggedIn();
  }

  isPatient() {
    return this.authService.getUserRoles().includes('Patient');
  }

  isDoctor() {
    return this.authService.getUserRoles().includes('Doctor');
  }

  handleLogout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
