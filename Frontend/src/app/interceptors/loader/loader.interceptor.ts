import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { LoaderService } from '../../Services/common/loader/loader.service';
import { finalize } from 'rxjs/operators';

export const loaderInterceptor: HttpInterceptorFn = (req, next) => {
  const loaderService = inject(LoaderService);

  // Skip loader for background requests, exports, or table data loads
  if (req.headers.has('X-Skip-Loader') || 
      req.url.includes('/export/') || 
      req.url.includes('/getAll') ||
      req.url.includes('/common/') ||
      req.url.includes('/getBy')) {
    return next(req);
  }

  loaderService.showLoader();

  return next(req).pipe(
    finalize(() => loaderService.hideLoader())
  );
};
