import { inject } from '@angular/core';
import {
  CanActivateFn,
  Router
} from '@angular/router';

import { AuthService } from '../services/auth-service';

export const firstLoginGuard: CanActivateFn = () => {

  const authService = inject(AuthService);
  const router = inject(Router);

  const pendingEmail =
    authService.getPendingFirstLoginEmail();

  if (pendingEmail) {

    router.navigate([
      '/doctor/first-login'
    ]);

    return false;
  }

  return true;
};