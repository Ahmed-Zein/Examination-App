import {ApplicationConfig, ErrorHandler, importProvidersFrom, provideZoneChangeDetection} from '@angular/core';
import {provideRouter} from '@angular/router';

import {routes} from './app.routes';
import {provideHttpClient, withInterceptors} from '@angular/common/http';
import {AddBaseUrl, authInterceptor} from './core/interceptors/interceptors';
import {JwtModule} from '@auth0/angular-jwt';
import {provideAnimationsAsync} from '@angular/platform-browser/animations/async';
import {providePrimeNG} from 'primeng/config';
import Aura from '@primeng/themes/lara';

export const appConfig: ApplicationConfig = {
  providers: [
    ErrorHandler,
    provideZoneChangeDetection({eventCoalescing: true}),
    provideRouter(routes),
    importProvidersFrom(JwtModule.forRoot({})),
    provideAnimationsAsync(),
    providePrimeNG({
      theme: {
        preset: Aura,
        options: {
          darkModeSelector: '.dark-mode',
        },
      },
    })
    , provideHttpClient(withInterceptors([AddBaseUrl, authInterceptor])),
  ]
};
