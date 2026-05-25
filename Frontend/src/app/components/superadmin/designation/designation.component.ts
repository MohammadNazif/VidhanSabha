import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Validators } from '@angular/forms';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';
import { TableAction, TableColumn, TableConfig } from '../../shared/generic-table/generic-table.types';
import { GenericModalButtonComponent } from '../../shared/generic-modal-form/generic-modal-button.component';
import { FormConfig, FormResult } from '../../shared/generic-modal-form/generic-form.types';

import { PageHeaderComponent } from '../../shared/page-header/page-header.component';
import { DesignationService } from '../../../Services/Admin/designation/designation.service';
import { AuthServiceService } from '../../../Services/Auth/auth.service';
import { ToastService } from '../../../Services/common/toast/toast.service';
import { CrudHandlerService } from '../../../Services/common/crud-handler.service';
import { GenericExportComponent } from '../../shared/generic-export/generic-export.component';

@Component({
  selector: 'app-designation',
  standalone: true,
  imports: [CommonModule, PageHeaderComponent, GenericTableComponent, GenericModalButtonComponent, GenericExportComponent],
  templateUrl: './designation.component.html',
  styleUrl: './designation.component.css'
})
export class DesignationComponent implements OnInit {
  @ViewChild('designationModal') designationModal!: GenericModalButtonComponent;
  designationList: any[] = [];
  isExporting = false;
  searchTerm = '';
  pageNumber = 1;
  pageSize = 10;
  sortBy = '';
  isDescending = false;
  totalCount = 0;
  loading = false;

  columns: TableColumn[] = [
    { key: 'designationName', label: 'Designation Name' },
  ];

  config: TableConfig = {
    selectable: false,
    paginated: true,
    defaultPageSize: 10,
    pageSizeOptions: [10, 20, 50],
    searchable: true,
    searchPlaceholder: 'Search designations...',
    showRowNumbers: true,
    striped: true,
    hoverable: true
  };

  actions: TableAction[] = [
    { id: 'edit', label: '', variant: 'default', icon: 'edit' },
    { id: 'delete', label: '', variant: 'danger', icon: 'delete' }
  ];

  addDesignationConfig: FormConfig = {
    title: 'Add New Designation',
    submitLabel: 'Create Designation',
    fields: [
      {
        id: 'designationName',
        name: 'designationName',
        label: 'Designation Name',
        type: 'text',
        placeholder: 'Enter designation name',
        validations: [Validators.required],
        gridColSpan: 12
      }
    ]
  };

  constructor(
    private designationService: DesignationService,
    private authService: AuthServiceService,
    private toastService: ToastService,
    private crudHandler: CrudHandlerService
  ) { }

  ngOnInit() {
    this.loadDesignations();
  }

  loadDesignations() {
    this.loading = true;
    const userId = this.authService.getUserId();
    const params = {
      userId: userId,
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      searchTerm: this.searchTerm,
      sortBy: this.sortBy,
      isDescending: this.isDescending
    };

    this.designationService.getAllDesignations(params).subscribe({
      next: (response) => {
        const dataWrap = response.data;
        if (dataWrap && dataWrap.items) {
          this.designationList = dataWrap.items;
          this.totalCount = dataWrap.totalCount || 0;
        } else {
          this.designationList = Array.isArray(dataWrap) ? dataWrap : (Array.isArray(response) ? response : []);
          this.totalCount = this.designationList.length;
        }
        this.loading = false;
      },
      error: (err) => {
        console.error('Error fetching designations:', err);
        this.loading = false;
      }
    });
  }

  handlePageChange(event: any) {
    this.pageNumber = event.currentPage;
    this.pageSize = event.pageSize;
    this.loadDesignations();
  }

  handleSortChange(event: any) {
    this.sortBy = event.column;
    this.isDescending = event.direction === 'desc';
    this.pageNumber = 1;
    this.loadDesignations();
  }

  handleSearchChange(term: string) {
    this.searchTerm = term;
    this.pageNumber = 1;
    this.loadDesignations();
  }

  handleAction(event: any) {
    const { action, row } = event;
    if (action.id === 'delete') {
      this.crudHandler.handleRequest(
        this.designationService.deleteDesignation(row.id),
        'Deleted',
        'Designation deleted successfully!',
        () => this.loadDesignations()
      );
    } else if (action.id === 'edit') {
      this.designationModal.openModal(row);
    }
  }

  handleFormSubmit(result: FormResult) {
    if (!result.status) return;
    const userId = this.authService.getUserId();
    if (userId) {
      result.data.userId = String(userId);
    }
    const isUpdate = result.data.id || (this.designationModal.initialData && this.designationModal.initialData.id);
    if (isUpdate && !result.data.id) {
      result.data.id = this.designationModal.initialData.id;
    }

    const request = isUpdate
      ? this.designationService.updateDesignation(result.data)
      : this.designationService.createDesignation(result.data);

    this.crudHandler.handleRequest(
      request,
      isUpdate ? 'Updated' : 'Success',
      `Designation ${isUpdate ? 'updated' : 'created'} successfully!`,
      () => this.loadDesignations()
    );
  }

  handleExport(format: string) {
    if (!format || this.isExporting) return;
    this.isExporting = true;
    
    const params = {
      searchTerm: this.searchTerm
    };

    this.designationService.export('designation', format as 'excel' | 'pdf', params).subscribe({
      next: (blob: Blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `Designations_List.${format === 'excel' ? 'xlsx' : 'pdf'}`;
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
        document.body.removeChild(a);
        this.isExporting = false;
        this.toastService.showSuccess('Success', `Designation list exported to ${format.toUpperCase()} successfully!`);
      },
      error: (err) => {
        console.error(`Error exporting to ${format}:`, err);
        this.toastService.showError('Error', `Failed to export designation list to ${format.toUpperCase()}`);
        this.isExporting = false;
      }
    });
  }
}
