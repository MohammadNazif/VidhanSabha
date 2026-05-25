import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MandalService } from '../../../../Services/Admin/mandal/mandal.service';
import { PageHeaderComponent } from '../../../shared/page-header/page-header.component';
import { Router } from '@angular/router';
import { LucideAngularModule } from 'lucide-angular';
import { ToastService } from '../../../../Services/common/toast/toast.service';
import { GenericTableComponent } from '../../../shared/generic-table/generic-table.component';
import { TableColumn, TableConfig } from '../../../shared/generic-table/generic-table.types';
import { HttpClient } from '@angular/common/http';
import { GenericExportComponent } from '../../../shared/generic-export/generic-export.component';

@Component({
  selector: 'app-mandal-report',
  standalone: true,
  imports: [CommonModule, PageHeaderComponent, LucideAngularModule, GenericTableComponent, GenericExportComponent],
  templateUrl: './mandal-report.component.html',
  styleUrl: './mandal-report.component.css'
})
export class MandalReportComponent implements OnInit {
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
    { key: 'mandalName', label: 'Mandal Name', sortable: true },
    { key: 'totalSectors', label: 'Total Sectors', sortable: true, align: 'center' },
    { key: 'totalBooths', label: 'Total Booths', sortable: true, align: 'center' },
    { key: 'totalVotes', label: 'Total Votes', sortable: true },
    { key: 'seniorCitizen', label: 'Senior Citizen', sortable: true },
    { key: 'handicap', label: 'Handicap', sortable: true },
    { key: 'doubleVotes', label: 'Double Votes', sortable: true },
    { key: 'pravasi', label: 'Pravasi', sortable: true },
    { key: 'effectivePerson', label: 'Effective Person', sortable: true }
  ];

  config: TableConfig = {
    searchable: true,
    searchPlaceholder: 'Search mandal report...',
    paginated: true,
    defaultPageSize: 50,
    showRowNumbers: true,
    striped: true,
    hoverable: true,
    serverSide: true,
    compact: true,
    filterable: true,
    filters: [
      { key: 'MandalIds', label: 'Mandal', type: 'select', options: [], placeholder: '-- Select Mandal --', multiple: true }
    ]
  };

  private currentFilters: any = {};

  constructor(
    private mandalService: MandalService,
    private toastService: ToastService,
    private http: HttpClient,
    private router: Router
  ) { }

  navigateToBooths(row: any) {
    if (row.mandalId) {
      this.router.navigate(['/booth-report'], { queryParams: { mandalId: row.mandalId } });
    }
  }

  navigateToSectors(row: any) {
    if (row.mandalId) {
      this.router.navigate(['/sector-report'], { queryParams: { mandalId: row.mandalId } });
    }
  }

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
          mandalFilter.options = mandals.map((m: any) => ({ 
            label: m.name || m.mandalName, 
            value: String(m.id || m.mandalId) 
          }));
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

    this.mandalService.getAllMandalReport(params).subscribe({
      next: (res: any) => {
        try {
          if (res && (res.isSuccess || res.status === 200) && res.data) {
            this.reportData = res.data.items || [];
            this.totalCount = res.data.totalCount || 0;
            this.totalPages = res.data.totalPages || 0;
          }
        } catch (err) {
          console.error('Error parsing mandal report data:', err);
        } finally {
          this.loading = false;
        }
      },
      error: (err: any) => {
        this.toastService.showError('Error', 'Failed to load mandal report');
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
    
    this.mandalService.exportMandalReport(format as 'excel' | 'pdf', this.currentFilters).subscribe({
      next: (blob: Blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `Mandal_Report.${format === 'excel' ? 'xlsx' : 'pdf'}`;
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
        document.body.removeChild(a);
        this.exporting = false;
        this.toastService.showSuccess('Success', `Mandal report exported to ${format.toUpperCase()} successfully!`);
      },
      error: (err: any) => {
        console.error(`Error exporting to ${format}:`, err);
        this.toastService.showError('Error', `Failed to export Mandal report to ${format.toUpperCase()}`);
        this.exporting = false;
      }
    });
  }
}
