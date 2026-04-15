import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { ReactiveFormsModule, FormGroup, FormControl, Validators } from '@angular/forms';
import { AuthServiceService } from '../../../Services/Auth/auth.service';
import { ToastService } from '../../../Services/common/toast/toast.service';

import { CommonModule } from '@angular/common';

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
      this.router.navigate(['/superadmin/dashboard']);
      return;
    } else if (mobile === '9999999999') {
      this.authService.setRole('ADMIN');
      this.toastService.showSuccess('Test Mode', 'Static Login Successful as ADMIN');
      this.router.navigate(['/']);
      return;
    }

    this.http.post('https://localhost:7093/api/auth/login', payload)
      .subscribe({
        next: (res: any) => {
          this.toastService.showSuccess('Success', 'Login Successful!');
          const role = res.role || 'ADMIN';
          this.authService.setRole(role);
          this.router.navigate(['/']);
        },
        error: (err: any) => {
          const msg = err.error?.detail || 'Invalid credentials';
          this.toastService.showError('Login Failed', msg);
        }
      });
  }
}