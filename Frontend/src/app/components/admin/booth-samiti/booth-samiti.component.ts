import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageHeaderComponent } from '../../shared/page-header/page-header.component';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';
import { TableAction, TableColumn, TableConfig } from '../../shared/generic-table/generic-table.types';
import { BoothSamitiService } from '../../../Services/Admin/booth-voter/booth-samiti.service';
import { CrudHandlerService } from '../../../Services/common/crud-handler.service';
import { ModulePermission } from '../../../models/module-permission.enum';
import { LucideAngularModule } from 'lucide-angular';
import { ToastService } from '../../../Services/common/toast/toast.service';

import { GenericModalButtonComponent } from '../../shared/generic-modal-form/generic-modal-button.component';
import { ActivatedRoute } from '@angular/router';
import { FormConfig, FormResult } from '../../shared/generic-modal-form/generic-form.types';
import { Validators } from '@angular/forms';

@Component({
  selector: 'app-booth-samiti',
  standalone: true,
  imports: [CommonModule, PageHeaderComponent, GenericTableComponent, LucideAngularModule, GenericModalButtonComponent],
  template: `
    <div class="h-screen p-6 flex flex-col overflow-hidden bg-slate-50/50">
      <app-page-header title="Booth Samiti Members" subtitle="List of all registered booth committee members">
        <!-- Hidden modal for Editing, triggered via table actions -->
        <app-generic-modal-button *ngIf="canManage()" #samitiModal [config]="samitiFormConfig" (formSubmit)="handleFormSubmit($event)"
          label="" class="hidden">
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

  memberList: any[] = [];
  totalCount = 0;
  loading = false;

  pageNumber = 1;
  pageSize = 50;
  searchTerm = '';
  sortBy = '';
  isDescending = false;
  isListView = false;

  canManage(): boolean {
    return !this.isListView;
  }

  columns: TableColumn[] = [
    { key: 'name', label: 'Name', sortable: true },
    { key: 'contact', label: 'Contact', sortable: true },
    { key: 'age', label: 'Age', sortable: true },
    { key: 'designationName', label: 'Designation', sortable: true },
    { key: 'occupation', label: 'Occupation', sortable: true },
    { key: 'categoryName', label: 'Category', sortable: true },
    { key: 'casteName', label: 'Caste', sortable: true },
    { key: 'boothNumber', label: 'Booth No.', sortable: true }
  ];

  config: TableConfig = {
    selectable: false,
    loading: false,
    searchable: true,
    searchPlaceholder: 'Search...',
  };

  actions: TableAction[] = [
    { id: 'edit', label: '', variant: 'default', icon: 'edit', show: () => this.canManage() },
    { id: 'delete', label: '', variant: 'danger', icon: 'delete', show: () => this.canManage() },
    { id: 'add_samiti', label: 'Add', variant: 'primary', icon: 'users', show: () => this.canManage() }
  ];

  samitiFormConfig: FormConfig = {
    title: 'Booth Samiti Member',
    submitLabel: 'Save Member',
    fields: [
      { id: 'id', name: 'id', label: 'ID', type: 'hidden' },
      { id: 'boothId', name: 'boothId', label: 'Booth ID', type: 'hidden' },
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
        apiUrl: 'common/category',
        apiMapper: (data: any) => (data.data || data).map((item: any) => ({ value: String(item.id), label: item.name })),
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
        apiUrl: 'boothsamiti-designation/getAll',
        apiMapper: (data: any) => (data.data || data).map((item: any) => ({ value: String(item.id), label: item.designationName })),
        validations: [Validators.required],
        gridColSpan: 12
      }
    ]
  };

  constructor(
    private boothSamitiService: BoothSamitiService,
    private crudHandler: CrudHandlerService,
    private route: ActivatedRoute,
    private toastService: ToastService
  ) { }

  ngOnInit() {
    this.route.url.subscribe(url => {
      const path = url[0]?.path || '';
      this.isListView = path.includes('-list');
      this.loadMembers();
    });
  }

  loadMembers() {
    this.loading = true;
    const params = {
      PageNumber: this.pageNumber,
      PageSize: this.pageSize,
      SearchTerm: this.searchTerm,
      SortBy: this.sortBy || 'id',
      IsDescending: this.isDescending
    };

    this.boothSamitiService.getBoothSamiti(params).subscribe({
      next: (res) => {
        this.memberList = Array.isArray(res.data) ? res.data : [];
        this.totalCount = this.memberList.length;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  handleAction(event: { action: TableAction, row: any }) {
    if (event.action.id === 'delete') {
      this.crudHandler.handleRequest(
        this.boothSamitiService.deleteBoothSamiti(event.row.id),
        'Deleted',
        'Member removed successfully!',
        () => this.loadMembers(),
        true,
        ModulePermission.BoothSamiti
      );
    } else if (event.action.id === 'edit') {
      const editData = { ...event.row };
      ['id', 'boothId', 'categoryId', 'casteId', 'designationId'].forEach(key => {
        if (editData[key]) editData[key] = String(editData[key]);
      });
      this.samitiModal.openModal(editData);
    } else if (event.action.id === 'add_samiti') {
      this.samitiModal.openModal({
        boothId: String(event.row.boothId || '')
      });
    }
  }

  handleFormSubmit(result: FormResult) {
    if (!result.status) return;

    const raw = result.data;
    const submitData = {
      ...raw,
      id: raw.id ? Number(raw.id) : undefined,
      categoryId: Number(raw.categoryId),
      casteId: Number(raw.casteId),
      age: Number(raw.age),
      designationId: Number(raw.designationId)
    };

    const request = submitData.id ?
      this.boothSamitiService.updateBoothSamiti(submitData) :
      this.boothSamitiService.createBoothSamiti(submitData);

    this.crudHandler.handleRequest(
      request,
      submitData.id ? 'Updated' : 'Created',
      `Member ${submitData.id ? 'updated' : 'added'} successfully!`,
      () => this.loadMembers(),
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
    this.toastService.showSuccess('Export Started', `Successfully generated ${format.toUpperCase()} export!`);
  }
}
