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

@Component({
  selector: 'app-designation',
  standalone: true,
  imports: [CommonModule, PageHeaderComponent, GenericTableComponent, GenericModalButtonComponent],
  templateUrl: './designation.component.html',
  styleUrl: './designation.component.css'
})
export class DesignationComponent implements OnInit {
  @ViewChild('designationModal') designationModal!: GenericModalButtonComponent;
  designationList: any[] = [];

  columns: TableColumn[] = [
    { key: 'designationName', label: 'Designation Name', type: 'avatar', sortable: true, avatarFallbackKey: 'name' },
  ];

  config: TableConfig = {
    selectable: false,
    filterable: true,
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
    const userId = this.authService.getUserId();
    this.designationService.getAllDesignations({ userId: userId }).subscribe({
      next: (response) => {
        if (response && response.isSuccess) {
          this.designationList = response.data;
        } else if (Array.isArray(response)) {
          this.designationList = response;
        } else if (response && Array.isArray(response.data)) {
          this.designationList = response.data;
        } else {
          this.designationList = [];
        }
      },
      error: (err) => {
        console.error('Error fetching designations:', err);
      }
    });
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
    } else {
      this.toastService.showWarning('Action Selected', `Action ${action.id} clicked for ${row.name || 'this item'}`);
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
    if (!format) return;
    this.toastService.showSuccess('Export Started', `Successfully generated ${format.toUpperCase()} export!`);
  }
}
