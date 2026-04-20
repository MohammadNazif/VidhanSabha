import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Validators } from '@angular/forms';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';
import { TableAction, TableColumn, TableConfig } from '../../shared/generic-table/generic-table.types';
import { GenericModalButtonComponent } from '../../shared/generic-modal-form/generic-modal-button.component';
import { FormConfig, FormResult } from '../../shared/generic-modal-form/generic-form.types';

import { PageHeaderComponent } from '../../shared/page-header/page-header.component';
import { StatePrabhariService } from '../../../Services/Admin/state-prabhari/state-prabhari.service';
import { ToastService } from '../../../Services/common/toast/toast.service';
import { CrudHandlerService } from '../../../Services/common/crud-handler.service';

@Component({
  selector: 'app-state-prabhari-list',
  standalone: true,
  imports: [CommonModule, PageHeaderComponent, GenericTableComponent, GenericModalButtonComponent],
  templateUrl: './state-prabhari.component.html',
  styleUrl: './state-prabhari.component.css'
})
export class StatePrabhariListComponent implements OnInit {
  @ViewChild('prabhariModal') prabhariModal!: GenericModalButtonComponent;

  prabhariList: any[] = [];

  columns: TableColumn[] = [
    { key: 'stateName', label: 'State', sortable: true },
    { key: 'prabhariName', label: 'Prabhari', sortable: true },
    { key: 'prabhariEmail', label: 'Email', sortable: true },
    { key: 'gender', label: 'Gender', sortable: true },
    { key: 'contactNumber', label: 'Contact', sortable: true },
    { key: 'categoryName', label: 'Category', sortable: true },
    { key: 'castName', label: 'Caste', sortable: true },
    { key: 'education', label: 'Education', sortable: true },
    { key: 'profession', label: 'Profession', sortable: true },
    { key: 'currentAddress', label: 'Address', sortable: true }

  ];

  config: TableConfig = {
    selectable: false,
    filterable: true,
    paginated: true,
    defaultPageSize: 10,
    pageSizeOptions: [10, 20, 50],
    searchable: true,
    searchPlaceholder: 'Search prabharis...',
    showRowNumbers: true,
    striped: true,
    hoverable: true
  };

  actions: TableAction[] = [
    { id: 'edit', label: '', variant: 'default', icon: 'edit' },
    { id: 'delete', label: '', variant: 'danger', icon: 'delete' }
  ];

  addPrabhariConfig: FormConfig = {
    title: 'Register State Prabhari',
    submitLabel: 'Save Prabhari',
    fields: [
      {
        id: 'stateId',
        name: 'stateId',
        label: 'Select State',
        type: 'select',
        apiUrl: 'common/getstates',
        apiMapper: (data: any, formValues?: any) => {
          const list = Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : []);
          const existingStateIds = this.prabhariList.map(p => String(p.stateId));
          const currentFormStateId = formValues?.stateId;

          return list.map((item: any) => ({
            value: String(item.id),
            label: item.stateName,
            disabled: existingStateIds.includes(String(item.id)) && String(item.id) !== String(currentFormStateId)
          }));
        },
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'prabhariName',
        name: 'prabhariName',
        label: 'Prabhari Name',
        type: 'text',
        placeholder: 'Enter full name',
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'prabhariEmail',
        name: 'prabhariEmail',
        label: 'Prabhari Email',
        type: 'email',
        placeholder: 'Enter email',
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'gender',
        name: 'gender',
        label: 'Gender',
        type: 'select',
        options: [
          { label: 'Male', value: 'Male' },
          { label: 'Female', value: 'Female' }
        ],
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'contactNumber',
        name: 'contactNumber',
        label: 'Contact Number',
        type: 'text',
        placeholder: 'Enter phone number',
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
        apiUrl: (catId: any) => `common/cast?id=${catId}`,
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
        id: 'education',
        name: 'education',
        label: 'Education',
        type: 'text',
        placeholder: 'Enter education',
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'profession',
        name: 'profession',
        label: 'Profession',
        type: 'text',
        placeholder: 'Enter profession',
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'currentAddress',
        name: 'currentAddress',
        label: 'Current Address',
        type: 'textarea',
        placeholder: 'Enter full address',
        gridColSpan: 12
      },
      {
        id: 'profile',
        name: 'profile',
        label: 'Profile Photo',
        type: 'file',
        gridColSpan: 12
      }
    ]
  };

  constructor(
    private prabhariService: StatePrabhariService,
    private toastService: ToastService,
    private crudHandler: CrudHandlerService
  ) { }

  ngOnInit() {
    this.loadPrabharis();
  }

  loadPrabharis() {
    this.prabhariService.getAllPrabharis().subscribe({
      next: (response) => {
        if (response && response.isSuccess) {
          this.prabhariList = response.data;
        } else if (Array.isArray(response)) {
          this.prabhariList = response;
        } else if (response && Array.isArray(response.data)) {
          this.prabhariList = response.data;
        } else {
          this.prabhariList = [];
        }
      },
      error: (err) => console.error('Error fetching prabharis:', err)
    });
  }

  handleAction(event: any) {
    const { action, row } = event;
    if (action.id === 'delete') {
      const payload = { data: [row] };
      this.crudHandler.handleRequest(
        this.prabhariService.deletePrabhari(row.id, row.userId, payload),
        'Deleted',
        'Prabhari deleted successfully!',
        () => this.loadPrabharis()
      );
    } else if (action.id === 'edit') {
      this.prabhariModal.openModal({
        ...row,
        stateId: String(row.stateId),
        categoryId: String(row.categoryId),
        castId: String(row.castId)
      });
    }
  }

  handleFormSubmit(result: FormResult) {
    if (!result.status) return;

    const raw = result.data;
    const initialId = this.prabhariModal.initialData?.id;
    const isUpdate = (raw.id !== undefined && raw.id !== null) || (initialId !== undefined && initialId !== null);

    const submitData = {
      ...raw,
      id: isUpdate ? (raw.id ?? initialId) : null,
      stateId: Number(raw.stateId),
      castId: Number(raw.castId),
      categoryId: Number(raw.categoryId)
    };

    const request = isUpdate
      ? this.prabhariService.updatePrabhari(submitData)
      : this.prabhariService.createPrabhari(submitData);

    this.crudHandler.handleRequest(
      request,
      isUpdate ? 'Updated' : 'Created',
      `Prabhari successfully ${isUpdate ? 'updated' : 'created'}!`,
      () => this.loadPrabharis()
    );
  }
}
