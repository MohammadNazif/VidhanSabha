import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { ToastService } from './toast/toast.service';
import { LoaderService } from './loader/loader.service';

@Injectable({
  providedIn: 'root'
})
export class CrudHandlerService {

  constructor(
    private toastService: ToastService,
    private loaderService: LoaderService
  ) { }

  /**
   * Standard handler for CRUD operations.
   * Handles: Loader, Success Toast, Error Toast, and Success Callback.
   * 
   * @param request The API observable to execute.
   * @param successTitle Title for the success toast.
   * @param successMessage Message for the success toast.
   * @param onComplete Optional callback to refresh data on success.
   * @param showSuccessToast Whether to show a success toast (default: true).
   */
  handleRequest<T>(
    request: Observable<T>,
    successTitle: string,
    successMessage: string,
    onComplete?: (response: T) => void,
    showSuccessToast: boolean = true
  ): void {
    this.loaderService.showLoader();

    request.pipe(
      finalize(() => this.loaderService.hideLoader())
    ).subscribe({
      next: (response: any) => {
        if (showSuccessToast) {
          this.toastService.showSuccess(successTitle, successMessage);
        }
        if (onComplete) {
          onComplete(response);
        }
      },
      error: (err: any) => {
        console.error('API Error:', err);
        let errorMsg = 'An unexpected error occurred. Please try again.';

        if (err.error?.errors) {
          errorMsg = Object.values(err.error.errors).flat().join(' ');
        } else if (err.error?.detail) {
          errorMsg = err.error.detail;
        } else if (err.error?.message) {
          errorMsg = err.error.message;
        }

        this.toastService.showError('Operation Failed', errorMsg);
      }
    });
  }
}
