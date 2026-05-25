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

  private sectorIdSubject = new BehaviorSubject<string | null>(localStorage.getItem('sectorId'));
  sectorId$ = this.sectorIdSubject.asObservable();

  private mandalIdSubject = new BehaviorSubject<string | null>(localStorage.getItem('mandalId'));
  mandalId$ = this.mandalIdSubject.asObservable();

  private stateIdSubject = new BehaviorSubject<string | null>(localStorage.getItem('stateId'));
  stateId$ = this.stateIdSubject.asObservable();

  private refreshTokenSubject = new BehaviorSubject<string | null>(localStorage.getItem('refreshToken'));
  refreshToken$ = this.refreshTokenSubject.asObservable();

  private expiresAtSubject = new BehaviorSubject<string | null>(localStorage.getItem('expiresAt'));
  expiresAt$ = this.expiresAtSubject.asObservable();

  private profileDataSubject = new BehaviorSubject<any>(null);
  profileData$ = this.profileDataSubject.asObservable();

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

  setSectorId(sectorId: string) {
    localStorage.setItem('sectorId', sectorId);
    this.sectorIdSubject.next(sectorId);
  }

  getSectorId(): string | null {
    return this.sectorIdSubject.value;
  }

  setMandalId(mandalId: string) {
    localStorage.setItem('mandalId', mandalId);
    this.mandalIdSubject.next(mandalId);
  }

  getMandalId(): string | null {
    return this.mandalIdSubject.value;
  }

  setStateId(stateId: string) {
    localStorage.setItem('stateId', stateId);
    this.stateIdSubject.next(stateId);
  }

  getStateId(): string | null {
    return this.stateIdSubject.value;
  }

  setProfileData(data: any) {
    this.profileDataSubject.next(data);
  }

  getProfileData(): any {
    return this.profileDataSubject.value;
  }

  setRefreshToken(token: string) {
    localStorage.setItem('refreshToken', token);
    this.refreshTokenSubject.next(token);
  }

  getRefreshToken(): string | null {
    return this.refreshTokenSubject.value;
  }

  setExpiresAt(expiresAt: string) {
    localStorage.setItem('expiresAt', expiresAt);
    this.expiresAtSubject.next(expiresAt);
  }

  getExpiresAt(): string | null {
    return this.expiresAtSubject.value;
  }

  clearRole() {
    localStorage.removeItem('userRole');
    localStorage.removeItem('userId');
    localStorage.removeItem('token');
    localStorage.removeItem('boothId');
    localStorage.removeItem('sectorId');
    localStorage.removeItem('mandalId');
    localStorage.removeItem('stateId');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('expiresAt');
    this.userRoleSubject.next(null);
    this.userIdSubject.next(null);
    this.tokenSubject.next(null);
    this.boothIdSubject.next(null);
    this.sectorIdSubject.next(null);
    this.mandalIdSubject.next(null);
    this.stateIdSubject.next(null);
    this.refreshTokenSubject.next(null);
    this.expiresAtSubject.next(null);
    this.profileDataSubject.next(null);
  }
}
