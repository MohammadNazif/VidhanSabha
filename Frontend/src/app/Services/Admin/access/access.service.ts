import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../common/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class AccessService extends BaseApiService {
  private entity = 'allowaccess';

  /**
   * Save permissions for a specific user/designation using the new simplified format.
   */
  createPermission(data: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/permission/create`, data);
  }

  updatePermission(data: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/permission/update`, data);
  }

  /**
   * Old method - deprecated
   */
  savePermissions(data: any): Observable<any> {
    console.log('Mock: Saving permissions', data);
    return this.create(this.entity, data);
  }

  /**
   * Get permissions for all users.
   */
  getAllAccess(params?: any): Observable<any> {
    return this.getAllByParams(this.entity, params);
  }

  /**
   * Update existing access.
   */
  updateAccess(data: any): Observable<any> {
    return this.update(this.entity, data);
  }

  /**
   * Get permissions for a specific entity.
   */
  getAccessById(id: number | string): Observable<any> {
    return this.getById(this.entity, id);
  }

  /**
   * Get permissions for a specific user using the new endpoint.
   */
  getPermissionByUserId(userId: string | number): Observable<any> {
    return this.http.post(`${this.apiUrl}/permission/getbyuserid?UserId=${userId}`, {});
  }

  /**
   * Delete access for a specific entity.
   */
  deleteAccess(id: number): Observable<any> {
    return this.delete(this.entity, id);
  }
}
