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

@Component({
  selector: 'app-sahmat-asahmat',
  standalone: true,
  imports: [CommonModule, PageHeaderComponent, GenericTableComponent, GenericModalButtonComponent],
  templateUrl: './sahmat-asahmat.component.html',
  styleUrl: './sahmat-asahmat.component.css'
})
export class SahmatAsahmatComponent implements OnInit {
  @ViewChild('voterModal') voterModal!: GenericModalButtonComponent;

  voterList: any[] = [];
  totalCount = 0;
  
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
    defaultPageSize: 50
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
      this.loadVoters();
    });
  }

  loadVoters() {
    const params: any = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      searchTerm: this.searchTerm,
      sortBy: this.sortBy,
      isDescending: this.isDescending
    };

    // Filter by type on server if possible, or handle locally if backend doesn't support TypeId filter
    // For now, we'll assume we want the full list and we can filter locally or pass type if supported
    if (this.isAsahmatView) {
      params['TypeId'] = 2;
    } else if (this.route.snapshot.url[0]?.path === 'sahmat-list') {
      params['TypeId'] = 1;
    }

    this.voterService.getAllSahmatAsahmat(params).subscribe({
      next: (response) => {
        const dataWrap = response.data;
        const items = dataWrap?.items || (Array.isArray(dataWrap) ? dataWrap : []);
        
        this.voterList = items.map((item: any) => ({
          ...item,
          isAsahmat: item.typeId === 2,
          villageName: Array.isArray(item.villages) ? item.villages.map((v: any) => v.villageName).join(', ') : '',
          villageId: Array.isArray(item.villages) ? item.villages.map((v: any) => v.villageId) : []
        }));

        this.totalCount = dataWrap?.totalCount || this.voterList.length;

        // Visibility adjustments for reason column
        if (this.isAsahmatView) {
          this.columns = this.columns.map(col => col.key === 'reason' ? { ...col, visible: true } : col);
        } else if (this.route.snapshot.url[0]?.path === 'sahmat-list') {
          this.columns = this.columns.map(col => col.key === 'reason' ? { ...col, visible: false } : col);
        } else {
          this.columns = this.columns.map(col => col.key === 'reason' ? { ...col, visible: true } : col);
        }
      },
      error: (err) => {
        console.error('Error fetching voters:', err);
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
        () => this.loadVoters()
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
      voterId: raw.voterId
    };

    const isUpdate = !!submitData.id;
    const request = isUpdate
      ? this.voterService.updateSahmatAsahmat(submitData)
      : this.voterService.createSahmatAsahmat(submitData);

    this.crudHandler.handleRequest(
      request,
      isUpdate ? 'Updated' : 'Success',
      `Voter ${isUpdate ? 'updated' : 'created'} successfully!`,
      () => this.loadVoters()
    );
  }
}
