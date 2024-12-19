import { HttpEvent, HttpHandlerFn, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { inject } from '@angular/core';
import Configuration from '../config/configuration';
import { AuthService } from '../services/auth.service';

export function AddBaseUrl(
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
): Observable<HttpEvent<unknown>> {
  const config = inject(Configuration);
  const url = config.Url + req.url;
  const newReq = req.clone({ url });
  return next(newReq);
}

export function authInterceptor(
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
) {
  const authToken = inject(AuthService).GetAuthToken();
  const newReq = req.clone({
    headers: req.headers.append('Authorization', `Bearer ${authToken}`),
  });
  return next(newReq);
}
