import {HttpEvent, HttpHandlerFn, HttpRequest} from '@angular/common/http';
import {catchError, Observable, retry, throwError, timer} from 'rxjs';
import {inject} from '@angular/core';
import Configuration from '../config/configuration';
import {AuthService} from '../services/auth.service';
import {MessageService} from 'primeng/api';
import {JsonResponse} from '../models/jsonResponse';

export function addBaseUrlInterceptor(
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
): Observable<HttpEvent<unknown>> {
  const config = inject(Configuration);
  const fullUrl = `${config.Url}${req.url}`;
  const updatedRequest = req.clone({url: fullUrl});
  return next(updatedRequest);
}

export function authInterceptor(
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
): Observable<HttpEvent<unknown>> {
  const authService = inject(AuthService);
  const authToken = authService.GetAuthToken();

  const updatedRequest = req.clone({
    headers: req.headers.has('Authorization')
      ? req.headers
      : req.headers.append('Authorization', `Bearer ${authToken}`),
  });
  return next(updatedRequest);
}

export function retryInterceptor(
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
): Observable<HttpEvent<unknown>> {
  const messageService = inject(MessageService);

  return next(req).pipe(
    retry({count: 2, delay: (error, retryCount) => timer(retryCount * 500)}),// delay 1, 2, 3 second each attempt
    catchError((error) => {
      console.warn('HTTP Error:', error);
      const responseError: JsonResponse<any> = error.error;
      switch (error.status) {
        case 500: {
          messageService.add({
            severity: 'error',
            summary: 'Network Error',
            detail: 'A network error occurred. Please try again.',
          });
          break
        }
        default:
          messageService.add({
            severity: 'error',
            summary: 'Network Error',
            detail: responseError.errors.map(error => error + ' ').toString(),
          });
      }
      return throwError(() => error);
    })
  );
}

// return next(req).pipe(
//   retry({count: 2, delay: (error, retryCount) => timer(retryCount * 1000)}),// delay 1, 2, 3 second each attempt
//   catchError(async (error) => {
//     console.log("Error caught in the interceptor");
//     alert('A network error occurred. Please try again.');
//     return throwError(() => error);
//   })
// );
