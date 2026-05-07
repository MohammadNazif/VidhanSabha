import { Component, OnInit } from '@angular/core';
import { CommonModule, Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { BoothService } from '../../../../Services/Admin/booth/booth.service';
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
  selector: 'app-booth-report',
  standalone: true,
  imports: [CommonModule, PageHeaderComponent, LucideAngularModule, GenericTableComponent, GenericExportComponent],
  templateUrl: './booth-report.component.html',
  styleUrl: './booth-report.component.css'
})
export class BoothReportComponent implements OnInit {
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
    { key: 'boothNo', label: 'Booth No.', sortable: true },
    { key: 'pollingStation', label: 'Polling Station', sortable: true },
    { key: 'boothAdhyaksh', label: 'Booth Adhyaksh', sortable: true },
    { key: 'mobile', label: 'Mobile', sortable: true },
    { key: 'villageNames', label: 'Villages', sortable: false },
    { key: 'cast', label: 'Caste', sortable: true },
    { key: 'totalVotes', label: 'Total Votes', sortable: true },
    { key: 'seniorCitizen', label: 'Senior Citizen', sortable: true },
    { key: 'handicap', label: 'Handicap', sortable: true },
    { key: 'doubleVotes', label: 'Double Votes', sortable: true },
    { key: 'pravasi', label: 'Pravasi', sortable: true }
  ];

  config: TableConfig = {
    searchable: true,
    searchPlaceholder: 'Search booth report...',
    paginated: true,
    defaultPageSize: 50,
    showRowNumbers: true,
    striped: true,
    hoverable: true,
    serverSide: true,
    compact: true,
    filterable: true,
    filters: [

      { key: 'BoothIds', label: 'Booth', type: 'select', options: [], placeholder: '-- Select Booth --', multiple: true },
      { key: 'VillageIds', label: 'Village', type: 'select', options: [], placeholder: '-- Select Village --', multiple: true },
      { key: 'CastIds', label: 'Caste', type: 'select', options: [], placeholder: '-- Select Caste --', multiple: true }
    ]
  };

  private currentFilters: any = {};

  constructor(
    private boothService: BoothService,
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
        // Pre-fill the filter UI if possible (though GenericTable might need adjustment to show this)
        const filter = this.config.filters?.find(f => f.key === 'MandalIds');
        if (filter) {
          filter.value = [params['mandalId']];
        }
      }
      this.loadReport();
    });
    this.loadFilterOptions();
    this.loadBoothOptions();
  }

  loadBoothOptions(mandalIds: string[] = []): void {
    let boothUrl = `${environment.apiUrl}/common/boothNumber`;
    if (mandalIds && mandalIds.length > 0) {
      boothUrl = `${environment.apiUrl}/common/boothsByMandalId?mandalId=${mandalIds.join(',')}&pageSize=1000`;
    }

    this.http.get(boothUrl).subscribe((res: any) => {
      const filter = this.config.filters?.find(f => f.key === 'BoothIds');
      if (filter) {
        const list = Array.isArray(res?.data?.items) ? res.data.items : (Array.isArray(res?.items) ? res.items : (Array.isArray(res?.data) ? res.data : (Array.isArray(res) ? res : [])));
        filter.options = list.map((b: any) => ({
          label: ` ${b.boothName || b.boothName}`,
          value: String(b.id || b.boothId)
        }));
        // Update references for change detection
        if (this.config.filters) {
          this.config.filters = [...this.config.filters];
          this.config = { ...this.config };
        }
      }
    });
  }

  loadFilterOptions(boothIds: string[] = []): void {
    // Clear current village options immediately to provide visual feedback
    const villageFilter = this.config.filters?.find(f => f.key === 'VillageIds');
    if (villageFilter) {
      villageFilter.options = [];
      if (this.config.filters) {
        this.config.filters = [...this.config.filters];
        this.config = { ...this.config };
      }
    }

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

    // Load Villages
    let villageUrl = `${environment.apiUrl}/common/villagesByBoothId`;


    this.http.get(villageUrl).subscribe((res: any) => {
      const filter = this.config.filters?.find(f => f.key === 'VillageIds');
      if (filter) {
        const list = Array.isArray(res?.data?.items) ? res.data.items : (Array.isArray(res?.items) ? res.items : (Array.isArray(res?.data) ? res.data : (Array.isArray(res) ? res : [])));
        filter.options = list.map((v: any) => ({
          label: v.name || v.villageName,
          value: String(v.id || v.villageId)
        }));
        // Update references for change detection
        if (this.config.filters) {
          this.config.filters = [...this.config.filters];
          this.config = { ...this.config };
        }
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
        // Update references for change detection
        if (this.config.filters) {
          this.config.filters = [...this.config.filters];
          this.config = { ...this.config };
        }
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

    this.boothService.getAllBoothReports(params).subscribe({
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
          console.error('Error parsing booth report data:', err);
        } finally {
          this.loading = false;
        }
      },
      error: (err: any) => {
        this.toastService.showError('Error', 'Failed to load booth report');
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
    const previousMandals = this.currentFilters['MandalIds'] || [];
    const newMandals = filters['MandalIds'] || [];
    const previousBooths = this.currentFilters['BoothIds'] || [];
    const newBooths = filters['BoothIds'] || [];

    // If Mandal selection changed, reload booths and clear dependent selections
    if (JSON.stringify(previousMandals) !== JSON.stringify(newMandals)) {
      this.loadBoothOptions(newMandals);

      // Clear Booth and Village selections
      filters['BoothIds'] = [];
      const boothFilter = this.config.filters?.find(f => f.key === 'BoothIds');
      if (boothFilter) boothFilter.value = [];

      filters['VillageIds'] = [];
      const villageFilter = this.config.filters?.find(f => f.key === 'VillageIds');
      if (villageFilter) villageFilter.value = [];

      // Reload villages for all available booths in the new Mandals (optional, or just clear)
      this.loadFilterOptions([]);
    }
    // Else if Booth selection changed, reload villages
    else if (JSON.stringify(previousBooths) !== JSON.stringify(newBooths)) {
      this.loadFilterOptions(newBooths);

      // Clear Village selection
      filters['VillageIds'] = [];
      const villageFilter = this.config.filters?.find(f => f.key === 'VillageIds');
      if (villageFilter) villageFilter.value = [];
    }

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

    this.boothService.exportBoothReport(format as 'excel' | 'pdf', this.currentFilters).subscribe({
      next: (blob: Blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `Booth_Report.${format === 'excel' ? 'xlsx' : 'pdf'}`;
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
        document.body.removeChild(a);
        this.exporting = false;
        this.toastService.showSuccess('Success', `Booth report exported to ${format.toUpperCase()} successfully!`);
      },
      error: (err) => {
        console.error(`Error exporting to ${format}:`, err);
        this.toastService.showError('Error', `Failed to export Booth report to ${format.toUpperCase()}`);
        this.exporting = false;
      }
    });
  }

  getVillageNames(villages: any[]): string {
    if (!villages || villages.length === 0) return 'No villages';
    return villages.map(v => v.villageName).join(', ');
  }
}
