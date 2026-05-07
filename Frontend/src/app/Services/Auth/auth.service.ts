import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthServiceService {
  private userRoleSubject = new BehaviorSubject<string | null>(localStorage.getItem('userRole'));
  userRole$ = this.userRoleSubject.asObservable();

  private userIdSubject = new BehaviorSubject<string | null>(localStorage.getItem('userId'));
  userId$ = this.userIdSubject.asObservable();

  private tokenSubject = new BehaviorSubject<string | null>(localStorage.getItem('token'));
  token$ = this.tokenSubject.asObservable();

  private boothIdSubject = new BehaviorSubject<string | null>(localStorage.getItem('boothId'));
  boothId$ = this.boothIdSubject.asObservable();

  private stateIdSubject = new BehaviorSubject<string | null>(localStorage.getItem('stateId'));
  stateId$ = this.stateIdSubject.asObservable();

  constructor() { }

  setRole(role: string) {
    localStorage.setItem('userRole', role);
    this.userRoleSubject.next(role);
  }

  getRole(): string | null {
    return this.userRoleSubject.value;
  }

  setUserId(userId: string) {
    localStorage.setItem('userId', userId);
    this.userIdSubject.next(userId);
  }

  getUserId(): string | null {
    return this.userIdSubject.value;
  }

  setToken(token: string) {
    localStorage.setItem('token', token);
    this.tokenSubject.next(token);
  }

  getToken(): string | null {
    return this.tokenSubject.value;
  }

  setBoothId(boothId: string) {
    localStorage.setItem('boothId', boothId);
    this.boothIdSubject.next(boothId);
  }

  getBoothId(): string | null {
    return this.boothIdSubject.value;
  }

  setStateId(stateId: string) {
    localStorage.setItem('stateId', stateId);
    this.stateIdSubject.next(stateId);
  }

  getStateId(): string | null {
    return this.stateIdSubject.value;
  }

  clearRole() {
    localStorage.removeItem('userRole');
    localStorage.removeItem('userId');
    localStorage.removeItem('token');
    localStorage.removeItem('boothId');
    localStorage.removeItem('stateId');
    this.userRoleSubject.next(null);
    this.userIdSubject.next(null);
    this.tokenSubject.next(null);
    this.boothIdSubject.next(null);
    this.stateIdSubject.next(null);
  }
}
