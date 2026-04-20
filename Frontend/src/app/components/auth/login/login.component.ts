import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { ReactiveFormsModule, FormGroup, FormControl, Validators } from '@angular/forms';
import { AuthServiceService } from '../../../Services/Auth/auth.service';
import { ToastService } from '../../../Services/common/toast/toast.service';


import { CommonModule } from '@angular/common';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule]  // ✅ Import here
})
export class LoginComponent {

  loginForm = new FormGroup({
    mobileNumber: new FormControl('', [Validators.required, Validators.pattern(/^\d{10}$/)]),
    password: new FormControl('', [Validators.required, Validators.minLength(6)])
  });

  constructor(private router: Router, private http: HttpClient, private authService: AuthServiceService, private toastService: ToastService) { }

  onLogin(event: Event) {
    event.preventDefault();

    if (this.loginForm.invalid) {
      this.toastService.showWarning('Invalid Input', 'Please enter a valid mobile number and password.');
      return;
    }

    const payload = this.loginForm.value;
    const mobile = payload.mobileNumber;

    // Static Bypass for Testing
    if (mobile === '8888888888') {
      this.authService.setRole('SUPERADMIN');
      this.toastService.showSuccess('Test Mode', 'Static Login Successful as SUPERADMIN');
      this.redirectByRole('SUPERADMIN');
      return;
    } else if (mobile === '9999999999') {
      this.authService.setRole('ADMIN');
      this.toastService.showSuccess('Test Mode', 'Static Login Successful as ADMIN');
      this.redirectByRole('ADMIN');
      return;
    } else if (mobile === '7777777777') {
      this.authService.setRole('STATEPRABHARI');
      this.toastService.showSuccess('Test Mode', 'Static Login Successful as STATEPRABHARI');
      this.redirectByRole('STATEPRABHARI');
      return;
    }

    this.http.post(`${environment.apiUrl}/auth/login`, payload)
      .subscribe({
        next: (res: any) => {
          this.toastService.showSuccess('Success', 'Login Successful!');
          const role = res.data?.role || 'ADMIN';
          const userId = res.data?.userId || '';
          this.authService.setRole(role);
          if (userId) this.authService.setUserId(userId);
          this.redirectByRole(role);
        },
        error: (err: any) => {
          const msg = err.error?.detail || 'Invalid credentials';
          this.toastService.showError('Login Failed', msg);
        }
      });
  }

  private redirectByRole(role: string) {
    const r = (role || '').toUpperCase().trim();
    console.log('Redirecting for role:', r);
    if (r === 'SUPERADMIN') {
      this.router.navigate(['/superadmin/dashboard']);
    } else if (r === 'STATEPRABHARI') {
      this.router.navigate(['/state-prabhari/dashboard']);
    } else {
      this.router.navigate(['/']);
    }
  }
}