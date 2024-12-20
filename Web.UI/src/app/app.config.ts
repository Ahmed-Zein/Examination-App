import {ApplicationConfig, ErrorHandler, importProvidersFrom, provideZoneChangeDetection} from '@angular/core';
import {provideRouter} from '@angular/router';
import {routes} from './app.routes';
import {provideHttpClient, withInterceptors} from '@angular/common/http';
import {addBaseUrlInterceptor, authInterceptor, retryInterceptor} from './core/interceptors/interceptors';
import {JwtModule} from '@auth0/angular-jwt';
import {provideAnimationsAsync} from '@angular/platform-browser/animations/async';
import {providePrimeNG} from 'primeng/config';
import Aura from '@primeng/themes/lara';
import {GlobalErrorHandler} from './core/error/global.error.handler';
import {MessageService} from 'primeng/api';

export const appConfig: ApplicationConfig = {
  providers: [
    ErrorHandler,
    provideZoneChangeDetection({eventCoalescing: true}),
    provideRouter(routes),
    importProvidersFrom(JwtModule.forRoot({})),
    provideAnimationsAsync(),
    {provide: ErrorHandler, useClass: GlobalErrorHandler},
    {provide: MessageService, useClass: MessageService},
    providePrimeNG({
      theme: {
        preset: Aura,
      },
    })
    , provideHttpClient(withInterceptors([addBaseUrlInterceptor, authInterceptor, retryInterceptor])), provideAnimationsAsync(),
  ]
};
