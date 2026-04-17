import { Component } from '@angular/core';
import { TableAction, TableColumn, TableConfig } from '../../shared/generic-table/generic-table.types';
import { Validators } from '@angular/forms';
import { FormConfig, FormResult, FormField } from '../../shared/generic-modal-form/generic-form.types';
import { CommonModule } from '@angular/common';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';
import { GenericModalButtonComponent } from '../../shared/generic-modal-form/generic-modal-button.component';

import { PageHeaderComponent } from '../../shared/page-header/page-header.component';
import { ToastService } from '../../../Services/common/toast/toast.service';
import { SectorService } from '../../../Services/Admin/sector/sector.service';
import { StateService } from '../../../Services/Admin/state/state.service';
import { AuthServiceService } from '../../../Services/Auth/auth.service';
import { CrudHandlerService } from '../../../Services/common/crud-handler.service';
import { ViewChild, OnInit } from '@angular/core';

@Component({
  selector: 'app-sector',
  standalone: true,
  imports: [CommonModule, GenericTableComponent, GenericModalButtonComponent, PageHeaderComponent],
  templateUrl: './sector.component.html',
  styleUrl: './sector.component.css'
})
export class SectorComponent implements OnInit {
  @ViewChild('sectorModal') sectorModal!: GenericModalButtonComponent;

  membersData: any[] = [];
  defaultStateId: string | null = null;

  isStatePrabhari(): boolean {
    return (this.authService.getRole() || '').toUpperCase().trim() === 'STATEPRABHARI';
  }

  columns: TableColumn[] = [
    { key: 'mandalName', label: 'Mandal', type: 'avatar', sortable: true, avatarFallbackKey: 'name' },
    { key: 'villageName', label: 'Village', sortable: true },
    { key: 'sectorName', label: 'Sector', sortable: true },
    { key: 'inchargeName', label: 'Sector Sanyojak', sortable: true },
    { key: 'phoneNumber', label: 'Contact', sortable: true },
    // { key: 'profileImage', label: 'Profile Image', sortable: true, avatarFallbackKey: 'name' },
  ];

  config: TableConfig = {
    selectable: false,
    filterable: true,
    paginated: true,
    defaultPageSize: 10,
    pageSizeOptions: [10, 20, 50],
    searchable: true,
    searchPlaceholder: 'Search sectors...',
    showRowNumbers: true,
    striped: true,
    hoverable: true
  };

  actions: TableAction[] = [
    { id: 'edit', label: '', variant: 'default', icon: 'edit' },
    { id: 'delete', label: '', variant: 'danger', icon: 'delete' }
  ];

  addSectorConfig: FormConfig = {
    title: 'Register New Sector',
    submitLabel: 'Register Sector',
    fields: [
      {
        id: 'districtId',
        name: 'districtId',
        label: 'Select District',
        type: 'select',
        placeholder: '--Select District--',
        apiUrl: () => `district/getAll?stateId=${this.defaultStateId || ''}`,
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
        id: 'vidhanId',
        name: 'vidhanId',
        label: 'Select Vidhan Sabha',
        type: 'select',
        placeholder: '--Select Vidhan Sabha--',
        dependsOn: 'districtId',
        apiUrl: (districtId: any) => `vidhansabha/getAll?districtId=${districtId}`,
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
        id: 'mandalId',
        name: 'mandalId',
        label: 'Mandal',
        type: 'select',
        placeholder: '--Select Mandal--',
        dependsOn: 'vidhanId',
        apiUrl: (vidhanId: any) => `mandal/getall?vidhanId=${vidhanId}`,
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
        id: 'villageId',
        name: 'Village',
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
        gridColSpan: 6,
        multiple: true
      },
      {
        id: 'sectorName',
        name: 'sectorName',
        label: 'Sector Name',
        type: 'text',
        placeholder: 'Enter sector name',
        validations: [Validators.required],
        gridColSpan: 6
      },

      {
        id: 'isSectorSanyojak',
        name: 'isSectorSanyojak',
        label: 'Sector Sanyojak',
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
        visibleIf: { field: 'isSectorSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 6
      },
      {
        id: 'age',
        name: 'age',
        label: 'Age',
        type: 'number',
        placeholder: 'Age',
        visibleIf: { field: 'isSectorSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 2
      },
      {
        id: 'fatherName',
        name: 'fatherName',
        label: 'Father Name',
        type: 'text',
        placeholder: "Enter father's full name",
        visibleIf: { field: 'isSectorSanyojak', operator: '==', value: 'Yes' },
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
        visibleIf: { field: 'isSectorSanyojak', operator: '==', value: 'Yes' },
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
        visibleIf: { field: 'isSectorSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 6
      },
      {
        id: 'educationLevel',
        name: 'educationLevel',
        label: 'Education Level',
        type: 'text',
        placeholder: 'Enter highest education (e.g., B.A., 12th)',
        visibleIf: { field: 'isSectorSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 6
      },
      {
        id: 'phoneNumber',
        name: 'phoneNumber',
        label: 'Phone Number',
        type: 'text',
        placeholder: 'Enter 10-digit mobile number',
        validations: [Validators.pattern('^[0-9]{10}$')],
        visibleIf: { field: 'isSectorSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 6
      },
      {
        id: 'address',
        name: 'address',
        label: 'Address',
        type: 'textarea',
        placeholder: 'Enter full address with landmark',
        visibleIf: { field: 'isSectorSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 12
      },
      {
        id: 'profileImage',
        name: 'profileImage',
        label: 'Profile Image',
        type: 'file',
        visibleIf: { field: 'isSectorSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 12
      }
    ]
  };

  constructor(
    private sectorService: SectorService,
    private stateService: StateService,
    private authService: AuthServiceService,
    private crudHandler: CrudHandlerService,
    private toastService: ToastService
  ) { }

  ngOnInit() {
    if (this.isStatePrabhari()) {
      // Fetch the assigned state ID
      this.stateService.getAllStates().subscribe({
        next: (response) => {
          const list = response?.data || response || [];
          if (list.length > 0) {
            this.defaultStateId = String(list[0].stateId || list[0].id);

            // Simplify fields for State Prabhari
            const districtField = this.addSectorConfig.fields.find(f => f.id === 'districtId');
            if (districtField) {
              delete districtField.dependsOn;
              districtField.apiUrl = () => `district/getAll?stateId=${this.defaultStateId}`;
            }
          }
        }
      });
    }
    this.loadSectors();
  }

  loadSectors() {
    this.sectorService.getAllSectors().subscribe({
      next: (response) => {
        this.membersData = response.data || response;
      },
      error: (err) => console.error('Error loading sectors:', err)
    });
  }

  handleFormSubmit(result: FormResult) {
    if (!result.status) return;
    console.log(result.data);
    const raw = { ...result.data };
    const userId = this.authService.getUserId();
    const isUpdate = !!(raw.id || (this.sectorModal.initialData && this.sectorModal.initialData.id));

    const submitData: any = {
      ...raw,
      id: isUpdate ? (raw.id || this.sectorModal.initialData.id) : null,
      mandalId: Number(raw.mandalId),
      stateId: Number(raw.stateId || this.defaultStateId),
      villageId: Array.isArray(raw.villageId) ? raw.villageId.map((v: any) => Number(v)) : Number(raw.villageId),
      sectorName: raw.sectorName,
      isSectorSanyojak: raw.isSectorSanyojak === 'Yes',
      userId: userId ? String(userId) : null
    };

    if (submitData.isSectorSanyojak) {
      submitData.inchargeName = raw.inchargeName || null;
      submitData.age = raw.age ? Number(raw.age) : null;
      submitData.fatherName = raw.fatherName || null;
      submitData.categoryId = raw.categoryId ? Number(raw.categoryId) : null;
      submitData.castId = raw.castId ? Number(raw.castId) : null;
      submitData.educationLevel = raw.educationLevel || null;
      submitData.phoneNumber = raw.phoneNumber || null;
      submitData.address = raw.address || null;
      submitData.profileImage = raw.profileImage || null;
    }

    if (isUpdate && !submitData.id) {
      submitData.id = this.sectorModal.initialData.id;
    }

    const request = isUpdate
      ? this.sectorService.updateSector(submitData)
      : this.sectorService.createSector(submitData);

    this.crudHandler.handleRequest(
      request,
      isUpdate ? 'Updated' : 'Registered',
      `Sector ${isUpdate ? 'updated' : 'registered'} successfully!`,
      () => this.loadSectors()
    );
  }

  handleAction(event: any) {
    const { action, row } = event;
    if (action.id === 'delete') {
      this.crudHandler.handleRequest(
        this.sectorService.deleteSector(row.id),
        'Deleted',
        'Sector deleted successfully!',
        () => this.loadSectors()
      );
    } else if (action.id === 'edit') {
      const editData = { ...row };

      // Convert IDs to strings to ensure matching with dropdown values
      ['mandalId', 'villageId', 'categoryId', 'castId'].forEach(key => {
        if (editData[key]) editData[key] = String(editData[key]);
      });

      if (editData.isSectorSanyojak !== undefined) {
        editData.isSectorSanyojak = editData.isSectorSanyojak ? 'Yes' : 'No';
      }
      this.sectorModal.openModal(editData);
    } else {
      this.toastService.showWarning('Action Selected', `Action ${action.id} clicked for ${row.name || 'this item'}`);
    }
  }

  handleSelection(selected: any[]) {
    console.log('Selected rows:', selected);
  }

  handleExport(format: string) {
    if (!format) return;
    console.log(`Generating ${format.toUpperCase()} export...`);
    this.toastService.showSuccess('Export Started', `Successfully generated ${format.toUpperCase()} export!`);
  }
}