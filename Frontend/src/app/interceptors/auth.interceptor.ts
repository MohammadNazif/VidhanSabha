import { HttpInterceptorFn, HttpErrorResponse, HttpClient } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthServiceService } from '../Services/Auth/auth.service';
import { catchError, switchMap, throwError, BehaviorSubject, filter, take } from 'rxjs';
import { environment } from '../../environments/environment';

let isRefreshing = false;
let refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthServiceService);
  const httpClient = inject(HttpClient);
  const token = authService.getToken();

  const addToken = (request: any, tkn: string) => {
    return request.clone({
      setHeaders: {
        Authorization: `Bearer ${tkn}`
      }
    });
  };

  let modifiedReq = req;
  if (token) {
    modifiedReq = addToken(req, token);
  }

  return next(modifiedReq).pipe(
    catchError((error: HttpErrorResponse) => {
      // If 401 and we have a refresh token (and not already trying to refresh)
      if (error.status === 401 && authService.getRefreshToken() && !req.url.includes('/auth/refresh')) {
        if (!isRefreshing) {
          isRefreshing = true;
          refreshTokenSubject.next(null);

          return httpClient.post<any>(`${environment.apiUrl}/auth/refresh?RefreshToken=${encodeURIComponent(authService.getRefreshToken() || '')}`, {}).pipe(
            switchMap((res: any) => {
              isRefreshing = false;
              if (res.isSuccess && res.data?.token) {
                authService.setToken(res.data.token);
                if (res.data.refreshToken) {
                  authService.setRefreshToken(res.data.refreshToken);
                }
                if (res.data.expiresAt) {
                  authService.setExpiresAt(res.data.expiresAt);
                }
                refreshTokenSubject.next(res.data.token);
                return next(addToken(req, res.data.token));
              }
              // Failed to refresh properly
              authService.clearRole();
              window.location.href = '/login';
              return throwError(() => new Error('Token refresh failed'));
            }),
            catchError((refreshErr) => {
              isRefreshing = false;
              authService.clearRole();
              window.location.href = '/login';
              return throwError(() => refreshErr);
            })
          );
        } else {
          // Wait for the new token
          return refreshTokenSubject.pipe(
            filter(tkn => tkn != null),
            take(1),
            switchMap(jwt => {
              return next(addToken(req, jwt));
            })
          );
        }
      }

      // Pass other errors through
      return throwError(() => error);
    })
  );
};
