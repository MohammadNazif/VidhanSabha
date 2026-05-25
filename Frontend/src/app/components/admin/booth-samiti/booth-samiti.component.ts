import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { PageHeaderComponent } from '../../shared/page-header/page-header.component';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';
import { TableAction, TableColumn, TableConfig } from '../../shared/generic-table/generic-table.types';
import { BoothSamitiService } from '../../../Services/Admin/booth-voter/booth-samiti.service';
import { CrudHandlerService } from '../../../Services/common/crud-handler.service';
import { ModulePermission } from '../../../models/module-permission.enum';
import { LucideAngularModule } from 'lucide-angular';
import { ToastService } from '../../../Services/common/toast/toast.service';
import { AuthServiceService } from '../../../Services/Auth/auth.service';

import { GenericModalButtonComponent } from '../../shared/generic-modal-form/generic-modal-button.component';
import { ActivatedRoute } from '@angular/router';
import { DropdownOption, FormConfig, FormResult } from '../../shared/generic-modal-form/generic-form.types';
import { BehaviorSubject } from 'rxjs';
import { Validators } from '@angular/forms';
import { environment } from '../../../../environments/environment';
import { PermissionService } from '../../../Services/common/permission.service';
import { GenericExportComponent } from '../../shared/generic-export/generic-export.component';


@Component({
  selector: 'app-booth-samiti',
  standalone: true,
  imports: [CommonModule, PageHeaderComponent, GenericTableComponent, LucideAngularModule, GenericModalButtonComponent, GenericExportComponent],
  template: `
    <div class="h-screen p-6 flex flex-col overflow-hidden bg-slate-50/50">
      <app-page-header [title]="isMemberView ? selectedBoothName : (isListView ? 'Booth Samiti List' : 'Booth Samiti Management')" 
                       [subtitle]="isMemberView ? 'Manage members assigned to this booth' : 'Overview of all registered booth samitis'">
        
        <div class="flex items-center gap-3">
          <button *ngIf="isMemberView" (click)="backToList()" 
            class="flex items-center justify-center gap-2 px-4 py-2 text-sm font-bold text-slate-700 bg-white border border-slate-200 rounded-xl hover:bg-slate-50 hover:border-slate-300 transition-all shadow-sm active:scale-95 min-w-[100px]">
            <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round" class="text-slate-500"><path d="m15 18-6-6 6-6"/></svg>
            <span>Back</span>
          </button>

          <app-generic-modal-button *ngIf="canManage() && !isMemberView" #samitiModal [config]="samitiFormConfig" (formSubmit)="handleFormSubmit($event)"
            label="Add Samiti" icon="+" variant="primary">
          </app-generic-modal-button>

          <app-generic-export [show]="(isListView || isMemberView) && memberList && memberList.length > 0" 
              [isExporting]="isExporting" (export)="handleExport($event)">
          </app-generic-export>
        </div>

        <app-generic-modal-button #memberModal [config]="memberFormConfig" (formSubmit)="handleMemberSubmit($event)"
          label="" [hideButton]="true">
        </app-generic-modal-button>
      </app-page-header>

      <div class="flex-1 min-h-0 mt-6 bg-white rounded-2xl shadow-sm border border-slate-200 overflow-hidden flex flex-col">
        <app-generic-table class="flex-1 min-h-0" 
          [columns]="columns" 
          [data]="memberList" 
          [config]="config" 
          [actions]="actions" 
          [totalItems]="totalCount" 
          [loading]="loading"
          (actionClick)="handleAction($event)"
          (pageChange)="handlePageChange($event)"
          (sortChange)="handleSortChange($event)"
          (searchChange)="handleSearchChange($event)">
        </app-generic-table>
      </div>
    </div>
  `
})
export class BoothSamitiComponent implements OnInit {
  @ViewChild('samitiModal') samitiModal!: GenericModalButtonComponent;
  @ViewChild('memberModal') memberModal!: GenericModalButtonComponent;

  memberList: any[] = [];
  totalCount = 0;
  loading = false;
  isExporting = false;

  pageNumber = 1;
  pageSize = 50;
  searchTerm = '';
  sortBy = '';
  isDescending = false;
  isListView = false;

  // Member View State
  isMemberView = false;
  selectedBoothId: number | null = null;
  selectedBoothName = '';
  currentBoothDesignationIds: number[] = [];

  // Cached options to minimize API calls
  designations: any[] = [];
  categories: any[] = [];
  existingBoothIds = new Set<number>();

  // Observables for form fields
  private designationSubject = new BehaviorSubject<DropdownOption[]>([]);
  private categorySubject = new BehaviorSubject<DropdownOption[]>([]);
  designationOptions$ = this.designationSubject.asObservable();
  categoryOptions$ = this.categorySubject.asObservable();

  updateDropdownOptions() {
    // Update Designation Options with dynamic disabling logic
    const mappedDesignations = this.designations.map((item: any) => {
      const isTakenInList = this.isMemberView && this.memberList.some(m =>
        Number(m.designationId) === Number(item.id) &&
        m.id !== Number(this.memberModal?.initialData?.id)
      );
      const isTakenInBooth = !this.isMemberView && (this.currentBoothDesignationIds || []).includes(item.id);

      return {
        value: String(item.id),
        label: item.designationName,
        disabled: isTakenInList || isTakenInBooth || item.isExists === true
      };
    });
    this.designationSubject.next(mappedDesignations);

    // Update Category Options (static)
    if (this.categories.length > 0 && this.categorySubject.value.length === 0) {
      this.categorySubject.next(this.categories.map(c => ({ value: String(c.id), label: c.name })));
    }
  }

  canManage(): boolean {
    if (this.isListView) return false;
    const role = (this.authService.getRole() || '').toUpperCase().trim();
    if (role === 'VIDHANSABHAPRABHARI') return true;
    return this.permissionService.hasPermission(ModulePermission.BoothSamiti);
  }

  backToList() {
    this.isMemberView = false;
    this.selectedBoothId = null;
    this.selectedBoothName = '';
    this.currentBoothDesignationIds = [];
    this.pageNumber = 1;
    this.loadMembers();
    this.updateDropdownOptions();
    this.updateTableConfig();
  }

  columns: TableColumn[] = [];
  actions: TableAction[] = [];

  updateTableConfig() {
    if (this.isMemberView) {
      this.columns = [
        { key: 'name', label: 'Name', sortable: true },
        { key: 'designationName', label: 'Designation', sortable: true },
        { key: 'categoryName', label: 'Category', sortable: true },
        { key: 'casteName', label: 'Caste', sortable: true },
        { key: 'age', label: 'Age', sortable: true },
        { key: 'contact', label: 'Contact', sortable: true },
        { key: 'occupation', label: 'Occupation', sortable: true }
      ];
      this.actions = [
        { id: 'edit_mem', label: '', variant: 'default', icon: 'edit', show: () => this.canManage() },
        { id: 'delete_mem', label: '', variant: 'danger', icon: 'delete', show: () => this.canManage() }
      ];
    } else {
      if (this.isListView) {
        this.columns = [
          { key: 'boothNo', label: 'Booth No.', sortable: true },
          { key: 'village', label: 'Village', sortable: true },
          { key: 'pollingStation', label: 'Polling Station', sortable: true },
          { key: 'boothAdhayaksh', label: 'Booth Adhayaksh', sortable: true },
          { key: 'designation', label: 'Designation', sortable: true },
          { key: 'categoryName', label: 'Category', sortable: true },
          { key: 'castName', label: 'Caste', sortable: true },
          { key: 'age', label: 'Age', sortable: true },
          { key: 'contact', label: 'Contact', sortable: true },
          { key: 'totalMember', label: 'Total Members', sortable: true }
        ];
      } else {
        this.columns = [
          { key: 'boothNo', label: 'Booth No.', sortable: true },
          { key: 'village', label: 'Village', sortable: true },
          { key: 'pollingStation', label: 'Polling Station', sortable: true },
          { key: 'boothAdhayaksh', label: 'Booth Adhayaksh', sortable: true },
          { key: 'totalMember', label: 'Total Members', sortable: true },
          { key: 'contact', label: 'Contact', sortable: true }
        ];
      }
      this.actions = [
        { id: 'view', label: 'View', variant: 'primary', icon: 'view' },
        { id: 'add_samiti', label: 'Add Member', variant: 'primary', icon: 'user', show: () => this.canManage() }
      ];
    }
  }

  memberFormConfig: FormConfig = {
    title: 'Booth Samiti Member',
    submitLabel: 'Save Member',
    fields: [
      { id: 'id', name: 'id', label: 'ID', type: 'hidden' },
      { id: 'boothIdMem', name: 'boothIdMem', label: 'Booth ID', type: 'hidden' },
      { id: 'name', name: 'name', label: 'Full Name', type: 'text', placeholder: 'Enter name', validations: [Validators.required], gridColSpan: 6 },
      { id: 'contact', name: 'contact', label: 'Contact', type: 'text', placeholder: 'Enter phone', validations: [Validators.required], gridColSpan: 6 },
      { id: 'age', name: 'age', label: 'Age', type: 'number', placeholder: 'Enter age', validations: [Validators.required], gridColSpan: 6 },
      { id: 'occupation', name: 'occupation', label: 'Occupation', type: 'text', placeholder: 'Enter occupation', validations: [Validators.required], gridColSpan: 6 },
      {
        id: 'categoryId',
        name: 'categoryId',
        label: 'Category',
        type: 'select',
        placeholder: '-- Select Category --',
        options: this.categoryOptions$,
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'casteId',
        name: 'casteId',
        label: 'Caste',
        type: 'select',
        placeholder: '-- Select Caste --',
        dependsOn: 'categoryId',
        apiUrl: (catId: string) => `common/cast?id=${catId}`,
        apiMapper: (data: any) => (data.data || data).map((item: any) => ({ value: String(item.id), label: item.name })),
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'designationId',
        name: 'designationId',
        label: 'Designation',
        type: 'select',
        placeholder: '-- Select Designation --',
        options: this.designationOptions$,
        validations: [Validators.required],
        gridColSpan: 12
      }
    ]
  };

  config: TableConfig = {
    selectable: false,
    loading: false,
    searchable: true,
    searchPlaceholder: 'Search...',
  };

  samitiFormConfig: FormConfig = {
    title: 'Booth Samiti Member',
    submitLabel: 'Save Member',
    fields: [
      { id: 'id', name: 'id', label: 'ID', type: 'hidden' },
      {
        id: 'boothId',
        name: 'boothId',
        label: 'Booth',
        type: 'select',
        placeholder: '-- Select Booth --',
        apiUrl: () => {
          const role = (this.authService.getRole() || '').toUpperCase().trim();
          if (role === 'SECTORSANYOJAK') {
            return `booth/getAllBoothBySectorid?sectorid=${this.authService.getUserId()}`;
          }
          return 'common/boothNumber';
        },
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : []);
          return list.map((item: any) => {
            const bId = Number(item.boothId || item.id);
            return {
              value: String(bId),
              label: `Booth No. ${item.boothNumber}`,
              disabled: this.existingBoothIds.has(bId)
            };
          });
        },
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'sanyojakName',
        name: 'sanyojakName',
        label: 'Sanyojak Name',
        type: 'text',
        readonly: true,
        placeholder: 'Auto-fills based on booth...',
        dependsOn: 'boothId',
        apiUrl: (boothId: string) => `boothsamiti/getById?boothId=${boothId}`,
        apiMapper: (data: any) => {
          const item = data?.data || data;
          if (item && item.boothAdhayaksh) {
            return [{ value: item.boothAdhayaksh, label: item.boothAdhayaksh }];
          }
          if (item && item.name) {
            return [{ value: item.name, label: item.name }];
          }
          return [];
        },
        gridColSpan: 6
      }
    ]
  };

  constructor(
    private boothSamitiService: BoothSamitiService,
    private crudHandler: CrudHandlerService,
    private route: ActivatedRoute,
    private toastService: ToastService,
    private http: HttpClient,
    private authService: AuthServiceService,
    private permissionService: PermissionService
  ) { }

  ngOnInit() {
    this.route.url.subscribe((url: any) => {
      const path = url[0]?.path || '';
      this.isListView = path.includes('-list');
      this.updateTableConfig();
      this.loadMembers();
    });

    // Fetch common data once to minimize API calls
    this.boothSamitiService.getDesignations().subscribe(res => {
      this.designations = res.data || res || [];
      this.updateDropdownOptions();
    });
    this.http.get(`${environment.apiUrl}/common/category`).subscribe((res: any) => {
      this.categories = res.data || res || [];
      this.updateDropdownOptions();
    });

    this.loadExistingSamitis();
  }

  loadExistingSamitis() {
    this.boothSamitiService.getBoothSamiti({ PageNumber: 1, PageSize: 10000 }).subscribe(res => {
      const dataWrap = res.data || res;
      const list = Array.isArray(dataWrap) ? dataWrap : (dataWrap.items || []);
      this.existingBoothIds = new Set(list.map((m: any) => Number(m.id || m.boothId)));
    });
  }

  loadMembers() {
    this.loading = true;
    if (this.isMemberView && this.selectedBoothId) {
      this.boothSamitiService.getAllMembers(this.selectedBoothId).subscribe({
        next: (res) => {
          const dataWrap = res.data || res;
          const apiMembers = dataWrap.members || (Array.isArray(dataWrap) ? dataWrap : []);
          const sanyojak = dataWrap.boothSanyojak;

          const mapped = apiMembers.map((item: any, index: number) => ({
            ...item,
            srNo: index + 1
          }));

          if (this.isListView && sanyojak) {
            const adhyaksh = {
              id: -1,
              name: sanyojak.inchargeName,
              designationName: sanyojak.designation,
              categoryName: sanyojak.categoryName,
              casteName: sanyojak.castName,
              age: sanyojak.age,
              contact: sanyojak.phoneNumber,
              occupation: 'Booth Adhayaksh',
              isAdhyakshRow: true
            };
            this.memberList = [adhyaksh, ...mapped];
          } else {
            this.memberList = mapped;
          }

          this.totalCount = this.memberList.length;
          this.updateDropdownOptions();
          this.loading = false;
        },
        error: () => this.loading = false
      });
      return;
    }

    const params: any = {
      PageNumber: this.pageNumber,
      PageSize: this.pageSize,
      SearchTerm: this.searchTerm,
      SortBy: this.sortBy || 'id',
      IsDescending: this.isDescending,
      userId: this.authService.getRole() === 'BoothSanyojak' ? this.authService.getUserId() : null,
      roleFilterFlag: !this.isListView
    };

    this.boothSamitiService.getBoothSamiti(params).subscribe({
      next: (res) => {
        const dataWrap = res.data || res;
        const list = Array.isArray(dataWrap) ? dataWrap : (dataWrap.items || []);
        this.memberList = list.map((item: any, index: number) => ({
          ...item,
          srNo: (this.pageNumber - 1) * this.pageSize + index + 1
        }));
        this.totalCount = dataWrap.totalCount || list.length;
        this.updateDropdownOptions();
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  handleAction(event: { action: TableAction, row: any }) {
    if (event.action.id === 'view') {
      this.isMemberView = true;
      this.updateTableConfig();
      this.selectedBoothId = event.row.id;
      this.selectedBoothName = `Booth No. ${event.row.boothNo}`;
      this.loadMembers();
    } else if (event.action.id === 'add_samiti') {
      this.currentBoothDesignationIds = event.row.designationIds || [];
      this.updateDropdownOptions();
      this.memberModal.openModal({
        boothIdMem: Number(event.row.id || 0)
      });
    } else if (event.action.id === 'edit_mem') {
      const editData = { ...event.row };
      ['id', 'categoryId', 'casteId', 'designationId', 'boothIdMem'].forEach(key => {
        if (editData[key]) editData[key] = String(editData[key]);
      });
      this.updateDropdownOptions();
      this.memberModal.openModal(editData);
    } else if (event.action.id === 'delete_mem') {
      this.crudHandler.handleRequest(
        this.boothSamitiService.deleteBoothSamitiMem(event.row.id),
        'Deleted',
        'Member removed successfully!',
        () => this.loadMembers(),
        true,
        ModulePermission.BoothSamiti
      );
    }
  }

  handleMemberSubmit(result: FormResult) {
    if (!result.status) return;

    const submitData = {
      ...result.data,
      id: result.data.id ? Number(result.data.id) : undefined,
      categoryId: Number(result.data.categoryId),
      casteId: Number(result.data.casteId),
      age: Number(result.data.age),
      designationId: Number(result.data.designationId),
      boothIdMem: Number(result.data.boothIdMem)
    };

    const request = submitData.id ?
      this.boothSamitiService.updateBoothSamitiMem(submitData) :
      this.boothSamitiService.createBoothSamitiMem(submitData);

    this.crudHandler.handleRequest(
      request,
      submitData.id ? 'Updated' : 'Created',
      `Member ${submitData.id ? 'updated' : 'added'} successfully!`,
      () => this.loadMembers(),
      true,
      ModulePermission.BoothSamiti
    );
  }

  handleFormSubmit(result: FormResult) {
    if (!result.status) return;

    const raw = result.data;
    let submitData: any;

    if (raw.id) {
      // Update logic
      submitData = {
        ...raw,
        id: Number(raw.id),
        categoryId: Number(raw.categoryId),
        casteId: Number(raw.casteId),
        age: Number(raw.age),
        designationId: Number(raw.designationId)
      };
    } else {
      // Create logic - ONLY send boothId as requested
      const bId = Number(raw.boothId);
      if (this.existingBoothIds.has(bId)) {
        this.toastService.showError('Duplicate', 'A samiti for this booth already exists!');
        return;
      }
      submitData = {
        boothId: bId
      };
    }

    const request = raw.id ?
      this.boothSamitiService.updateBoothSamiti(submitData) :
      this.boothSamitiService.createBoothSamiti(submitData);

    this.crudHandler.handleRequest(
      request,
      submitData.id ? 'Updated' : 'Created',
      `Member ${submitData.id ? 'updated' : 'added'} successfully!`,
      () => {
        this.loadMembers();
        this.loadExistingSamitis();
      },
      true,
      ModulePermission.BoothSamiti
    );
  }

  handlePageChange(event: any) {
    this.pageNumber = event.currentPage;
    this.pageSize = event.pageSize;
    this.loadMembers();
  }

  handleSortChange(event: any) {
    this.sortBy = event.sortBy;
    this.isDescending = event.isDescending;
    this.loadMembers();
  }

  handleSearchChange(term: string) {
    this.searchTerm = term;
    this.pageNumber = 1;
    this.loadMembers();
  }

  handleExport(format: string) {
    if (!format) return;
    this.isExporting = true;

    // Determine entity and parameters
    // isMemberView: exports members of a specific booth
    // !isMemberView (ListView or Management): exports the list of samitis/booths
    const entity = this.isMemberView ? 'boothsamitimembers' : 'boothsamiti';
    const params: any = {
      UserId: this.authService.getUserId()
    };

    if (this.isMemberView && this.selectedBoothId) {
      params.BoothMemId = this.selectedBoothId;
    }

    // Use the generic export service from BaseApiService
    this.boothSamitiService.export(entity, format as 'pdf' | 'excel', params).subscribe({
      next: (blob: Blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        
        let fileName = 'Booth_Samiti_List';
        if (this.isMemberView) {
          fileName = this.selectedBoothName ? `${this.selectedBoothName}_Members` : 'Booth_Members';
        }

        a.download = `${fileName}.${format === 'excel' ? 'xlsx' : 'pdf'}`;
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
        document.body.removeChild(a);

        this.isExporting = false;
        this.toastService.showSuccess('Success', `List exported to ${format.toUpperCase()} successfully!`);
      },
      error: (err) => {
        console.error(`Error exporting to ${format}:`, err);
        this.toastService.showError('Error', `Failed to export list to ${format.toUpperCase()}`);
        this.isExporting = false;
      }
    });
  }
}
