import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Validators } from '@angular/forms';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';
import { TableAction, TableColumn, TableConfig } from '../../shared/generic-table/generic-table.types';
import { GenericModalButtonComponent } from '../../shared/generic-modal-form/generic-modal-button.component';
import { FormConfig, FormResult } from '../../shared/generic-modal-form/generic-form.types';

import { PageHeaderComponent } from '../../shared/page-header/page-header.component';
import { MandalService } from '../../../Services/Admin/mandal/mandal.service';
import { StateService } from '../../../Services/Admin/state/state.service';
import { AuthServiceService } from '../../../Services/Auth/auth.service';
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
    private stateService: StateService,
    private authService: AuthServiceService,
    private toastService: ToastService,
    private crudHandler: CrudHandlerService
  ) { }

  isStatePrabhari(): boolean {
    return (this.authService.getRole() || '').toUpperCase().trim() === 'STATEPRABHARI';
  }

  defaultStateId: string | null = null;

  ngOnInit() {
    if (this.isStatePrabhari()) {
      // Fetch the assigned state ID
      this.stateService.getAllStates().subscribe({
        next: (response) => {
          const list = response?.data || response || [];
          if (list.length > 0) {
            this.defaultStateId = String(list[0].stateId || list[0].id);

            // Simplify fields for State Prabhari
            const districtField = this.addMandalConfig.fields.find(f => f.id === 'districtId');
            if (districtField) {
              delete districtField.dependsOn;
              districtField.apiUrl = () => `district/getAll?stateId=${this.defaultStateId}`;
            }
          }
        }
      });
    }
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
        id: 'districtId',
        name: 'districtId',
        label: 'Select District',
        type: 'select',
        placeholder: '--Select District--',
        apiUrl: () => `district/getAll?stateId=${this.defaultStateId || ''}`,
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : []);
          return list.map((item: any) => ({
            value: String(item.id),
            label: item.name
          }));
        },
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'vidhanId',
        name: 'vidhanId',
        label: 'Select Vidhan Sabha',
        type: 'select',
        placeholder: '--Select Vidhan Sabha--',
        dependsOn: 'districtId',
        apiUrl: (districtId: any) => `vidhansabha/getAll?districtId=${districtId}`,
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : []);
          return list.map((item: any) => ({
            value: String(item.id),
            label: item.name
          }));
        },
        validations: [Validators.required],
        gridColSpan: 6
      },
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
    { id: 'edit', label: '', variant: 'default', icon: 'edit' },
    { id: 'delete', label: '', variant: 'danger', icon: 'delete' }
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

    const raw = result.data;
    const userId = this.authService.getUserId();
    const isUpdate = !!(raw.id || (this.mandalModal.initialData && this.mandalModal.initialData.id));

    const submitData: any = {
      ...raw,
      id: isUpdate ? (raw.id || this.mandalModal.initialData.id) : null,
      vidhanId: Number(raw.vidhanId),
      stateId: Number(raw.stateId || this.defaultStateId),
      userId: userId ? String(userId) : null
    };

    const request = isUpdate
      ? this.mandalService.updateMandal(submitData)
      : this.mandalService.createMandal(submitData);

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
