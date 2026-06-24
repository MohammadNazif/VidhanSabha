import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, Validators } from '@angular/forms';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';
import { TableConfig, TableColumn, TableAction } from '../../shared/generic-table/generic-table.types';
import { FormConfig, FormResult } from '../../shared/generic-modal-form/generic-form.types';
import { PradhanService } from '../../../Services/Admin/pradhan/pradhan.service';
import { CrudHandlerService } from '../../../Services/common/crud-handler.service';
import { ToastService } from '../../../Services/common/toast/toast.service';
import { GenericModalButtonComponent } from '../../shared/generic-modal-form/generic-modal-button.component';
import { PageHeaderComponent } from '../../shared/page-header/page-header.component';
import { ActivatedRoute } from '@angular/router';
import { AuthServiceService } from '../../../Services/Auth/auth.service';

import { GenericExportComponent } from '../../shared/generic-export/generic-export.component';

@Component({
  selector: 'app-pradhan',
  standalone: true,
  imports: [CommonModule, FormsModule, GenericTableComponent, GenericModalButtonComponent, PageHeaderComponent, GenericExportComponent],
  template: `
    <div class="h-full flex flex-col p-4 gap-4 overflow-hidden">
      <app-page-header [title]="isListView ? 'Pradhan List' : 'Pradhan Management'" subtitle="Manage village pradhans and their assignments">
        <app-generic-modal-button 
            *ngIf="canManage()"
            #pradhanModal 
            [config]="addPradhanConfig" 
            label="Add Pradhan" 
            icon="+"
            variant="primary" 
            (formSubmit)="handleFormSubmit($event)">
        </app-generic-modal-button>

        <app-generic-export [show]="isListView" [options]="exportOptions"
            [isExporting]="isExporting" (export)="handleExport($event)">
        </app-generic-export>

        <input type="file" id="importFileInput" class="hidden" accept=".xlsx, .xls" (change)="onFileImport($event)">
      </app-page-header>

      <div class="flex-1 min-h-0 bg-white rounded-xl shadow-sm border border-slate-200 overflow-hidden flex flex-col p-2">
        <app-generic-table
          [config]="tableConfig"
          [columns]="columns"
          [actions]="actions"
          [data]="pradhans"
          [loading]="loading"
          [totalItems]="totalItems"
          (actionClick)="handleAction($event)"
          (searchChange)="handleSearchChange($event)"
          (pageChange)="handlePageChange($event)"
          (sortChange)="handleSortChange($event)">
        </app-generic-table>
      </div>
    </div>
  `
})
export class PradhanComponent implements OnInit {
  @ViewChild('pradhanModal') pradhanModal!: GenericModalButtonComponent;

  pradhans: any[] = [];
  loading = false;
  totalItems = 0;
  searchTerm = '';
  isListView = false;
  isExporting = false;
  exportOptions = [
    { label: 'Export PDF', value: 'pdf' },
    { label: 'Export Excel', value: 'excel' },
    { label: 'Import Excel', value: 'import' }
  ];

  canManage(): boolean {
    if (this.isListView) return false;
    const role = (this.authService.getRole() || '').toUpperCase().trim();
    return ['SUPERADMIN', 'ADMIN', 'VIDHANSABHAPRABHARI'].includes(role);
  }

  columns: TableColumn[] = [
    { key: 'name', label: 'Name', sortable: true },
    { key: 'contact', label: 'Contact' },
    { key: 'genderValue', label: 'Gender' },
    {
      key: 'villageNames', label: 'Assigned Villages', formatter: (val: any, row: any) => {
        if (row.villages && Array.isArray(row.villages)) {
          return row.villages.map((v: any) => v.villageName || v.name || 'na').join(', ');
        }
        return val || 'N/A';
      }
    }
  ];

  actions: TableAction[] = [
    { id: 'edit', label: '', icon: 'edit', variant: 'primary', show: () => this.canManage() },
    { id: 'delete', label: '', icon: 'delete', variant: 'danger', show: () => this.canManage() }
  ];

  tableConfig: TableConfig = {
    selectable: false,
    paginated: true,
    searchable: true,
    serverSide: true,
    defaultPageSize: 10
  };

  addPradhanConfig: FormConfig = {
    title: 'Add Pradhan',
    submitLabel: 'Save Pradhan',
    fields: [
      {
        id: 'name',
        name: 'name',
        label: 'Name',
        type: 'text',
        placeholder: 'Enter pradhan name',
        gridColSpan: 6,
        validations: [Validators.required]
      },
      {
        id: 'gender',
        name: 'gender',
        label: 'Gender',
        type: 'select',
        options: [
          { value: '1', label: 'Male' },
          { value: '2', label: 'Female' },
          { value: '3', label: 'Other' }
        ],
        gridColSpan: 6,
        validations: [Validators.required]
      },
      {
        id: 'contact',
        name: 'contact',
        label: 'Contact Number',
        type: 'text',
        placeholder: 'Enter 10-digit number',
        gridColSpan: 6,
        validations: [Validators.required, Validators.pattern('^[0-9]{10}$')]
      },

      {
        id: 'villageId',
        name: 'villageId',
        label: 'Assigned Villages',
        type: 'select',
        gridColSpan: 6,
        multiple: true,
        validations: [Validators.required],
        apiUrl: 'common/getallvillages',
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data?.items) ? data.data.items : (Array.isArray(data?.items) ? data.items : (Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : [])));
          return list.map((item: any) => ({
            value: String(item.id || item.villageId),
            label: item.name || item.villageName
          }));
        }
      }
    ]
  };

  constructor(
    private pradhanService: PradhanService,
    private toastService: ToastService,
    private crudHandler: CrudHandlerService,
    private route: ActivatedRoute,
    private authService: AuthServiceService
  ) { }

  ngOnInit() {
    this.route.url.subscribe(url => {
      const path = url[0]?.path || '';
      this.isListView = path.includes('-list');
      this.loadPradhans();
    });
  }

  pageNumber = 1;
  pageSize = 50;
  sortBy = '';
  isDescending = false;

  loadPradhans() {
    this.loading = true;
    const params: any = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      searchTerm: this.searchTerm,
      sortBy: this.sortBy,
      isDescending: this.isDescending
    };

    this.pradhanService.getAllPradhans(params).subscribe({
      next: (res: any) => {
        this.pradhans = res.data?.items || res.data || [];
        this.totalItems = res.data?.totalCount || this.pradhans.length;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.toastService.showError('Error', 'Failed to load pradhans');
      }
    });
  }

  handlePageChange(event: any) {
    this.pageNumber = event.currentPage;
    this.pageSize = event.pageSize;
    this.loadPradhans();
  }

  handleSortChange(event: any) {
    this.sortBy = event.column;
    this.isDescending = event.direction === 'desc';
    this.pageNumber = 1;
    this.loadPradhans();
  }

  handleAction(event: { action: TableAction; row: any; index: number }) {
    const { action, row } = event;
    if (action.id === 'delete') {
      this.crudHandler.handleRequest(
        this.pradhanService.deletePradhan(row.id),
        'Deleted',
        'Pradhan deleted successfully!',
        () => this.loadPradhans()
      );
    } else if (action.id === 'edit') {
      const editData = { ...row };

      // Standardize select values as strings
      if (editData.designationId) editData.designationId = String(editData.designationId);
      if (editData.gender) editData.gender = String(editData.gender);

      // Transform villages for multi-select binding (expects array of string IDs)
      if (row.villages && Array.isArray(row.villages)) {
        editData.villageId = row.villages.map((v: any) => String(v.villageId || v.id));
      }

      this.pradhanModal.openModal(editData);
    }
  }

  handleSearchChange(term: string) {
    this.searchTerm = term;
    this.loadPradhans();
  }

  handleFormSubmit(result: FormResult) {
    if (!result.status) return;

    const raw = result.data;
    const isUpdate = !!(raw.id || (this.pradhanModal.initialData && this.pradhanModal.initialData.id));

    const submitData: any = {
      name: raw.name,
      designationId: 1,
      contact: String(raw.contact),
      gender: Number(raw.gender),
      villageId: Array.isArray(raw.villageId)
        ? raw.villageId.map((v: any) => Number(v.id || v))
        : [Number(raw.villageId)]
    };

    if (isUpdate) {
      submitData.id = Number(raw.id || this.pradhanModal.initialData.id);
    }

    const request = isUpdate
      ? this.pradhanService.updatePradhan(submitData)
      : this.pradhanService.createPradhan(submitData);

    this.crudHandler.handleRequest(
      request,
      isUpdate ? 'Updated' : 'Created',
      `Pradhan ${isUpdate ? 'updated' : 'created'} successfully!`,
      () => this.loadPradhans()
    );
  }

  handleExport(format: string) {
    if (!format) return;
    if (format === 'import') {
      const fileInput = document.getElementById('importFileInput') as HTMLInputElement;
      if (fileInput) fileInput.click();
      return;
    }

    this.isExporting = true;
    const request = format === 'excel' ? this.pradhanService.exportToExcel() : this.pradhanService.exportToPdf();

    request.subscribe({
      next: (blob: Blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `Pradhan_List.${format === 'excel' ? 'xlsx' : 'pdf'}`;
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
        document.body.removeChild(a);
        this.isExporting = false;
        this.toastService.showSuccess('Success', `List exported to ${format.toUpperCase()} successfully!`);
      },
      error: (err: any) => {
        console.error(`Error exporting to ${format}:`, err);
        this.toastService.showError('Error', `Failed to export list to ${format.toUpperCase()}`);
        this.isExporting = false;
      }
    });
  }

  onFileImport(event: any) {
    const file = event.target.files[0];
    if (!file) return;

    this.loading = true;
    this.pradhanService.importExcel(file).subscribe({
      next: (res: any) => {
        this.toastService.showSuccess('Success', 'Pradhan data imported successfully!');
        this.loadPradhans();
        event.target.value = '';
      },
      error: (err: any) => {
        console.error('Import error:', err);
        this.toastService.showError('Error', 'Failed to import pradhan data');
        this.loading = false;
        event.target.value = '';
      }
    });
  }
}
