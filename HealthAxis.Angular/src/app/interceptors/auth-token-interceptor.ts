import { HttpInterceptorFn } from '@angular/common/http';
import { AuthService } from '../services/auth-service';
import { inject } from '@angular/core';

export const authTokenInterceptor: HttpInterceptorFn = (req, next) => {

  const authService = inject(AuthService)
  const token = authService.getToken();
  if(!token || req.url.includes("/auth/login")|| req.url.includes("/auth/register")){
    return next(req);
  }

  const authRequest=req.clone({
    setHeaders:{
      Authorization: `Bearer ${token}`
    }
  });
  return next(authRequest);
};
