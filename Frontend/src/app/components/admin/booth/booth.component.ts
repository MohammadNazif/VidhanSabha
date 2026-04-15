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
        type: 'number',
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
    {
      key: 'villageName',
      label: 'Village',
      sortable: true,
      formatter: (val: any, row: any) => {
        if (row.villages && Array.isArray(row.villages)) {
          return row.villages.map((v: any) => v.villageName).join(', ');
        }
        return val || 'N/A';
      }
    },
    { key: 'pollingStationName', label: 'Polling Station Name', sortable: true },
    {
      key: 'boothAathyaksh',
      label: 'Booth Aathyaksh',
      sortable: true,
      formatter: (val: any, row: any) => row.sanyojak?.inchargeName || 'N/A'
    },
    {
      key: 'contactNumber',
      label: 'Contact Number',
      sortable: true,
      formatter: (val: any, row: any) => row.sanyojak?.phoneNumber || 'N/A'
    },
    {
      key: 'castName',
      label: 'Cast',
      sortable: true,
      formatter: (val: any, row: any) => row.sanyojak?.castName || 'N/A'
    },
    { key: 'pollingStationLocation', label: 'Location', sortable: true }
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
    { id: 'edit', label: '', variant: 'default', icon: 'edit' },
    { id: 'delete', label: '', variant: 'danger', icon: 'delete' }
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
      // Flatten nested data for form editing
      const editData: any = {
        ...row,
        mandalId: String(row.mandalId),
        sectorId: String(row.sectorId),
        isBoothSanyojak: row.isBoothSanyojak ? 'Yes' : 'No'
      };

      // Flatten sanyojak fields
      if (row.sanyojak) {
        editData.inchargeName = row.sanyojak.inchargeName;
        editData.age = row.sanyojak.age;
        editData.fatherName = row.sanyojak.fatherName;
        editData.categoryId = String(row.sanyojak.categoryId);
        editData.castId = String(row.sanyojak.castId);
        editData.educationLevel = row.sanyojak.educationLevel;
        editData.phoneNumber = row.sanyojak.phoneNumber;
        editData.address = row.sanyojak.address;
      }

      // Transform villages for selection-table and multi-select
      if (row.villages && Array.isArray(row.villages)) {
        editData.villageId = row.villages.map((v: any) => String(v.villageId));
        editData.anshikData = row.villages.map((v: any) => ({
          id: String(v.villageId),
          name: v.villageName,
          anshik: v.hasAnshik ? 'Yes' : 'No'
        }));
      }

      this.boothModal.openModal(editData);
    } else {
      this.toastService.showWarning('Action Selected', `Action ${action.id} clicked for ${row.boothName || 'this item'}`);
    }
  }

  handleFormSubmit(result: FormResult) {
    if (!result.status) return;

    const raw = result.data;
    const isSanyojak = raw.isBoothSanyojak === 'Yes';

    const submitData: any = {
      id: raw.id || (this.boothModal.initialData && this.boothModal.initialData.id),
      mandalId: Number(raw.mandalId),
      sectorId: Number(raw.sectorId),
      boothNumber: Number(raw.boothNumber),
      pollingStationName: raw.pollingStationName,
      pollingStationLocation: raw.pollingStationLocation,
      isBoothSanyojak: isSanyojak,
      villages: []
    };

    // Transform anshikData to villages array
    if (raw.anshikData && Array.isArray(raw.anshikData)) {
      submitData.villages = raw.anshikData.map((v: any) => ({
        villageId: Number(v.id || v.villageId),
        hasAnshik: v.anshik === 'Yes'
      }));
    } else if (raw.villageId) {
      const ids = Array.isArray(raw.villageId) ? raw.villageId : [raw.villageId];
      submitData.villages = ids.map((id: any) => ({
        villageId: Number(id),
        hasAnshik: false
      }));
    }

    // Nest sanyojak details if applicable
    if (isSanyojak) {
      submitData.sanyojak = {
        inchargeName: raw.inchargeName,
        age: Number(raw.age),
        fatherName: raw.fatherName,
        categoryId: Number(raw.categoryId),
        castId: Number(raw.castId),
        educationLevel: raw.educationLevel,
        phoneNumber: raw.phoneNumber,
        address: raw.address
      };
    }

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
