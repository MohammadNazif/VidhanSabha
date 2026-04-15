import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { BaseApiService } from '../../common/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class DesignationService extends BaseApiService {
  private entity = 'designation';

  /**
   * Creates a new Designation through the API.
   */
  createDesignation(data: any): Observable<any> {
    // For now, returning a mock success response to allow UI development
    console.log('Mock: Creating designation', data);
    return this.create(this.entity, data);
  }

  /**
   * Fetches all Designations from the API.
   */
  getAllDesignations(): Observable<any> {
    // Attempting to fetch from API, default to empty if not exists for now
    return this.getAll(this.entity);
  }

  /**
   * Deletes a Designation by its ID.
   */
  deleteDesignation(id: number): Observable<any> {
    return this.delete(this.entity, id);
  }

  /**
   * Updates an existing Designation.
   */
  updateDesignation(data: any): Observable<any> {
    return this.update(this.entity, data);
  }
}
