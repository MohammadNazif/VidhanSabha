import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Validators } from '@angular/forms';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';
import { TableAction, TableColumn, TableConfig } from '../../shared/generic-table/generic-table.types';
import { GenericModalButtonComponent } from '../../shared/generic-modal-form/generic-modal-button.component';
import { FormConfig, FormResult } from '../../shared/generic-modal-form/generic-form.types';

import { PageHeaderComponent } from '../../shared/page-header/page-header.component';
import { StateService } from '../../../Services/Admin/state/state.service';
import { ToastService } from '../../../Services/common/toast/toast.service';
import { CrudHandlerService } from '../../../Services/common/crud-handler.service';
import { StatePrabhariService } from '../../../Services/Admin/state-prabhari/state-prabhari.service';
import { AuthServiceService } from '../../../Services/Auth/auth.service';
import { DistrictService } from '../../../Services/Admin/district/district.service';

@Component({
  selector: 'app-state',
  standalone: true,
  imports: [CommonModule, PageHeaderComponent, GenericTableComponent, GenericModalButtonComponent],
  templateUrl: './state.component.html',
  styleUrl: './state.component.css'
})
export class StateComponent implements OnInit {
  @ViewChild('stateModal') stateModal!: GenericModalButtonComponent;
  @ViewChild('prabhariModal') prabhariModal!: GenericModalButtonComponent;
  @ViewChild('districtModal') districtModal!: GenericModalButtonComponent;

  stateList: any[] = [];

  columns: TableColumn[] = [
    { key: 'stateName', label: 'State Name', type: 'avatar', sortable: true, avatarFallbackKey: 'stateName' },
    { key: 'vidhanSabhaCount', label: 'Vidhansabha Count', sortable: true }
  ];

  config: TableConfig = {
    selectable: false,
    filterable: true,
    paginated: true,
    defaultPageSize: 10,
    pageSizeOptions: [10, 20, 50],
    searchable: true,
    searchPlaceholder: 'Search states...',
    showRowNumbers: true,
    striped: true,
    hoverable: true
  };

  actions: TableAction[] = [
    { id: 'add_prabhari', label: 'Prabhari', variant: 'primary', icon: 'user', show: () => !this.isStatePrabhari() },
    { id: 'assign_district', label: 'Assign District', variant: 'primary', icon: 'map', show: () => this.isStatePrabhari() },
    { id: 'edit', label: '', variant: 'default', icon: 'edit' },
    { id: 'delete', label: '', variant: 'danger', icon: 'delete' }
  ];

  isStatePrabhari(): boolean {
    return (this.authService.getRole() || '').toUpperCase().trim() === 'STATEPRABHARI';
  }

  addStateConfig: FormConfig = {
    title: 'Add New State',
    submitLabel: 'Create State',
    fields: [
      {
        id: 'stateId',
        name: 'stateId',
        label: 'State Name',
        type: 'select',
        placeholder: 'Select state name',
        apiUrl: 'common/getstates',
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : []);
          return list.map((item: any) => ({
            value: String(item.id),
            label: item.stateName
          }));
        },
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'vidhanSabhaCount',
        name: 'vidhanSabhaCount',
        label: 'Vidhansabha Count',
        type: 'number',
        placeholder: 'Enter vidhansabha count',
        validations: [Validators.required],
        gridColSpan: 6
      }
    ]
  };

  addPrabhariConfig: FormConfig = {
    title: 'Register State Prabhari',
    submitLabel: 'Assign Prabhari',
    fields: [
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

  assignDistrictConfig: FormConfig = {
    title: 'Assign District Count',
    submitLabel: 'Assign',
    fields: [
      {
        id: 'stateId',
        name: 'stateId',
        label: '',
        type: 'hidden'
      },
      {
        id: 'districtId',
        name: 'districtId',
        label: 'Select District',
        type: 'select',
        placeholder: 'Select District',
        dependsOn: 'stateId',
        apiUrl: (stateId: any) => `district/getAll?stateId=${stateId}`,
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
        id: 'vidhanSabhaCount',
        name: 'vidhanSabhaCount',
        label: 'Vidhansabha Count',
        type: 'number',
        placeholder: 'Enter vidhansabha count',
        validations: [Validators.required],
        gridColSpan: 6
      }
    ]
  };

  openAssignDistrictModalTop() {
    const stateId = this.stateList && this.stateList.length > 0 ? (this.stateList[0].stateId || this.stateList[0].id) : null;
    if (stateId) {
      this.districtModal.openModal({ stateId });
    } else {
      this.toastService.showError('Error', 'State context missing. Please wait for load or refresh.');
    }
  }

  constructor(
    private stateService: StateService,
    private statePrabhariService: StatePrabhariService,
    private districtService: DistrictService,
    private toastService: ToastService,
    private crudHandler: CrudHandlerService,
    private authService: AuthServiceService
  ) { }

  ngOnInit() {
    const role = (this.authService.getRole() || '').toUpperCase().trim();
    if (role === 'STATEPRABHARI') {
      this.columns = [
        { key: 'stateName', label: 'State Name', type: 'avatar', sortable: true, avatarFallbackKey: 'stateName' },
        { key: 'vidhanSabhaCount', label: 'Vidhansabha Count', sortable: true },
        { key: 'remainingCount', label: 'Remaining Count', sortable: true }
      ];
    }
    this.loadStates();
  }

  loadStates() {
    this.stateService.getAllStates().subscribe({
      next: (response) => {
        if (response && response.isSuccess) {
          this.stateList = response.data;
        } else if (Array.isArray(response)) {
          this.stateList = response;
        } else if (response && Array.isArray(response.data)) {
          this.stateList = response.data;
        } else {
          this.stateList = [];
        }
      },
      error: (err) => {
        console.error('Error fetching states:', err);
      }
    });
  }

  handleAction(event: any) {
    const { action, row } = event;

    if (action.id === 'delete') {
      this.crudHandler.handleRequest(
        this.stateService.deleteState(row.id),
        'Deleted',
        'State deleted successfully!',
        () => this.loadStates()
      );
    } else if (action.id === 'edit') {
      this.stateModal.openModal(row);
    } else if (action.id === 'add_prabhari') {
      this.prabhariModal.openModal({
        stateId: row.stateId,
        ...(row.prabhari || {})
      });
    } else if (action.id === 'assign_district') {
      this.districtModal.openModal({
        stateId: row.stateId || row.id
      });
    }
  }

  handleFormSubmit(result: FormResult) {
    if (!result.status) return;

    const isUpdate = result.data.id || (this.stateModal.initialData && this.stateModal.initialData.id);
    if (isUpdate && !result.data.id) {
      result.data.id = this.stateModal.initialData.id;
    }

    const request = isUpdate
      ? this.stateService.updateState(result.data)
      : this.stateService.createState(result.data);

    this.crudHandler.handleRequest(
      request,
      isUpdate ? 'Updated' : 'Success',
      `State ${isUpdate ? 'updated' : 'created'} successfully!`,
      () => this.loadStates()
    );
  }

  handlePrabhariSubmit(result: FormResult) {
    if (!result.status) return;

    const raw = result.data;
    const stateId = this.prabhariModal.initialData?.stateId;

    if (!stateId) {
      this.toastService.showError('Error', 'State ID missing');
      return;
    }

    const isUpdate = !!(raw.id || this.prabhariModal.initialData?.id);
    const submitData = {
      ...raw,
      id: isUpdate ? (raw.id || this.prabhariModal.initialData.id) : null,
      stateId: Number(stateId),
      castId: Number(raw.castId),
      categoryId: Number(raw.categoryId)
    };

    const request = isUpdate
      ? this.statePrabhariService.updatePrabhari(submitData)
      : this.statePrabhariService.createPrabhari(submitData);

    this.crudHandler.handleRequest(
      request,
      isUpdate ? 'Updated' : 'Created',
      `Prabhari successfully ${isUpdate ? 'updated' : 'assigned'} to the state!`,
      () => this.loadStates()
    );
  }

  handleExport(format: string) {
    if (!format) return;
    this.toastService.showSuccess('Export Started', `Successfully generated ${format.toUpperCase()} export!`);
  }

  handleDistrictSubmit(result: FormResult) {
    if (!result.status) return;

    const raw = result.data;
    const stateId = this.districtModal.initialData?.stateId;

    if (!stateId) {
      this.toastService.showError('Error', 'State ID missing');
      return;
    }

    const submitData = {
      id: Number(raw.districtId),
      stateId: Number(stateId),
      vidhanSabhaCount: Number(raw.vidhanSabhaCount)
    };

    // Doing partial update via District Service if an update is meant
    this.crudHandler.handleRequest(
      this.districtService.updateDistrict(submitData),
      'Assigned',
      `District assigned successfully with count ${submitData.vidhanSabhaCount}!`,
      () => this.loadStates()
    );
  }
}
