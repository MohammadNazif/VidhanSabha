import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Validators } from '@angular/forms';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';
import { TableAction, TableColumn, TableConfig } from '../../shared/generic-table/generic-table.types';
import { GenericModalButtonComponent } from '../../shared/generic-modal-form/generic-modal-button.component';
import { FormConfig, FormResult } from '../../shared/generic-modal-form/generic-form.types';

import { PageHeaderComponent } from '../../shared/page-header/page-header.component';
import { BoothService } from '../../../Services/Admin/booth/booth.service';
import { StateService } from '../../../Services/Admin/state/state.service';
import { AuthServiceService } from '../../../Services/Auth/auth.service';
import { ToastService } from '../../../Services/common/toast/toast.service';
import { CrudHandlerService } from '../../../Services/common/crud-handler.service';
import { ActivatedRoute } from '@angular/router';
import { ModulePermission } from '../../../models/module-permission.enum';
import { MandalService } from '../../../Services/Admin/mandal/mandal.service';
import { SectorService } from '../../../Services/Admin/sector/sector.service';


import { GenericExportComponent } from '../../shared/generic-export/generic-export.component';

@Component({
  selector: 'app-booth',
  standalone: true,
  imports: [CommonModule, PageHeaderComponent, GenericTableComponent, GenericModalButtonComponent, GenericExportComponent],
  templateUrl: './booth.component.html',
  styleUrl: './booth.component.css'
})
export class BoothComponent implements OnInit {
  @ViewChild('boothModal') boothModal!: GenericModalButtonComponent;

  constructor(
    private boothService: BoothService,
    private stateService: StateService,
    private authService: AuthServiceService,
    private toastService: ToastService,
    private crudHandler: CrudHandlerService,
    private route: ActivatedRoute,
    private mandalService: MandalService,
    private sectorService: SectorService
  ) { }

  isListView = false;
  totalCount = 0;
  loading = false;

  // Server-side state
  pageNumber = 1;
  pageSize = 10;
  searchTerm = '';
  sortBy = '';
  isDescending = true;
  mandalId: string | number | null = null;
  sectorId: string | number | null = null;

  canManage(): boolean {
    if (this.isListView) return false;
    // Admins can manage Master Data, but we can add role specific logic here if needed
    return true;
  }

  isStatePrabhari(): boolean {
    return (this.authService.getRole() || '').toUpperCase().trim() === 'STATEPRABHARI';
  }

  defaultStateId: string | null = null;

  ngOnInit() {
    this.route.url.subscribe((url: any) => {
      const path = url[0]?.path || '';
      this.isListView = path.includes('-list');
      this.config.filterable = this.isListView;
      this.loading = true;
      if (this.isListView) {
        this.loadFilterOptions();
      }
      this.loadBooths();
    });

    if (this.isStatePrabhari()) {
      // Fetch the assigned state ID
      this.stateService.getAllStates().subscribe({
        next: (response) => {
          const list = response?.data || response || [];
          if (list.length > 0) {
            this.defaultStateId = String(list[0].stateId || list[0].id);

            // Simplify fields for State Prabhari
            const districtField = this.addBoothConfig.fields.find(f => f.id === 'districtId');
            if (districtField) {
              delete (districtField as any).dependsOn;
              districtField.apiUrl = () => `district/getAll?stateId=${this.defaultStateId}`;
            }
            this.loadBooths();
          } else {
            this.loadBooths();
          }
        },
        error: () => this.loadBooths()
      });
    } else {
      this.loadBooths();
    }

  }

  loadFilterOptions() {
    // Load Mandals
    this.mandalService.getAllMandals({ pageSize: 500000 }).subscribe({
      next: (res) => {
        const list = res?.data?.items || res?.data || res || [];
        const options = list.map((m: any) => ({ label: m.name, value: m.id }));

        // Setup initial filters
        this.config = {
          ...this.config,
          filters: [
            { key: 'mandalId', label: 'Mandal', type: 'select', options: options, placeholder: '-- Select Mandal --', multiple: true },
            { key: 'sectorId', label: 'Sector', type: 'select', options: [], placeholder: '-- Select Sector --', multiple: true },
            { key: 'boothNumber', label: 'Booth No', type: 'text', placeholder: 'Enter Booth No...' }
          ]
        };
      }
    });
  }

  handleFilterChange(filterState: Record<string, any>) {
    // filterState for multiple selects is an array. Convert to comma separated string if backend expects string.
    // If backend expects numeric ID, and we pass array of IDs, it might fail if backend doesn't support array.
    // Assuming backend might take comma separated for mandalId/sectorId if we pass multiple. 
    // Wait, the API for booths takes mandalId as int usually? Let's check Booth API. 
    // Usually, list APIs support CSV or we send the first one. Let's send CSV.
    const mIds = filterState['mandalId'];
    if (Array.isArray(mIds)) {
      this.mandalId = mIds.length > 0 ? mIds.join(',') : null;
    } else {
      this.mandalId = mIds || null;
    }

    const sIds = filterState['sectorId'];
    if (Array.isArray(sIds)) {
      this.sectorId = sIds.length > 0 ? sIds.join(',') : null;
    } else {
      this.sectorId = sIds || null;
    }

    // Check if boothNumber/boothName search is needed
    const boothNoFilter = this.config.filters?.find(f => f.key === 'boothNumber');
    const boothNameFilter = this.config.filters?.find(f => f.key === 'boothName');

    if (boothNoFilter?.value) {
      this.searchTerm = String(boothNoFilter.value);
    } else if (boothNameFilter?.value) {
      this.searchTerm = String(boothNameFilter.value);
    } else {
      this.searchTerm = '';
    }

    // Load sectors if Mandal changed (fetch sectors for first selected mandal, or all if backend supports CSV)
    if (this.mandalId && this.config.filters) {
      const sectorFilter = this.config.filters.find(f => f.key === 'sectorId');
      if (sectorFilter) {
        // Sector API takes mandalId. We'll pass the first or CSV
        this.sectorService.getAllSectors({ mandalId: Array.isArray(mIds) ? mIds[0] : this.mandalId, pageSize: 500000 }).subscribe(res => {
          const list = res?.data?.items || res?.data || res || [];
          sectorFilter.options = list.map((s: any) => ({ label: s.name || s.sectorName, value: s.sectorId || s.id }));
        });
      }
    } else if (!this.mandalId && this.config.filters) {
      // Clear sector options
      const sectorFilter = this.config.filters.find(f => f.key === 'sectorId');
      if (sectorFilter) {
        sectorFilter.options = [];
        filterState['sectorId'] = null;
        this.sectorId = null;
        sectorFilter.value = null;
      }
    }

    this.pageNumber = 1;
    this.loading = true;
    this.loadBooths();
  }

  loadBooths() {
    this.loading = true;
    const params: any = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      searchTerm: this.searchTerm,
      sortBy: this.sortBy,
      isDescending: this.isDescending,
      mandalIds: this.mandalId,
      sectorIds: this.sectorId
    };

    const userId = this.authService.getUserId();
    if (userId) {
      params.userId = userId;
    }

    // Clean up empty params to prevent URL clutter (e.g. sectorId=)
    Object.keys(params).forEach(key => {
      if (params[key] === null || params[key] === undefined || params[key] === '') {
        delete params[key];
      }
    });

    this.boothService.getAllBooths(params).subscribe({
      next: (res: any) => {
        if (res.isSuccess && res.data) {
          this.boothList = res.data.items || [];
          this.totalCount = res.data.totalCount || 0;
        } else {
          this.boothList = Array.isArray(res?.data) ? res.data : [];
          this.totalCount = this.boothList.length;
        }
        this.loading = false;
      },
      error: (err) => {
        console.error('Error fetching booths:', err);
        this.toastService.showError('Error', 'Failed to load booths');
        this.loading = false;
      }
    });
  }

  addBoothConfig: FormConfig = {
    title: 'Add New Booth',
    submitLabel: 'Create Booth',
    fields: [
      {
        id: 'mandalId',
        name: 'mandalId',
        label: 'Mandal',
        type: 'select',
        placeholder: '--Select Mandal--',
        apiUrl: `mandal/getAll?PageNumber=1&PageSize=500000&IsDescending=false`,
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
        id: 'sectorId',
        name: 'sectorId',
        label: 'Sector',
        type: 'select',
        dependsOn: 'mandalId',
        placeholder: '--Select Sector--',
        apiUrl: (mandalId: any) => `sector/getByMandalId?id=${mandalId}&pageSize=1000`,
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data?.items) ? data.data.items : (Array.isArray(data?.items) ? data.items : (Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : [])));
          return list.map((item: any) => ({
            value: String(item.sectorId),
            label: item.sectorName || item.name
          }));
        },
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'villageId',
        name: 'villageId',
        label: 'Village',
        type: 'select',
        dependsOn: 'mandalId',
        placeholder: '--Select Village--',
        apiUrl: (mandalId: any) => `common/village?id=${mandalId}`,
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : []);
          return list.map((item: any) => ({
            value: String(item.id),
            label: item.name
          }));
        },
        validations: [Validators.required],
        gridColSpan: 12,
        multiple: true
      },
      {
        id: 'anshikData',
        name: 'anshikData',
        label: 'Village Selection Details',
        type: 'selection-table',
        dependsOn: 'villageId',
        gridColSpan: 12
      },
      {
        id: 'boothNumber',
        name: 'boothNumber',
        label: 'Booth Number',
        type: 'number',
        placeholder: 'Enter booth number',
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'pollingStationName',
        name: 'pollingStationName',
        label: 'Polling Station Name',
        type: 'text',
        placeholder: 'Enter polling station name',
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'pollingStationLocation',
        name: 'pollingStationLocation',
        label: 'Polling Station Location',
        type: 'text',
        placeholder: 'Enter polling station location',
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'isBoothSanyojak',
        name: 'isBoothSanyojak',
        label: 'Booth Sanyojak',
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
        label: 'Incharge Name',
        type: 'text',
        placeholder: 'Enter incharge full name',
        visibleIf: { field: 'isBoothSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 6
      },
      {
        id: 'age',
        name: 'age',
        label: 'Age',
        type: 'number',
        placeholder: 'Age',
        visibleIf: { field: 'isBoothSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 2
      },
      {
        id: 'fatherName',
        name: 'fatherName',
        label: 'Father Name',
        type: 'text',
        placeholder: "Enter father's full name",
        visibleIf: { field: 'isBoothSanyojak', operator: '==', value: 'Yes' },
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
          if (Array.isArray(data?.data)) {
            return data.data.map((item: any) => ({
              value: String(item.id),
              label: item.name
            }));
          }
          return [];
        },
        visibleIf: { field: 'isBoothSanyojak', operator: '==', value: 'Yes' },
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
          if (Array.isArray(data?.data)) {
            return data.data.map((item: any) => ({
              value: String(item.id),
              label: item.name
            }));
          }
          return [];
        },
        visibleIf: { field: 'isBoothSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 6
      },
      {
        id: 'educationLevel',
        name: 'educationLevel',
        label: 'Education Level',
        type: 'text',
        placeholder: 'Enter highest education (e.g., B.A., 12th)',
        visibleIf: { field: 'isBoothSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 6
      },
      {
        id: 'phoneNumber',
        name: 'phoneNumber',
        label: 'Phone Number',
        type: 'text',
        placeholder: 'Enter 10-digit mobile number',
        validations: [Validators.pattern('^[0-9]{10}$')],
        visibleIf: { field: 'isBoothSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 6
      },
      {
        id: 'address',
        name: 'address',
        label: 'Address',
        type: 'textarea',
        placeholder: 'Enter full address with landmark',
        visibleIf: { field: 'isBoothSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 12
      },
      {
        id: 'profileImage',
        name: 'profileImage',
        label: 'Profile Image',
        type: 'file',
        visibleIf: { field: 'isBoothSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 12
      }
    ]
  };

  boothList: any[] = [];

  columns: TableColumn[] = [
    { key: 'mandalName', label: 'Mandal', sortable: true },
    { key: 'sectorName', label: 'Sector', sortable: true },
    { key: 'boothNumber', label: 'Booth No.', sortable: true },
    {
      key: 'villageName',
      label: 'Village',
      sortable: true,
      formatter: (val: any, row: any) => {
        if (row.villages && Array.isArray(row.villages)) {
          return row.villages.map((v: any) => v.villageName).join(', ');
        }
        return val || 'N/A';
      }
    },
    { key: 'pollingStationName', label: 'Polling Station', sortable: true },
    {
      key: 'boothAathyaksh',
      label: 'Booth Aathyaksh',
      sortable: true,
      formatter: (val: any, row: any) => row.sanyojak?.inchargeName || 'N/A'
    },
    {
      key: 'contactNumber',
      label: 'Contact Number',
      sortable: true,
      formatter: (val: any, row: any) => row.sanyojak?.phoneNumber || 'N/A'
    },
    {
      key: 'castName',
      label: 'Cast',
      sortable: true,
      formatter: (val: any, row: any) => row.sanyojak?.castName || 'N/A'
    }
  ];

  config: TableConfig = {
    selectable: false,
    filterable: false,
    paginated: true,
    defaultPageSize: 10,
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
    if (action.id === 'delete') {
      this.crudHandler.handleRequest(
        this.boothService.deleteBooth(row.id),
        'Deleted',
        'Booth deleted successfully!',
        () => this.loadBooths(),
        true,
        ModulePermission.BoothVoterDescrition
      );
    } else if (action.id === 'edit') {
      // Flatten nested data for form editing
      const editData: any = {
        ...row,
        mandalId: String(row.mandalId),
        sectorId: String(row.sectorId),
        isBoothSanyojak: row.isBoothSanyojak ? 'Yes' : 'No'
      };

      // Flatten sanyojak fields
      if (row.sanyojak) {
        editData.inchargeName = row.sanyojak.inchargeName;
        editData.age = row.sanyojak.age;
        editData.fatherName = row.sanyojak.fatherName;
        editData.categoryId = String(row.sanyojak.categoryId);
        editData.castId = String(row.sanyojak.castId);
        editData.educationLevel = row.sanyojak.educationLevel;
        editData.phoneNumber = row.sanyojak.phoneNumber;
        editData.address = row.sanyojak.address;
      }

      // Transform villages for selection-table and multi-select
      if (row.villages && Array.isArray(row.villages)) {
        editData.villageId = row.villages.map((v: any) => String(v.villageId));
        editData.anshikData = row.villages.map((v: any) => ({
          id: String(v.villageId),
          name: v.villageName,
          anshik: v.hasAnshik ? 'Yes' : 'No'
        }));
      }

      this.boothModal.openModal(editData);
    }
  }

  handlePageChange(event: any) {
    this.pageNumber = event.currentPage;
    this.pageSize = event.pageSize;
    this.loadBooths();
  }

  handleSortChange(event: any) {
    this.sortBy = event.column;
    this.isDescending = event.direction === 'desc';
    this.pageNumber = 1; // Reset to first page
    this.loadBooths();
  }

  handleSearchChange(term: string) {
    this.searchTerm = term;
    this.pageNumber = 1; // Reset to first page
    this.loadBooths();
  }

  handleFormSubmit(result: FormResult) {
    if (!result.status) return;

    const raw = result.data;
    const isSanyojak = raw.isBoothSanyojak === 'Yes';
    const isUpdate = !!(raw.id || (this.boothModal.initialData && this.boothModal.initialData.id));

    const submitData: any = {
      MandalId: Number(raw.mandalId),
      SectorId: Number(raw.sectorId),
      Villages: [],
      BoothNumber: Number(raw.boothNumber),
      PollingStationName: raw.pollingStationName || "",
      PollingStationLocation: raw.pollingStationLocation || "",
      IsBoothSanyojak: isSanyojak,
      Sanyojak: isSanyojak ? {
        InchargeName: raw.inchargeName || "",
        Age: raw.age ? Number(raw.age) : 0,
        FatherName: raw.fatherName || "",
        CategoryId: raw.categoryId ? Number(raw.categoryId) : 0,
        CastId: raw.castId ? Number(raw.castId) : 0,
        EducationLevel: raw.educationLevel || "",
        PhoneNumber: raw.phoneNumber || "",
        Address: raw.address || ""
      } : null,
      userId: this.authService.getUserId(),
      stateId: this.defaultStateId ? Number(this.defaultStateId) : null
    };

    // Transform anshikData or villageId to Villages array
    if (raw.anshikData && Array.isArray(raw.anshikData)) {
      submitData.Villages = raw.anshikData.map((v: any) => ({
        VillageId: Number(v.id || v.villageId),
        HasAnshik: v.anshik === 'Yes'
      }));
    } else if (raw.villageId) {
      const ids = Array.isArray(raw.villageId) ? raw.villageId : [raw.villageId];
      submitData.Villages = ids.map((id: any) => ({
        VillageId: Number(id),
        HasAnshik: false
      }));
    }

    if (isUpdate) {
      submitData.Id = Number(raw.id || this.boothModal.initialData.id);
    }

    const request = isUpdate
      ? this.boothService.updateBooth(submitData)
      : this.boothService.createBooth(submitData);

    this.crudHandler.handleRequest(
      request,
      isUpdate ? 'Updated' : 'Success',
      `Booth ${isUpdate ? 'updated' : 'created'} successfully!`,
      () => this.loadBooths(),
      true,
      ModulePermission.BoothVoterDescrition
    );
  }

  handleSelection(selected: any[]) {
    console.log('Selected rows:', selected);
  }

  isExporting = false;

  handleExport(format: string) {
    if (!format || this.isExporting) return;

    this.isExporting = true;

    let fileName = `booths_${new Date().getTime()}`;
    let exportObs;

    if (format === 'excel') {
      exportObs = this.boothService.exportToExcel();
      fileName += '.xlsx';
    } else if (format === 'pdf') {
      exportObs = this.boothService.exportToPdf();
      fileName += '.pdf';
    } else {
      this.isExporting = false;
      this.toastService.showError('Error', 'Unsupported export format');
      return;
    }

    exportObs.subscribe({
      next: (blob: Blob) => {
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = fileName;
        link.click();
        window.URL.revokeObjectURL(url);
        this.isExporting = false;
        this.toastService.showSuccess('Export Success', `${format.toUpperCase()} file downloaded!`);
      },
      error: (err: any) => {
        console.error('Export error:', err);
        this.isExporting = false;
        this.toastService.showError('Export Failed', 'An error occurred while generating the file.');
      }
    });
  }
}
