import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Validators } from '@angular/forms';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';
import { TableAction, TableColumn, TableConfig } from '../../shared/generic-table/generic-table.types';
import { GenericModalButtonComponent } from '../../shared/generic-modal-form/generic-modal-button.component';
import { FormConfig, FormResult } from '../../shared/generic-modal-form/generic-form.types';
import { PageHeaderComponent } from '../../shared/page-header/page-header.component';
import { DoubleVoterService } from '../../../Services/Admin/double-voter/double-voter.service';
import { ToastService } from '../../../Services/common/toast/toast.service';
import { CrudHandlerService } from '../../../Services/common/crud-handler.service';
import { AuthServiceService } from '../../../Services/Auth/auth.service';
import { ModulePermission } from '../../../models/module-permission.enum';

@Component({
  selector: 'app-double-voter',
  standalone: true,
  imports: [CommonModule, PageHeaderComponent, GenericTableComponent, GenericModalButtonComponent],
  templateUrl: './double-voter.component.html',
  styleUrl: './double-voter.component.css'
})
export class DoubleVoterComponent implements OnInit {
  @ViewChild('voterModal') voterModal!: GenericModalButtonComponent;

  voterList: any[] = [];
  totalCount = 0;
  
  // Server-side state
  pageNumber = 1;
  pageSize = 50;
  searchTerm = '';
  sortBy = '';
  isDescending = false;

  columns: TableColumn[] = [
    { key: 'boothNumber', label: 'Booth No.', sortable: true },
    { key: 'villageName', label: 'Village', sortable: true },
    { key: 'name', label: 'Voter Name', sortable: true },
    { key: 'fatherName', label: 'Father Name', sortable: true },
    { key: 'voterId', label: 'Voter ID', sortable: true },
    { key: 'currentAddress', label: 'Current Address', sortable: true },
    { key: 'description', label: 'Description', sortable: true }
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
    { id: 'edit', label: '', variant: 'default', icon: 'edit' },
    { id: 'delete', label: '', variant: 'danger', icon: 'delete' }
  ];

  addVoterConfig: FormConfig = {
    title: 'Register Double Voter',
    submitLabel: 'Save Voter',
    fields: [
      {
        id: 'boothId',
        name: 'boothId',
        label: 'Booth No',
        type: 'select',
        placeholder: '-- Select Booth --',
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
        label: 'Voter Name',
        type: 'text',
        placeholder: 'Enter name',
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'fatherName',
        name: 'fatherName',
        label: 'Father\'s Name',
        type: 'text',
        placeholder: 'Enter father\'s name',
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
        id: 'previousAddress',
        name: 'previousAddress',
        label: 'Previous Address',
        type: 'textarea',
        placeholder: 'Enter previous registration address',
        gridColSpan: 6
      },
      {
        id: 'currentAddress',
        name: 'currentAddress',
        label: 'Current Address',
        type: 'textarea',
        placeholder: 'Enter current address',
        validations: [Validators.required],
        gridColSpan: 12
      },
      {
        id: 'description',
        name: 'description',
        label: 'Description/Notes',
        type: 'textarea',
        placeholder: 'Additional information...',
        gridColSpan: 12
      }
    ]
  };

  constructor(
    private voterService: DoubleVoterService,
    private toastService: ToastService,
    private crudHandler: CrudHandlerService,
    private authService: AuthServiceService
  ) { }

  ngOnInit() {
    this.loadVoters();
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

    this.voterService.getAllDoubleVoters(params).subscribe({
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
      error: (err) => console.error('Error fetching double voters:', err)
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

  handleAction(event: any) {
    const { action, row } = event;
    if (action.id === 'delete') {
      this.crudHandler.handleRequest(
        this.voterService.deleteDoubleVoter(row.id),
        'Deleted',
        'Voter entry deleted successfully!',
        () => this.loadVoters(),
        true,
        ModulePermission.DoubleVoter
      );
    } else if (action.id === 'edit') {
      const editData = { ...row };
      ['id', 'boothId'].forEach(key => {
        if (editData[key]) editData[key] = String(editData[key]);
      });
      if (editData.villageId) {
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
      voterId: raw.voterId,
      previousAddress: raw.previousAddress,
      currentAddress: raw.currentAddress,
      description: raw.description,
      userId: this.authService.getUserId()
    };

    const isUpdate = !!submitData.id;
    const request = isUpdate
      ? this.voterService.updateDoubleVoter(submitData)
      : this.voterService.createDoubleVoter(submitData);

    this.crudHandler.handleRequest(
      request,
      isUpdate ? 'Updated' : 'Success',
      `Double voter ${isUpdate ? 'updated' : 'registered'} successfully!`,
      () => this.loadVoters(),
      true,
      ModulePermission.DoubleVoter
    );
  }

  handleSelection(selected: any[]) {
    console.log('Selected voters:', selected);
  }
}
