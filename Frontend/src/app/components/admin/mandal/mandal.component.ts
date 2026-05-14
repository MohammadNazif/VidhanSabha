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


import { GenericExportComponent } from '../../shared/generic-export/generic-export.component';

@Component({
  selector: 'app-mandal',
  standalone: true,
  imports: [CommonModule, PageHeaderComponent, GenericTableComponent, GenericModalButtonComponent, GenericExportComponent],
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
  isExporting = false;

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
        gridColSpan: 6
      },
      {
        id: 'isMandalSanyojak',
        name: 'isMandalSanyojak',
        label: 'Has Mandal Sanyojak?',
        type: 'select',
        placeholder: '-- Select Yes/No --',
        options: [
          { label: 'Yes', value: 'Yes' },
          { label: 'No', value: 'No' }
        ],
        validations: [Validators.required],
        gridColSpan: 6
      },
      // Sanyojak Fields
      {
        id: 'inchargeName',
        name: 'inchargeName',
        label: 'Sanyojak Name',
        type: 'text',
        placeholder: 'Enter sanyojak name',
        visibleIf: { field: 'isMandalSanyojak', operator: '==', value: 'Yes' },
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'age',
        name: 'age',
        label: 'Age',
        type: 'number',
        placeholder: 'Enter age',
        visibleIf: { field: 'isMandalSanyojak', operator: '==', value: 'Yes' },
        validations: [Validators.required, Validators.min(18)],
        gridColSpan: 6
      },
      {
        id: 'fatherName',
        name: 'fatherName',
        label: 'Father Name',
        type: 'text',
        placeholder: 'Enter father name',
        visibleIf: { field: 'isMandalSanyojak', operator: '==', value: 'Yes' },
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'categoryId',
        name: 'categoryId',
        label: 'Category',
        type: 'select',
        placeholder: '-- Select Category --',
        apiUrl: 'common/category',
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data?.items) ? data.data.items : (Array.isArray(data?.items) ? data.items : (Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : [])));
          return list.map((item: any) => ({
            value: String(item.id),
            label: item.name || item.category
          }));
        },
        visibleIf: { field: 'isMandalSanyojak', operator: '==', value: 'Yes' },
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'castId',
        name: 'castId',
        label: 'Caste',
        type: 'select',
        placeholder: '-- Select Caste --',
        dependsOn: 'categoryId',
        apiUrl: (catId: string) => `common/cast?id=${catId}`,
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data?.items) ? data.data.items : (Array.isArray(data?.items) ? data.items : (Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : [])));
          return list.map((item: any) => ({
            value: String(item.id),
            label: item.name || item.castName
          }));
        },
        visibleIf: { field: 'isMandalSanyojak', operator: '==', value: 'Yes' },
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'phoneNumber',
        name: 'phoneNumber',
        label: 'Phone Number',
        type: 'text',
        placeholder: 'Enter phone number',
        visibleIf: { field: 'isMandalSanyojak', operator: '==', value: 'Yes' },
        validations: [Validators.required, Validators.pattern('^[0-9]{10}$')],
        gridColSpan: 6
      },
      {
        id: 'educationLevel',
        name: 'educationLevel',
        label: 'Education Level',
        type: 'text',
        placeholder: 'Enter education level',
        visibleIf: { field: 'isMandalSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 6
      },
      {
        id: 'address',
        name: 'address',
        label: 'Address',
        type: 'textarea',
        placeholder: 'Enter address',
        visibleIf: { field: 'isMandalSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 12
      },
      {
        id: 'profileImage',
        name: 'profileImage',
        label: 'Profile Image',
        type: 'file',
        visibleIf: { field: 'isMandalSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 12
      }
    ]
  };

  mandalList: any[] = []; // Will be populated from API

  columns: TableColumn[] = [
    { key: 'profile', label: 'Profile', type: 'avatar' },
    { key: 'name', label: 'Mandal Name', sortable: true },
    { key: 'mandalSanyojak', label: 'Mandal Adhyakshya', sortable: true },
    { key: 'fatherName', label: 'Father Name', sortable: true },
    { key: 'castName', label: 'Cast', sortable: true },
    { key: 'contact', label: 'Contact', sortable: true }


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
      // Robust mapping for both flat and nested structures
      const editData: any = {
        ...row,
        vidhanId: String(row.vidhanId || 0),
        isMandalSanyojak: row.isMandalSanyojak ? 'Yes' : 'No',
        // Flat mapping
        inchargeName: row.mandalSanyojak || '',
        phoneNumber: row.contact || '',
        categoryId: row.categoryId ? String(row.categoryId) : '',
        castId: row.castId ? String(row.castId) : '',
        fatherName: row.fatherName || '',
        age: row.age || null,
        educationLevel: row.education || row.educationLevel || '',
        address: row.address || ''
      };

      // Sub-object fallback
      if (row.sanyojak) {
        editData.inchargeName = editData.inchargeName || row.sanyojak.inchargeName;
        editData.age = editData.age || row.sanyojak.age;
        editData.fatherName = editData.fatherName || row.sanyojak.fatherName;
        editData.categoryId = editData.categoryId || (row.sanyojak.categoryId ? String(row.sanyojak.categoryId) : '');
        editData.castId = editData.castId || (row.sanyojak.castId ? String(row.sanyojak.castId) : '');
        editData.educationLevel = editData.educationLevel || row.sanyojak.educationLevel;
        editData.phoneNumber = editData.phoneNumber || row.sanyojak.phoneNumber;
        editData.address = editData.address || row.sanyojak.address;
      }

      this.mandalModal.openModal(editData);
    } else {
      this.toastService.showWarning('Action Selected', `Action ${action.id} clicked for ${row.name || 'this item'}`);
    }
  }

  handleFormSubmit(result: FormResult) {
    if (!result.status) return;

    const raw = result.data;
    const isSanyojak = raw.isMandalSanyojak === 'Yes';
    const isUpdate = !!(raw.id || (this.mandalModal.initialData && this.mandalModal.initialData.id));

    const formData = new FormData();

    // Mandal Base Data
    formData.append('Name', raw.name);
    formData.append('IsMandalSanyojak', String(isSanyojak));
    formData.append('VidhanId', String(raw.vidhanId || 0));

    const userId = this.authService.getUserId();
    if (userId) formData.append('UserId', userId);

    if (isUpdate) {
      formData.append('Id', String(raw.id || this.mandalModal.initialData.id));
    }

    // Sanyojak Data (Nested structure using dot notation for FormData)
    if (isSanyojak) {
      formData.append('Sanyojak.InchargeName', raw.inchargeName || '');
      formData.append('Sanyojak.Age', String(raw.age || 0));
      formData.append('Sanyojak.FatherName', raw.fatherName || '');
      formData.append('Sanyojak.CategoryId', String(raw.categoryId || 0));
      formData.append('Sanyojak.CastId', String(raw.castId || 0));
      formData.append('Sanyojak.EducationLevel', raw.educationLevel || '');
      formData.append('Sanyojak.PhoneNumber', raw.phoneNumber || '');
      formData.append('Sanyojak.Address', raw.address || '');

      if (result.files && result.files['profileImage']) {
        formData.append('Sanyojak.ProfileImagePath', result.files['profileImage']);
      }
    }

    const request = isUpdate
      ? this.mandalService.updateMandal(formData)
      : this.mandalService.createMandal(formData);

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
    if (format !== 'excel') {
      this.toastService.showWarning('Format Not Supported', 'Only Excel export is supported for Mandal list.');
      return;
    }

    this.isExporting = true;
    const params = {
      searchTerm: this.searchTerm,
      sortBy: this.sortBy,
      isDescending: this.isDescending
    };

    this.mandalService.exportMandalExcel(params).subscribe({
      next: (blob: Blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `Mandal_List_${new Date().getTime()}.xlsx`;
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
        document.body.removeChild(a);
        this.isExporting = false;
        this.toastService.showSuccess('Export Successful', 'Mandal list has been exported to Excel.');
      },
      error: (err) => {
        console.error('Export error:', err);
        this.isExporting = false;
        this.toastService.showError('Export Failed', 'Failed to generate Excel export for Mandal list.');
      }
    });
  }
}
