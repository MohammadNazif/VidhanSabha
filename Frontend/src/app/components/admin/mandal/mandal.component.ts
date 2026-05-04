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
import { ActivatedRoute } from '@angular/router';


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
    private crudHandler: CrudHandlerService,
    private route: ActivatedRoute
  ) { }

  isListView = false;
  totalCount = 0;
  loading = false;

  canManage(): boolean {
    const role = (this.authService.getRole() || '').toUpperCase().trim();
    return !this.isListView && (role === 'VIDHANSABHAPRABHARI' || role === 'SUPERADMIN' || role === 'ADMIN');
  }

  // Server-side state
  pageNumber = 1;
  pageSize = 50;
  searchTerm = '';
  sortBy = '';
  isDescending = false;

  isStatePrabhari(): boolean {
    return (this.authService.getRole() || '').toUpperCase().trim() === 'STATEPRABHARI';
  }

  defaultStateId: string | null = null;

  ngOnInit() {
    this.route.url.subscribe((url: any) => {
      const path = url[0]?.path || '';
      this.isListView = path.includes('-list');
      this.loadMandals();
    });
  }

  loadMandals() {
    this.loading = true;
    const params = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      searchTerm: this.searchTerm,
      sortBy: this.sortBy,
      isDescending: this.isDescending
    };

    this.mandalService.getAllMandals(params).subscribe({
      next: (response) => {
        const dataWrap = response.data;
        if (dataWrap && dataWrap.items) {
          this.mandalList = dataWrap.items;
          this.totalCount = dataWrap.totalCount || 0;
        } else {
          this.mandalList = Array.isArray(dataWrap) ? dataWrap : [];
          this.totalCount = this.mandalList.length;
        }
        this.loading = false;
      },
      error: (err) => {
        console.error('Error fetching mandals:', err);
        this.loading = false;
      }
    });
  }

  handlePageChange(event: any) {
    this.pageNumber = event.currentPage;
    this.pageSize = event.pageSize;
    this.loadMandals();
  }

  handleSortChange(event: any) {
    this.sortBy = event.column;
    this.isDescending = event.direction === 'desc';
    this.pageNumber = 1;
    this.loadMandals();
  }

  handleSearchChange(term: string) {
    this.searchTerm = term;
    this.pageNumber = 1;
    this.loadMandals();
  }

  addMandalConfig: FormConfig = {
    title: 'Add New Mandal',
    submitLabel: 'Create Mandal',
    fields: [
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
    filterable: false,
    paginated: true,
    defaultPageSize: 50,
    pageSizeOptions: [10, 20, 50],
    searchable: true,
    searchPlaceholder: 'Search...',
    showRowNumbers: true,
    striped: true,
    hoverable: true,
    serverSide: true
  };

  actions: TableAction[] = [
    { id: 'edit', label: '', variant: 'default', icon: 'edit', show: () => this.canManage() },
    { id: 'delete', label: '', variant: 'danger', icon: 'delete', show: () => this.canManage() }
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
    const isUpdate = !!(raw.id || (this.mandalModal.initialData && this.mandalModal.initialData.id));

    const submitData: any = {
      VidhanId: 0, // Static VidhanId as requested
      Name: raw.name
    };

    if (isUpdate) {
      submitData.Id = Number(raw.id || this.mandalModal.initialData.id);
    }

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
