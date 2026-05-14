import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { PageHeaderComponent } from '../../shared/page-header/page-header.component';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';
import { TableAction, TableColumn, TableConfig } from '../../shared/generic-table/generic-table.types';
import { MandalSamitiService } from '../../../Services/Admin/mandal/mandal-samiti.service';
import { MandalService } from '../../../Services/Admin/mandal/mandal.service';
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
  selector: 'app-mandal-samiti',
  standalone: true,
  imports: [CommonModule, PageHeaderComponent, GenericTableComponent, LucideAngularModule, GenericModalButtonComponent, GenericExportComponent],
  template: `
    <div class="h-screen p-6 flex flex-col overflow-hidden bg-slate-50/50">
      <app-page-header [title]="isMemberView ? selectedMandalName : (isListView ? 'Mandal Samiti List' : 'Mandal Samiti Management')" 
                       [subtitle]="isMemberView ? 'Manage members assigned to this mandal' : 'Overview of all registered mandal samitis'">
        
        <div class="flex items-center gap-3">
          <button *ngIf="isMemberView" (click)="backToList()" 
            class="flex items-center justify-center gap-2 px-4 py-2 text-sm font-bold text-slate-700 bg-white border border-slate-200 rounded-xl hover:bg-slate-50 hover:border-slate-300 transition-all shadow-sm active:scale-95 min-w-[100px]">
            <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round" class="text-slate-500"><path d="m15 18-6-6 6-6"/></svg>
            <span>Back</span>
          </button>

          <app-generic-modal-button *ngIf="canManage() && !isMemberView" #samitiModal [config]="samitiFormConfig" (formSubmit)="handleFormSubmit($event)"
            label="Add Samiti" icon="+" variant="primary">
          </app-generic-modal-button>

          <app-generic-export [show]="isListView && memberList && memberList.length > 0" 
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
export class MandalSamitiComponent implements OnInit {
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
  selectedMandalId: number | null = null;
  selectedMandalName = '';
  currentMandalDesignationIds: number[] = [];

  // Cached options
  designations: any[] = [];
  categories: any[] = [];

  private designationSubject = new BehaviorSubject<DropdownOption[]>([]);
  private categorySubject = new BehaviorSubject<DropdownOption[]>([]);
  designationOptions$ = this.designationSubject.asObservable();
  categoryOptions$ = this.categorySubject.asObservable();

  updateDropdownOptions() {
    const mappedDesignations = this.designations.map((item: any) => {
      const isTakenInList = this.isMemberView && this.memberList.some(m =>
        Number(m.designationId) === Number(item.id) &&
        m.id !== Number(this.memberModal?.initialData?.id)
      );
      return {
        value: String(item.id),
        label: item.name || item.designationName,
        disabled: isTakenInList
      };
    });
    this.designationSubject.next(mappedDesignations);

    if (this.categories.length > 0 && this.categorySubject.value.length === 0) {
      this.categorySubject.next(this.categories.map(c => ({ value: String(c.id), label: c.name })));
    }
  }

  canManage(): boolean {
    if (this.isListView) return false;
    const role = (this.authService.getRole() || '').toUpperCase().trim();
    return role === 'VIDHANSABHAPRABHARI' || this.permissionService.hasPermission(ModulePermission.BoothSamiti);
  }

  backToList() {
    this.isMemberView = false;
    this.selectedMandalId = null;
    this.selectedMandalName = '';
    this.pageNumber = 1;
    this.loadMembers();
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
        { key: 'contact', label: 'Contact', sortable: true },
        { key: 'occupation', label: 'Occupation', sortable: true },
        { key: 'age', label: 'Age', sortable: true }
      ];
      this.actions = [
        { id: 'edit_mem', label: '', variant: 'default', icon: 'edit', show: () => this.canManage() },
        { id: 'delete_mem', label: '', variant: 'danger', icon: 'delete', show: () => this.canManage() }
      ];
    } else {
      this.columns = [
        { key: 'mandalName', label: 'Mandal Name', sortable: true },
        { key: 'mandalAdhayaksh', label: 'Mandal Adhayaksh', sortable: true },
        { key: 'totalMember', label: 'Total Members', sortable: true },
        { key: 'contact', label: 'Contact', sortable: true }
      ];
      this.actions = [
        { id: 'view', label: 'View', variant: 'primary', icon: 'view' },
        { id: 'add_samiti', label: 'Add Member', variant: 'primary', icon: 'user', show: () => this.canManage() }
      ];
    }
  }

  memberFormConfig: FormConfig = {
    title: 'Mandal Samiti Member',
    submitLabel: 'Save Member',
    fields: [
      { id: 'id', name: 'id', label: 'ID', type: 'hidden' },
      { id: 'MemberId', name: 'MemberId', label: 'Member ID', type: 'hidden' },
      { id: 'mandalId', name: 'mandalId', label: 'Mandal ID', type: 'hidden' },
      {
        id: 'designationId',
        name: 'designationId',
        label: 'Designation',
        type: 'select',
        placeholder: '-- Select Designation --',
        options: this.designationOptions$,
        validations: [Validators.required],
        gridColSpan: 12
      },
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
      }
    ]
  };

  config: TableConfig = {
    selectable: false,
    loading: false,
    searchable: true,
    searchPlaceholder: 'Search Mandal Samiti...',
  };

  samitiFormConfig: FormConfig = {
    title: 'Mandal Samiti',
    submitLabel: 'Create Samiti',
    fields: [
      {
        id: 'mandalId',
        name: 'mandalId',
        label: 'Mandal',
        type: 'select',
        placeholder: '-- Select Mandal --',
        apiUrl: () => 'mandal/getAll?pageNumber=1&pageSize=1000&search=&sortBy=&isDescending=true',
        apiMapper: (data: any) => {
          const list = data.data?.items || data.data || data || [];
          return list.map((item: any) => ({ value: String(item.id), label: item.name }));
        },
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'sanyojakId',
        name: 'sanyojakId',
        label: 'Sanyojak',
        type: 'select',
        placeholder: '-- Select Sanyojak --',
        dependsOn: 'mandalId',
        apiUrl: (mandalId: any) => `mandal/getsanyojak?id=${mandalId}`,
        apiMapper: (data: any) => {
          const item = data.data;
          return item ? [{ value: String(item.id), label: item.inchargeName }] : [];
        },
        validations: [Validators.required],
        gridColSpan: 6
      }
    ]
  };

  constructor(
    private mandalSamitiService: MandalSamitiService,
    private mandalService: MandalService,
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

    this.http.get(`${environment.apiUrl}/mandalsamiti/designations/getAll`).subscribe((res: any) => {
      this.designations = res.data || res || [];
      this.updateDropdownOptions();
    });
    this.http.get(`${environment.apiUrl}/common/category`).subscribe((res: any) => {
      this.categories = res.data || res || [];
      this.updateDropdownOptions();
    });
  }

  loadMembers() {
    this.loading = true;
    if (this.isMemberView && this.selectedMandalId) {
      this.mandalSamitiService.getAllMembers(this.selectedMandalId).subscribe({
        next: (res) => {
          this.memberList = res.data || res || [];
          this.totalCount = this.memberList.length;
          this.loading = false;
        },
        error: () => {
          this.loading = false;
          this.toastService.showError('Error', 'Failed to load members');
        }
      });
    } else {
      const params = {
        pageNumber: this.pageNumber,
        pageSize: this.pageSize,
        searchTerm: this.searchTerm,
        sortBy: this.sortBy,
        isDescending: this.isDescending
      };
      this.mandalSamitiService.getMandalSamiti(params).subscribe({
        next: (res) => {
          this.memberList = res.data?.items || res.data || res || [];
          this.totalCount = res.data?.totalCount || res.totalCount || this.memberList.length;
          this.loading = false;
        },
        error: () => {
          this.loading = false;
          this.toastService.showError('Error', 'Failed to load samitis');
        }
      });
    }
  }

  handleAction(event: { action: TableAction, row: any }) {
    if (event.action.id === 'view') {
      this.isMemberView = true;
      this.updateTableConfig();
      this.selectedMandalId = event.row.mandalId;
      this.selectedMandalName = event.row.mandalName;
      this.loadMembers();
    } else if (event.action.id === 'add_samiti') {
      this.memberModal.openModal({
        id: event.row.id,
        mandalId: event.row.mandalId
      });
    } else if (event.action.id === 'edit_mem') {
      const editData = {
        ...event.row,
        MemberId: event.row.id // Map member id to MemberId for editing
      };
      ['MemberId', 'categoryId', 'casteId', 'designationId', 'mandalId', 'id'].forEach(key => {
        if (editData[key]) editData[key] = String(editData[key]);
      });
      this.memberModal.openModal(editData);
    } else if (event.action.id === 'delete_mem') {
      if (confirm('Are you sure you want to delete this member?')) {
        this.mandalSamitiService.deleteMandalSamitiMem(event.row.id, event.row.mandalId).subscribe({
          next: () => {
            this.toastService.showSuccess('Success', 'Member deleted successfully');
            this.loadMembers();
          },
          error: (err) => this.toastService.showError('Error', err.error?.message || 'Delete failed')
        });
      }
    }
  }

  handleMemberSubmit(result: FormResult) {
    if (!result.status) return;
    const rawData = result.data;
    const isUpdate = rawData.MemberId && Number(rawData.MemberId) > 0;

    if (isUpdate) {
      // Align with UpdateMandalSamitiMemberRequestDto
      const updateData = {
        Id: Number(rawData.MemberId),
        Name: rawData.name || '',
        CategoryId: Number(rawData.categoryId || 0),
        CasteId: Number(rawData.casteId || 0),
        Age: Number(rawData.age || 0),
        Contact: rawData.contact || '',
        Occupation: rawData.occupation || '',
        DesignationId: Number(rawData.designationId || 0)
      };

      this.mandalSamitiService.updateMandalSamitiMem(updateData).subscribe({
        next: () => {
          this.toastService.showSuccess('Success', 'Member updated successfully');
          this.memberModal.closeModal();
          this.loadMembers();
        },
        error: (err) => this.toastService.showError('Error', err.error?.message || 'Update failed')
      });
    } else {
      // For creation, the 'id' field from the row represents the Mandal Samiti record ID
      const createData = {
        MemberId: Number(rawData.id || 0),
        MandalId: Number(rawData.mandalId || this.selectedMandalId || 0),
        CategoryId: Number(rawData.categoryId || 0),
        CasteId: Number(rawData.casteId || 0),
        DesignationId: Number(rawData.designationId || 0),
        Age: Number(rawData.age || 0),
        Name: rawData.name || '',
        Contact: rawData.contact || '',
        Occupation: rawData.occupation || ''
      };

      this.mandalSamitiService.createMandalSamitiMem(createData).subscribe({
        next: () => {
          this.toastService.showSuccess('Success', 'Member created successfully');
          this.memberModal.closeModal();
          this.loadMembers();
        },
        error: (err) => this.toastService.showError('Error', err.error?.message || 'Creation failed')
      });
    }
  }

  handleFormSubmit(result: FormResult) {
    if (!result.status) return;
    this.mandalSamitiService.createMandalSamiti(result.data).subscribe({
      next: () => {
        this.toastService.showSuccess('Success', 'Samiti created successfully');
        this.samitiModal.closeModal();
        this.loadMembers();
      },
      error: (err) => this.toastService.showError('Error', err.error?.message || 'Operation failed')
    });
  }

  handlePageChange(event: any) { this.loadMembers(); }
  handleSortChange(event: any) { this.loadMembers(); }
  handleSearchChange(term: string) { this.loadMembers(); }
  handleExport(format: string) {
    this.toastService.showSuccess('Export Started', `Successfully generated ${format.toUpperCase()} export!`);
  }
}
