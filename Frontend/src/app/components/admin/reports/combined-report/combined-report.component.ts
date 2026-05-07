import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MandalService } from '../../../../Services/Admin/mandal/mandal.service';
import { PageHeaderComponent } from '../../../shared/page-header/page-header.component';
import { LucideAngularModule } from 'lucide-angular';
import { ToastService } from '../../../../Services/common/toast/toast.service';
import { GenericTableComponent } from '../../../shared/generic-table/generic-table.component';
import { TableColumn, TableConfig, TableFilter } from '../../../shared/generic-table/generic-table.types';
import { SectorService } from '../../../../Services/Admin/sector/sector.service';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../../environments/environment';

import { GenericExportComponent } from '../../../shared/generic-export/generic-export.component';

@Component({
  selector: 'app-combined-report',
  standalone: true,
  imports: [CommonModule, PageHeaderComponent, LucideAngularModule, GenericTableComponent, GenericExportComponent],
  templateUrl: './combined-report.component.html',
  styleUrl: './combined-report.component.css'
})
export class CombinedReportComponent implements OnInit {
  reportData: any[] = [];
  flattenedData: any[] = [];
  loading = false;
  exporting = false;
  totalCount = 0;

  // Pagination
  pageNumber = 1;
  pageSize = 50;
  searchTerm: string = '';
  sortBy: string = 'mandalName';
  isDescending: boolean = false;
  totalPages = 0;

  columns: TableColumn[] = [
    { key: 'mandalName', label: 'Mandal', sortable: true },
    { key: 'sectorName', label: 'Sector', sortable: true },
    { key: 'inchargeName', label: 'Sector Sanyojak', sortable: true },
    { key: 'inchargeFatherName', label: 'Sanyojak Father', sortable: true },
    { key: 'phoneNumber', label: 'Sector Sanyojak Phone', sortable: true },
    { key: 'boothNumber', label: 'Booth No.', sortable: true },
    { key: 'pollingStationName', label: 'Polling Station', sortable: true },
    { key: 'sanyojakProfile', label: 'Booth Adhyaksh Photo', type: 'avatar', align: 'center', sortable: false, avatarFallbackKey: 'sanyojakName' },
    { key: 'sanyojakName', label: 'Booth Adhyaksh', sortable: true },
    { key: 'sanyojakFatherName', label: 'Adhyaksh Father', sortable: true },
    { key: 'sanyojakPhone', label: 'Booth Adhyaksh Phone', sortable: true },
    { key: 'villageNames', label: 'Villages', sortable: false }
  ];

  config: TableConfig = {
    searchable: true,
    searchPlaceholder: 'Search combined report...',
    paginated: true,
    defaultPageSize: 50,
    showRowNumbers: true,
    striped: true,
    hoverable: true,
    serverSide: true,
    compact: true,
    filterable: true,
    // selectable: true,
    filters: [
      { key: 'MandalIds', label: 'Mandal', type: 'select', options: [], placeholder: '-- Select Mandal --', multiple: true },
      { key: 'SectorIds', label: 'Sector', type: 'select', options: [], placeholder: '-- Select Sector --', multiple: true },
      { key: 'VillageIds', label: 'Village', type: 'select', options: [], placeholder: '-- Select Village --', multiple: true },
      { key: 'CastIds', label: 'Caste', type: 'select', options: [], placeholder: '-- Select Caste --', multiple: true }
    ]
  };

  private currentFilters: any = {};

  constructor(
    private mandalService: MandalService,
    private sectorService: SectorService,
    private toastService: ToastService,
    private http: HttpClient
  ) { }

  ngOnInit(): void {
    this.loadReport();
    this.loadFilterOptions();
  }

  loadFilterOptions(): void {
    // Load Mandals
    this.mandalService.getAllMandals({ pageSize: 1000 }).subscribe(res => {
      if (res.isSuccess && res.data) {
        const mandals = res.data.items || res.data || [];
        const mandalFilter = this.config.filters?.find(f => f.key === 'MandalIds');
        if (mandalFilter) {
          mandalFilter.options = mandals.map((m: any) => ({ label: m.name, value: String(m.id) }));
        }
      }
    });

    // Load Sectors
    this.sectorService.getAllSectors({ pageSize: 1000 }).subscribe(res => {
      if (res.isSuccess && res.data) {
        const sectors = res.data.items || res.data || [];
        const sectorFilter = this.config.filters?.find(f => f.key === 'SectorIds');
        if (sectorFilter) {
          sectorFilter.options = sectors.map((s: any) => ({ label: s.sectorName, value: String(s.id) }));
        }
      }
    });

    // Load Villages
    this.sectorService.getAllSectorVillages().subscribe((res: any) => {
      const filter = this.config.filters?.find(f => f.key === 'VillageIds');
      if (filter) {
        const list = Array.isArray(res?.data?.items) ? res.data.items : (Array.isArray(res?.items) ? res.items : (Array.isArray(res?.data) ? res.data : (Array.isArray(res) ? res : [])));
        filter.options = list.map((v: any) => ({
          label: v.name || v.villageName,
          value: String(v.id || v.villageId)
        }));
      }
    });

    // Load Castes
    this.http.get(`${environment.apiUrl}/common/getAllCast`).subscribe((res: any) => {
      const filter = this.config.filters?.find(f => f.key === 'CastIds');
      if (filter) {
        const list = Array.isArray(res?.data?.items) ? res.data.items : (Array.isArray(res?.items) ? res.items : (Array.isArray(res?.data) ? res.data : (Array.isArray(res) ? res : [])));
        filter.options = list.map((c: any) => ({
          label: c.name || c.castName,
          value: String(c.id)
        }));
      }
    });
  }

  loadReport(): void {
    this.loading = true;
    const params = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      searchTerm: this.searchTerm,
      sortBy: this.sortBy,
      isDescending: this.isDescending,
      ...this.currentFilters
    };

    this.mandalService.getAllCombinedReports(params).subscribe({
      next: (res: any) => {
        try {
          if (res.isSuccess && res.data) {
            this.reportData = res.data.items || [];
            this.flattenedData = this.flattenReportData(this.reportData);
            this.totalCount = res.data.totalCount || 0;
            this.totalPages = res.data.totalPages || 0;
          }
        } catch (err) {
          console.error('Error parsing report data:', err);
        } finally {
          this.loading = false;
        }
      },
      error: (err: any) => {
        this.toastService.showError('Error', 'Failed to load combined report');
        this.loading = false;
      }
    });
  }

  handlePageChange(event: any) {
    this.pageNumber = event.currentPage;
    this.pageSize = event.pageSize;
    this.loadReport();
  }

  handleSearchChange(term: string) {
    this.searchTerm = term;
    this.pageNumber = 1;
    this.loadReport();
  }

  handleFilterChange(filters: any) {
    this.currentFilters = filters;
    this.pageNumber = 1;
    this.loadReport();
  }

  handleSortChange(event: any) {
    this.sortBy = event.column;
    this.isDescending = event.direction === 'desc';
    this.loadReport();
  }

  handleSelection(selectedItems: any[]) {
    // Selection logic if needed
  }

  private flattenReportData(mandals: any[]): any[] {
    const flattened: any[] = [];
    mandals.forEach(mandal => {
      if (mandal.sectors && mandal.sectors.length > 0) {
        mandal.sectors.forEach((sector: any) => {
          flattened.push({
            mandalName: mandal.name,
            sectorName: sector.sectorName,
            inchargeName: sector.inchargeName,
            inchargeFatherName: sector.fatherName,
            phoneNumber: sector.phoneNumber,
            boothNumber: sector.booth?.boothNumber || 'N/A',
            pollingStationName: sector.booth?.pollingStationName || 'N/A',
            sanyojakProfile: sector.booth?.sanyojak?.profilePath,
            sanyojakName: sector.booth?.sanyojak?.name || 'N/A',
            sanyojakFatherName: sector.booth?.sanyojak?.fatherName || 'N/A',
            sanyojakPhone: sector.booth?.sanyojak?.phone || 'N/A',
            villageNames: this.getVillageNames(sector.booth?.villages)
          });
        });
      } else {
        flattened.push({
          mandalName: mandal.name,
          sectorName: 'N/A',
          inchargeName: 'N/A',
          phoneNumber: 'N/A',
          boothNumber: 'N/A',
          pollingStationName: 'N/A',
          sanyojakName: 'N/A',
          sanyojakPhone: 'N/A',
          villageNames: 'N/A'
        });
      }
    });
    return flattened;
  }

  handleExport(format: string) {
    if (!format) return;
    this.exporting = true;
    const request = format === 'excel'
      ? this.mandalService.exportCombinedReportExcel()
      : this.mandalService.exportCombinedReportPdf();

    request.subscribe({
      next: (blob: Blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `Combined_Report.${format === 'excel' ? 'xlsx' : 'pdf'}`;
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
        document.body.removeChild(a);
        this.exporting = false;
        this.toastService.showSuccess('Success', `Combined report exported to ${format.toUpperCase()} successfully!`);
      },
      error: (err) => {
        console.error(`Error exporting to ${format}:`, err);
        this.toastService.showError('Error', `Failed to export combined report to ${format.toUpperCase()}`);
        this.exporting = false;
      }
    });
  }

  getVillageNames(villages: any[]): string {
    if (!villages || villages.length === 0) return 'No villages';
    return villages.map(v => v.name).join(', ');
  }
}
