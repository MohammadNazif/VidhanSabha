import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Validators } from '@angular/forms';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';
import { TableAction, TableColumn, TableConfig } from '../../shared/generic-table/generic-table.types';
import { GenericModalButtonComponent } from '../../shared/generic-modal-form/generic-modal-button.component';
import { FormConfig, FormResult } from '../../shared/generic-modal-form/generic-form.types';

import { PageHeaderComponent } from '../../shared/page-header/page-header.component';
import { DistrictService } from '../../../Services/Admin/district/district.service';
import { StateService } from '../../../Services/Admin/state/state.service';
import { ToastService } from '../../../Services/common/toast/toast.service';
import { CrudHandlerService } from '../../../Services/common/crud-handler.service';
import { DistrictPrabhariService } from '../../../Services/Admin/district-prabhari/district-prabhari.service';
import { VidhanSabhaCountService } from '../../../Services/Admin/vidhansabha-count/vidhansabha-count.service';
import { VidhanSabhaService } from '../../../Services/Admin/vidhansabha/vidhansabha.service';
import { VidhanSabhaPrabhariService } from '../../../Services/Admin/vidhansabha-prabhari/vidhansabha-prabhari.service';
import { AuthServiceService } from '../../../Services/Auth/auth.service';

@Component({
  selector: 'app-district',
  standalone: true,
  imports: [CommonModule, PageHeaderComponent, GenericTableComponent, GenericModalButtonComponent],
  templateUrl: './district.component.html',
  styleUrl: './district.component.css'
})
export class DistrictComponent implements OnInit {
  @ViewChild('districtModal') districtModal!: GenericModalButtonComponent;
  @ViewChild('vidhanSabhaModal') vidhanSabhaModal!: GenericModalButtonComponent;

  districtList: any[] = [];
  defaultStateId: string | null = null;

  isStatePrabhari(): boolean {
    return (this.authService.getRole() || '').toUpperCase().trim() === 'STATEPRABHARI';
  }

  columns: TableColumn[] = [
    { key: 'dsitrictName', label: 'District Name', sortable: true },
    { key: 'vidhanSabhaCount', label: 'VS Count', sortable: true },
    { key: 'remainingCount', label: 'Remaining Count', sortable: true }
  ];

  config: TableConfig = {
    selectable: false,
    filterable: true,
    paginated: true,
    defaultPageSize: 10,
    pageSizeOptions: [10, 20, 50],
    searchable: true,
    searchPlaceholder: 'Search districts...',
    showRowNumbers: true,
    striped: true,
    hoverable: true
  };

  actions: TableAction[] = [
    { id: 'add_vidhansabha', label: 'Vidhan Sabha', variant: 'primary', icon: 'layout' },
    { id: 'edit', label: '', variant: 'default', icon: 'edit' },
    { id: 'delete', label: '', variant: 'danger', icon: 'delete' }
  ];

  addDistrictConfig: FormConfig = {
    title: 'Add New District',
    submitLabel: 'Create District',
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
        label: ' District',
        type: 'select',
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

  addVidhanSabhaConfig: FormConfig = {
    title: 'Add Vidhan Sabha',
    submitLabel: 'Create',
    fields: [
      {
        id: 'name',
        name: 'name',
        label: 'Vidhan Sabha Name',
        type: 'text',
        placeholder: 'Enter name',
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'vidhanSabhaNumber',
        name: 'vidhanSabhaNumber',
        label: 'Vidhan Sabha Number',
        type: 'number',
        placeholder: 'Enter number',
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'assignPrabhari',
        name: 'assignPrabhari',
        label: 'Assign Prabhari?',
        type: 'select',
        options: [
          { label: 'Yes', value: 'Yes' },
          { label: 'No', value: 'No' }
        ],
        defaultValue: 'No',
        validations: [Validators.required],
        gridColSpan: 12
      },
      {
        id: 'prabhariName',
        name: 'prabhariName',
        label: 'Prabhari Name',
        type: 'text',
        placeholder: 'Enter full name',
        visibleIf: { field: 'assignPrabhari', operator: '==', value: 'Yes' },
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'prabhariEmail',
        name: 'prabhariEmail',
        label: 'Prabhari Email',
        type: 'email',
        placeholder: 'Enter email',
        visibleIf: { field: 'assignPrabhari', operator: '==', value: 'Yes' },
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
        visibleIf: { field: 'assignPrabhari', operator: '==', value: 'Yes' },
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'contactNumber',
        name: 'contactNumber',
        label: 'Contact Number',
        type: 'text',
        placeholder: 'Enter phone number',
        visibleIf: { field: 'assignPrabhari', operator: '==', value: 'Yes' },
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
        visibleIf: { field: 'assignPrabhari', operator: '==', value: 'Yes' },
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
        visibleIf: { field: 'assignPrabhari', operator: '==', value: 'Yes' },
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'education',
        name: 'education',
        label: 'Education',
        type: 'text',
        placeholder: 'Enter education',
        visibleIf: { field: 'assignPrabhari', operator: '==', value: 'Yes' },
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'profession',
        name: 'profession',
        label: 'Profession',
        type: 'text',
        placeholder: 'Enter profession',
        visibleIf: { field: 'assignPrabhari', operator: '==', value: 'Yes' },
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'currentAddress',
        name: 'currentAddress',
        label: 'Current Address',
        type: 'textarea',
        placeholder: 'Enter full address',
        visibleIf: { field: 'assignPrabhari', operator: '==', value: 'Yes' },
        gridColSpan: 12
      },
      {
        id: 'profile',
        name: 'profile',
        label: 'Profile Photo',
        type: 'file',
        visibleIf: { field: 'assignPrabhari', operator: '==', value: 'Yes' },
        gridColSpan: 12
      }
    ]
  };


  constructor(
    private districtService: DistrictService,
    private districtPrabhariService: DistrictPrabhariService,
    private stateService: StateService,
    private vidhanSabhaCountService: VidhanSabhaCountService,
    private vidhanSabhaService: VidhanSabhaService,
    private vidhanSabhaPrabhariService: VidhanSabhaPrabhariService,
    private authService: AuthServiceService,
    private toastService: ToastService,
    private crudHandler: CrudHandlerService
  ) { }

  ngOnInit() {
    if (this.isStatePrabhari()) {
      // Automatically hide state selection for State Prabhari
      const stateField = this.addDistrictConfig.fields.find(f => f.id === 'stateId');
      if (stateField) {
        stateField.type = 'hidden';
      }

      // Fetch the assigned state ID and configure modal
      this.stateService.getAllStates().subscribe({
        next: (response) => {
          const list = response?.data || response || [];
          if (list.length > 0) {
            this.defaultStateId = String(list[0].stateId || list[0].id);

            // Standardize Add District configuration for State Prabhari
            this.addDistrictConfig.fields = this.addDistrictConfig.fields.filter(f => f.id !== 'stateId');
            const districtField = this.addDistrictConfig.fields.find(f => f.id === 'districtId');
            if (districtField) {
              delete (districtField as any).dependsOn;
              districtField.id = 'districtId';
              districtField.name = 'districtId';
              districtField.apiUrl = () => `common/getdistrict?id=${this.defaultStateId}`;
              districtField.apiMapper = (data: any) => {
                const list = Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : []);
                return list.map((item: any) => ({
                  value: String(item.id),
                  label: item.districtName,
                  disabled: this.districtList.some(d => d.districtId === item.id)
                }));
              };
            }
          }
        }
      });
    }
    this.loadDistricts();
  }

  loadDistricts() {
    const userId = this.authService.getUserId();
    const isPrabhari = this.isStatePrabhari();

    const request = (isPrabhari && userId)
      ? this.vidhanSabhaCountService.getDistrictCountAllByUserId(userId)
      : this.districtService.getAllDistricts();

    request.subscribe({
      next: (response) => {
        if (response && response.isSuccess) {
          this.districtList = response.data;
        } else if (Array.isArray(response)) {
          this.districtList = response;
        } else {
          this.districtList = [];
        }
      },
      error: (err) => console.error('Error fetching districts:', err)
    });
  }

  handleAction(event: any) {
    const { action, row } = event;
    if (action.id === 'delete') {
      this.crudHandler.handleRequest(
        this.districtService.deleteDistrict(row.id),
        'Deleted',
        'District deleted successfully!',
        () => this.loadDistricts()
      );
    } else if (action.id === 'edit') {
      this.districtModal.openModal({
        ...row,
        stateId: String(row.stateId)
      });
    } else if (action.id === 'add_vidhansabha') {
      this.vidhanSabhaModal.openModal({
        districtId: row.districtId || row.id,
        stateId: row.stateId
      });
    }
  }

  handleFormSubmit(result: FormResult) {
    if (!result.status) return;

    const raw = result.data;
    const submitData = {
      ...raw,
      id: raw.id ? Number(raw.id) : null,
      userId: this.authService.getUserId(),
      stateId: Number(raw.stateId || this.defaultStateId),
      districtId: Number(raw.districtId || raw.id),
      vidhanSabhaCount: Number(raw.vidhanSabhaCount)
    };

    const isUpdate = !!submitData.id;
    const request = isUpdate
      ? this.vidhanSabhaCountService.createVidhanSabhaCount(submitData) // Assuming update uses same endpoint or standard service
      : this.vidhanSabhaCountService.createVidhanSabhaCount(submitData);

    this.crudHandler.handleRequest(
      request,
      isUpdate ? 'Updated' : 'Success',
      `District count ${isUpdate ? 'updated' : 'created'} successfully!`,
      () => this.loadDistricts()
    );
  }

  handleVidhanSabhaSubmit(result: FormResult) {
    if (!result.status) return;

    const raw = result.data;
    const districtId = this.vidhanSabhaModal.initialData?.districtId;

    if (!districtId) {
      this.toastService.showError('Error', 'District ID missing');
      return;
    }

    const vsData = {
      name: raw.name,
      districtId: Number(districtId),
      vidhanSabhaNumber: Number(raw.vidhanSabhaNumber)
    };

    this.vidhanSabhaService.createVidhanSabha(vsData).subscribe({
      next: (response) => {
        if (response?.isSuccess && raw.assignPrabhari === 'Yes') {
          const vsId = response.data?.id;
          const prabhariData = {
            ...raw,
            vidhanSabhaId: Number(vsId),
            castId: Number(raw.castId),
            categoryId: Number(raw.categoryId)
          };
          this.vidhanSabhaPrabhariService.createPrabhari(prabhariData).subscribe({
            next: () => {
              this.toastService.showSuccess('Success', 'Vidhan Sabha and Prabhari created successfully!');
              this.loadDistricts();
            },
            error: (err) => {
              this.toastService.showError('Error', 'Vidhan Sabha created but Prabhari registration failed');
              console.error(err);
              this.loadDistricts();
            }
          });
        } else {
          this.toastService.showSuccess('Success', 'Vidhan Sabha created successfully!');
          this.loadDistricts();
        }
      },
      error: (err) => {
        this.toastService.showError('Error', 'Failed to create Vidhan Sabha');
        console.error(err);
      }
    });
  }

  handleExport(format: string) {
    if (!format) return;
    this.toastService.showSuccess('Export Started', `Successfully generated ${format.toUpperCase()} export!`);
  }
}
