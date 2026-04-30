import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MandalService } from '../../../../Services/Admin/mandal/mandal.service';
import { PageHeaderComponent } from '../../../shared/page-header/page-header.component';
import { LucideAngularModule } from 'lucide-angular';
import { ToastService } from '../../../../Services/common/toast/toast.service';

@Component({
  selector: 'app-mandal-report',
  standalone: true,
  imports: [CommonModule, PageHeaderComponent, LucideAngularModule],
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
  pageSize = 10;
  totalPages = 0;

  constructor(
    private mandalService: MandalService,
    private toastService: ToastService
  ) { }

  ngOnInit(): void {
    this.loadReport();
  }

  loadReport(): void {
    this.loading = true;
    const params = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize
    };

    this.mandalService.getAllCombinedReports(params).subscribe({
      next: (res: any) => {
        try {
          if (res.isSuccess && res.data) {
            this.reportData = res.data.items || [];
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

  handleSortChange(event: any) {
    // Add sorting logic if needed
    this.loadReport();
  }

  handleSearchChange(term: string) {
    // Add search logic if needed
    this.loadReport();
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
        a.download = `Mandal_Combined_Report.${format === 'excel' ? 'xlsx' : 'pdf'}`;
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
        document.body.removeChild(a);
        this.exporting = false;
        this.toastService.showSuccess('Success', `Mandal report exported to ${format.toUpperCase()} successfully!`);
      },
      error: (err) => {
        console.error(`Error exporting to ${format}:`, err);
        this.toastService.showError('Error', `Failed to export Mandal report to ${format.toUpperCase()}`);
        this.exporting = false;
      }
    });
  }

  getVillageNames(villages: any[]): string {
    if (!villages || villages.length === 0) return 'No villages';
    return villages.map(v => v.name).join(', ');
  }
}
