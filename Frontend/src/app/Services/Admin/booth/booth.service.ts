import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../common/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class BoothService extends BaseApiService {
  private entity = 'booth';

  createBooth(data: any): Observable<any> {
    return this.create(this.entity, data);
  }

  getAllBooths(queryParams: any = {}): Observable<any> {
    const params: any = {
      PageNumber: queryParams.pageNumber || 1,
      PageSize: queryParams.pageSize || 10,
      IsDescending: queryParams.isDescending === true || queryParams.isDescending === 'true'
    };

    if (queryParams.mandalId) params['MandalId'] = queryParams.mandalId;
    if (queryParams.sectorId) params['SectorId'] = queryParams.sectorId;
    if (queryParams.searchTerm) params['SearchTerm'] = queryParams.searchTerm;
    if (queryParams.sortBy) params['SortBy'] = queryParams.sortBy;

    return this.getAllByParams(this.entity, params);
  }

  deleteBooth(id: number | string): Observable<any> {
    return this.delete(this.entity, id);
  }

  updateBooth(data: any): Observable<any> {
    return this.update(this.entity, data);
  }
}
