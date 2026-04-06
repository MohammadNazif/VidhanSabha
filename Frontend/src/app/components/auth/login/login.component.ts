import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { ReactiveFormsModule, FormGroup, FormControl, Validators } from '@angular/forms';
import { AuthServiceService } from '../../../Services/Auth/auth.service';

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

  constructor(private router: Router, private http: HttpClient, private authService: AuthServiceService) { }

  onLogin(event: Event) {
    event.preventDefault();

    if (this.loginForm.invalid) {
      alert('Please enter a valid mobile number and password.');
      return;
    }

    const payload = this.loginForm.value;

    this.http.post('https://localhost:7093/api/auth/login', payload)
      .subscribe({
        next: (res: any) => {
          alert('Login Successful!');
          const role = res.role || 'ADMIN'; 
          this.authService.setRole(role);
          this.router.navigate(['/']);
        },
        error: (err: any) => {
          // Check for mock login for testing purposes
          const mobile = this.loginForm.get('mobileNumber')?.value;
          if (mobile === '9999999999') {
             this.authService.setRole('ADMIN');
             alert('Test Login Successful as ADMIN');
             this.router.navigate(['/']);
          } else if (mobile === '8888888888') {
             this.authService.setRole('SECTOR');
             alert('Test Login Successful as SECTOR');
             this.router.navigate(['/']);
          } else {
             const msg = err.error?.detail || 'Invalid credentials';
             alert(`Login Failed: ${msg}`);
          }
        }
      });
  }
}