import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth-service';

export const roleGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const expectedRole = route.data['role'] as string;

  if (authService.isLoggedIn() && authService.getUserRoles().includes(expectedRole)) {
    return true;
  }

  router.navigate(['/login']);
  return false;
};
