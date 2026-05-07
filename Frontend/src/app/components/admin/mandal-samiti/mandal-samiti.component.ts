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
        label: item.designationName,
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
      { id: 'mandalIdMem', name: 'mandalIdMem', label: 'Mandal ID', type: 'hidden' },
      { id: 'name', name: 'name', label: 'Full Name', type: 'text', placeholder: 'Enter name', validations: [Validators.required], gridColSpan: 6 },
      { id: 'contact', name: 'contact', label: 'Contact', type: 'text', placeholder: 'Enter phone', validations: [Validators.required], gridColSpan: 6 },
      { id: 'age', name: 'age', label: 'Age', type: 'number', placeholder: 'Enter age', validations: [Validators.required], gridColSpan: 6 },
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
        apiUrl: () => 'mandal/getAll',
        apiMapper: (data: any) => {
          const list = data.data || data || [];
          return list.map((item: any) => ({ value: String(item.id), label: item.name }));
        },
        validations: [Validators.required],
        gridColSpan: 12
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

    this.boothSamitiService.getDesignations().subscribe(res => {
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
    // Mocking load for now as per user request for "only frontend design"
    setTimeout(() => {
      this.memberList = []; // Start with empty list
      this.loading = false;
    }, 500);
  }

  handleAction(event: { action: TableAction, row: any }) {
    if (event.action.id === 'view') {
      this.isMemberView = true;
      this.updateTableConfig();
      this.selectedMandalId = event.row.id;
      this.selectedMandalName = event.row.mandalName;
      this.loadMembers();
    } else if (event.action.id === 'add_samiti') {
      this.memberModal.openModal({ mandalIdMem: event.row.id });
    }
  }

  handleMemberSubmit(result: FormResult) {
    if (!result.status) return;
    this.toastService.showSuccess('Success', 'Member updated successfully (Mock)');
  }

  handleFormSubmit(result: FormResult) {
    if (!result.status) return;
    this.toastService.showSuccess('Success', 'Samiti created successfully (Mock)');
  }

  handlePageChange(event: any) { this.loadMembers(); }
  handleSortChange(event: any) { this.loadMembers(); }
  handleSearchChange(term: string) { this.loadMembers(); }
  handleExport(format: string) {
    this.toastService.showSuccess('Export Started', `Successfully generated ${format.toUpperCase()} export!`);
  }
}
