import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthServiceService {
  private userRoleSubject = new BehaviorSubject<string | null>(localStorage.getItem('userRole'));
  userRole$ = this.userRoleSubject.asObservable();

  constructor() { }

  setRole(role: string) {
    localStorage.setItem('userRole', role);
    this.userRoleSubject.next(role);
  }

  getRole(): string | null {
    return this.userRoleSubject.value;
  }

  clearRole() {
    localStorage.removeItem('userRole');
    this.userRoleSubject.next(null);
  }
}
