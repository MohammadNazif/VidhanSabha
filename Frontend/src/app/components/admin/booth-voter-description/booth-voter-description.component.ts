import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';
import { TableAction, TableColumn, TableConfig } from '../../shared/generic-table/generic-table.types';
import { GenericModalButtonComponent } from '../../shared/generic-modal-form/generic-modal-button.component';
import { FormConfig, FormResult } from '../../shared/generic-modal-form/generic-form.types';
import { PageHeaderComponent } from '../../shared/page-header/page-header.component';
import { BoothVoterService } from '../../../Services/Admin/booth-voter/booth-voter.service';
import { CasteVoterService } from '../../../Services/Admin/booth-voter/caste-voter.service';
import { BoothSamitiService } from '../../../Services/Admin/booth-voter/booth-samiti.service';
import { ToastService } from '../../../Services/common/toast/toast.service';
import { CrudHandlerService } from '../../../Services/common/crud-handler.service';
import { AuthServiceService } from '../../../Services/Auth/auth.service';
import { ActivatedRoute } from '@angular/router';
import { ModulePermission } from '../../../models/module-permission.enum';
import { GenericExportComponent } from '../../shared/generic-export/generic-export.component';
import { PermissionService } from '../../../Services/common/permission.service';

@Component({
  selector: 'app-booth-voter-description',
  standalone: true,
  imports: [CommonModule, GenericTableComponent, GenericModalButtonComponent, PageHeaderComponent, FormsModule, ReactiveFormsModule, GenericExportComponent],
  templateUrl: './booth-voter-description.component.html',
  styleUrl: './booth-voter-description.component.css'
})
export class BoothVoterDescriptionComponent implements OnInit {
  @ViewChild('voterModal') voterModal!: GenericModalButtonComponent;
  @ViewChild('casteModal') casteModal!: GenericModalButtonComponent;
  @ViewChild('samitiModal') samitiModal!: GenericModalButtonComponent;

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

  // Caste-wise detail modal state
  selectedBoothVoter: any = null;
  casteWiseList: any[] = [];
  loadingCasteWise = false;
  showCasteDetailsModal = false;
  editingCasteId: number | null = null;
  editingNumberValue: number | null = null;
  editingSubCasteId: number | null = null;
  castesList: any[] = [];
  columns: TableColumn[] = [
    { key: 'boothNumber', label: 'Booth No.', sortable: true },
    { key: 'villageName', label: 'Village', sortable: true },
    { key: 'totalVoter', label: 'Total Voters', sortable: true },
    { key: 'male', label: 'Male', sortable: true },
    { key: 'female', label: 'Female', sortable: true },
    { key: 'other', label: 'Other', sortable: true },
    {
      key: 'casteBreakdown',
      label: 'Caste breakdown',
      type: 'badge-html',
      align: 'center',
      sortable: false,
      formatter: () => '<i class="fa-solid fa-chart-pie mr-1"></i> View',
      badgeVariant: () => 'info'
    }
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
    { id: 'delete', label: '', variant: 'danger', icon: 'delete', show: () => this.canManage() },
    { id: 'add_caste', label: 'Caste Wise', variant: 'primary', icon: 'add', show: () => this.canManage() }
  ];

  constructor(
    private boothVoterService: BoothVoterService,
    private casteVoterService: CasteVoterService,
    private boothSamitiService: BoothSamitiService,
    private toastService: ToastService,
    private crudHandler: CrudHandlerService,
    private authService: AuthServiceService,
    private route: ActivatedRoute,
    private permissionService: PermissionService
  ) { }

  canManage(): boolean {
    if (this.isListView) return false;
    const role = (this.authService.getRole() || '').toUpperCase().trim();
    if (role === 'VIDHANSABHAPRABHARI') return true;
    return this.permissionService.hasPermission(ModulePermission.BoothVoterDescrition);
  }

  addVoterConfig: FormConfig = {
    title: 'Add Booth Voter Description',
    submitLabel: 'Save Description',
    fields: [
      {
        id: 'boothId',
        name: 'boothId',
        label: 'Booth',
        type: 'select',
        placeholder: '--Select Booth--',
        apiUrl: () => {
          const role = (this.authService.getRole() || '').toUpperCase().trim();
          if (role === 'SECTORSANYOJAK') {
            return `booth/getAllBoothBySectorid?sectorid=${this.authService.getUserId()}`;
          }
          return `common/boothNumber?PageNumber=1&PageSize=1000&IsDescending=true&SortBy=id&userId=${this.authService.getUserId()}`;
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
        apiUrl: (boothId: string) => `common/villagesByBoothId?boothId=${boothId}&PageNumber=1&PageSize=1000&IsDescending=true&SortBy=id&userId=${this.authService.getUserId()}`,
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
        id: 'totalVoter',
        name: 'totalVoter',
        label: 'Total Voters',
        type: 'number',
        placeholder: 'Enter Total Voters',
        validations: [Validators.required, Validators.min(0)],
        gridColSpan: 6
      },
      {
        id: 'male',
        name: 'male',
        label: 'Male Voters',
        type: 'number',
        placeholder: 'Enter Male Voters',
        validations: [Validators.required, Validators.min(0)],
        gridColSpan: 6
      },
      {
        id: 'female',
        name: 'female',
        label: 'Female Voters',
        type: 'number',
        placeholder: 'Enter Female Voters',
        validations: [Validators.required, Validators.min(0)],
        gridColSpan: 6
      },
      {
        id: 'other',
        name: 'other',
        label: 'Other Voters',
        type: 'number',
        placeholder: 'Enter Other Voters',
        validations: [Validators.required, Validators.min(0)],
        gridColSpan: 6
      }
    ]
  };

  addCasteConfig: FormConfig = {
    title: 'Add Caste Wise Voters',
    submitLabel: 'Save Caste Voters',
    fields: [
      {
        id: 'casteVoterId',
        name: 'casteVoterId',
        label: 'Booth Voter ID',
        type: 'hidden'
      },
      {
        id: 'casteVoters',
        name: 'casteVoters',
        label: 'Caste Wise Details',
        type: 'form-array',
        gridColSpan: 12,
        subFields: [
          {
            id: 'subCasteId',
            name: 'subCasteId',
            label: 'Caste',
            type: 'select',
            apiUrl: 'common/getAllCast', // Using category as placeholder if direct cast isn't found
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
            id: 'number',
            name: 'number',
            label: 'Number of Voters',
            type: 'number',
            placeholder: 'Enter number',
            validations: [Validators.required, Validators.min(0)],
            gridColSpan: 6
          }
        ]
      }
    ]
  };

  addSamitiConfig: FormConfig = {
    title: 'Add Booth Samiti Member',
    submitLabel: 'Save Member',
    fields: [
      {
        id: 'boothId',
        name: 'boothId',
        label: 'Booth ID',
        type: 'hidden'
      },
      {
        id: 'name',
        name: 'name',
        label: 'Full Name',
        type: 'text',
        placeholder: 'Enter member name',
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'contact',
        name: 'contact',
        label: 'Contact Number',
        type: 'text',
        placeholder: 'Enter phone number',
        validations: [Validators.required, Validators.pattern('^[0-9]{10}$')],
        gridColSpan: 6
      },
      {
        id: 'age',
        name: 'age',
        label: 'Age',
        type: 'number',
        placeholder: 'Enter age',
        validations: [Validators.required, Validators.min(18)],
        gridColSpan: 6
      },
      {
        id: 'occupation',
        name: 'occupation',
        label: 'Occupation',
        type: 'text',
        placeholder: 'Enter occupation',
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
        id: 'casteId',
        name: 'casteId',
        label: 'Caste',
        type: 'select',
        placeholder: '-- Select Caste --',
        dependsOn: 'categoryId',
        apiUrl: (catId: string) => `common/cast?id=${catId}`,
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
        id: 'designationId',
        name: 'designationId',
        label: 'Designation',
        type: 'select',
        placeholder: '-- Select Designation --',
        apiUrl: 'boothsamiti-designation/getAll',
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : []);
          return list.map((item: any) => ({
            value: String(item.id),
            label: item.designationName
          }));
        },
        validations: [Validators.required],
        gridColSpan: 12
      }
    ]
  };

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
      this.loading = true;
      if (this.isListView) {
        this.loadFilterOptions(isBoothSanyojak);
      }
      this.loadVoters();
    });

    this.loadAllCastes();
  }

  loadFilterOptions(isBoothSanyojak: boolean = false) {
    this.config.filters = [
      { key: 'boothIds', label: 'Booth', type: 'select', options: [], placeholder: '-- Select Booth --', multiple: true, visible: !isBoothSanyojak },
      { key: 'villageIds', label: 'Village', type: 'select', options: [], placeholder: '-- Select Village --', multiple: true }
    ];

    // Load Booths
    if (!isBoothSanyojak) {
      const role = (this.authService.getRole() || '').toUpperCase().trim();
      const isSectorSanyojak = role === 'SECTORSANYOJAK';

      const request = isSectorSanyojak
        ? this.boothVoterService.getCustom(`booth/getAllBoothBySectorid?sectorid=${this.authService.getUserId()}`)
        : this.boothVoterService.getCommonData('boothNumber');

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

    this.boothVoterService.getCommonData(villageUrl, null, 500000).subscribe((res: any) => {
      const filter = this.config.filters?.find(f => f.key === 'villageIds');
      if (filter) {
        const list = Array.isArray(res?.data?.items) ? res.data.items : (Array.isArray(res?.items) ? res.items : (Array.isArray(res?.data) ? res.data : (Array.isArray(res) ? res : [])));
        filter.options = list.map((v: any) => ({
          label: v.name || v.villageName,
          value: String(v.id || v.villageId)
        }));
      }
    });
  }

  boothIds: string | null = null;
  villageIds: string | null = null;

  handleFilterChange(filterState: Record<string, any>) {
    const processIds = (ids: any) => Array.isArray(ids) ? (ids.length > 0 ? ids.join(',') : null) : (ids || null);

    this.boothIds = processIds(filterState['boothIds']);
    this.villageIds = processIds(filterState['villageIds']);

    // Cascade villages when booth changes
    if (filterState['boothIds'] && Array.isArray(filterState['boothIds']) && filterState['boothIds'].length === 1) {
      const boothId = filterState['boothIds'][0];
      this.boothVoterService.getCommonData(`villagesByBoothId?boothId=${boothId}`).subscribe((res: any) => {
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
    this.loadVoters();
  }

  loadVoters() {
    this.loading = true;
    const params: any = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      searchTerm: this.searchTerm,
      sortBy: this.sortBy,
      isDescending: this.isDescending,
      boothIds: this.boothIds,
      villageIds: this.villageIds,
      roleFilterFlag: !this.isListView
    };

    const userId = this.authService.getUserId();
    if (userId) {
      params.userId = userId;
    }

    this.boothVoterService.getAllBoothVoters(params).subscribe({
      next: (response: any) => {
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
        console.error('Error fetching booth voters:', err);
        this.loading = false;
      }
    });
  }

  handlePageChange(event: any) {
    this.pageNumber = event.currentPage;
    this.pageSize = event.pageSize;
    this.loadVoters();
  }

  handleSortChange(event: any) {
    this.sortBy = event.column;
    this.isDescending = event.direction === 'desc';
    this.pageNumber = 1;
    this.loadVoters();
  }

  handleSearchChange(term: string) {
    this.searchTerm = term;
    this.pageNumber = 1;
    this.loadVoters();
  }

  handleSelection(selected: any[]) {
    console.log('Selected items:', selected);
  }

  handleRowClick(event: any) {
    if (!event || !event.row) return;
    const row = event.row;
    this.selectedBoothVoter = row;
    this.showCasteDetailsModal = true;
    this.loadCasteWiseData(row.id);
  }

  loadCasteWiseData(boothVoterId: number) {
    this.loadingCasteWise = true;
    this.casteWiseList = [];
    this.casteVoterService.getCasteVotersByBoothVoterId(boothVoterId).subscribe({
      next: (response: any) => {
        if (response && response.data) {
          const dataWrap = response.data;
          if (dataWrap && Array.isArray(dataWrap.items)) {
            this.casteWiseList = dataWrap.items;
          } else if (Array.isArray(dataWrap)) {
            this.casteWiseList = dataWrap;
          } else {
            this.casteWiseList = [];
          }
        } else {
          this.casteWiseList = [];
        }
        this.loadingCasteWise = false;
      },
      error: (err) => {
        console.error('Error fetching caste-wise voters:', err);
        this.loadingCasteWise = false;
        this.toastService.showError('Error', 'Failed to load caste-wise voter details.');
      }
    });
  }

  closeCasteDetailsModal() {
    this.showCasteDetailsModal = false;
    this.selectedBoothVoter = null;
    this.casteWiseList = [];
  }

  calculatePercentage(casteCount: number): number {
    const total = this.selectedBoothVoter?.totalVoter || 0;
    if (!total || !casteCount) return 0;
    return Math.round((casteCount / total) * 100);
  }

  loadAllCastes() {
    this.boothVoterService.getCommonData('getAllCast').subscribe({
      next: (res: any) => {
        const list = Array.isArray(res?.data) ? res.data : (Array.isArray(res) ? res : []);
        this.castesList = list.map((item: any) => ({
          id: Number(item.id),
          name: item.name
        }));
      },
      error: (err) => {
        console.error('Error loading castes:', err);
      }
    });
  }

  startEditCaste(item: any) {
    this.editingCasteId = item.id;
    this.editingNumberValue = item.number;
    this.editingSubCasteId = item.subCasteId;
  }

  cancelEditCaste() {
    this.editingCasteId = null;
    this.editingNumberValue = null;
    this.editingSubCasteId = null;
  }

  saveEditCaste(item: any) {
    if (this.editingNumberValue === null || this.editingNumberValue < 0) {
      this.toastService.showError('Error', 'Please enter a valid number of voters.');
      return;
    }
    if (!this.editingSubCasteId) {
      this.toastService.showError('Error', 'Please select a caste.');
      return;
    }

    const updatedCasteVoters = this.casteWiseList.map(cv => {
      if (cv.id === item.id) {
        return {
          subCasteId: Number(this.editingSubCasteId),
          number: Number(this.editingNumberValue)
        };
      } else {
        return {
          subCasteId: Number(cv.subCasteId),
          number: Number(cv.number)
        };
      }
    });

    const payload = {
      casteVoterId: Number(item.casteVoterId),
      casteVoters: updatedCasteVoters
    };

    this.casteVoterService.updateCasteVoter(payload).subscribe({
      next: (res: any) => {
        this.toastService.showSuccess('Success', 'Caste voter updated successfully!');
        this.editingCasteId = null;
        this.editingNumberValue = null;
        this.editingSubCasteId = null;
        if (this.selectedBoothVoter) {
          this.loadCasteWiseData(this.selectedBoothVoter.id);
        }
      },
      error: (err) => {
        console.error('Error updating caste voter:', err);
        this.toastService.showError('Error', 'Failed to update caste voter.');
      }
    });
  }

  async deleteCasteRecord(item: any) {
    const { default: Swal } = await import('sweetalert2');
    const result = await Swal.fire({
      title: 'Delete caste entry?',
      text: `Are you sure you want to delete ${item.subCasteName || 'this caste'}?`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#ef4444',
      cancelButtonColor: '#6b7280',
      confirmButtonText: 'Delete',
      cancelButtonText: 'Cancel',
      reverseButtons: true,
      width: '340px',
      padding: '1.25rem'
    });

    if (!result.isConfirmed) return;

    this.casteVoterService.deleteCasteVoter(item.id).subscribe({
      next: (res: any) => {
        this.toastService.showSuccess('Success', 'Caste voter entry deleted successfully!');
        if (this.selectedBoothVoter) {
          this.loadCasteWiseData(this.selectedBoothVoter.id);
        }
      },
      error: (err) => {
        console.error('Error deleting caste voter:', err);
        this.toastService.showError('Error', 'Failed to delete caste voter entry.');
      }
    });
  }

  handleAction(event: any) {
    const { action, row } = event;
    if (action.id === 'delete') {
      this.crudHandler.handleRequest(
        this.boothVoterService.deleteBoothVoter(row.id),
        'Deleted',
        'Booth Voter Description deleted successfully!',
        () => this.loadVoters(),
        true,
        ModulePermission.BoothVoterDescrition
      );
    } else if (action.id === 'edit') {
      const editData = { ...row };
      ['id', 'boothId'].forEach(key => {
        if (editData[key] !== null && editData[key] !== undefined) editData[key] = String(editData[key]);
      });
      if (editData.villageId !== null && editData.villageId !== undefined) {
        editData.villageId = Array.isArray(editData.villageId) ? editData.villageId.map(String) : [String(editData.villageId)];
      }
      this.voterModal.openModal(editData);
    } else if (action.id === 'add_caste') {
      this.casteModal.openModal({
        casteVoterId: String(row.id),
        casteVoters: [{ subCasteId: '', number: '' }]
      });
    } else if (action.id === 'add_samiti') {
      this.samitiModal.openModal({
        boothId: String(row.boothId)
      });
    }
  }

  handleSamitiSubmit(result: FormResult) {
    if (!result.status) return;

    const raw = result.data;
    const submitData = {
      name: raw.name,
      categoryId: Number(raw.categoryId),
      casteId: Number(raw.casteId),
      age: Number(raw.age),
      contact: raw.contact,
      occupation: raw.occupation,
      designationId: Number(raw.designationId)
    };

    this.crudHandler.handleRequest(
      this.boothSamitiService.createBoothSamiti(submitData),
      'Created',
      'Booth Samiti member added successfully!',
      () => this.loadVoters(),
      true,
      ModulePermission.BoothVoterDescrition
    );
  }

  handleCasteSubmit(result: FormResult) {
    if (!result.status) return;

    const raw = result.data;
    const submitData = {
      casteVoterId: Number(raw.casteVoterId),
      casteVoters: raw.casteVoters.map((cv: any) => ({
        subCasteId: Number(cv.subCasteId),
        number: Number(cv.number)
      }))
    };

    this.crudHandler.handleRequest(
      this.casteVoterService.createCasteVoter(submitData),
      'Created',
      'Caste wise voters added successfully!',
      () => this.loadVoters(),
      true,
      ModulePermission.BoothVoterDescrition
    );
  }

  handleFormSubmit(result: FormResult) {
    if (!result.status) return;

    const raw = result.data;
    const rowId = raw.id || (this.voterModal.initialData && this.voterModal.initialData.id);

    const submitData: any = {
      id: rowId ? Number(rowId) : null,
      boothId: Number(raw.boothId),
      villageId: Array.isArray(raw.villageId) ? raw.villageId.map((v: any) => Number(v)) : (raw.villageId ? [Number(raw.villageId)] : []),
      totalVoter: Number(raw.totalVoter),
      male: Number(raw.male),
      female: Number(raw.female),
      other: Number(raw.other),
      userId: this.authService.getUserId()
    };

    const isUpdate = !!submitData.id;
    const request = isUpdate
      ? this.boothVoterService.updateBoothVoter(submitData)
      : this.boothVoterService.createBoothVoter(submitData);

    this.crudHandler.handleRequest(
      request,
      isUpdate ? 'Updated' : 'Success',
      `Booth Voter Description ${isUpdate ? 'updated' : 'created'} successfully!`,
      () => this.loadVoters(),
      true,
      ModulePermission.BoothVoterDescrition
    );
  }

  handleExport(format: string) {
    if (!format) return;
    this.isExporting = true;
    const request = format === 'excel' ? this.boothVoterService.exportToExcel() : this.boothVoterService.exportToPdf();

    request.subscribe({
      next: (blob: Blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `Booth_Voter_Description.${format === 'excel' ? 'xlsx' : 'pdf'}`;
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
        document.body.removeChild(a);
        this.isExporting = false;
        this.toastService.showSuccess('Success', `Booth Voter Description exported to ${format.toUpperCase()} successfully!`);
      },
      error: (err: any) => {
        console.error(`Error exporting to ${format}:`, err);
        this.toastService.showError('Error', `Failed to export Booth Voter Description to ${format.toUpperCase()}`);
        this.isExporting = false;
      }
    });
  }
}
