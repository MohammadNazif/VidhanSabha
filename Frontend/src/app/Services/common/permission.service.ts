import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { map, tap, catchError } from 'rxjs/operators';
import { AuthServiceService } from '../Auth/auth.service';
import { AccessService } from '../Admin/access/access.service';

@Injectable({
  providedIn: 'root'
})
export class PermissionService {
  private permissionsSubject = new BehaviorSubject<any[]>([]);
  permissions$ = this.permissionsSubject.asObservable();
  
  private lastUserId: string | null = null;

  constructor(
    private authService: AuthServiceService,
    private accessService: AccessService
  ) {
    // Automatically load permissions when user changes
    this.authService.userId$.subscribe(userId => {
      if (userId && userId !== this.lastUserId) {
        this.loadPermissions(userId);
        this.lastUserId = userId;
      } else if (!userId) {
        this.permissionsSubject.next([]);
        this.lastUserId = null;
      }
    });
  }

  /**
   * Load permissions for the given user ID and cache them.
   */
  loadPermissions(userId: string | number): void {
    this.accessService.getPermissionByUserId(userId).pipe(
      catchError(err => {
        console.error('Error loading permissions:', err);
        return of({ isSuccess: false, data: [] });
      })
    ).subscribe(res => {
      if (res && res.isSuccess && Array.isArray(res.data)) {
        this.permissionsSubject.next(res.data);
      } else {
        this.permissionsSubject.next([]);
      }
    });
  }

  /**
   * Check if the current user has permission for a specific module.
   * @param moduleId The ID of the module to check (from ModulePermission enum).
   */
  hasPermission(moduleId: number): boolean {
    const role = (this.authService.getRole() || '').toUpperCase();
    
    // Admins and Superadmins usually have all permissions
    if (role === 'ADMIN' || role === 'SUPERADMIN') {
      return true;
    }

    const permissions = this.permissionsSubject.value;
    const perm = permissions.find(p => p.module === moduleId);
    return perm ? perm.hasPermission : false;
  }

  /**
   * Refresh permissions manually.
   */
  refreshPermissions(): void {
    const userId = this.authService.getUserId();
    if (userId) {
      this.loadPermissions(userId);
    }
  }
}
