import { Component } from '@angular/core';
import { TableAction, TableColumn, TableConfig } from '../../shared/generic-table/generic-table.types';
import { Validators } from '@angular/forms';
import { FormConfig, FormResult, FormField } from '../../shared/generic-modal-form/generic-form.types';
import { CommonModule } from '@angular/common';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';
import { GenericModalButtonComponent } from '../../shared/generic-modal-form/generic-modal-button.component';

import { PageHeaderComponent } from '../../shared/page-header/page-header.component';
import { ToastService } from '../../../Services/common/toast/toast.service';
import { SectorService } from '../../../Services/Admin/sector/sector.service';
import { StateService } from '../../../Services/Admin/state/state.service';
import { AuthServiceService } from '../../../Services/Auth/auth.service';
import { CrudHandlerService } from '../../../Services/common/crud-handler.service';
import { ViewChild, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { GenericExportComponent } from '../../shared/generic-export/generic-export.component';

@Component({
  selector: 'app-sector',
  standalone: true,
  imports: [CommonModule, GenericTableComponent, GenericModalButtonComponent, PageHeaderComponent, GenericExportComponent],
  templateUrl: './sector.component.html',
  styleUrl: './sector.component.css'
})
export class SectorComponent implements OnInit {
  @ViewChild('sectorModal') sectorModal!: GenericModalButtonComponent;

  sectorList: any[] = [];
  totalCount = 0;
  loading = false;
  isExporting = false;

  // Server-side state
  pageNumber = 1;
  pageSize = 50;
  searchTerm = '';
  sortBy = '';
  isDescending = false;

  defaultStateId: string | null = null;

  isStatePrabhari(): boolean {
    return (this.authService.getRole() || '').toUpperCase().trim() === 'STATEPRABHARI';
  }

  columns: TableColumn[] = [
    { key: 'profile', label: 'Profile', type: 'avatar', align: 'center', sortable: false, avatarFallbackKey: 'inchargeName' },
    { key: 'mandalName', label: 'Mandal', sortable: true },
    {
      key: 'villageName',
      label: 'Village',
      sortable: true,
      formatter: (val: any, row: any) => {
        if (row.villages && Array.isArray(row.villages)) {
          return row.villages.map((v: any) => v.villageName || v.name).join(', ');
        }
        return val || 'N/A';
      }
    },
    { key: 'sectorName', label: 'Sector', sortable: true },
    { key: 'inchargeName', label: 'Sector Sanyojak', sortable: true },
    { key: 'phoneNumber', label: 'Contact', sortable: true },
    // { key: 'profileImage', label: 'Profile Image', sortable: true, avatarFallbackKey: 'name' },
  ];

  config: TableConfig = {
    selectable: false,
    filterable: false,
    paginated: true,
    defaultPageSize: 50,
    pageSizeOptions: [10, 20, 50],
    searchable: true,
    searchPlaceholder: 'Search sectors...',
    showRowNumbers: true,
    striped: true,
    hoverable: true,
    serverSide: true
  };

  actions: TableAction[] = [
    { id: 'edit', label: '', variant: 'default', icon: 'edit', show: () => this.canManage() },
    { id: 'delete', label: '', variant: 'danger', icon: 'delete', show: () => this.canManage() }
  ];

  addSectorConfig: FormConfig = {
    title: 'Register New Sector',
    submitLabel: 'Register Sector',
    fields: [
      {
        id: 'mandalId',
        name: 'mandalId',
        label: 'Mandal',
        type: 'select',
        placeholder: '--Select Mandal--',
        apiUrl: `mandal/getAll?PageNumber=1&PageSize=50&IsDescending=false`,
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data?.items) ? data.data.items : (Array.isArray(data?.items) ? data.items : (Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : [])));
          return list.map((item: any) => ({
            value: String(item.id),
            label: item.name
          }));
        },
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'villageId',
        name: 'Village',
        label: 'Village',
        type: 'select',
        dependsOn: 'mandalId',
        placeholder: '--Select Village--',
        apiUrl: (mandalId: any) => `common/village?id=${mandalId}`,
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data?.items) ? data.data.items : (Array.isArray(data?.items) ? data.items : (Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : [])));
          return list.map((item: any) => ({
            value: String(item.id),
            label: item.name || item.villageName
          }));
        },
        validations: [Validators.required],
        gridColSpan: 6,
        multiple: true
      },
      {
        id: 'sectorName',
        name: 'sectorName',
        label: 'Sector Name',
        type: 'text',
        placeholder: 'Enter sector name',
        validations: [Validators.required],
        gridColSpan: 6
      },

      {
        id: 'isSectorSanyojak',
        name: 'isSectorSanyojak',
        label: 'Sector Sanyojak',
        type: 'select',
        placeholder: '-- Select Yes/No --',
        options: [
          { label: 'Yes', value: 'Yes' },
          { label: 'No', value: 'No' }
        ],
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'inchargeName',
        name: 'inchargeName',
        label: 'Sector Sanyojak Name',
        type: 'text',
        placeholder: 'Enter sector sanyojak full name',
        visibleIf: { field: 'isSectorSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 6
      },
      {
        id: 'age',
        name: 'age',
        label: 'Age',
        type: 'number',
        placeholder: 'Age',
        visibleIf: { field: 'isSectorSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 2
      },
      {
        id: 'fatherName',
        name: 'fatherName',
        label: 'Father Name',
        type: 'text',
        placeholder: "Enter father's full name",
        visibleIf: { field: 'isSectorSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 4
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
        visibleIf: { field: 'isSectorSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 6
      },
      {
        id: 'castId',
        name: 'castId',
        label: 'Caste',
        type: 'select',
        placeholder: '-- Select Caste --',
        dependsOn: 'categoryId',
        apiUrl: (catId: any) => `common/cast?id=${catId}`,
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data?.items) ? data.data.items : (Array.isArray(data?.items) ? data.items : (Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : [])));
          return list.map((item: any) => ({
            value: String(item.id),
            label: item.name || item.castName
          }));
        },
        visibleIf: { field: 'isSectorSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 6
      },
      {
        id: 'educationLevel',
        name: 'educationLevel',
        label: 'Education Level',
        type: 'text',
        placeholder: 'Enter highest education (e.g., B.A., 12th)',
        visibleIf: { field: 'isSectorSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 6
      },
      {
        id: 'phoneNumber',
        name: 'phoneNumber',
        label: 'Phone Number',
        type: 'text',
        placeholder: 'Enter 10-digit mobile number',
        validations: [Validators.pattern('^[0-9]{10}$')],
        visibleIf: { field: 'isSectorSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 6
      },
      {
        id: 'address',
        name: 'address',
        label: 'Address',
        type: 'textarea',
        placeholder: 'Enter full address with landmark',
        visibleIf: { field: 'isSectorSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 12
      },
      {
        id: 'profileImage',
        name: 'profileImage',
        label: 'Profile Image',
        type: 'file',
        visibleIf: { field: 'isSectorSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 12
      }
    ]
  };

  constructor(
    private sectorService: SectorService,
    private stateService: StateService,
    private authService: AuthServiceService,
    private crudHandler: CrudHandlerService,
    private toastService: ToastService,
    private route: ActivatedRoute
  ) { }

  isListView = false;

  canManage(): boolean {
    if (this.isListView) return false;
    const role = (this.authService.getRole() || '').toUpperCase().trim();
    return ['SUPERADMIN', 'ADMIN', 'VIDHANSABHAPRABHARI'].includes(role);
  }

  ngOnInit() {
    this.route.url.subscribe((url: any) => {
      const path = url[0]?.path || '';
      this.isListView = path.includes('-list');

      if (this.isStatePrabhari()) {
        const savedStateId = this.authService.getStateId();
        if (savedStateId) {
          this.defaultStateId = savedStateId;

          // Simplify fields for State Prabhari
          const districtField = this.addSectorConfig.fields.find(f => f.id === 'districtId');
          if (districtField) {
            delete (districtField as any).dependsOn;
            districtField.apiUrl = () => `district/getAll?stateId=${this.defaultStateId}`;
          }
          this.loadSectors();
        } else {
          // Fetch the assigned state ID
          this.stateService.getAllStates().subscribe({
            next: (response) => {
              const list = response?.data || response || [];
              if (list.length > 0) {
                this.defaultStateId = String(list[0].stateId || list[0].id);

                // Simplify fields for State Prabhari
                const districtField = this.addSectorConfig.fields.find(f => f.id === 'districtId');
                if (districtField) {
                  delete (districtField as any).dependsOn;
                  districtField.apiUrl = () => `district/getAll?stateId=${this.defaultStateId}`;
                }
                this.loadSectors();
              } else {
                this.loadSectors();
              }
            },
            error: () => this.loadSectors()
          });
        }
      } else {
        this.loadSectors();
      }
    });
  }

  loadSectors() {
    this.loading = true;
    const params: any = {
      PageNumber: this.pageNumber,
      PageSize: this.pageSize,
      SearchTerm: this.searchTerm,
      SortBy: this.sortBy,
      IsDescending: this.isDescending,
      roleFilterFlag: !this.isListView
    };

    this.sectorService.getAllSectors(params).subscribe({
      next: (response) => {
        const dataWrap = response.data;
        if (dataWrap && dataWrap.items) {
          this.sectorList = dataWrap.items;
          this.totalCount = dataWrap.totalCount || 0;
        } else {
          this.sectorList = Array.isArray(dataWrap) ? dataWrap : [];
          this.totalCount = this.sectorList.length;
        }
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading sectors:', err);
        this.loading = false;
      }
    });
  }

  handlePageChange(event: any) {
    this.pageNumber = event.currentPage;
    this.pageSize = event.pageSize;
    this.loadSectors();
  }

  handleSortChange(event: any) {
    this.sortBy = event.column;
    this.isDescending = event.direction === 'desc';
    this.pageNumber = 1;
    this.loadSectors();
  }

  handleSearchChange(term: string) {
    this.searchTerm = term;
    this.pageNumber = 1;
    this.loadSectors();
  }

  handleFormSubmit(result: FormResult) {
    if (!result.status) return;
    const raw = result.data;
    const isUpdate = !!(raw.id || (this.sectorModal.initialData && this.sectorModal.initialData.id));
    const isSanyojak = raw.isSectorSanyojak === 'Yes';

    const formData = new FormData();
    formData.append('MandalId', String(raw.mandalId));

    // Support multiple village IDs as per new DTO
    if (raw.villageId) {
      const vIds = Array.isArray(raw.villageId) ? raw.villageId : [raw.villageId];
      vIds.forEach((id: any) => {
        formData.append('VillageIds', String(id));
      });
    }

    formData.append('SectorName', raw.sectorName || "");
    formData.append('IsSectorSanyojak', String(isSanyojak));

    const userId = this.authService.getUserId();
    if (userId) {
      formData.append('userId', String(userId));
    }
    if (this.defaultStateId) {
      formData.append('stateId', String(this.defaultStateId));
    }

    if (isSanyojak) {
      formData.append('InchargeName', raw.inchargeName || "");
      formData.append('Age', String(raw.age || 0));
      formData.append('FatherName', raw.fatherName || "");
      formData.append('CategoryId', String(raw.categoryId || 0));
      formData.append('CastId', String(raw.castId || 0));
      formData.append('EducationLevel', raw.educationLevel || "");
      formData.append('PhoneNumber', raw.phoneNumber || "");
      formData.append('Address', raw.address || "");

      if (result.files && result.files['profileImage']) {
        formData.append('ProfileImage', result.files['profileImage']);
      }
    }

    if (isUpdate) {
      formData.append('Id', String(raw.id || this.sectorModal.initialData.id));
    }

    const request = isUpdate
      ? this.sectorService.updateSector(formData)
      : this.sectorService.createSector(formData);

    this.crudHandler.handleRequest(
      request,
      isUpdate ? 'Updated' : 'Registered',
      `Sector ${isUpdate ? 'updated' : 'registered'} successfully!`,
      () => this.loadSectors()
    );
  }

  handleAction(event: any) {
    const { action, row } = event;
    if (action.id === 'delete') {
      this.crudHandler.handleRequest(
        this.sectorService.deleteSector(row.id),
        'Deleted',
        'Sector deleted successfully!',
        () => this.loadSectors()
      );
    } else if (action.id === 'edit') {
      const editData = { ...row };

      // Map villages array to villageId field for the form
      if (row.villages && Array.isArray(row.villages)) {
        editData.villageId = row.villages.map((v: any) => String(v.villageId || v.id));
      }

      // Convert IDs to strings to ensure matching with dropdown values
      ['id', 'mandalId', 'categoryId', 'castId'].forEach(key => {
        if (editData[key]) {
          editData[key] = String(editData[key]);
        }
      });

      if (editData.isSectorSanyojak !== undefined) {
        editData.isSectorSanyojak = editData.isSectorSanyojak ? 'Yes' : 'No';
      }
      this.sectorModal.openModal(editData);
    } else {
      this.toastService.showWarning('Action Selected', `Action ${action.id} clicked for ${row.name || 'this item'}`);
    }
  }

  handleSelection(selected: any[]) {
    console.log('Selected rows:', selected);
  }

  handleExport(format: string) {
    if (!format) return;
    this.isExporting = true;
    
    const params: any = {
      SearchTerm: this.searchTerm,
      SortBy: this.sortBy,
      IsDescending: this.isDescending,
      roleFilterFlag: !this.isListView
    };

    if (this.defaultStateId) {
       params.stateId = this.defaultStateId;
    }

    this.sectorService.exportSector(format as 'excel' | 'pdf', params).subscribe({
      next: (blob: Blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `Sectors_${new Date().getTime()}.${format === 'excel' ? 'xlsx' : 'pdf'}`;
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        window.URL.revokeObjectURL(url);
        this.isExporting = false;
        this.toastService.showSuccess('Export Successful', `Successfully downloaded ${format.toUpperCase()} export!`);
      },
      error: (err) => {
        console.error('Export error:', err);
        this.isExporting = false;
        this.toastService.showError('Export Failed', 'Failed to generate export file.');
      }
    });
  }
}