import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, Validators } from '@angular/forms';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';
import { TableConfig, TableColumn, TableAction } from '../../shared/generic-table/generic-table.types';
import { GenericModalButtonComponent } from '../../shared/generic-modal-form/generic-modal-button.component';
import { FormConfig, FormResult } from '../../shared/generic-modal-form/generic-form.types';
import { PageHeaderComponent } from '../../shared/page-header/page-header.component';
import { BdcService } from '../../../Services/Admin/bdc/bdc.service';
import { ToastService } from '../../../Services/common/toast/toast.service';
import { CrudHandlerService } from '../../../Services/common/crud-handler.service';
import { ActivatedRoute } from '@angular/router';
import { AuthServiceService } from '../../../Services/Auth/auth.service';

import { GenericExportComponent } from '../../shared/generic-export/generic-export.component';

@Component({
  selector: 'app-bdc',
  standalone: true,
  imports: [CommonModule, FormsModule, GenericTableComponent, GenericModalButtonComponent, PageHeaderComponent, GenericExportComponent],
  template: `
    <div class="h-screen p-4 flex flex-col overflow-hidden bg-slate-50/50">
      <app-page-header [title]="isListView ? 'BDC List' : 'BDC Management'" subtitle="Manage Block Development Council members and assignments">
        <div class="flex items-center gap-3">
          <app-generic-modal-button 
              *ngIf="canManage()"
              #bdcModal 
              [config]="addBdcConfig" 
              label="Add BDC" 
              icon="+"
              variant="primary" 
              (formSubmit)="handleFormSubmit($event)">
          </app-generic-modal-button>

          <app-generic-export [show]="isListView && bdcs && bdcs.length > 0" 
              [isExporting]="isExporting" (export)="handleExport($event)">
          </app-generic-export>
        </div>
      </app-page-header>

      <div class="flex-1 min-h-0 mt-4 bg-white rounded-2xl shadow-sm border border-slate-200 overflow-hidden flex flex-col">
        <app-generic-table
          class="flex-1 min-h-0"
          [config]="tableConfig"
          [columns]="columns"
          [actions]="actions"
          [data]="bdcs"
          [loading]="loading"
          [totalItems]="totalItems"
          (actionClick)="handleAction($event)"
          (searchChange)="handleSearchChange($event)">
        </app-generic-table>
      </div>
    </div>
  `
})
export class BdcComponent implements OnInit {
  @ViewChild('bdcModal') bdcModal!: GenericModalButtonComponent;

  bdcs: any[] = [];
  loading = false;
  totalItems = 0;
  searchTerm = '';
  isListView = false;
  isExporting = false;

  canManage(): boolean {
    if (this.isListView) return false;
    const role = (this.authService.getRole() || '').toUpperCase().trim();
    return ['SUPERADMIN', 'ADMIN', 'VIDHANSABHAPRABHARI'].includes(role);
  }

  columns: TableColumn[] = [
    { key: 'profile', label: 'Profile', type: 'avatar', align: 'center', sortable: false, avatarFallbackKey: 'name' },
    { key: 'name', label: 'Name', sortable: true },
    { key: 'block', label: 'Block', sortable: true },
    { key: 'wardNumber', label: 'Ward No.', sortable: true },
    { key: 'mobile', label: 'Mobile' },
    { key: 'education', label: 'Education' },
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
    { id: 'edit', label: '', icon: 'edit', variant: 'primary', show: () => this.canManage() },
    { id: 'delete', label: '', icon: 'delete', variant: 'danger', show: () => this.canManage() }
  ];

  tableConfig: TableConfig = {
    selectable: false,
    paginated: true,
    searchable: true,
    searchPlaceholder: 'Search...',
    serverSide: true,
    defaultPageSize: 10
  };

  addBdcConfig: FormConfig = {
    title: 'Add BDC Member',
    submitLabel: 'Save BDC',
    fields: [
      {
        id: 'name',
        name: 'name',
        label: 'Name',
        type: 'text',
        placeholder: 'Enter member name',
        gridColSpan: 6,
        validations: [Validators.required]
      },
      {
        id: 'blockId',
        name: 'blockId',
        label: 'Block',
        type: 'select',
        placeholder: '-- Select Block --',
        apiUrl: 'block/getAllBlockName',
        apiMapper: (data: any) => {
          const list = data?.data || data?.items || data || [];
          return list.map((item: any) => ({
            value: String(item.id || item.blockId),
            label: item.blockName || item.name || item
          }));
        },
        gridColSpan: 6,
        validations: [Validators.required]
      },
      {
        id: 'wardNumber',
        name: 'wardNumber',
        label: 'Ward Number',
        type: 'text',
        placeholder: 'Enter ward number',
        gridColSpan: 6,
        validations: [Validators.required]
      },
      {
        id: 'mobile',
        name: 'mobile',
        label: 'Mobile Number',
        type: 'text',
        placeholder: 'Enter 10-digit number',
        gridColSpan: 6,
        validations: [Validators.required, Validators.pattern('^[0-9]{10}$')]
      },
      {
        id: 'age',
        name: 'age',
        label: 'Age',
        type: 'number',
        placeholder: 'Enter age',
        gridColSpan: 6,
        validations: [Validators.required, Validators.min(18)]
      },
      {
        id: 'education',
        name: 'education',
        label: 'Education',
        type: 'text',
        placeholder: 'Enter education level',
        gridColSpan: 6,
        validations: [Validators.required]
      },
      {
        id: 'categoryId',
        name: 'categoryId',
        label: 'Category',
        type: 'select',
        gridColSpan: 6,
        validations: [Validators.required],
        apiUrl: 'common/category',
        apiMapper: (data: any) => {
          const list = data?.data || data?.items || data || [];
          return list.map((item: any) => ({
            value: String(item.id),
            label: item.name || item.categoryName
          }));
        }
      },
      {
        id: 'castId',
        name: 'castId',
        label: 'Caste',
        type: 'select',
        gridColSpan: 6,
        dependsOn: 'categoryId',
        validations: [Validators.required],
        apiUrl: (catId: any) => `common/cast?id=${catId}`,
        apiMapper: (data: any) => {
          const list = data?.data || data?.items || data || [];
          return list.map((item: any) => ({
            value: String(item.id),
            label: item.name || item.castName
          }));
        }
      },
      {
        id: 'partyId',
        name: 'partyId',
        label: 'Party',
        type: 'select',
        gridColSpan: 6,
        validations: [Validators.required],
        apiUrl: 'common/getparty',
        apiMapper: (data: any) => {
          const list = data?.data?.items || data?.items || data?.data || data || [];
          return list.map((item: any) => ({
            value: String(item.id),
            label: item.partyName || item.party || item.name
          }));
        }
      },
      {
        id: 'villageId',
        name: 'villageId',
        label: 'Villages',
        type: 'select',
        gridColSpan: 6,
        validations: [Validators.required],
        apiUrl: 'common/getallvillages',
        multiple: true,
        apiMapper: (data: any) => {
          const list = data?.data?.items || data?.items || data?.data || data || [];
          return list.map((item: any) => ({
            value: String(item.id || item.villageId),
            label: item.villageName || item.name,
          }));
        }
      },
      {
        id: 'profile',
        name: 'profile',
        label: 'Profile Image',
        type: 'file',
        gridColSpan: 12
      }
    ]
  };

  constructor(
    private bdcService: BdcService,
    private toastService: ToastService,
    private crudHandler: CrudHandlerService,
    private route: ActivatedRoute,
    private authService: AuthServiceService
  ) { }

  ngOnInit() {
    this.route.url.subscribe(url => {
      const path = url[0]?.path || '';
      this.isListView = path.includes('-list');
      this.loadBdcs();
    });
  }

  loadBdcs() {
    this.loading = true;
    this.bdcService.getAllBdcs({ searchTerm: this.searchTerm }).subscribe({
      next: (res: any) => {
        this.bdcs = res.data?.items || res.data || [];
        this.totalItems = res.data?.totalCount || this.bdcs.length;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.toastService.showError('Error', 'Failed to load BDC members');
      }
    });
  }

  handleAction(event: { action: TableAction; row: any; index: number }) {
    const { action, row } = event;
    if (action.id === 'delete') {
      this.crudHandler.handleRequest(
        this.bdcService.deleteBdc(row.id),
        'Deleted',
        'BDC member deleted successfully!',
        () => this.loadBdcs()
      );
    } else if (action.id === 'edit') {
      const editData = { ...row };
      // Standardize IDs for select controls
      ['blockId', 'categoryId', 'castId', 'partyId'].forEach(key => {
        if (editData[key]) editData[key] = String(editData[key]);
      });
      // Map all assigned villages for multi-select binding
      if (row.villages && Array.isArray(row.villages)) {
        editData.villageId = row.villages.map((v: any) => String(v.villageId || v.id));
      }
      this.bdcModal.openModal(editData);
    }
  }

  handleSearchChange(term: string) {
    this.searchTerm = term;
    this.loadBdcs();
  }

  handleFormSubmit(result: FormResult) {
    if (!result.status) return;

    const raw = result.data;
    const rowId = raw.id || (this.bdcModal.initialData && this.bdcModal.initialData.id);

    const formData = new FormData();
    if (rowId) formData.append('Id', String(rowId));
    formData.append('BlockId', String(raw.blockId || 0));
    formData.append('Name', raw.name || '');
    formData.append('WardNumber', raw.wardNumber || '');
    formData.append('CategoryId', String(raw.categoryId || 0));
    formData.append('CastId', String(raw.castId || 0));
    formData.append('Age', String(raw.age || 0));
    formData.append('Mobile', String(raw.mobile || ''));
    formData.append('PartyId', String(raw.partyId || 0));
    formData.append('Education', raw.education || '');

    if (Array.isArray(raw.villageId)) {
      raw.villageId.forEach((v: any) => {
        formData.append('VillageId', String(v.id || v));
      });
    } else if (raw.villageId) {
      formData.append('VillageId', String(raw.villageId));
    }

    if (result.files && result.files['profile']) {
      formData.append('Profile', result.files['profile']);
    }

    const isUpdate = !!rowId;
    const request = isUpdate
      ? this.bdcService.updateBdc(formData)
      : this.bdcService.createBdc(formData);

    this.crudHandler.handleRequest(
      request,
      isUpdate ? 'Updated' : 'Created',
      `BDC member ${isUpdate ? 'updated' : 'created'} successfully!`,
      () => this.loadBdcs()
    );
  }

  handleExport(format: string) {
    if (!format) return;
    this.isExporting = true;
    const exportFormat = format as 'excel' | 'pdf';
    const request = exportFormat === 'excel' ? this.bdcService.exportToExcel() : this.bdcService.exportToPdf();

    request.subscribe({
      next: (blob: Blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `BDC_List.${format === 'excel' ? 'xlsx' : 'pdf'}`;
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
        document.body.removeChild(a);
        this.isExporting = false;
        this.toastService.showSuccess('Success', `BDC list exported to ${format.toUpperCase()} successfully!`);
      },
      error: (err: any) => {
        console.error(`Error exporting to ${format}:`, err);
        this.toastService.showError('Error', `Failed to export BDC list to ${format.toUpperCase()}`);
        this.isExporting = false;
      }
    });
  }
}

