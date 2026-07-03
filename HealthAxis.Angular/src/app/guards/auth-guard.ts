import { inject } from '@angular/core';
import {
  CanActivateFn,
  Router
} from '@angular/router';
import { AuthService } from '../services/auth-service';


export const authGuard: CanActivateFn = () => {

  const authService =
    inject(AuthService);

  const router =
    inject(Router);

  const token =
    authService.getToken();

  if (!token) {

    router.navigate([
      '/login'
    ]);

    return false;
  }

  try {

    const payload = JSON.parse(
      atob(token.split('.')[1])
    );

    const expiration =
      payload.exp * 1000;

    if (
      Date.now() > expiration
    ) {

      authService.logout();

      router.navigate([
        '/login'
      ]);

      return false;
    }

    return true;
  }
  catch {

    authService.logout();

    router.navigate([
      '/login'
    ]);

    return false;
  }
};