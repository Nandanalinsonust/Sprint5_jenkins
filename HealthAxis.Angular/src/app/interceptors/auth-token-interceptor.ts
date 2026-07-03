import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth-service';

export const authTokenInterceptor: HttpInterceptorFn = (request, next) => {

  const authService = inject(AuthService);
  const token = authService.getToken();

  console.log("INTERCEPTOR TOKEN:", token);

  // skip auth endpoints
  if (
    !token ||
    request.url.includes('/auth/login') ||
    request.url.includes('/auth/register') ||
    request.url.includes('/auth/change-password')
  ) {
    return next(request);
  }

  const authRequest = request.clone({
    setHeaders: {
      Authorization: `Bearer ${token}`
    }
  });

  return next(authRequest);
};