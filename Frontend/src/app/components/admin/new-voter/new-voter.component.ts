import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Validators } from '@angular/forms';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';
import { TableAction, TableColumn, TableConfig } from '../../shared/generic-table/generic-table.types';
import { GenericModalButtonComponent } from '../../shared/generic-modal-form/generic-modal-button.component';
import { FormConfig, FormResult } from '../../shared/generic-modal-form/generic-form.types';
import { PageHeaderComponent } from '../../shared/page-header/page-header.component';
import { NewvoterService } from '../../../Services/Admin/newvoter/newvoter.service';
import { ToastService } from '../../../Services/common/toast/toast.service';
import { CrudHandlerService } from '../../../Services/common/crud-handler.service';
import { AuthServiceService } from '../../../Services/Auth/auth.service';
import { ActivatedRoute } from '@angular/router';
import { ModulePermission } from '../../../models/module-permission.enum';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { GenericExportComponent } from '../../shared/generic-export/generic-export.component';
import { PermissionService } from '../../../Services/common/permission.service';

@Component({
  selector: 'app-new-voter',
  standalone: true,
  imports: [CommonModule, PageHeaderComponent, GenericTableComponent, GenericModalButtonComponent, GenericExportComponent],
  templateUrl: './new-voter.component.html',
  styleUrl: './new-voter.component.css'
})
export class NewVoterComponent implements OnInit {
  @ViewChild('voterModal') voterModal!: GenericModalButtonComponent;

  voterList: any[] = [];
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

  boothIds: string | null = null;
  villageIds: string | null = null;
  castIds: string | null = null;

  columns: TableColumn[] = [
    { key: 'boothNumber', label: 'Booth No.', sortable: true },
    { key: 'villageName', label: 'Village', sortable: true },
    { key: 'name', label: 'Name', sortable: true },
    { key: 'fatherName', label: 'Father Name', sortable: true },
    { key: 'mobile', label: 'Mobile No.', sortable: true },
    { key: 'categoryName', label: 'Category', sortable: true },
    { key: 'castName', label: 'Caste', sortable: true },
    { key: 'dob', label: 'DOB', type: 'date', sortable: true },
    { key: 'age', label: 'Age', sortable: true },
    { key: 'voterId', label: 'Voter ID', sortable: true }
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
    { id: 'edit', label: '', variant: 'default', icon: 'edit', show: () => this.canManageVoters() },
    { id: 'delete', label: '', variant: 'danger', icon: 'delete', show: () => this.canManageVoters() }
  ];

  canManageVoters(): boolean {
    if (this.isListView) return false;
    const role = (this.authService.getRole() || '').toUpperCase().trim();
    if (role === 'VIDHANSABHAPRABHARI') return true;
    return this.permissionService.hasPermission(ModulePermission.NewVoter);
  }

  addVoterConfig: FormConfig = {
    title: 'Add New Voter',
    submitLabel: 'Save',
    fields: [
      {
        id: 'boothId',
        name: 'boothId',
        label: 'Booth No',
        type: 'select',
        placeholder: '-- Select Booth No --',
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
        label: 'Village',
        type: 'select',
        placeholder: '-- Select Village --',
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
        placeholder: 'Enter Name',
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'fatherName',
        name: 'fatherName',
        label: 'Father Name',
        type: 'text',
        placeholder: 'Enter Father Name',
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'mobile',
        name: 'mobile',
        label: 'Mobile No',
        type: 'text',
        placeholder: 'Enter Mobile No',
        validations: [Validators.required, Validators.pattern('^[0-9]{10}$')],
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
        apiUrl: (categoryId: string) => `common/cast?id=${categoryId}`,
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
        id: 'dob',
        name: 'dob',
        label: 'Date of Birth',
        type: 'date',
        placeholder: 'Select DOB',
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'age',
        name: 'age',
        label: 'Age',
        type: 'number',
        placeholder: 'Enter Age',
        validations: [Validators.required, Validators.min(0), Validators.max(150)],
        gridColSpan: 6
      },
      {
        id: 'voterId',
        name: 'voterId',
        label: 'Voter ID',
        type: 'text',
        placeholder: 'Enter Voter ID',
        validations: [Validators.required],
        gridColSpan: 6
      }
    ]
  };

  constructor(
    private voterService: NewvoterService,
    private toastService: ToastService,
    private crudHandler: CrudHandlerService,
    private authService: AuthServiceService,
    private route: ActivatedRoute,
    private http: HttpClient,
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
      this.isListView = path.includes('-list');
      this.config.filterable = this.isListView;
      if (this.isListView) {
        this.loading = true;
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
      const boothUrl = isSectorSanyojak
        ? `${environment.apiUrl}/booth/getAllBoothBySectorid?sectorid=${this.authService.getUserId()}`
        : `${environment.apiUrl}/common/boothNumber`;

      this.http.get<any>(boothUrl).subscribe(res => {
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
      ? `${environment.apiUrl}/common/villagesByBoothId?boothId=${this.boothIds}`
      : `${environment.apiUrl}/common/village?pageSize=500000`;

    this.http.get<any>(villageUrl).subscribe(res => {
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
    this.http.get<any>(`${environment.apiUrl}/common/getAllCast`).subscribe(res => {
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

  handleFilterChange(filterState: Record<string, any>) {
    const processIds = (ids: any) => Array.isArray(ids) ? (ids.length > 0 ? ids.join(',') : null) : (ids || null);

    this.boothIds = processIds(filterState['boothIds']);
    this.villageIds = processIds(filterState['villageIds']);
    this.castIds = processIds(filterState['castIds']);

    // Cascade villages when booth changes
    if (filterState['boothIds'] && Array.isArray(filterState['boothIds']) && filterState['boothIds'].length === 1) {
      const boothId = filterState['boothIds'][0];
      this.http.get<any>(`${environment.apiUrl}/common/villagesByBoothId?boothId=${boothId}`).subscribe(res => {
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
      castIds: this.castIds,
      roleFilterFlag: !this.isListView
    };

    // Clean up empty params
    Object.keys(params).forEach(key => {
      if (params[key] === null || params[key] === undefined || params[key] === '') {
        delete params[key];
      }
    });

    const userId = this.authService.getUserId();
    if (userId) {
      params.userId = userId;
    }

    this.voterService.getAllNewvoters(params).subscribe({
      next: (response) => {
        const dataWrap = response.data;
        if (dataWrap && dataWrap.items) {
          this.voterList = dataWrap.items.map((item: any) => ({
            ...item,
            villageName: Array.isArray(item.villages) ? item.villages.map((v: any) => v.villageName).join(', ') : '',
            villageId: Array.isArray(item.villages) ? item.villages.map((v: any) => v.villageId) : []
          }));
          this.totalCount = dataWrap.totalCount || 0;
        } else {
          const rawList = Array.isArray(dataWrap) ? dataWrap : [];
          this.voterList = rawList.map((item: any) => ({
            ...item,
            villageName: Array.isArray(item.villages) ? item.villages.map((v: any) => v.villageName).join(', ') : '',
            villageId: Array.isArray(item.villages) ? item.villages.map((v: any) => v.villageId) : []
          }));
          this.totalCount = this.voterList.length;
        }
        this.loading = false;
      },
      error: (err) => {
        console.error('Error fetching new voters:', err);
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
    console.log('Selected voters:', selected);
  }

  handleAction(event: any) {
    const { action, row } = event;
    if (action.id === 'delete') {
      this.crudHandler.handleRequest(
        this.voterService.deleteNewvoter(row.id),
        'Deleted',
        'New Voter deleted successfully!',
        () => this.loadData(),
        true,
        ModulePermission.NewVoter
      );
    } else if (action.id === 'edit') {
      const editData = { ...row };

      if (editData.dob && editData.dob.includes('T')) {
        editData.dob = editData.dob.split('T')[0];
      }

      ['id', 'boothId', 'categoryId', 'castId'].forEach(key => {
        if (editData[key] !== null && editData[key] !== undefined) editData[key] = String(editData[key]);
      });
      if (editData.villageId !== null && editData.villageId !== undefined) {
        editData.villageId = Array.isArray(editData.villageId) ? editData.villageId.map(String) : [String(editData.villageId)];
      }
      this.voterModal.openModal(editData);
    }
  }

  handleFormSubmit(result: FormResult) {
    if (!result.status) return;

    const raw = result.data;
    const rowId = raw.id || (this.voterModal.initialData && this.voterModal.initialData.id);

    const submitData: any = {
      id: rowId ? Number(rowId) : null,
      boothId: Number(raw.boothId),
      villageId: Array.isArray(raw.villageId) ? raw.villageId.map((v: any) => Number(v)) : (raw.villageId ? [Number(raw.villageId)] : []),
      name: raw.name,
      fatherName: raw.fatherName,
      mobile: raw.mobile,
      categoryId: Number(raw.categoryId),
      castId: Number(raw.castId),
      dob: raw.dob,
      age: Number(raw.age),
      voterId: raw.voterId,
      userId: this.authService.getUserId()
    };

    const isUpdate = !!submitData.id;
    const request = isUpdate
      ? this.voterService.updateNewvoter(submitData)
      : this.voterService.createNewvoter(submitData);

    this.crudHandler.handleRequest(
      request,
      isUpdate ? 'Updated' : 'Success',
      `New Voter ${isUpdate ? 'updated' : 'created'} successfully!`,
      () => this.loadData(),
      true,
      ModulePermission.NewVoter
    );
  }

  handleExport(format: string) {
    if (!format) return;
    this.isExporting = true;
    const request = format === 'excel' ? this.voterService.exportToExcel() : this.voterService.exportToPdf();

    request.subscribe({
      next: (blob: Blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `New_Voter_List.${format === 'excel' ? 'xlsx' : 'pdf'}`;
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
        document.body.removeChild(a);
        this.isExporting = false;
        this.toastService.showSuccess('Success', `New Voter list exported to ${format.toUpperCase()} successfully!`);
      },
      error: (err) => {
        console.error(`Error exporting to ${format}:`, err);
        this.toastService.showError('Error', `Failed to export New Voter list to ${format.toUpperCase()}`);
        this.isExporting = false;
      }
    });
  }
}
