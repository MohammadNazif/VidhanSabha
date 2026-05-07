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
import { VidhanSabhaCountService } from '../../../Services/Admin/vidhansabha-count/vidhansabha-count.service';

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
    { key: 'stateName', label: 'State Name' },
    { key: 'vidhanSabhaCount', label: 'Vidhansabha Count', sortable: true }
  ];

  config: TableConfig = {
    selectable: false,
    // filterable: true,
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
    { id: 'edit', label: '', variant: 'default', icon: 'edit', show: () => !this.isStatePrabhari() },
    { id: 'delete', label: '', variant: 'danger', icon: 'delete', show: () => !this.isStatePrabhari() }
  ];

  isStatePrabhari(): boolean {
    return (this.authService.getRole() || '').toUpperCase().trim() === 'STATEPRABHARI';
  }

  defaultStateId: string | null = null;

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

  addDistrictConfig: FormConfig = {
    title: 'Assign District Count',
    submitLabel: 'Assign',
    fields: [
      {
        id: 'stateId',
        name: 'stateId',
        label: 'Select State',
        type: 'select',
        apiUrl: () => `state/getAll`,
        apiMapper: (list: any[]) => list.map(item => ({ value: String(item.id), label: item.name })),
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'districtId',
        name: 'districtId',
        label: 'Select District',
        type: 'select',
        placeholder: 'Select District',
        dependsOn: 'stateId',
        apiUrl: (stateId: any) => `common/getdistrict?id=${stateId}`,
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : []);
          return list.map((item: any) => ({
            value: String(item.id),
            label: item.districtName
          }));
        },
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'vidhanSabhaCount',
        name: 'vidhanSabhaCount',
        label: 'Vidhan Sabha Count',
        type: 'number',
        placeholder: 'Enter count',
        validations: [Validators.required, Validators.min(0)],
        gridColSpan: 6
      }
    ]
  };


  constructor(
    private stateService: StateService,
    private statePrabhariService: StatePrabhariService,
    private districtService: DistrictService,
    private vidhanSabhaCountService: VidhanSabhaCountService,
    private toastService: ToastService,
    private crudHandler: CrudHandlerService,
    private authService: AuthServiceService
  ) { }

  ngOnInit() {
    const role = (this.authService.getRole() || '').toUpperCase().trim();
    if (role === 'STATEPRABHARI') {
      this.columns = [
        { key: 'stateName', label: 'State Name' },
        { key: 'vidhanSabhaCount', label: 'Vidhansabha Count', sortable: true },
        { key: 'remainingCount', label: 'Remaining Count', sortable: true }
      ];

    }
    this.loadStates();
  }

  loadStates() {
    const userId = this.authService.getUserId();
    const isPrabhari = this.isStatePrabhari();

    const request = (isPrabhari && userId)
      ? this.vidhanSabhaCountService.getAllByUserId(userId)
      : this.stateService.getAllStates();

    request.subscribe({
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

        // Set default state ID and simplify fields for State Prabhari
        if (isPrabhari && this.stateList.length > 0) {
          const stateRecord = this.stateList[0];
          this.defaultStateId = String(stateRecord.stateId || stateRecord.id);

          this.addDistrictConfig.fields = this.addDistrictConfig.fields.filter(f => f.id !== 'stateId');
          const districtField = this.addDistrictConfig.fields.find(f => f.id === 'districtId');
          if (districtField) {
            delete districtField.dependsOn;
            districtField.apiUrl = () => `common/getdistrict?id=${this.defaultStateId}`;
            districtField.apiMapper = (data: any) => {
              const list = Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : []);
              return list.map((item: any) => ({
                value: String(item.id),
                label: item.districtName,
                disabled: this.stateList.some(d => d.districtId === item.id)
              }));
            };
          }
        }
      },
      error: (err) => {
        console.error('Error fetching states:', err);
      }
    });
  }

  handleAction(event: any) {
    const { action, row } = event;
    console.log(row, "sasdd");
    if (action.id === 'delete') {
      this.crudHandler.handleRequest(
        this.stateService.deleteState(row.id),
        'Deleted',
        'State deleted successfully!',
        () => this.loadStates()
      );
    } else if (action.id === 'edit') {
      this.stateModal.openModal({
        ...row,
        stateId: row.stateId || row.id ? String(row.stateId || row.id) : null
      });
    } else if (action.id === 'add_prabhari') {
      console.log(row, "sd");
      this.prabhariModal.openModal({
        stateId: row.stateId || row.id,
        ...(row.prabhari || {})
      });
    } else if (action.id === 'assign_district') {
      console.log(row, "sd");
      this.districtModal.openModal({
        stateId: row.stateId || row.id,

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
    const userId = this.authService.getUserId();

    if (!userId) {
      this.toastService.showError('Error', 'User ID missing');
      return;
    }

    const countData = {
      userId: String(userId),
      stateId: Number(raw.stateId || this.defaultStateId),
      districtId: Number(raw.districtId),
      vidhanSabhaCount: Number(raw.vidhanSabhaCount)
    };

    this.crudHandler.handleRequest(
      this.vidhanSabhaCountService.createVidhanSabhaCount(countData),
      'Assigned',
      'District Vidhan Sabha count assigned successfully!',
      () => this.loadStates()
    );
  }
}
