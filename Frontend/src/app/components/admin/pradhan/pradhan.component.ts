import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';

import { TableConfig, TableColumn, TableAction } from '../../shared/generic-table/generic-table.types';
import { FormConfig, FormResult } from '../../shared/generic-modal-form/generic-form.types';
import { PradhanService } from '../../../Services/Admin/pradhan/pradhan.service';
import { CrudHandlerService } from '../../../Services/common/crud-handler.service';
import { Validators } from '@angular/forms';
import { ToastService } from '../../../Services/common/toast/toast.service';
import { GenericModalButtonComponent } from '../../shared/generic-modal-form/generic-modal-button.component';
import { PageHeaderComponent } from '../../shared/page-header/page-header.component';


@Component({
  selector: 'app-pradhan',
  standalone: true,
  imports: [CommonModule, FormsModule, GenericTableComponent, GenericModalButtonComponent, PageHeaderComponent],
  template: `
    <div class="h-full flex flex-col p-4 gap-4 overflow-hidden">
      <app-page-header title="Pradhan Management" subtitle="Manage village pradhans and their assignments">
        <app-generic-modal-button 
            #pradhanModal 
            [config]="addPradhanConfig" 
            label="Add Pradhan" 
            icon="+"
            variant="primary" 
            (formSubmit)="handleFormSubmit($event)">
        </app-generic-modal-button>
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
          (searchChange)="handleSearchChange($event)">
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

  columns: TableColumn[] = [
    { key: 'name', label: 'Name', sortable: true },
    { key: 'designationName', label: 'Designation' },
    { key: 'contact', label: 'Contact' },
    { key: 'gender', label: 'Gender' },
    {
      key: 'villageNames', label: 'Assigned Villages', formatter: (val: any, row: any) => {
        if (row.villages && Array.isArray(row.villages)) {
          return row.villages.map((v: any) => v.villageName || 'na').join(', ');
        }
        return val || 'N/A';
      }
    }
  ];

  actions: TableAction[] = [
    { id: 'edit', label: '', icon: 'edit', variant: 'primary' },
    { id: 'delete', label: '', icon: 'delete', variant: 'danger' }
  ];

  tableConfig: TableConfig = {
    selectable: false,
    paginated: true,
    searchable: true,
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
          { value: 'Male', label: 'Male' },
          { value: 'Female', label: 'Female' },
          { value: 'Other', label: 'Other' }
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
        id: 'designationId',
        name: 'designationId',
        label: 'Designation',
        type: 'select',
        gridColSpan: 6,
        validations: [Validators.required],
        apiUrl: 'common/getadmindesignation',
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data?.items) ? data.data.items : (Array.isArray(data?.items) ? data.items : (Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : [])));
          return list.map((item: any) => ({
            value: String(item.id),
            label: item.designationName || item.name || item.Name
          }));
        }
      },
      {
        id: 'villageId',
        name: 'villageId',
        label: 'Assigned Villages',
        type: 'selection-table',
        gridColSpan: 12,
        validations: [Validators.required],
        apiUrl: 'common/village',
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data?.items) ? data.data.items : (Array.isArray(data?.items) ? data.items : (Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : [])));
          return list.map((item: any) => ({
            id: item.id || item.villageId || item.VillageId,
            name: item.villageName || item.name || item.Name,
            anshik: 'No'
          }));
        }
      }
    ]
  };

  constructor(
    private pradhanService: PradhanService,
    private toastService: ToastService,
    private crudHandler: CrudHandlerService
  ) { }

  ngOnInit() {
    this.loadPradhans();
  }

  loadPradhans() {
    this.loading = true;
    this.pradhanService.getAllPradhans({ searchTerm: this.searchTerm }).subscribe({
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
      this.pradhanModal.openModal(row);
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

    // Mapping to camelCase as strictly requested in the JSON snippet
    const submitData: any = {
      name: raw.name,
      designationId: Number(raw.designationId),
      contact: String(raw.contact),
      gender: raw.gender,
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
}
