import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Validators } from '@angular/forms';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';
import { TableAction, TableColumn, TableConfig } from '../../shared/generic-table/generic-table.types';
import { GenericModalButtonComponent } from '../../shared/generic-modal-form/generic-modal-button.component';
import { FormConfig, FormResult } from '../../shared/generic-modal-form/generic-form.types';

import { PageHeaderComponent } from '../../shared/page-header/page-header.component';
import { MandalService } from '../../../Services/Admin/mandal/mandal.service';
import { ToastService } from '../../../Services/common/toast/toast.service';
import { CrudHandlerService } from '../../../Services/common/crud-handler.service';


@Component({
  selector: 'app-mandal',
  standalone: true,
  imports: [CommonModule, PageHeaderComponent, GenericTableComponent, GenericModalButtonComponent],
  templateUrl: './mandal.component.html',
  styleUrl: './mandal.component.css'
})
export class MandalComponent implements OnInit {
  @ViewChild('mandalModal') mandalModal!: GenericModalButtonComponent;

  constructor(
    private mandalService: MandalService,
    private toastService: ToastService,
    private crudHandler: CrudHandlerService
  ) { }

  ngOnInit() {
    this.loadMandals();
  }

  loadMandals() {
    this.mandalService.getAllMandals().subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.mandalList = response.data;
        } else if (Array.isArray(response)) {
          this.mandalList = response;
        } else if (response && Array.isArray(response.data)) {
          this.mandalList = response.data;
        } else {
          console.warn('Unexpected response format:', response);
          this.mandalList = [];
        }
      },
      error: (err) => {
        console.error('Error fetching mandals:', err);
      }
    });
  }

  addMandalConfig: FormConfig = {
    title: 'Add New Mandal',
    submitLabel: 'Create Mandal',
    fields: [
      {
        id: 'name',
        name: 'name',
        label: 'Mandal Name',
        type: 'text',
        placeholder: 'Enter mandal name',
        validations: [Validators.required],
        gridColSpan: 12
      }
    ]
  };

  mandalList: any[] = []; // Will be populated from API

  columns: TableColumn[] = [
    { key: 'name', label: 'Name', type: 'avatar', sortable: true, avatarFallbackKey: 'name' },

  ];

  config: TableConfig = {
    selectable: false,
    filterable: true,
    paginated: true,
    defaultPageSize: 10,
    pageSizeOptions: [10, 20, 50],
    searchable: true,
    searchPlaceholder: 'Search members...',
    showRowNumbers: true,
    striped: true,
    hoverable: true
  };

  actions: TableAction[] = [
    { id: 'edit', label: '', variant: 'default', icon: '✏️' },
    { id: 'delete', label: '', variant: 'danger', icon: '🗑️' }
  ];




  handleAction(event: any) {
    const { action, row } = event;
    console.log(`Action [${action.id}] triggered for:`, row);

    if (action.id === 'delete') {
      this.crudHandler.handleRequest(
        this.mandalService.deleteMandal(row.id),
        'Deleted',
        'Mandal deleted successfully!',
        () => this.loadMandals()
      );
    } else if (action.id === 'edit') {
      this.mandalModal.openModal(row);
    } else {
      this.toastService.showWarning('Action Selected', `Action ${action.id} clicked for ${row.name || 'this item'}`);
    }
  }

  handleFormSubmit(result: FormResult) {
    if (!result.status) return;

    const isUpdate = result.data.id || (this.mandalModal.initialData && this.mandalModal.initialData.id);
    if (isUpdate && !result.data.id) {
      result.data.id = this.mandalModal.initialData.id;
    }

    const request = isUpdate
      ? this.mandalService.updateMandal(result.data)
      : this.mandalService.createMandal(result.data);

    this.crudHandler.handleRequest(
      request,
      isUpdate ? 'Updated' : 'Success',
      `Mandal ${isUpdate ? 'updated' : 'created'} successfully!`,
      () => this.loadMandals()
    );
  }
  handleSelection(selected: any[]) {
    console.log('Selected rows:', selected);
  }

  handleExport(format: string) {
    if (!format) return;
    console.log(`Generating ${format.toUpperCase()} export...`);
    this.toastService.showSuccess('Export Started', `Successfully generated ${format.toUpperCase()} export!`);
  }
}
