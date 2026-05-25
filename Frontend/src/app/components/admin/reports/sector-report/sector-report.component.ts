import { Component, OnInit } from '@angular/core';
import { CommonModule, Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { SectorService } from '../../../../Services/Admin/sector/sector.service';
import { PageHeaderComponent } from '../../../shared/page-header/page-header.component';
import { LucideAngularModule } from 'lucide-angular';
import { ToastService } from '../../../../Services/common/toast/toast.service';
import { GenericTableComponent } from '../../../shared/generic-table/generic-table.component';
import { TableColumn, TableConfig } from '../../../shared/generic-table/generic-table.types';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../../environments/environment';
import { GenericExportComponent } from '../../../shared/generic-export/generic-export.component';
import { MandalService } from '../../../../Services/Admin/mandal/mandal.service';

@Component({
  selector: 'app-sector-report',
  standalone: true,
  imports: [CommonModule, PageHeaderComponent, LucideAngularModule, GenericTableComponent, GenericExportComponent],
  templateUrl: './sector-report.component.html',
  styleUrl: './sector-report.component.css'
})
export class SectorReportComponent implements OnInit {
  reportData: any[] = [];
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
    // { key: 'mandalName', label: 'Mandal', sortable: true },
    { key: 'sectorName', label: 'Sector Name', sortable: true },
    { key: 'sectorSanyojak', label: 'Sector Sanyojak', sortable: true },
    { key: 'mobile', label: 'Mobile', sortable: true },
    { key: 'villageNames', label: 'Villages', sortable: false },
    { key: 'cast', label: 'Caste', sortable: true },
    { key: 'totalBooth', label: 'Total Booths', sortable: true },
    { key: 'totaVotes', label: 'Total Votes', sortable: true },
    { key: 'seniorCitizen', label: 'Senior Citizen', sortable: true },
    { key: 'handicap', label: 'Handicap', sortable: true },
    { key: 'doubleVoter', label: 'Double Voter', sortable: true },
    { key: 'pravasiVoter', label: 'Pravasi Voter', sortable: true }
  ];

  config: TableConfig = {
    searchable: true,
    searchPlaceholder: 'Search sector report...',
    paginated: true,
    defaultPageSize: 50,
    showRowNumbers: true,
    striped: true,
    hoverable: true,
    serverSide: true,
    compact: true,
    filterable: true,
    filters: [
      { key: 'MandalIds', label: 'Mandal', type: 'select', options: [], placeholder: '-- Select Mandal --', multiple: true },
      { key: 'VillageIds', label: 'Village', type: 'select', options: [], placeholder: '-- Select Village --', multiple: true },
      { key: 'CastIds', label: 'Caste', type: 'select', options: [], placeholder: '-- Select Caste --', multiple: true }
    ]
  };

  private currentFilters: any = {};

  constructor(
    private sectorService: SectorService,
    private mandalService: MandalService,
    private toastService: ToastService,
    private http: HttpClient,
    private route: ActivatedRoute,
    private location: Location
  ) { }

  goBack(): void {
    this.location.back();
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      if (params['mandalId']) {
        this.currentFilters['MandalIds'] = [params['mandalId']];
        const filter = this.config.filters?.find(f => f.key === 'MandalIds');
        if (filter) {
          filter.value = [params['mandalId']];
        }
      }
      this.loadReport();
    });
    this.loadFilterOptions();
  }

  loadFilterOptions(): void {
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

    // Load Mandals
    this.mandalService.getAllMandals({ pageSize: 1000 }).subscribe((res: any) => {
      const filter = this.config.filters?.find(f => f.key === 'MandalIds');
      if (filter) {
        const list = Array.isArray(res?.data?.items) ? res.data.items : (Array.isArray(res?.items) ? res.items : (Array.isArray(res?.data) ? res.data : (Array.isArray(res) ? res : [])));
        filter.options = list.map((m: any) => ({
          label: m.name || m.mandalName,
          value: String(m.id || m.mandalId)
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

    this.sectorService.getAllAdminSectorReports(params).subscribe({
      next: (res: any) => {
        try {
          if (res && (res.isSuccess || res.status === 200) && res.data) {
            this.reportData = (res.data.items || []).map((item: any) => ({
              ...item,
              villageNames: this.getVillageNames(item.villages)
            }));
            this.totalCount = res.data.totalCount || 0;
            this.totalPages = res.data.totalPages || 0;
          }
        } catch (err) {
          console.error('Error parsing sector report data:', err);
        } finally {
          this.loading = false;
        }
      },
      error: (err: any) => {
        this.toastService.showError('Error', 'Failed to load sector report');
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

  handleExport(format: string) {
    if (!format) return;
    this.exporting = true;

    this.sectorService.exportAdminSectorReport(format as 'excel' | 'pdf', this.currentFilters).subscribe({
      next: (blob: Blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `Sector_Report.${format === 'excel' ? 'xlsx' : 'pdf'}`;
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
        document.body.removeChild(a);
        this.exporting = false;
        this.toastService.showSuccess('Success', `Sector report exported to ${format.toUpperCase()} successfully!`);
      },
      error: (err) => {
        console.error(`Error exporting to ${format}:`, err);
        this.toastService.showError('Error', `Failed to export Sector report to ${format.toUpperCase()}`);
        this.exporting = false;
      }
    });
  }

  getVillageNames(villages: any[]): string {
    if (!villages || villages.length === 0) return 'No villages';
    return villages
      .filter(v => v.name)
      .map(v => v.name)
      .join(', ') || 'No villages';
  }
}
