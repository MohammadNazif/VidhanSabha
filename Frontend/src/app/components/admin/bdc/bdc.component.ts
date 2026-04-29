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

@Component({
  selector: 'app-bdc',
  standalone: true,
  imports: [CommonModule, FormsModule, GenericTableComponent, GenericModalButtonComponent, PageHeaderComponent],
  template: `
    <div class="h-full flex flex-col p-4 gap-4 overflow-hidden">
      <app-page-header title="BDC Management" subtitle="Manage Block Development Council members and assignments">
        <app-generic-modal-button 
            *ngIf="canManage()"
            #bdcModal 
            [config]="addBdcConfig" 
            label="Add BDC" 
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

  canManage(): boolean {
    return !this.isListView;
  }

  columns: TableColumn[] = [
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
        id: 'block',
        name: 'block',
        label: 'Block',
        type: 'text',
        placeholder: 'Enter block name',
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
      }
    ]
  };

  constructor(
    private bdcService: BdcService,
    private toastService: ToastService,
    private crudHandler: CrudHandlerService,
    private route: ActivatedRoute
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
      ['categoryId', 'castId', 'partyId'].forEach(key => {
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
    const isUpdate = !!(raw.id || (this.bdcModal.initialData && this.bdcModal.initialData.id));

    const submitData: any = {
      block: raw.block,
      name: raw.name,
      wardNumber: raw.wardNumber,
      villageId: Array.isArray(raw.villageId)
        ? raw.villageId.map((v: any) => Number(v.id || v))
        : [Number(raw.villageId)],
      categoryId: Number(raw.categoryId),
      castId: Number(raw.castId),
      age: Number(raw.age),
      mobile: String(raw.mobile),
      partyId: Number(raw.partyId),
      education: raw.education
    };

    if (isUpdate) {
      submitData.id = Number(raw.id || this.bdcModal.initialData.id);
    }

    const request = isUpdate
      ? this.bdcService.updateBdc(submitData)
      : this.bdcService.createBdc(submitData);

    this.crudHandler.handleRequest(
      request,
      isUpdate ? 'Updated' : 'Created',
      `BDC member ${isUpdate ? 'updated' : 'created'} successfully!`,
      () => this.loadBdcs()
    );
  }

  handleExport(format: string) {
    if (!format) return;
    this.toastService.showSuccess('Export Started', `Successfully generated ${format.toUpperCase()} export!`);
  }
}
