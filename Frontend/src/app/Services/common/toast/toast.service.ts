import { Injectable } from '@angular/core';
import Swal from 'sweetalert2';

@Injectable({
  providedIn: 'root'
})
export class ToastService {

  private Toast = Swal.mixin({
    toast: true,
    position: 'center',
    showConfirmButton: false,
    timer: 1000,
    timerProgressBar: true,
    padding: '0.75rem 1rem',
    background: '#ffffff',
    color: '#333333',
    customClass: {
      popup: 'swal2-compact-toast',
      title: 'swal2-compact-title',
      timerProgressBar: 'swal2-compact-progress'
    },
    didOpen: (toast) => {
      toast.addEventListener('mouseenter', Swal.stopTimer);
      toast.addEventListener('mouseleave', Swal.resumeTimer);
    }
  });

  showSuccess(title: string, message: string) {
    this.Toast.fire({
      icon: 'success',
      iconColor: '#FF6B00',
      title: title,
      text: message
    });
  }

  showError(title: string, message: string) {
    this.Toast.fire({
      icon: 'error',
      iconColor: '#ef4444',
      title: title,
      text: message
    });
  }

  showWarning(title: string, message: string) {
    this.Toast.fire({
      icon: 'warning',
      iconColor: '#f59e0b',
      title: title,
      text: message
    });
  }

  showInfo(title: string, message: string) {
    this.Toast.fire({
      icon: 'info',
      iconColor: '#FF6B00',
      title: title,
      text: message
    });
  }

  async confirmDelete(title: string = 'Delete record?', text: string = "Do You Want Delete."): Promise<boolean> {
    const result = await Swal.fire({
      title: title,
      text: text,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#ef4444',
      cancelButtonColor: '#6b7280',
      confirmButtonText: 'Delete',
      cancelButtonText: 'Cancel',
      reverseButtons: true,
      background: '#ffffff',
      color: '#333',
      iconColor: '#ef4444',
      width: '320px',
      padding: '1.25rem',
      customClass: {
        title: 'swal2-compact-title',
        htmlContainer: 'swal2-compact-text',
        actions: 'swal2-compact-actions',
        confirmButton: 'swal2-compact-btn',
        cancelButton: 'swal2-compact-btn'
      }
    });
    return result.isConfirmed;
  }
}
