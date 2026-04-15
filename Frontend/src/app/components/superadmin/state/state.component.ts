import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Validators } from '@angular/forms';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';
import { TableAction, TableColumn, TableConfig } from '../../shared/generic-table/generic-table.types';
import { GenericModalButtonComponent } from '../../shared/generic-modal-form/generic-modal-button.component';
import { FormConfig, FormResult } from '../../shared/generic-modal-form/generic-form.types';

import { PageHeaderComponent } from '../../shared/page-header/page-header.component';
import { StateService } from '../../../Services/Admin/state/state.service';
import { ToastService } from '../../../Services/common/toast/toast.service';
import { CrudHandlerService } from '../../../Services/common/crud-handler.service';

@Component({
  selector: 'app-state',
  standalone: true,
  imports: [CommonModule, PageHeaderComponent, GenericTableComponent, GenericModalButtonComponent],
  templateUrl: './state.component.html',
  styleUrl: './state.component.css'
})
export class StateComponent implements OnInit {
  @ViewChild('stateModal') stateModal!: GenericModalButtonComponent;

  stateList: any[] = [];

  columns: TableColumn[] = [
    { key: 'stateName', label: 'State Name', type: 'avatar', sortable: true, avatarFallbackKey: 'stateName' },
    { key: 'vidhanSabhaCount', label: 'Vidhansabha Count', sortable: true },
    { key: 'remainingCount', label: 'Remaining Vidhansabha', sortable: true }
  ];

  config: TableConfig = {
    selectable: false,
    filterable: true,
    paginated: true,
    defaultPageSize: 10,
    pageSizeOptions: [10, 20, 50],
    searchable: true,
    searchPlaceholder: 'Search states...',
    showRowNumbers: true,
    striped: true,
    hoverable: true
  };

  actions: TableAction[] = [
    { id: 'add', label: 'Vidhansabha', variant: 'primary', icon: 'add' },
    { id: 'edit', label: '', variant: 'default', icon: 'edit' },
    { id: 'delete', label: '', variant: 'danger', icon: 'delete' }
  ];

  addStateConfig: FormConfig = {
    title: 'Add New State',
    submitLabel: 'Create State',
    fields: [
      {
        id: 'stateId',
        name: 'stateId',
        label: 'State Name',
        type: 'select',
        placeholder: 'Select state name',
        apiUrl: 'common/getstates',
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : []);
          return list.map((item: any) => ({
            value: String(item.id),
            label: item.stateName
          }));
        },
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'vidhanSabhaCount',
        name: 'vidhanSabhaCount',
        label: 'Vidhansabha Count',
        type: 'number',
        placeholder: 'Enter vidhansabha count',
        validations: [Validators.required],
        gridColSpan: 6
      }
    ]
  };

  constructor(
    private stateService: StateService,
    private toastService: ToastService,
    private crudHandler: CrudHandlerService
  ) { }

  ngOnInit() {
    this.loadStates();
  }

  loadStates() {
    this.stateService.getAllStates().subscribe({
      next: (response) => {
        if (response && response.isSuccess) {
          this.stateList = response.data;
        } else if (Array.isArray(response)) {
          this.stateList = response;
        } else if (response && Array.isArray(response.data)) {
          this.stateList = response.data;
        } else {
          this.stateList = [];
        }
      },
      error: (err) => {
        console.error('Error fetching states:', err);
      }
    });
  }

  handleAction(event: any) {
    const { action, row } = event;

    if (action.id === 'delete') {
      this.crudHandler.handleRequest(
        this.stateService.deleteState(row.id),
        'Deleted',
        'State deleted successfully!',
        () => this.loadStates()
      );
    } else if (action.id === 'edit') {
      this.stateModal.openModal(row);
    }
  }

  handleFormSubmit(result: FormResult) {
    if (!result.status) return;

    const isUpdate = result.data.id || (this.stateModal.initialData && this.stateModal.initialData.id);
    if (isUpdate && !result.data.id) {
      result.data.id = this.stateModal.initialData.id;
    }

    const request = isUpdate
      ? this.stateService.updateState(result.data)
      : this.stateService.createState(result.data);

    this.crudHandler.handleRequest(
      request,
      isUpdate ? 'Updated' : 'Success',
      `State ${isUpdate ? 'updated' : 'created'} successfully!`,
      () => this.loadStates()
    );
  }

  handleExport(format: string) {
    if (!format) return;
    this.toastService.showSuccess('Export Started', `Successfully generated ${format.toUpperCase()} export!`);
  }
}
