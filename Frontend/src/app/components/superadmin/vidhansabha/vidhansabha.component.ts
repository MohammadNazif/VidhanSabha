import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Validators } from '@angular/forms';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';
import { TableAction, TableColumn, TableConfig } from '../../shared/generic-table/generic-table.types';
import { GenericModalButtonComponent } from '../../shared/generic-modal-form/generic-modal-button.component';
import { FormConfig, FormResult } from '../../shared/generic-modal-form/generic-form.types';

import { PageHeaderComponent } from '../../shared/page-header/page-header.component';
import { VidhanSabhaService } from '../../../Services/Admin/vidhansabha/vidhansabha.service';
import { DistrictService } from '../../../Services/Admin/district/district.service';
import { ToastService } from '../../../Services/common/toast/toast.service';
import { CrudHandlerService } from '../../../Services/common/crud-handler.service';
import { VidhanSabhaPrabhariService } from '../../../Services/Admin/vidhansabha-prabhari/vidhansabha-prabhari.service';

@Component({
  selector: 'app-vidhansabha',
  standalone: true,
  imports: [CommonModule, PageHeaderComponent, GenericTableComponent, GenericModalButtonComponent],
  templateUrl: './vidhansabha.component.html',
  styleUrl: './vidhansabha.component.css'
})
export class VidhanSabhaComponent implements OnInit {
  @ViewChild('vidhanModal') vidhanModal!: GenericModalButtonComponent;
  @ViewChild('prabhariModal') prabhariModal!: GenericModalButtonComponent;

  vidhanList: any[] = [];

  columns: TableColumn[] = [
    { key: 'id', label: 'ID', sortable: true },
    { key: 'name', label: 'Vidhan Sabha Name', sortable: true },
    { key: 'districtName', label: 'District', sortable: true },
    { key: 'isPrabhariAssigned', label: 'Prabhari Assigned', type: 'badge', sortable: true }
  ];

  config: TableConfig = {
    selectable: false,
    filterable: true,
    paginated: true,
    defaultPageSize: 10,
    pageSizeOptions: [10, 20, 50],
    searchable: true,
    searchPlaceholder: 'Search constituencies...',
    showRowNumbers: true,
    striped: true,
    hoverable: true
  };

  actions: TableAction[] = [
    { id: 'add_prabhari', label: 'Prabhari', variant: 'primary', icon: 'user' },
    { id: 'edit', label: '', variant: 'default', icon: 'edit' },
    { id: 'delete', label: '', variant: 'danger', icon: 'delete' }
  ];

  addVidhanConfig: FormConfig = {
    title: 'Add New Vidhan Sabha',
    submitLabel: 'Create Vidhan Sabha',
    fields: [
      {
        id: 'districtId',
        name: 'districtId',
        label: 'Select District',
        type: 'select',
        apiUrl: () => `district/getAll`,
        apiMapper: (list: any[]) => list.map(item => ({ value: String(item.id), label: item.name })),
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'name',
        name: 'name',
        label: 'Vidhan Sabha Name',
        type: 'text',
        placeholder: 'Enter name',
        validations: [Validators.required],
        gridColSpan: 6
      }
    ]
  };

  addPrabhariConfig: FormConfig = {
    title: 'Register Prabhari',
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
          { label: 'Female', value: 'Female' },
          { label: 'Other', value: 'Other' }
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
    private vidhanService: VidhanSabhaService,
    private vidhanPrabhariService: VidhanSabhaPrabhariService,
    private districtService: DistrictService,
    private toastService: ToastService,
    private crudHandler: CrudHandlerService
  ) { }

  ngOnInit() {
    this.loadVidhanSabhas();
  }

  loadVidhanSabhas() {
    this.vidhanService.getAllVidhanSabhas().subscribe({
      next: (response) => {
        if (response && response.isSuccess) {
          this.vidhanList = response.data;
        } else if (Array.isArray(response)) {
          this.vidhanList = response;
        } else {
          this.vidhanList = [];
        }
      },
      error: (err) => console.error('Error fetching Vidhan Sabhas:', err)
    });
  }

  handleAction(event: any) {
    const { action, row } = event;
    if (action.id === 'delete') {
      this.crudHandler.handleRequest(
        this.vidhanService.deleteVidhanSabha(row.id),
        'Deleted',
        'Vidhan Sabha deleted successfully!',
        () => this.loadVidhanSabhas()
      );
    } else if (action.id === 'edit') {
      this.vidhanModal.openModal({
        ...row,
        districtId: String(row.districtId)
      });
    } else if (action.id === 'add_prabhari') {
      // Prepare data for prabhari modal
      const prabhariData = {
        vidhanSabhaId: row.id,
        // If row hasExistingPrabhari, map it here for edit mode
        ...(row.prabhari || {})
      };
      
      this.prabhariModal.openModal(prabhariData);
    }
  }

  handleFormSubmit(result: FormResult) {
    if (!result.status) return;

    const raw = result.data;
    const submitData = {
      ...raw,
      id: raw.id ? Number(raw.id) : null,
      districtId: Number(raw.districtId)
    };

    const isUpdate = !!submitData.id;
    const request = isUpdate
      ? this.vidhanService.updateVidhanSabha(submitData)
      : this.vidhanService.createVidhanSabha(submitData);

    this.crudHandler.handleRequest(
      request,
      isUpdate ? 'Updated' : 'Success',
      `Vidhan Sabha ${isUpdate ? 'updated' : 'created'} successfully!`,
      () => this.loadVidhanSabhas()
    );
  }

  handlePrabhariSubmit(result: FormResult) {
    if (!result.status) return;

    const raw = result.data;
    const vidhanSabhaId = this.prabhariModal.initialData?.vidhanSabhaId;

    if (!vidhanSabhaId) {
      this.toastService.showError('Error', 'Constituency ID missing');
      return;
    }

    const isUpdate = !!(raw.id || this.prabhariModal.initialData?.id);
    const submitData = {
      ...raw,
      vidhanSabhaId: Number(vidhanSabhaId),
      castId: Number(raw.castId),
      categoryId: Number(raw.categoryId)
    };

    const request = isUpdate
      ? this.vidhanPrabhariService.updatePrabhari(submitData)
      : this.vidhanPrabhariService.createPrabhari(submitData);

    this.crudHandler.handleRequest(
      request,
      isUpdate ? 'Updated' : 'Created',
      `Prabhari successfully ${isUpdate ? 'updated' : 'assigned'} to the constituency!`,
      () => this.loadVidhanSabhas()
    );
  }

  handleExport(format: string) {
    if (!format) return;
    this.toastService.showSuccess('Export Started', `Successfully generated ${format.toUpperCase()} export!`);
  }
}
