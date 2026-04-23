import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Validators } from '@angular/forms';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';
import { TableAction, TableColumn, TableConfig } from '../../shared/generic-table/generic-table.types';
import { GenericModalButtonComponent } from '../../shared/generic-modal-form/generic-modal-button.component';
import { FormConfig, FormResult } from '../../shared/generic-modal-form/generic-form.types';
import { PageHeaderComponent } from '../../shared/page-header/page-header.component';
import { PrabhavshaliService } from '../../../Services/Admin/prabhavshali/prabhavshali.service';
import { ToastService } from '../../../Services/common/toast/toast.service';
import { CrudHandlerService } from '../../../Services/common/crud-handler.service';

@Component({
  selector: 'app-prabhavshali',
  standalone: true,
  imports: [CommonModule, PageHeaderComponent, GenericTableComponent, GenericModalButtonComponent],
  templateUrl: './prabhavshali.component.html',
  styleUrl: './prabhavshali.component.css'
})
export class PrabhavshaliComponent implements OnInit {
  @ViewChild('prabhavshaliModal') prabhavshaliModal!: GenericModalButtonComponent;

  personList: any[] = [];
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
    { key: 'name', label: 'Name', sortable: true },
    { key: 'designationName', label: 'Designation', sortable: true },
    { key: 'mobile', label: 'Mobile No.', sortable: true },
    { key: 'categoryName', label: 'Category', sortable: true },
    { key: 'castName', label: 'Caste', sortable: true },
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
    private crudHandler: CrudHandlerService
  ) { }

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    const params = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      searchTerm: this.searchTerm,
      sortBy: this.sortBy,
      isDescending: this.isDescending
    };

    this.prabhavshaliService.getAllPrabhavshali(params).subscribe({
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
      },
      error: (err) => console.error('Error fetching Prabhavshali list:', err)
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
        () => this.loadData()
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
      description: raw.description
    };

    const isUpdate = !!submitData.id;
    const request = isUpdate
      ? this.prabhavshaliService.updatePrabhavshali(submitData)
      : this.prabhavshaliService.createPrabhavshali(submitData);

    this.crudHandler.handleRequest(
      request,
      isUpdate ? 'Updated' : 'Success',
      `Prabhavshali Vyakt ${isUpdate ? 'updated' : 'created'} successfully!`,
      () => this.loadData()
    );
  }
}
