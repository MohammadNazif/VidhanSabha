import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { LoaderService } from '../../Services/loader/loader.service';
import { finalize } from 'rxjs/operators';

export const loaderInterceptor: HttpInterceptorFn = (req, next) => {
  const loaderService = inject(LoaderService);
  
  // Skip loader for background requests (e.g., cascading dropdowns)
  if (req.headers.has('X-Skip-Loader')) {
    return next(req);
  }

  loaderService.showLoader();

  return next(req).pipe(
    finalize(() => loaderService.hideLoader())
  );
};
