import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Validators } from '@angular/forms';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';
import { TableAction, TableColumn, TableConfig } from '../../shared/generic-table/generic-table.types';
import { GenericModalButtonComponent } from '../../shared/generic-modal-form/generic-modal-button.component';
import { FormConfig, FormResult } from '../../shared/generic-modal-form/generic-form.types';

import { PageHeaderComponent } from '../../shared/page-header/page-header.component';
import { BoothService } from '../../../Services/Admin/booth/booth.service';
import { ToastService } from '../../../Services/common/toast/toast.service';
import { CrudHandlerService } from '../../../Services/common/crud-handler.service';


@Component({
  selector: 'app-booth',
  standalone: true,
  imports: [CommonModule, PageHeaderComponent, GenericTableComponent, GenericModalButtonComponent],
  templateUrl: './booth.component.html',
  styleUrl: './booth.component.css'
})
export class BoothComponent implements OnInit {
  @ViewChild('boothModal') boothModal!: GenericModalButtonComponent;

  constructor(
    private boothService: BoothService,
    private toastService: ToastService,
    private crudHandler: CrudHandlerService
  ) { }

  ngOnInit() {
    this.loadBooths();
  }

  loadBooths() {
    this.boothService.getAllBooths().subscribe({
      next: (response) => {
        this.boothList = response.data || (Array.isArray(response) ? response : []);
      },
      error: (err) => {
        console.error('Error fetching booths:', err);
      }
    });
  }

  addBoothConfig: FormConfig = {
    title: 'Add New Booth',
    submitLabel: 'Create Booth',
    fields: [
      {
        id: 'mandalId',
        name: 'mandalId',
        label: 'Mandal',
        type: 'select',
        placeholder: '--Select Mandal--',
        apiUrl: 'mandal/getall',
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
        id: 'sectorId',
        name: 'sectorId',
        label: 'Sector',
        type: 'select',
        dependsOn: 'mandalId',
        placeholder: '--Select Sector--',
        apiUrl: (mandalId: any) => `sector/getall?mandalId=${mandalId}`,
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : []);
          return list.map((item: any) => ({
            value: String(item.id),
            label: item.sectorName || item.name
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
        dependsOn: 'mandalId',
        placeholder: '--Select Village--',
        apiUrl: (mandalId: any) => `common/village?id=${mandalId}`,
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : []);
          return list.map((item: any) => ({
            value: String(item.id),
            label: item.name
          }));
        },
        validations: [Validators.required],
        gridColSpan: 12,
        multiple: true
      },
      {
        id: 'anshikData',
        name: 'anshikData',
        label: 'Village Selection Details',
        type: 'selection-table',
        dependsOn: 'villageId',
        gridColSpan: 12
      },
      {
        id: 'boothNumber',
        name: 'boothNumber',
        label: 'Booth Number',
        type: 'number',
        placeholder: 'Enter booth number',
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'pollingStationName',
        name: 'pollingStationName',
        label: 'Polling Station Name',
        type: 'text',
        placeholder: 'Enter polling station name',
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'pollingStationLocation',
        name: 'pollingStationLocation',
        label: 'Polling Station Location',
        type: 'text',
        placeholder: 'Enter polling station location',
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'isBoothSanyojak',
        name: 'isBoothSanyojak',
        label: 'Booth Sanyojak',
        type: 'select',
        placeholder: '-- Select Yes/No --',
        options: [
          { label: 'Yes', value: 'Yes' },
          { label: 'No', value: 'No' }
        ],
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'inchargeName',
        name: 'inchargeName',
        label: 'Incharge Name',
        type: 'text',
        placeholder: 'Enter incharge full name',
        visibleIf: { field: 'isBoothSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 6
      },
      {
        id: 'age',
        name: 'age',
        label: 'Age',
        type: 'number',
        placeholder: 'Age',
        visibleIf: { field: 'isBoothSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 2
      },
      {
        id: 'fatherName',
        name: 'fatherName',
        label: 'Father Name',
        type: 'text',
        placeholder: "Enter father's full name",
        visibleIf: { field: 'isBoothSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 4
      },
      {
        id: 'categoryId',
        name: 'categoryId',
        label: 'Category',
        type: 'select',
        placeholder: '-- Select Category --',
        apiUrl: 'common/category',
        apiMapper: (data: any) => {
          if (Array.isArray(data?.data)) {
            return data.data.map((item: any) => ({
              value: String(item.id),
              label: item.name
            }));
          }
          return [];
        },
        visibleIf: { field: 'isBoothSanyojak', operator: '==', value: 'Yes' },
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
          if (Array.isArray(data?.data)) {
            return data.data.map((item: any) => ({
              value: String(item.id),
              label: item.name
            }));
          }
          return [];
        },
        visibleIf: { field: 'isBoothSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 6
      },
      {
        id: 'educationLevel',
        name: 'educationLevel',
        label: 'Education Level',
        type: 'text',
        placeholder: 'Enter highest education (e.g., B.A., 12th)',
        visibleIf: { field: 'isBoothSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 6
      },
      {
        id: 'phoneNumber',
        name: 'phoneNumber',
        label: 'Phone Number',
        type: 'text',
        placeholder: 'Enter 10-digit mobile number',
        validations: [Validators.pattern('^[0-9]{10}$')],
        visibleIf: { field: 'isBoothSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 6
      },
      {
        id: 'address',
        name: 'address',
        label: 'Address',
        type: 'textarea',
        placeholder: 'Enter full address with landmark',
        visibleIf: { field: 'isBoothSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 12
      },
      {
        id: 'profileImage',
        name: 'profileImage',
        label: 'Profile Image',
        type: 'file',
        visibleIf: { field: 'isBoothSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 12
      }
    ]
  };

  boothList: any[] = [];

  columns: TableColumn[] = [
    { key: 'mandalName', label: 'Mandal', sortable: true },
    { key: 'sectorName', label: 'Sector', sortable: true },
    { key: 'villageName', label: 'Village', sortable: true },
    { key: 'pollingStationName', label: 'Polling Station Name', sortable: true },
    { key: 'boothAathyaksh', label: 'Booth Aathyaksh', sortable: true },
    { key: 'contactNumber', label: 'Contact Number', sortable: true },
    { key: 'castName', label: 'Cast', sortable: true },
    // {key:'profileImage',label:'Profile Image',sortable:true},
    { key: 'location', label: 'Location', sortable: true }

  ];

  config: TableConfig = {
    selectable: false,
    filterable: true,
    paginated: true,
    defaultPageSize: 10,
    pageSizeOptions: [10, 20, 50],
    searchable: true,
    searchPlaceholder: 'Search booths...',
    showRowNumbers: true,
    striped: true,
    hoverable: true
  };

  actions: TableAction[] = [
    { id: 'edit', label: '', variant: 'default', icon: '✏️' },
    { id: 'delete', label: '', variant: 'danger', icon: '🗑️' }
  ];

  handleAction(event: any) {
    const { action, row } = event;
    if (action.id === 'delete') {
      this.crudHandler.handleRequest(
        this.boothService.deleteBooth(row.id),
        'Deleted',
        'Booth deleted successfully!',
        () => this.loadBooths()
      );
    } else if (action.id === 'edit') {
      const editData = { ...row };
      ['mandalId', 'sectorId'].forEach(key => {
        if (editData[key]) editData[key] = String(editData[key]);
      });
      this.boothModal.openModal(editData);
    } else {
      this.toastService.showWarning('Action Selected', `Action ${action.id} clicked for ${row.boothName || 'this item'}`);
    }
  }

  handleFormSubmit(result: FormResult) {
    if (!result.status) return;

    const raw = result.data;
    const submitData = {
      ...raw,
      id: raw.id || (this.boothModal.initialData && this.boothModal.initialData.id),
      mandalId: Number(raw.mandalId),
      sectorId: Number(raw.sectorId)
    };

    const isUpdate = !!submitData.id;

    const request = isUpdate
      ? this.boothService.updateBooth(submitData)
      : this.boothService.createBooth(submitData);

    this.crudHandler.handleRequest(
      request,
      isUpdate ? 'Updated' : 'Success',
      `Booth ${isUpdate ? 'updated' : 'created'} successfully!`,
      () => this.loadBooths()
    );
  }

  handleSelection(selected: any[]) {
    console.log('Selected rows:', selected);
  }

  handleExport(format: string) {
    if (!format) return;
    this.toastService.showSuccess('Export Started', `Successfully generated ${format.toUpperCase()} export!`);
  }
}
