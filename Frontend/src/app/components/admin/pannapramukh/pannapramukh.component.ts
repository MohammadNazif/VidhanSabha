import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Validators } from '@angular/forms';
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

@Component({
  selector: 'app-pannapramukh',
  standalone: true,
  imports: [CommonModule, PageHeaderComponent, GenericTableComponent, GenericModalButtonComponent],
  templateUrl: './pannapramukh.component.html',
  styleUrl: './pannapramukh.component.css'
})
export class PannapramukhComponent implements OnInit {
  @ViewChild('pannaModal') pannaModal!: GenericModalButtonComponent;

  pannaList: any[] = [];
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
    return role !== 'STATEPRABHARI' && role !== 'ADHYAKSH';
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
        id: 'profilePicture',
        name: 'profilePicture',
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
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.route.url.subscribe(url => {
      const path = url[0]?.path || '';
      this.isListView = path.includes('-list');
      this.loadData();
    });
  }

  loadData() {
    const params = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      searchTerm: this.searchTerm,
      sortBy: this.sortBy,
      isDescending: this.isDescending
    };

    this.pannaService.getAllPannapramukhs(params).subscribe({
      next: (response) => {
        const dataWrap = response.data;
        const items = dataWrap?.items || (Array.isArray(dataWrap) ? dataWrap : []);

        this.pannaList = items.map((item: any) => ({
          ...item,
          villageName: Array.isArray(item.villages) ? item.villages.map((v: any) => v.villageName).join(', ') : '',
          villageId: Array.isArray(item.villages) ? item.villages.map((v: any) => v.villageId) : []
        }));

        this.totalCount = dataWrap?.totalCount || this.pannaList.length;
      },
      error: (err) => {
        console.error('Error fetching panna pramukhs:', err);
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
        () => this.loadData()
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
    const submitData: any = {
      ...raw,
      id: raw.id ? Number(raw.id) : null,
      boothId: Number(raw.boothId),
      categoryId: Number(raw.categoryId),
      castId: Number(raw.castId),
      pannaNumber: Number(raw.pannaNumber),
      voterId: String(raw.voterId), // voterId is string in response JSON
      villageId: Array.isArray(raw.villageId) ? raw.villageId.map((v: any) => Number(v)) : Number(raw.villageId)
    };

    const isUpdate = !!submitData.id;
    const request = isUpdate
      ? this.pannaService.updatePannapramukh(submitData)
      : this.pannaService.createPannapramukh(submitData);

    this.crudHandler.handleRequest(
      request,
      isUpdate ? 'Updated' : 'Success',
      `Panna Pramukh ${isUpdate ? 'updated' : 'created'} successfully!`,
      () => this.loadData()
    );
  }
}
