import { APP_INITIALIZER, ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { errorInterceptor } from '@gofit/shared/interceptors/error.interceptor';
import { loadingInterceptor } from '@gofit/shared/interceptors/loading.interceptor';
import { InitService } from '@gofit/shared/services/init.service';
import { lastValueFrom } from 'rxjs';
import { authInterceptor } from '@gofit/shared/interceptors/auth.interceptor';

function initializeApp(initService: InitService) {
  return () => lastValueFrom(initService.init());
}

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideAnimationsAsync(),
    provideHttpClient(withInterceptors([
      errorInterceptor,
      loadingInterceptor,
      authInterceptor
    ])),
    {
      provide: APP_INITIALIZER,
      useFactory: initializeApp,
      multi: true,
      deps: [InitService]
    }
  ]
};
