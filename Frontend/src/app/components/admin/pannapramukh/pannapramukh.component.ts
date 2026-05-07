import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, Validators } from '@angular/forms';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';
import { TableAction, TableColumn, TableConfig } from '../../shared/generic-table/generic-table.types';
import { GenericModalButtonComponent } from '../../shared/generic-modal-form/generic-modal-button.component';
import { FormConfig, FormResult } from '../../shared/generic-modal-form/generic-form.types';
import { PageHeaderComponent } from '../../shared/page-header/page-header.component';
import { PannapramukhService } from '../../../Services/Admin/pannapramukh/pannapramukh.service';
import { ToastService } from '../../../Services/common/toast/toast.service';
import { CrudHandlerService } from '../../../Services/common/crud-handler.service';
import { AuthServiceService } from '../../../Services/Auth/auth.service';
import { ActivatedRoute } from '@angular/router';
import { ModulePermission } from '../../../models/module-permission.enum';
import { PermissionService } from '../../../Services/common/permission.service';

import { GenericExportComponent } from '../../shared/generic-export/generic-export.component';

@Component({
  selector: 'app-pannapramukh',
  standalone: true,
  imports: [CommonModule, GenericTableComponent, GenericModalButtonComponent, PageHeaderComponent, ReactiveFormsModule, GenericExportComponent],
  templateUrl: './pannapramukh.component.html',
  styleUrl: './pannapramukh.component.css'
})
export class PannapramukhComponent implements OnInit {
  @ViewChild('pannaModal') pannaModal!: GenericModalButtonComponent;

  pannaList: any[] = [];
  totalCount = 0;
  loading = false;
  isExporting = false;

  // Server-side state
  pageNumber = 1;
  pageSize = 50;
  searchTerm = '';
  sortBy = '';
  isDescending = false;

  isListView = false;

  columns: TableColumn[] = [
    { key: 'boothNumber', label: 'Booth No.', sortable: true },
    {
      key: 'villageName', label: 'Village', sortable: true, formatter: (val: any, row: any) => {
        if (row.villages && Array.isArray(row.villages)) {
          return row.villages.map((v: any) => v.villageName).join(', ');
        }
        return val || 'N/A';
      }
    },
    { key: 'pannaPramukhName', label: 'Panna Pramukh', sortable: true },
    { key: 'pannaNumber', label: 'Panna No.', sortable: true },
    { key: 'castName', label: 'Cast', sortable: true },
    { key: 'voterId', label: 'Voter ID', sortable: true },
    { key: 'address', label: 'Address', sortable: true },
    { key: 'phoneNumber', label: 'Phone Number', sortable: true }

  ];

  config: TableConfig = {
    searchable: true,
    searchPlaceholder: 'Search...',
    paginated: true,
    filterable: false,
    showRowNumbers: true,
    striped: true,
    hoverable: true,
    serverSide: true,
    defaultPageSize: 50
  };

  actions: TableAction[] = [
    { id: 'edit', label: '', variant: 'default', icon: 'edit', show: () => this.canManageVoters() },
    { id: 'delete', label: '', variant: 'danger', icon: 'delete', show: () => this.canManageVoters() }
  ];

  canManageVoters(): boolean {
    if (this.isListView) return false;
    const role = (this.authService.getRole() || '').toUpperCase().trim();
    if (role === 'VIDHANSABHAPRABHARI') return true;
    return this.permissionService.hasPermission(ModulePermission.PannaPramukh);
  }

  addPannaConfig: FormConfig = {
    title: 'Add Panna Pramukh',
    submitLabel: 'Save',
    fields: [
      {
        id: 'id',
        name: 'id',
        label: 'ID',
        type: 'hidden'
      },
      {
        id: 'boothId',
        name: 'boothId',
        label: 'Booth',
        type: 'select',
        placeholder: '--Select Booth--',
        apiUrl: () => {
          const role = (this.authService.getRole() || '').toUpperCase().trim();
          if (role === 'SECTORSANYOJAK') {
            return `booth/getAllBoothBySectorid`;
          }
          return 'common/boothNumber';
        },
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : []);
          return list.map((item: any) => ({
            value: String(item.boothId || item.id),
            label: `Booth No. ${item.boothNumber} - ${item.boothName || item.pollingStationName || ''}`
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
        placeholder: '--Select Village--',
        dependsOn: 'boothId',
        apiUrl: (boothId: string) => `common/villagesByBoothId?boothId=${boothId}`,
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
        id: 'pannaPramukhName',
        name: 'pannaPramukhName',
        label: 'Panna Pramukh',
        type: 'text',
        placeholder: 'Enter Panna Pramukh Name',
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'pannaNumber',
        name: 'pannaNumber',
        label: 'Panna Number From',
        type: 'number',
        placeholder: 'Enter panna number',
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'categoryId',
        name: 'categoryId',
        label: 'Category',
        type: 'select',
        placeholder: '--Select Category--',
        apiUrl: 'common/category',
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : []);
          return list.map((item: any) => ({
            value: String(item.id),
            label: item.name
          }));
        },
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'castId',
        name: 'castId',
        label: 'Cast',
        type: 'select',
        placeholder: '--Select Cast--',
        dependsOn: 'categoryId',
        apiUrl: (categoryId: string) => `common/cast?id=${categoryId}`,
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : []);
          return list.map((item: any) => ({
            value: String(item.id),
            label: item.name
          }));
        },
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'voterId',
        name: 'voterId',
        label: 'Voter ID',
        type: 'number',
        placeholder: 'Enter voter id',
        validations: [Validators.required],
        gridColSpan: 6
      },

      {
        id: 'phoneNumber',
        name: 'phoneNumber',
        label: 'Phone Number',
        type: 'text',
        placeholder: 'Enter phone number',
        validations: [Validators.required, Validators.pattern('^[0-9]{10}$')],
        gridColSpan: 6
      },
      {
        id: 'address',
        name: 'address',
        label: 'Address',
        type: 'textarea',
        placeholder: 'Enter address',
        validations: [Validators.required],
        gridColSpan: 12
      },
      {
        id: 'ProfilePicture',
        name: 'ProfilePicture',
        label: 'Profile Picture',
        type: 'file',
        placeholder: 'Upload profile picture',
        validations: [],
        gridColSpan: 12
      }
    ]
  };

  constructor(
    private pannaService: PannapramukhService,
    private toastService: ToastService,
    private crudHandler: CrudHandlerService,
    private authService: AuthServiceService,
    private route: ActivatedRoute,
    private permissionService: PermissionService
  ) { }

  ngOnInit() {
    const role = (this.authService.getRole() || '').toUpperCase().trim();
    const isBoothSanyojak = role === 'BOOTHSANYOJAK';

    if (isBoothSanyojak) {
      this.boothIds = this.authService.getBoothId();
    }

    this.route.url.subscribe(url => {
      const path = url[0]?.path || '';
      this.isListView = path.includes('-list');
      this.config.filterable = this.isListView;
      this.loading = true;

      if (this.isListView) {
        this.loadFilterOptions(isBoothSanyojak);
      }
      this.loadData();
    });
  }

  loadFilterOptions(isBoothSanyojak: boolean = false) {
    this.config.filters = [
      { key: 'boothIds', label: 'Booth', type: 'select', options: [], placeholder: '-- Select Booth --', multiple: true, visible: !isBoothSanyojak },
      { key: 'villageIds', label: 'Village', type: 'select', options: [], placeholder: '-- Select Village --', multiple: true },
      { key: 'castIds', label: 'Caste', type: 'select', options: [], placeholder: '-- Select Caste --', multiple: true }
    ];


    // Load Booths
    if (!isBoothSanyojak) {
      const role = (this.authService.getRole() || '').toUpperCase().trim();
      const isSectorSanyojak = role === 'SECTORSANYOJAK';

      const request = isSectorSanyojak
        ? this.pannaService.getCustom(`booth/getAllBoothBySectorid?sectorid=${this.authService.getUserId()}`)
        : this.pannaService.getCommonData('boothNumber');

      request.subscribe((res: any) => {
        const filter = this.config.filters?.find(f => f.key === 'boothIds');
        if (filter) {
          const list = Array.isArray(res?.data) ? res.data : (Array.isArray(res) ? res : []);
          filter.options = list.map((b: any) => ({
            label: `Booth No. ${b.boothNumber}`,
            value: String(b.boothId || b.id)
          }));
        }
      });
    }

    // Load Villages (filtered by booth if BoothSanyojak)
    const villageUrl = isBoothSanyojak && this.boothIds
      ? `villagesByBoothId?boothId=${this.boothIds}`
      : 'village';

    this.pannaService.getCommonData(villageUrl, null, 500000).subscribe((res: any) => {
      const filter = this.config.filters?.find(f => f.key === 'villageIds');
      if (filter) {
        const list = Array.isArray(res?.data?.items) ? res.data.items : (Array.isArray(res?.items) ? res.items : (Array.isArray(res?.data) ? res.data : (Array.isArray(res) ? res : [])));
        filter.options = list.map((v: any) => ({
          label: v.name || v.villageName,
          value: String(v.id || v.villageId)
        }));
      }
    });

    // Load Castes
    this.pannaService.getCommonData('cast?id=', null, 500000).subscribe((res: any) => {
      const filter = this.config.filters?.find(f => f.key === 'castIds');
      if (filter) {
        const list = Array.isArray(res?.data?.items) ? res.data.items : (Array.isArray(res?.items) ? res.items : (Array.isArray(res?.data) ? res.data : (Array.isArray(res) ? res : [])));
        filter.options = list.map((c: any) => ({
          label: c.name || c.castName,
          value: String(c.id)
        }));
      }
    });
  }

  boothIds: string | null = null;
  villageIds: string | null = null;
  castIds: string | null = null;

  handleFilterChange(filterState: Record<string, any>) {
    const processIds = (ids: any) => Array.isArray(ids) ? (ids.length > 0 ? ids.join(',') : null) : (ids || null);

    this.boothIds = processIds(filterState['boothIds']);
    this.villageIds = processIds(filterState['villageIds']);
    this.castIds = processIds(filterState['castIds']);

    // Cascade villages when booth changes
    if (filterState['boothIds'] && Array.isArray(filterState['boothIds']) && filterState['boothIds'].length === 1) {
      const boothId = filterState['boothIds'][0];
      this.pannaService.getCommonData(`villagesByBoothId?boothId=${boothId}`).subscribe((res: any) => {
        const filter = this.config.filters?.find(f => f.key === 'villageIds');
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
    this.loading = true;
    this.loadData();
  }

  loadData() {
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

    this.pannaService.getAllPannapramukhs(params).subscribe({
      next: (response: any) => {
        const dataWrap = response.data;
        const items = dataWrap?.items || (Array.isArray(dataWrap) ? dataWrap : []);

        this.pannaList = items.map((item: any) => ({
          ...item,
          villageName: Array.isArray(item.villages) ? item.villages.map((v: any) => v.villageName).join(', ') : '',
          villageId: Array.isArray(item.villages) ? item.villages.map((v: any) => v.villageId) : []
        }));

        this.totalCount = dataWrap?.totalCount || this.pannaList.length;
        this.loading = false;
      },
      error: (err: any) => {
        console.error('Error fetching panna pramukhs:', err);
        this.loading = false;
      }
    });
  }

  handlePageChange(event: any) {
    this.pageNumber = event.currentPage;
    this.pageSize = event.pageSize;
    this.loadData();
  }

  handleSortChange(event: any) {
    this.sortBy = event.column;
    this.isDescending = event.direction === 'desc';
    this.pageNumber = 1;
    this.loadData();
  }

  handleSearchChange(term: string) {
    this.searchTerm = term;
    this.pageNumber = 1;
    this.loadData();
  }

  handleSelection(selected: any[]) {
    console.log('Selected panna pramukhs:', selected);
  }

  handleAction(event: any) {
    const { action, row } = event;
    if (action.id === 'delete') {
      this.crudHandler.handleRequest(
        this.pannaService.deletePannapramukh(row.id),
        'Deleted',
        'Panna Pramukh deleted successfully!',
        () => this.loadData(),
        true,
        ModulePermission.PannaPramukh
      );
    } else if (action.id === 'edit') {
      const editData = { ...row };
      // Map villages to villageId string array for multi-select binding
      if (editData.villages && Array.isArray(editData.villages)) {
        editData.villageId = editData.villages.map((v: any) => String(v.villageId));
      }

      // Convert critical IDs to strings for dropdown matching and cascading triggers
      ['id', 'boothId', 'categoryId', 'castId'].forEach(key => {
        if (editData[key]) {
          editData[key] = String(editData[key]);
        }
      });

      this.pannaModal.openModal(editData);
    }
  }

  handleFormSubmit(result: FormResult) {
    if (!result.status) return;

    const raw = result.data;
    console.log("raw", result)
    const formData = new FormData();

    formData.append('id', raw.id ? String(raw.id) : '0');
    formData.append('boothId', String(raw.boothId));
    formData.append('pannaPramukhName', raw.pannaPramukhName);
    formData.append('pannaNumber', String(raw.pannaNumber));
    formData.append('categoryId', String(raw.categoryId));
    formData.append('castId', String(raw.castId));
    formData.append('voterId', String(raw.voterId));
    formData.append('phoneNumber', raw.phoneNumber);
    formData.append('address', raw.address);
    formData.append('userId', this.authService.getUserId() || '');

    // Append file if exists in result.files
    if (result.files && result.files['ProfilePicture']) {
      formData.append('ProfilePicture', result.files['ProfilePicture']);
    }

    // Handle array fields (e.g., multiple village selection)
    if (Array.isArray(raw.villageId)) {
      raw.villageId.forEach((v: any) => formData.append('villageId', String(v)));
    } else if (raw.villageId) {
      formData.append('villageId', String(raw.villageId));
    }

    // Append file if exists
    if (result.files && result.files['profile']) {
      formData.append('profile', result.files['profile']);
    }

    const isUpdate = !!raw.id && raw.id !== '0';
    const request = isUpdate
      ? this.pannaService.updatePannapramukh(formData)
      : this.pannaService.createPannapramukh(formData);

    this.crudHandler.handleRequest(
      request,
      isUpdate ? 'Updated' : 'Success',
      `Panna Pramukh ${isUpdate ? 'updated' : 'created'} successfully!`,
      () => this.loadData(),
      true,
      ModulePermission.PannaPramukh
    );
  }

  handleExport(format: string) {
    if (!format) return;
    this.isExporting = true;

    // Collect all current filter parameters for the export matching the API schema
    const params: any = {
      Search: this.searchTerm,
      BoothId: this.boothIds,
      VillageId: this.villageIds,
      CastId: this.castIds,
      UserId: this.authService.getUserId()
    };

    const request = format === 'excel' ?
      this.pannaService.exportToExcel(params) :
      this.pannaService.exportToPdf(params);

    request.subscribe({
      next: (blob: Blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        const timestamp = new Date().toISOString().split('T')[0];
        a.download = `Panna_Pramukh_List_${timestamp}.${format === 'excel' ? 'xlsx' : 'pdf'}`;
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
        document.body.removeChild(a);
        this.isExporting = false;
        this.toastService.showSuccess('Success', `Panna Pramukh list exported to ${format.toUpperCase()} successfully!`);
      },
      error: (err: any) => {
        console.error(`Error exporting to ${format}:`, err);
        this.toastService.showError('Error', `Failed to export Panna Pramukh list to ${format.toUpperCase()}`);
        this.isExporting = false;
      }
    });
  }
}
