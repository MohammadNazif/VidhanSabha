import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Validators } from '@angular/forms';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';
import { TableAction, TableColumn, TableConfig } from '../../shared/generic-table/generic-table.types';
import { GenericModalButtonComponent } from '../../shared/generic-modal-form/generic-modal-button.component';
import { FormConfig, FormResult } from '../../shared/generic-modal-form/generic-form.types';
import { PageHeaderComponent } from '../../shared/page-header/page-header.component';
import { PravasivoterService } from '../../../Services/Admin/pravasivoter/pravasivoter.service';
import { ToastService } from '../../../Services/common/toast/toast.service';
import { CrudHandlerService } from '../../../Services/common/crud-handler.service';
import { AuthServiceService } from '../../../Services/Auth/auth.service';
import { ActivatedRoute } from '@angular/router';
import { ModulePermission } from '../../../models/module-permission.enum';

@Component({
  selector: 'app-pravasi-voter',
  standalone: true,
  imports: [CommonModule, PageHeaderComponent, GenericTableComponent, GenericModalButtonComponent],
  templateUrl: './pravasi-voter.component.html',
  styleUrl: './pravasi-voter.component.css'
})
export class PravasiVoterComponent implements OnInit {
  @ViewChild('voterModal') voterModal!: GenericModalButtonComponent;

  voterList: any[] = [];
  totalCount = 0;
  
  // Server-side state
  pageNumber = 1;
  pageSize = 50;
  searchTerm = '';
  sortBy = '';
  isDescending = false;

  isListView = false;

  columns: TableColumn[] = [
    { key: 'boothNumber', label: 'Booth No.', sortable: true },
    { key: 'villageName', label: 'Village', sortable: true },
    { key: 'name', label: 'Name', sortable: true },
    { key: 'mobile', label: 'Mobile No.', sortable: true },
    { key: 'categoryName', label: 'Category', sortable: true },
    { key: 'castName', label: 'Caste', sortable: true },
    { key: 'occupation', label: 'Occupation', sortable: true },
    { key: 'voterId', label: 'Voter ID', sortable: true }
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
    title: 'Add Pravasi Voter',
    submitLabel: 'Save',
    fields: [
      {
        id: 'boothId',
        name: 'boothId',
        label: 'Booth No',
        type: 'select',
        placeholder: '-- Select Booth No --',
        apiUrl: 'booth/getall',
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : []);
          return list.map((item: any) => ({
            value: String(item.id),
            label: `${item.boothNumber} - ${item.pollingStationName || ''}`
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
          const list = Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : []);
          return list.map((item: any) => ({
            value: String(item.id),
            label: item.name
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
        id: 'castId',
        name: 'castId',
        label: 'Caste',
        type: 'select',
        placeholder: '-- Select Caste --',
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
        id: 'occupationId',
        name: 'occupationId',
        label: 'Occupation',
        type: 'select',
        placeholder: '-- Select Occupation --',
        apiUrl: 'common/getoccupation',
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : []);
          return list.map((item: any) => ({
            value: String(item.id),
            label: item.occupation
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
        id: 'currentAddress',
        name: 'currentAddress',
        label: 'Current Address',
        type: 'textarea',
        placeholder: 'Enter Current Address',
        validations: [Validators.required],
        gridColSpan: 12
      }
    ]
  };

  constructor(
    private voterService: PravasivoterService,
    private toastService: ToastService,
    private crudHandler: CrudHandlerService,
    private authService: AuthServiceService,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.route.url.subscribe(url => {
      const path = url[0]?.path || '';
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

    const userId = this.authService.getUserId();
    if (userId) {
      params.userId = userId;
    }

    this.voterService.getAllPravasivoters(params).subscribe({
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
      },
      error: (err) => {
        console.error('Error fetching pravasi voters:', err);
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
        this.voterService.deletePravasivoter(row.id),
        'Deleted',
        'Pravasi Voter deleted successfully!',
        () => this.loadVoters(),
        true,
        ModulePermission.PravashiVoter
      );
    } else if (action.id === 'edit') {
      const editData = { ...row };
      ['id', 'boothId', 'categoryId', 'castId', 'occupationId'].forEach(key => {
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
      mobile: raw.mobile,
      categoryId: Number(raw.categoryId),
      castId: Number(raw.castId),
      occupationId: Number(raw.occupationId),
      voterId: raw.voterId,
      currentAddress: raw.currentAddress,
      userId: this.authService.getUserId()
    };

    const isUpdate = !!submitData.id;
    const request = isUpdate
      ? this.voterService.updatePravasivoter(submitData)
      : this.voterService.createPravasivoter(submitData);

    this.crudHandler.handleRequest(
      request,
      isUpdate ? 'Updated' : 'Success',
      `Pravasi Voter ${isUpdate ? 'updated' : 'created'} successfully!`,
      () => this.loadVoters(),
      true,
      ModulePermission.PravashiVoter
    );
  }
}
