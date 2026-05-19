import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, Validators } from '@angular/forms';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';
import { TableAction, TableColumn, TableConfig } from '../../shared/generic-table/generic-table.types';
import { GenericModalButtonComponent } from '../../shared/generic-modal-form/generic-modal-button.component';
import { FormConfig, FormResult } from '../../shared/generic-modal-form/generic-form.types';
import { PageHeaderComponent } from '../../shared/page-header/page-header.component';
import { PrabhavshaliService } from '../../../Services/Admin/prabhavshali/prabhavshali.service';
import { ToastService } from '../../../Services/common/toast/toast.service';
import { CrudHandlerService } from '../../../Services/common/crud-handler.service';
import { AuthServiceService } from '../../../Services/Auth/auth.service';
import { ModulePermission } from '../../../models/module-permission.enum';
import { ActivatedRoute } from '@angular/router';
import { GenericExportComponent } from '../../shared/generic-export/generic-export.component';
import { PermissionService } from '../../../Services/common/permission.service';

@Component({
  selector: 'app-prabhavshali',
  standalone: true,
  imports: [CommonModule, GenericTableComponent, GenericModalButtonComponent, PageHeaderComponent, ReactiveFormsModule, GenericExportComponent],
  templateUrl: './prabhavshali.component.html',
  styleUrl: './prabhavshali.component.css'
})
export class PrabhavshaliComponent implements OnInit {
  @ViewChild('prabhavshaliModal') prabhavshaliModal!: GenericModalButtonComponent;

  personList: any[] = [];
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
  isDoctorView = false;
  isAdvocateView = false;
  isGovtEmployeeView = false;
  isPradhanView = false;
  isSpecialView = false;
  pageTitle = 'Prabhavshali Vyakt Management';
  pageSubtitle = 'Manage influential people across booths and villages';

  canManage(): boolean {
    if (this.isListView) return false;
    const role = (this.authService.getRole() || '').toUpperCase().trim();
    if (role === 'VIDHANSABHAPRABHARI') return true;
    return !this.isSpecialView && this.permissionService.hasPermission(ModulePermission.EffectivePersion);
  }

  columns: TableColumn[] = [
    { key: 'boothNumber', label: 'Booth No.', sortable: true },
    { key: 'villageName', label: 'Village', sortable: true },
    { key: 'name', label: 'Name', sortable: true },
    { key: 'designationName', label: 'Designation', sortable: true },
    { key: 'mobile', label: 'Mobile No.', sortable: true },
    { key: 'categoryName', label: 'Category', sortable: true },
    { key: 'castName', label: 'Caste', sortable: true },
    { key: 'description', label: 'Description', sortable: true }
  ];

  config: TableConfig = {
    searchable: true,
    searchPlaceholder: 'Search...',
    paginated: true,
    showRowNumbers: true,
    striped: true,
    hoverable: true,
    serverSide: true,
    defaultPageSize: 50,
    filterable: false
  };

  actions: TableAction[] = [
    { id: 'edit', label: '', variant: 'default', icon: 'edit', show: () => this.canManage() },
    { id: 'delete', label: '', variant: 'danger', icon: 'delete', show: () => this.canManage() }
  ];

  formConfig: FormConfig = {
    title: 'Register Prabhavshali Vyakt',
    submitLabel: 'Save Entry',
    fields: [
      {
        id: 'boothId',
        name: 'boothId',
        label: 'Booth No',
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
        label: 'Villages',
        type: 'select',
        placeholder: '-- Select Villages --',
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
        id: 'name',
        name: 'name',
        label: 'Name',
        type: 'text',
        placeholder: 'Enter name',
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'mobile',
        name: 'mobile',
        label: 'Mobile No',
        type: 'text',
        placeholder: 'Enter mobile no',
        validations: [Validators.required, Validators.pattern('^[0-9]{10}$')],
        gridColSpan: 6
      },
      {
        id: 'designationId',
        name: 'designationId',
        label: 'Designation',
        type: 'select',
        placeholder: '-- Select Designation --',
        apiUrl: 'common/getadmindesignation',
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data?.items) ? data.data.items : (Array.isArray(data?.items) ? data.items : (Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : [])));
          return list.map((item: any) => ({
            value: String(item.id),
            label: item.designationName || item.name
          }));
        },
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
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'description',
        name: 'description',
        label: 'Description',
        type: 'textarea',
        placeholder: 'Additional details...',
        gridColSpan: 12
      }
    ]
  };

  constructor(
    private prabhavshaliService: PrabhavshaliService,
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

    this.route.url.subscribe((url: any) => {
      const path = url[0]?.path || '';
      const wasSpecialView = this.isSpecialView;

      this.isListView = path.includes('-list');
      this.isDoctorView = path === 'doctor-list';
      this.isAdvocateView = path === 'advocate-list';
      this.isGovtEmployeeView = path === 'government-employee-list';
      this.isPradhanView = path === 'pradhan-list';
      this.isSpecialView = this.isDoctorView || this.isAdvocateView || this.isGovtEmployeeView || this.isPradhanView;

      if (this.isDoctorView) {
        this.pageTitle = 'Doctor List';
        this.pageSubtitle = 'Manage registered doctors across the assembly';
      } else if (this.isAdvocateView) {
        this.pageTitle = 'Advocate List';
        this.pageSubtitle = 'Manage registered advocates across the assembly';
      } else if (this.isGovtEmployeeView) {
        this.pageTitle = 'Government Employee List';
        this.pageSubtitle = 'Manage registered government employees';
      } else if (this.isPradhanView) {
        this.pageTitle = 'Pradhan List';
        this.pageSubtitle = 'Manage registered pradhans across the assembly';
      } else {
        this.pageTitle = this.isListView ? 'Prabhavshali Vyakti List' : 'Prabhavshali Vyakti Management';
        this.pageSubtitle = 'Manage influential people across booths and villages';
      }

      if (this.isSpecialView) {
        this.actions = [];
      } else {
        this.actions = [
          { id: 'edit', label: '', variant: 'default', icon: 'edit', show: () => this.canManage() },
          { id: 'delete', label: '', variant: 'danger', icon: 'delete', show: () => this.canManage() }
        ];
      }

      this.config.filterable = this.isListView;

      // Only reload filters if we switched between special/regular view
      if (this.isListView && (wasSpecialView !== this.isSpecialView || !this.config.filters?.length)) {
        this.loadFilterOptions(isBoothSanyojak);
      }

      this.loadData();
    });
  }

  loadFilterOptions(isBoothSanyojak: boolean = false) {
    if (this.isSpecialView) {
      this.config.filters = [
        { key: 'boothIds', label: 'Booth', type: 'select', options: [], placeholder: '-- Select Booth --', multiple: true, visible: !isBoothSanyojak },
        { key: 'villageIds', label: 'Village', type: 'select', options: [], placeholder: '-- Select Village --', multiple: true }
      ];
    } else {
      this.config.filters = [
        { key: 'boothIds', label: 'Booth', type: 'select', options: [], placeholder: '-- Select Booth --', multiple: true, visible: !isBoothSanyojak },
        { key: 'villageIds', label: 'Village', type: 'select', options: [], placeholder: '-- Select Village --', multiple: true },
        { key: 'designationIds', label: 'Designation', type: 'select', options: [], placeholder: '-- Select Designation --', multiple: true }
      ];
    }

    // Load Booths
    if (!isBoothSanyojak) {
      const role = (this.authService.getRole() || '').toUpperCase().trim();
      const isSectorSanyojak = role === 'SECTORSANYOJAK';

      const request = isSectorSanyojak
        ? this.prabhavshaliService.getCustom(`booth/getAllBoothBySectorid?sectorid=${this.authService.getUserId()}`)
        : this.prabhavshaliService.getCommonData('boothNumber');

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

    this.prabhavshaliService.getCommonData(villageUrl, null, 500000).subscribe(res => {
      const filter = this.config.filters?.find(f => f.key === 'villageIds');
      if (filter) {
        const list = Array.isArray(res?.data?.items) ? res.data.items : (Array.isArray(res?.items) ? res.items : (Array.isArray(res?.data) ? res.data : (Array.isArray(res) ? res : [])));
        filter.options = list.map((v: any) => ({
          label: v.name || v.villageName,
          value: String(v.id || v.villageId)
        }));
      }
    });

    // Load Designations
    if (!this.isSpecialView) {
      this.prabhavshaliService.getDesignations().subscribe(res => {
        const filter = this.config.filters?.find(f => f.key === 'designationIds');
        if (filter) {
          const list = Array.isArray(res?.data?.items) ? res.data.items : (Array.isArray(res?.items) ? res.items : (Array.isArray(res?.data) ? res.data : (Array.isArray(res) ? res : [])));
          filter.options = list.map((d: any) => ({
            label: d.designationName || d.name,
            value: String(d.id)
          }));
        }
      });
    }
  }

  boothIds: string | null = null;
  villageIds: string | null = null;
  designationIds: string | null = null;

  handleFilterChange(filterState: Record<string, any>) {
    const processIds = (ids: any) => Array.isArray(ids) ? (ids.length > 0 ? ids.join(',') : null) : (ids || null);

    this.boothIds = processIds(filterState['boothIds']);
    this.villageIds = processIds(filterState['villageIds']);
    this.designationIds = processIds(filterState['designationIds']);

    // Cascade villages when booth changes
    if (filterState['boothIds'] && Array.isArray(filterState['boothIds']) && filterState['boothIds'].length === 1) {
      const boothId = filterState['boothIds'][0];
      this.prabhavshaliService.getCommonData(`villagesByBoothId?boothId=${boothId}`).subscribe(res => {
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
      designationIds: this.designationIds,
      roleFilterFlag: !this.isListView
    };

    if (this.isDoctorView) params.designationIds = 8;
    if (this.isAdvocateView) params.designationIds = 9;
    if (this.isGovtEmployeeView) params.designationIds = 10;
    if (this.isPradhanView) params.designationId = 1;

    const userId = this.authService.getUserId();
    if (userId) {
      params.userId = userId;
    }

    const request = (this.isDoctorView || this.isAdvocateView || this.isGovtEmployeeView)
      ? this.prabhavshaliService.getAllPrabhavshali(params)
      : (this.isPradhanView
        ? this.prabhavshaliService.getPradhans(params)
        : this.prabhavshaliService.getAllPrabhavshali(params));

    request.subscribe({
      next: (response) => {
        const dataWrap = response.data;
        if (dataWrap && dataWrap.items) {
          this.personList = dataWrap.items.map((item: any) => ({
            ...item,
            villageName: Array.isArray(item.villages) ? item.villages.map((v: any) => v.villageName).join(', ') : '',
            villageId: Array.isArray(item.villages) ? item.villages.map((v: any) => v.villageId) : []
          }));
          this.totalCount = dataWrap.totalCount || 0;
        } else {
          const rawList = Array.isArray(dataWrap) ? dataWrap : [];
          this.personList = rawList.map((item: any) => ({
            ...item,
            villageName: Array.isArray(item.villages) ? item.villages.map((v: any) => v.villageName).join(', ') : '',
            villageId: Array.isArray(item.villages) ? item.villages.map((v: any) => v.villageId) : []
          }));
          this.totalCount = this.personList.length;
        }
        this.loading = false;
      },
      error: (err) => {
        console.error('Error fetching Prabhavshali list:', err);
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
    console.log('Selected persons:', selected);
  }

  handleAction(event: any) {
    const { action, row } = event;
    if (action.id === 'delete') {
      this.crudHandler.handleRequest(
        this.prabhavshaliService.deletePrabhavshali(row.id),
        'Deleted',
        'Entry deleted successfully!',
        () => this.loadData(),
        true,
        ModulePermission.EffectivePersion
      );
    } else if (action.id === 'edit') {
      const editData = { ...row };
      ['id', 'boothId', 'designationId', 'categoryId', 'castId'].forEach(key => {
        if (editData[key]) editData[key] = String(editData[key]);
      });
      if (editData.villageId) {
        editData.villageId = Array.isArray(editData.villageId) ? editData.villageId.map(String) : [String(editData.villageId)];
      }
      this.prabhavshaliModal.openModal(editData);
    }
  }

  handleFormSubmit(result: FormResult) {
    if (!result.status) return;

    const raw = result.data;
    const rowId = raw.id || (this.prabhavshaliModal.initialData && this.prabhavshaliModal.initialData.id);

    const submitData: any = {
      id: rowId ? Number(rowId) : null,
      boothId: Number(raw.boothId),
      designationId: Number(raw.designationId),
      villageId: Array.isArray(raw.villageId) ? raw.villageId.map((v: any) => Number(v)) : (raw.villageId ? [Number(raw.villageId)] : []),
      name: raw.name,
      categoryId: Number(raw.categoryId),
      castId: Number(raw.castId),
      mobile: raw.mobile,
      description: raw.description,
      userId: this.authService.getUserId()
    };

    const isUpdate = !!submitData.id;
    const request = isUpdate
      ? this.prabhavshaliService.updatePrabhavshali(submitData)
      : this.prabhavshaliService.createPrabhavshali(submitData);

    this.crudHandler.handleRequest(
      request,
      isUpdate ? 'Updated' : 'Success',
      `Prabhavshali Vyakt ${isUpdate ? 'updated' : 'created'} successfully!`,
      () => this.loadData(),
      true,
      ModulePermission.EffectivePersion
    );
  }

  handleExport(format: string) {
    if (!format) return;
    this.isExporting = true;

    let request;
    const exportFormat = format as 'excel' | 'pdf';
    if (this.isDoctorView) {
      request = this.prabhavshaliService.exportSpecial('doctor', exportFormat);
    } else if (this.isAdvocateView) {
      request = this.prabhavshaliService.exportSpecial('advocate', exportFormat);
    } else if (this.isGovtEmployeeView) {
      request = this.prabhavshaliService.exportSpecial('governmentemoloyee', exportFormat);
    } else if (this.isPradhanView) {
      request = this.prabhavshaliService.exportSpecial('pradhan', exportFormat);
    } else {
      request = exportFormat === 'excel' ? this.prabhavshaliService.exportToExcel() : this.prabhavshaliService.exportToPdf();
    }

    request.subscribe({
      next: (blob: Blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;

        let fileNamePrefix = 'Prabhavshali';
        if (this.isDoctorView) fileNamePrefix = 'Doctor';
        else if (this.isAdvocateView) fileNamePrefix = 'Advocate';
        else if (this.isGovtEmployeeView) fileNamePrefix = 'Government_Employee';
        else if (this.isPradhanView) fileNamePrefix = 'Pradhan';

        a.download = `${fileNamePrefix}_List.${format === 'excel' ? 'xlsx' : 'pdf'}`;
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
        document.body.removeChild(a);
        this.isExporting = false;
        this.toastService.showSuccess('Success', `Prabhavshali list exported to ${format.toUpperCase()} successfully!`);
      },
      error: (err: any) => {
        console.error(`Error exporting to ${format}:`, err);
        this.toastService.showError('Error', `Failed to export Prabhavshali list to ${format.toUpperCase()}`);
        this.isExporting = false;
      }
    });
  }
}
