import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Validators } from '@angular/forms';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';
import { TableAction, TableColumn, TableConfig } from '../../shared/generic-table/generic-table.types';
import { GenericModalButtonComponent } from '../../shared/generic-modal-form/generic-modal-button.component';
import { FormConfig, FormResult } from '../../shared/generic-modal-form/generic-form.types';
import { PageHeaderComponent } from '../../shared/page-header/page-header.component';
import { SahmatAsahmatService } from '../../../Services/Admin/sahmatasahmat/sahmatasahmat.service';
import { ToastService } from '../../../Services/common/toast/toast.service';
import { CrudHandlerService } from '../../../Services/common/crud-handler.service';
import { ActivatedRoute } from '@angular/router';
import { AuthServiceService } from '../../../Services/Auth/auth.service';
import { ModulePermission } from '../../../models/module-permission.enum';
import { GenericExportComponent } from '../../shared/generic-export/generic-export.component';

@Component({
  selector: 'app-sahmat-asahmat',
  standalone: true,
  imports: [CommonModule, PageHeaderComponent, GenericTableComponent, GenericModalButtonComponent, GenericExportComponent],
  templateUrl: './sahmat-asahmat.component.html',
  styleUrl: './sahmat-asahmat.component.css'
})
export class SahmatAsahmatComponent implements OnInit {
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

  isAsahmatView = false;
  isListView = false;

  columns: TableColumn[] = [
    { key: 'boothNumber', label: 'Booth No.', sortable: true },
    { key: 'villageName', label: 'Village', sortable: true },
    { key: 'name', label: 'Name', sortable: true },
    { key: 'mobile', label: 'Mobile No.', sortable: true },
    { key: 'age', label: 'Age', sortable: true },
    // { key: 'typeName', label: 'Type', sortable: true },
    { key: 'party', label: 'Party', sortable: true },
    { key: 'occupation', label: 'Occupation', sortable: true },
    { key: 'voterId', label: 'Voter ID', sortable: true },
    {
      key: 'type', label: 'Status', type: 'badge', sortable: true,
      formatter: (val: any) => val,
      badgeVariant: (val: any) => val === 'Asahmat' ? 'danger' : 'success'
    },
    { key: 'reason', label: 'Reason', sortable: true, visible: true }
  ];

  config: TableConfig = {
    searchable: true,
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
    return role !== 'STATEPRABHARI' && role !== 'ADHYAKSH';
  }

  addVoterConfig: FormConfig = {
    title: 'Add Sahmat/Asahmat Voter',
    submitLabel: 'Save',
    fields: [
      {
        id: 'boothId',
        name: 'boothId',
        label: 'Booth No',
        type: 'select',
        placeholder: '-- Select Booth No --',
        apiUrl: 'common/boothNumber',
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
        id: 'mobile',
        name: 'mobile',
        label: 'Mobile No',
        type: 'text',
        placeholder: 'Enter Mobile No',
        validations: [Validators.required, Validators.pattern('^[0-9]{10}$')],
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
        id: 'typeId',
        name: 'typeId',
        label: 'Type',
        type: 'select',
        placeholder: '-- Select Type --',
        apiUrl: 'common/getsahmattype',
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data?.items) ? data.data.items : (Array.isArray(data?.items) ? data.items : (Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : [])));
          return list.map((item: any) => ({
            value: String(item.id),
            label: item.type || item.name
          }));
        },
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'partyId',
        name: 'partyId',
        label: 'Party',
        type: 'select',
        placeholder: '-- Select Party --',
        apiUrl: 'common/getparty',
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data?.items) ? data.data.items : (Array.isArray(data?.items) ? data.items : (Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : [])));
          return list.map((item: any) => ({
            value: String(item.id),
            label: item.partyName || item.party || item.name
          }));
        },
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'occupationId',
        name: 'occupationId',
        label: 'Occupation',
        type: 'select',
        placeholder: '-- Select Occupation --',
        apiUrl: 'common/getoccupation',
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data?.items) ? data.data.items : (Array.isArray(data?.items) ? data.items : (Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : [])));
          return list.map((item: any) => ({
            value: String(item.id),
            label: item.occupationName || item.occupation || item.name
          }));
        },
        validations: [Validators.required],
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
      },
      {
        id: 'reason',
        name: 'reason',
        label: 'Reason',
        type: 'textarea',
        placeholder: 'Enter Reason',
        dependsOn: 'typeId',
        validations: [Validators.required],
        gridColSpan: 6,
        visibleIf: { field: 'typeId', operator: '==', value: '2' }
      }
    ]
  };

  constructor(
    private voterService: SahmatAsahmatService,
    private toastService: ToastService,
    private crudHandler: CrudHandlerService,
    private route: ActivatedRoute,
    private authService: AuthServiceService
  ) { }

  ngOnInit() {
    this.route.url.subscribe(url => {
      const path = url[0]?.path || '';
      this.isAsahmatView = path === 'asahmat-list';
      this.isListView = path.includes('-list');
      this.config.filterable = this.isListView;
      this.loading = true;
      if (this.isListView) {
        this.loadFilterOptions();
      }
      this.loadVoters();
    });
  }

  loadFilterOptions() {
    this.config.filters = [
      { key: 'boothIds', label: 'Booth', type: 'select', options: [], placeholder: '-- Select Booth --', multiple: true },
      { key: 'villageIds', label: 'Village', type: 'select', options: [], placeholder: '-- Select Village --', multiple: true },
      { key: 'partyIds', label: 'Party', type: 'select', options: [], placeholder: '-- Select Party --', multiple: true }
    ];

    const userId = this.authService.getUserId();
    // Load Booths
    // Using a manual http call for filters to avoid global loader if common/ is not in interceptor yet (it is)
    // But we need to make sure we don't block.

    // Load Booths
    this.voterService.getCommonData('boothNumber', userId).subscribe((res: any) => {
      const filter = this.config.filters?.find(f => f.key === 'boothIds');
      if (filter) {
        const list = Array.isArray(res?.data) ? res.data : (Array.isArray(res) ? res : []);
        filter.options = list.map((b: any) => ({
          label: `Booth No. ${b.boothNumber}`,
          value: String(b.boothId || b.id)
        }));
      }
    });

    // Load All Villages initially
    this.voterService.getCommonData('village', null, 500000).subscribe((res: any) => {
      const filter = this.config.filters?.find(f => f.key === 'villageIds');
      if (filter) {
        const list = Array.isArray(res?.data?.items) ? res.data.items : (Array.isArray(res?.items) ? res.items : (Array.isArray(res?.data) ? res.data : (Array.isArray(res) ? res : [])));
        filter.options = list.map((v: any) => ({
          label: v.name || v.villageName,
          value: String(v.id || v.villageId)
        }));
      }
    });

    // Load Parties
    this.voterService.getCommonData('getparty', null, 500000).subscribe((res: any) => {
      const filter = this.config.filters?.find(f => f.key === 'partyIds');
      if (filter) {
        const list = Array.isArray(res?.data?.items) ? res.data.items : (Array.isArray(res?.items) ? res.items : (Array.isArray(res?.data) ? res.data : (Array.isArray(res) ? res : [])));
        filter.options = list.map((p: any) => ({
          label: p.partyName || p.party || p.name,
          value: String(p.id)
        }));
      }
    });
  }

  boothIds: string | null = null;
  villageIds: string | null = null;
  partyIds: string | null = null;

  handleFilterChange(filterState: Record<string, any>) {
    const processIds = (ids: any) => Array.isArray(ids) ? (ids.length > 0 ? ids.join(',') : null) : (ids || null);

    this.boothIds = processIds(filterState['boothIds']);
    this.villageIds = processIds(filterState['villageIds']);
    this.partyIds = processIds(filterState['partyIds']);

    // Cascade villages when booth changes
    if (filterState['boothIds'] && Array.isArray(filterState['boothIds']) && filterState['boothIds'].length === 1) {
      const boothId = filterState['boothIds'][0];
      this.voterService.getCommonData(`villagesByBoothId?boothId=${boothId}`).subscribe((res: any) => {
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
      partyIds: this.partyIds
    };

    // Set id and TypeId parameters only if we are in a specific list view
    if (this.isListView) {
      params.id = this.isAsahmatView ? 2 : 1;
      params['TypeId'] = this.isAsahmatView ? 2 : 1;
    }

    const userId = this.authService.getUserId();
    if (userId) {
      params.userId = userId;
    }

    this.voterService.getAllSahmatAsahmat(params).subscribe({
      next: (response) => {
        const dataWrap = response.data;
        const items = dataWrap?.items || (Array.isArray(dataWrap) ? dataWrap : []);

        this.voterList = items.map((item: any) => ({
          ...item,
          isAsahmat: item.typeId === 2,
          type: item.typeId === 2 ? 'Asahmat' : 'Sahmat',
          villageName: Array.isArray(item.villages) ? item.villages.map((v: any) => v.villageName).join(', ') : '',
          villageId: Array.isArray(item.villages) ? item.villages.map((v: any) => v.villageId) : []
        }));

        this.totalCount = dataWrap?.totalCount || this.voterList.length;

        // Visibility adjustments
        if (this.isAsahmatView) {
          // Asahmat only
          this.columns = this.columns.map(col => {
            if (col.key === 'reason') return { ...col, visible: true };
            if (col.key === 'type') return { ...col, visible: false };
            return col;
          });
        } else if (this.route.snapshot.url[0]?.path === 'sahmat-list') {
          // Sahmat only
          this.columns = this.columns.map(col => {
            if (col.key === 'reason') return { ...col, visible: false };
            if (col.key === 'type') return { ...col, visible: false };
            return col;
          });
        } else {
          // Master view (both)
          this.columns = this.columns.map(col => {
            if (col.key === 'reason') return { ...col, visible: true };
            if (col.key === 'type') return { ...col, visible: true };
            return col;
          });
        }
        this.loading = false;
      },
      error: (err) => {
        console.error('Error fetching voters:', err);
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
    console.log('Selected voters:', selected);
  }

  handleAction(event: any) {
    const { action, row } = event;
    if (action.id === 'delete') {
      this.crudHandler.handleRequest(
        this.voterService.deleteSahmatAsahmat(row.id),
        'Deleted',
        'Voter deleted successfully!',
        () => this.loadVoters(),
        true,
        ModulePermission.Sahmat
      );
    } else if (action.id === 'edit') {
      const editData = { ...row };
      ['id', 'boothId', 'typeId', 'partyId', 'occupationId'].forEach(key => {
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
      typeId: Number(raw.typeId),
      villageId: Array.isArray(raw.villageId) ? raw.villageId.map((v: any) => Number(v)) : (raw.villageId ? [Number(raw.villageId)] : []),
      name: raw.name,
      age: Number(raw.age),
      mobile: raw.mobile,
      partyId: Number(raw.partyId),
      occupationId: Number(raw.occupationId),
      reason: raw.reason,
      voterId: raw.voterId,
      userId: this.authService.getUserId()
    };

    const isUpdate = !!submitData.id;
    const request = isUpdate
      ? this.voterService.updateSahmatAsahmat(submitData)
      : this.voterService.createSahmatAsahmat(submitData);

    this.crudHandler.handleRequest(
      request,
      isUpdate ? 'Updated' : 'Success',
      `Voter ${isUpdate ? 'updated' : 'created'} successfully!`,
      () => this.loadVoters(),
      true,
      ModulePermission.Sahmat
    );
  }

  handleExport(format: string) {
    if (!format) return;
    this.isExporting = true;
    const request = format === 'excel' 
      ? this.voterService.exportToExcel(this.isAsahmatView) 
      : this.voterService.exportToPdf(this.isAsahmatView);

    const viewName = this.isAsahmatView ? 'Asahmat' : 'Sahmat';

    request.subscribe({
      next: (blob: Blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `${viewName}_List.${format === 'excel' ? 'xlsx' : 'pdf'}`;
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
        document.body.removeChild(a);
        this.isExporting = false;
        this.toastService.showSuccess('Success', `${viewName} list exported to ${format.toUpperCase()} successfully!`);
      },
      error: (err: any) => {
        console.error(`Error exporting to ${format}:`, err);
        this.toastService.showError('Error', `Failed to export Sahmat/Asahmat list to ${format.toUpperCase()}`);
        this.isExporting = false;
      }
    });
  }
}
