import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

export interface Toast {
  id: number;
  type: 'success' | 'error' | 'warning';
  title: string;
  message: string;
}

@Injectable({
  providedIn: 'root'
})
export class ToastService {
  private toastsSubject = new BehaviorSubject<Toast[]>([]);
  toasts$: Observable<Toast[]> = this.toastsSubject.asObservable();
  private nextId = 0;

  showSuccess(title: string, message: string) {
    this.addToast('success', title, message);
  }

  showError(title: string, message: string) {
    this.addToast('error', title, message);
  }

  showWarning(title: string, message: string) {
    this.addToast('warning', title, message);
  }

  private addToast(type: 'success' | 'error' | 'warning', title: string, message: string) {
    const id = ++this.nextId;
    const newToast: Toast = { id, type, title, message };
    
    this.toastsSubject.next([...this.toastsSubject.getValue(), newToast]);

    // Auto-remove after 2 seconds
    setTimeout(() => {
      this.removeToast(id);
    }, 2000);
  }

  removeToast(id: number) {
    const currentToasts = this.toastsSubject.getValue();
    this.toastsSubject.next(currentToasts.filter(t => t.id !== id));
  }
}
