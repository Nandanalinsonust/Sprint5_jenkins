import { inject } from '@angular/core';
import {
  CanActivateFn,
  ActivatedRouteSnapshot,
  Router
} from '@angular/router';
import { AuthService } from '../services/auth-service';


export const roleGuard: CanActivateFn = (
  route: ActivatedRouteSnapshot
) => {

  const authService = inject(AuthService);
  const router = inject(Router);

  const requiredRole =
    route.data['role'];

  const userRoles =
    authService.getUserRoles();

  if (
    authService.isLoggedIn() &&
    userRoles.includes(requiredRole)
  ) {
    return true;
  }

  router.navigate(['/login']);

  return false;
};