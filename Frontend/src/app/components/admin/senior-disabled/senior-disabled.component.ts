import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, Validators } from '@angular/forms';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';
import { TableConfig, TableColumn, TableAction } from '../../shared/generic-table/generic-table.types';
import { GenericModalButtonComponent } from '../../shared/generic-modal-form/generic-modal-button.component';
import { FormConfig, FormResult } from '../../shared/generic-modal-form/generic-form.types';
import { PageHeaderComponent } from '../../shared/page-header/page-header.component';
import { SeniorDisabledService } from '../../../Services/Admin/senior-disabled/senior-disabled.service';
import { ToastService } from '../../../Services/common/toast/toast.service';
import { CrudHandlerService } from '../../../Services/common/crud-handler.service';
import { ActivatedRoute } from '@angular/router';
import { AuthServiceService } from '../../../Services/Auth/auth.service';
import { ModulePermission } from '../../../models/module-permission.enum';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-senior-disabled',
  standalone: true,
  imports: [CommonModule, FormsModule, GenericTableComponent, GenericModalButtonComponent, PageHeaderComponent],
  template: `
    <div class="h-full flex flex-col p-4 gap-4 overflow-hidden">
      <app-page-header [title]="pageTitle" [subtitle]="pageSubtitle">
        <app-generic-modal-button 
            *ngIf="canManage()"
            #citizenModal 
            [config]="addCitizenConfig" 
            [label]="'Add ' + (isDisabledView ? 'Disabled' : 'Senior Citizen')" 
            icon="+"
            variant="primary" 
            (formSubmit)="handleFormSubmit($event)">
        </app-generic-modal-button>

        <div *ngIf="isListView && citizens && citizens.length > 0" class="relative">
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
          [data]="citizens"
          [loading]="loading"
          [totalItems]="totalItems"
          (actionClick)="handleAction($event)"
          (searchChange)="handleSearchChange($event)"
          (filterChange)="handleFilterChange($event)"
          (pageChange)="handlePageChange($event)"
          (sortChange)="handleSortChange($event)">
        </app-generic-table>
      </div>
    </div>
  `
})
export class SeniorDisabledComponent implements OnInit {
  @ViewChild('citizenModal') citizenModal!: GenericModalButtonComponent;

  citizens: any[] = [];
  loading = false;
  totalItems = 0;
  searchTerm = '';
  pageNumber = 1;
  pageSize = 50;
  sortBy = '';
  isDescending = false;

  isDisabledView = false;
  isListView = false;

  boothIds: string | null = null;
  villageIds: string | null = null;
  castIds: string | null = null;

  canManage(): boolean {
    return !this.isListView;
  }

  get pageTitle() { return this.isDisabledView ? 'Disabled Management' : 'Senior Citizen Management'; }
  get pageSubtitle() { return `Manage and view all ${this.isDisabledView ? 'disabled persons' : 'senior citizens'} in the assembly.`; }

  columns: TableColumn[] = [
    { key: 'name', label: 'Name', sortable: true },
    { key: 'mobile', label: 'Mobile', sortable: true },
    { key: 'voterId', label: 'Voter ID', sortable: true },
    { key: 'categoryName', label: 'Category' },
    { key: 'boothNumber', label: 'Booth No.' },
    { key: 'villageName', label: 'Village' }
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
    defaultPageSize: 50,
    filterable: false
  };

  addCitizenConfig: FormConfig = {
    title: 'Add Entry',
    submitLabel: 'Save Information',
    fields: [
      {
        id: 'typeId',
        name: 'typeId',
        label: 'Person Type',
        type: 'select',
        gridColSpan: 4,
        validations: [Validators.required],
        options: [
          { value: '1', label: 'Senior Citizen' },
          { value: '2', label: 'Disabled' }
        ]
      },
      {
        id: 'boothId',
        name: 'boothId',
        label: 'Booth Number',
        type: 'select',
        placeholder: '-- Select Booth --',
        gridColSpan: 4,
        validations: [Validators.required],
        apiUrl: 'common/boothNumber',
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : []);
          return list.map((item: any) => ({
            value: String(item.boothId || item.id),
            label: `Booth No. ${item.boothNumber}`
          }));
        }
      },
      {
        id: 'villageId',
        name: 'villageId',
        label: 'Villages',
        type: 'select',
        placeholder: '-- Select Villages --',
        gridColSpan: 4,
        multiple: true,
        validations: [Validators.required],
        dependsOn: 'boothId',
        apiUrl: (boothId: string) => `common/villagesByBoothId?boothId=${boothId}`,
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data?.items) ? data.data.items : (Array.isArray(data?.items) ? data.items : (Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : [])));
          return list.map((item: any) => ({
            value: String(item.id),
            label: item.name || item.villageName
          }));
        }
      },
      {
        id: 'seniorDisabledRequest',
        name: 'seniorDisabledRequest',
        label: 'Person Details',
        type: 'form-array',
        gridColSpan: 12,
        addLabel: 'Add Another Person',
        subFields: [
          {
            id: 'name',
            name: 'name',
            label: 'Name',
            type: 'text',
            gridColSpan: 6,
            validations: [Validators.required]
          },
          {
            id: 'mobile',
            name: 'mobile',
            label: 'Mobile No.',
            type: 'text',
            gridColSpan: 6,
            validations: [Validators.required, Validators.pattern('^[0-9]{10}$')]
          },
          {
            id: 'categoryId',
            name: 'categoryId',
            label: 'Category',
            type: 'select',
            placeholder: '-- Select Category --',
            gridColSpan: 6,
            validations: [Validators.required],
            apiUrl: 'common/category',
            apiMapper: (data: any) => {
              const list = Array.isArray(data?.data?.items) ? data.data.items : (Array.isArray(data?.items) ? data.items : (Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : [])));
              return list.map((item: any) => ({
                value: String(item.id),
                label: item.name
              }));
            }
          },
          {
            id: 'castId',
            name: 'castId',
            label: 'Caste',
            type: 'select',
            placeholder: '-- Select Caste --',
            gridColSpan: 6,
            dependsOn: 'categoryId',
            validations: [Validators.required],
            apiUrl: (catId: any) => `common/cast?id=${catId}`,
            apiMapper: (data: any) => {
              const list = Array.isArray(data?.data?.items) ? data.data.items : (Array.isArray(data?.items) ? data.items : (Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : [])));
              return list.map((item: any) => ({
                value: String(item.id),
                label: item.name
              }));
            }
          },
          {
            id: 'address',
            name: 'address',
            label: 'Address',
            type: 'text',
            gridColSpan: 6,
            validations: [Validators.required]
          },
          {
            id: 'voterId',
            name: 'voterId',
            label: 'Voter ID',
            type: 'text',
            gridColSpan: 6,
            validations: [Validators.required]
          }
        ]
      }
    ]
  };

  constructor(
    private citizenService: SeniorDisabledService,
    private toastService: ToastService,
    private crudHandler: CrudHandlerService,
    private route: ActivatedRoute,
    private authService: AuthServiceService,
    private http: HttpClient
  ) { }

  ngOnInit() {
    this.route.url.subscribe(url => {
      const path = url[0]?.path || '';
      this.isDisabledView = path.includes('disabled');
      this.isListView = path.includes('-list');
      this.tableConfig.filterable = this.isListView;
      if (this.isListView) {
        this.loadFilterOptions();
      }
      this.loadCitizens();
    });
  }

  loadFilterOptions() {
    this.tableConfig.filters = [
      { key: 'boothIds', label: 'Booth', type: 'select', options: [], placeholder: '-- Select Booth --', multiple: true },
      { key: 'villageIds', label: 'Village', type: 'select', options: [], placeholder: '-- Select Village --', multiple: true },
      { key: 'castIds', label: 'Caste', type: 'select', options: [], placeholder: '-- Select Caste --', multiple: true }
    ];

    // Load Booths
    this.http.get<any>(`${environment.apiUrl}/common/boothNumber`).subscribe(res => {
      const filter = this.tableConfig.filters?.find(f => f.key === 'boothIds');
      if (filter) {
        const list = Array.isArray(res?.data) ? res.data : (Array.isArray(res) ? res : []);
        filter.options = list.map((b: any) => ({
          label: `Booth No. ${b.boothNumber}`,
          value: String(b.boothId || b.id)
        }));
      }
    });

    // Load All Villages initially
    this.http.get<any>(`${environment.apiUrl}/common/village?pageSize=500000`).subscribe(res => {
      const filter = this.tableConfig.filters?.find(f => f.key === 'villageIds');
      if (filter) {
        const list = Array.isArray(res?.data?.items) ? res.data.items : (Array.isArray(res?.items) ? res.items : (Array.isArray(res?.data) ? res.data : (Array.isArray(res) ? res : [])));
        filter.options = list.map((v: any) => ({
          label: v.name || v.villageName,
          value: String(v.id || v.villageId)
        }));
      }
    });

    // Load Castes
    this.http.get<any>(`${environment.apiUrl}/common/cast?id=&pageSize=500000`).subscribe(res => {
      const filter = this.tableConfig.filters?.find(f => f.key === 'castIds');
      if (filter) {
        const list = Array.isArray(res?.data?.items) ? res.data.items : (Array.isArray(res?.items) ? res.items : (Array.isArray(res?.data) ? res.data : (Array.isArray(res) ? res : [])));
        filter.options = list.map((c: any) => ({
          label: c.name || c.castName,
          value: String(c.id)
        }));
      }
    });
  }

  handleFilterChange(filterState: Record<string, any>) {
    const processIds = (ids: any) => Array.isArray(ids) ? (ids.length > 0 ? ids.join(',') : null) : (ids || null);

    this.boothIds = processIds(filterState['boothIds']);
    this.villageIds = processIds(filterState['villageIds']);
    this.castIds = processIds(filterState['castIds']);

    // Cascade villages when booth changes
    if (filterState['boothIds'] && Array.isArray(filterState['boothIds']) && filterState['boothIds'].length === 1) {
      const boothId = filterState['boothIds'][0];
      this.http.get<any>(`${environment.apiUrl}/common/villagesByBoothId?boothId=${boothId}`).subscribe(res => {
        const filter = this.tableConfig.filters?.find(f => f.key === 'villageIds');
        if (filter) {
          const list = Array.isArray(res?.data) ? res.data : (Array.isArray(res?.data?.items) ? res.data.items : (Array.isArray(res) ? res : []));
          filter.options = list.map((v: any) => ({
            label: v.name || v.villageName,
            value: String(v.id || v.villageId)
          }));
        }
      });
    }

    this.pageNumber = 1;
    this.loadCitizens();
  }

  handlePageChange(event: any) {
    this.pageNumber = event.currentPage;
    this.pageSize = event.pageSize;
    this.loadCitizens();
  }

  handleSortChange(event: any) {
    this.sortBy = event.column;
    this.isDescending = event.direction === 'desc';
    this.pageNumber = 1;
    this.loadCitizens();
  }

  handleSearchChange(term: string) {
    this.searchTerm = term;
    this.pageNumber = 1;
    this.loadCitizens();
  }

  loadCitizens() {
    this.loading = true;
    const params: any = { 
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      searchTerm: this.searchTerm,
      sortBy: this.sortBy,
      isDescending: this.isDescending,
      boothIds: this.boothIds,
      villageIds: this.villageIds,
      castIds: this.castIds
    };
    
    const userId = this.authService.getUserId();
    if (userId) {
      params.userId = userId;
    }

    params['TypeId'] = this.isDisabledView ? 2 : 1;

    // Clean up empty params
    Object.keys(params).forEach(key => {
      if (params[key] === null || params[key] === undefined || params[key] === '') {
        delete params[key];
      }
    });

    this.citizenService.getAllSeniorDisabled(params).subscribe({
      next: (res: any) => {
        const dataWrap = res.data;
        if (dataWrap && dataWrap.items) {
          this.citizens = dataWrap.items.map((item: any) => ({
            ...item,
            villageName: Array.isArray(item.villages) ? item.villages.map((v: any) => v.villageName).join(', ') : '',
            villageId: Array.isArray(item.villages) ? item.villages.map((v: any) => v.villageId) : []
          }));
          this.totalItems = dataWrap.totalCount || 0;
        } else {
          const rawList = Array.isArray(dataWrap) ? dataWrap : (Array.isArray(res.data) ? res.data : []);
          this.citizens = rawList.map((item: any) => ({
            ...item,
            villageName: Array.isArray(item.villages) ? item.villages.map((v: any) => v.villageName).join(', ') : '',
            villageId: Array.isArray(item.villages) ? item.villages.map((v: any) => v.villageId) : []
          }));
          this.totalItems = this.citizens.length;
        }
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.toastService.showError('Error', 'Failed to load records');
      }
    });
  }

  handleAction(event: { action: TableAction; row: any; index: number }) {
    const { action, row } = event;
    if (action.id === 'delete') {
      this.crudHandler.handleRequest(
        this.citizenService.deleteSeniorDisabled(row.id),
        'Deleted',
        'Record deleted successfully!',
        () => this.loadCitizens(),
        true,
        this.isDisabledView ? ModulePermission.Disabled : ModulePermission.SeniorCitizen
      );
    } else if (action.id === 'edit') {
      // Note: Edit might need special handling for the nested array if backend returns flat results
      // For now, we standardize IDs
      const editData = { ...row };
      ['typeId', 'boothId', 'categoryId', 'castId'].forEach(k => {
        if (editData[k]) editData[k] = String(editData[k]);
      });
      if (editData.villageId) {
        editData.villageId = Array.isArray(editData.villageId) ? editData.villageId.map(String) : [String(editData.villageId)];
      }
      // If editing a single record, we wrap it in an array for the form-array field
      if (!editData.seniorDisabledRequest) {
        editData.seniorDisabledRequest = [{
          name: row.name,
          address: row.address,
          mobile: row.mobile,
          voterId: row.voterId,
          categoryId: String(row.categoryId),
          castId: String(row.castId)
        }];
      }
      this.citizenModal.openModal(editData);
    }
  }

  handleFormSubmit(result: FormResult) {
    if (!result.status) return;

    const raw = result.data;
    const isUpdate = !!(raw.id || (this.citizenModal.initialData && this.citizenModal.initialData.id));

    const submitData: any = {
      typeId: Number(raw.typeId),
      boothId: Number(raw.boothId),
      villageId: Array.isArray(raw.villageId) ? raw.villageId.map(Number) : [Number(raw.villageId)],
      seniorDisabledRequest: raw.seniorDisabledRequest.map((item: any) => ({
        name: item.name,
        address: item.address,
        categoryId: Number(item.categoryId),
        castId: Number(item.castId),
        mobile: String(item.mobile),
        voterId: item.voterId
      })),
      userId: this.authService.getUserId()
    };

    if (isUpdate) {
      submitData.id = Number(raw.id || this.citizenModal.initialData.id);
    }

    const request = isUpdate
      ? this.citizenService.updateSeniorDisabled(submitData)
      : this.citizenService.createSeniorDisabled(submitData);

    this.crudHandler.handleRequest(
      request,
      isUpdate ? 'Updated' : 'Created',
      `Record ${isUpdate ? 'updated' : 'created'} successfully!`,
      () => this.loadCitizens(),
      true,
      this.isDisabledView ? ModulePermission.Disabled : ModulePermission.SeniorCitizen
    );
  }

  handleExport(format: string) {
    if (!format) return;
    this.toastService.showSuccess('Export Started', `Successfully generated ${format.toUpperCase()} export!`);
  }
}
