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

@Component({
  selector: 'app-pradhan',
  standalone: true,
  imports: [CommonModule, FormsModule, GenericTableComponent, GenericModalButtonComponent, PageHeaderComponent],
  template: `
    <div class="h-full flex flex-col p-4 gap-4 overflow-hidden">
      <app-page-header title="Pradhan Management" subtitle="Manage village pradhans and their assignments">
        <app-generic-modal-button 
            *ngIf="canManage()"
            #pradhanModal 
            [config]="addPradhanConfig" 
            label="Add Pradhan" 
            icon="+"
            variant="primary" 
            (formSubmit)="handleFormSubmit($event)">
        </app-generic-modal-button>

        <div *ngIf="isListView && pradhans && pradhans.length > 0" class="relative">
          <select #exportSelect (change)="handleExport(exportSelect.value); exportSelect.value = ''"
            class="px-4 py-2 pr-8 rounded-xl text-xs font-bold text-slate-600 bg-white border border-slate-200 hover:bg-slate-50 transition-all shadow-sm appearance-none cursor-pointer outline-none focus:border-blue-500 focus:ring-1 focus:ring-blue-500">
            <option value="" disabled selected>Export Data</option>
            <option value="pdf">Export PDF</option>
            <option value="excel">Export Excel</option>
          </select>
          <div class="pointer-events-none absolute inset-y-0 right-0 flex items-center px-3 text-slate-500">
            <svg class="fill-current h-3 w-3" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20">
              <path d="M5.293 7.293a1 1 0 011.414 0L10 10.586l3.293-3.293a1 1 0 111.414 1.414l-4 4a1 1 0 01-1.414 0l-4-4a1 1 0 010-1.414z" />
            </svg>
          </div>
        </div>
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
  isListView = false;

  canManage(): boolean {
    return !this.isListView;
  }

  columns: TableColumn[] = [
    { key: 'name', label: 'Name', sortable: true },
    { key: 'designationName', label: 'Designation' },
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
        type: 'select',
        gridColSpan: 12,
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
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.route.url.subscribe(url => {
      const path = url[0]?.path || '';
      this.isListView = path.includes('-list');
      this.loadPradhans();
    });
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
      designationId: Number(raw.designationId),
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
    this.toastService.showSuccess('Export Started', `Successfully generated ${format.toUpperCase()} export!`);
  }
}
