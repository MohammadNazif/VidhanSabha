import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Validators } from '@angular/forms';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';
import { TableAction, TableColumn, TableConfig } from '../../shared/generic-table/generic-table.types';
import { GenericModalButtonComponent } from '../../shared/generic-modal-form/generic-modal-button.component';
import { FormConfig, FormResult } from '../../shared/generic-modal-form/generic-form.types';

import { PageHeaderComponent } from '../../shared/page-header/page-header.component';
import { BoothService } from '../../../Services/Admin/booth/booth.service';
import { StateService } from '../../../Services/Admin/state/state.service';
import { AuthServiceService } from '../../../Services/Auth/auth.service';
import { ToastService } from '../../../Services/common/toast/toast.service';
import { CrudHandlerService } from '../../../Services/common/crud-handler.service';
import { ActivatedRoute } from '@angular/router';


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
    private stateService: StateService,
    private authService: AuthServiceService,
    private toastService: ToastService,
    private crudHandler: CrudHandlerService,
    private route: ActivatedRoute
  ) { }

  isListView = false;
  totalCount = 0;

  // Server-side state
  pageNumber = 1;
  pageSize = 10;
  searchTerm = '';
  sortBy = '';
  isDescending = true;
  mandalId: number | null = null;
  sectorId: number | null = null;

  canManage(): boolean {
    if (this.isListView) return false;
    // Admins can manage Master Data, but we can add role specific logic here if needed
    return true;
  }

  isStatePrabhari(): boolean {
    return (this.authService.getRole() || '').toUpperCase().trim() === 'STATEPRABHARI';
  }

  defaultStateId: string | null = null;

  ngOnInit() {
    this.route.url.subscribe(url => {
      const path = url[0]?.path || '';
      this.isListView = path.includes('-list');
      this.loadBooths();
    });

    if (this.isStatePrabhari()) {
      // Fetch the assigned state ID
      this.stateService.getAllStates().subscribe({
        next: (response) => {
          const list = response?.data || response || [];
          if (list.length > 0) {
            this.defaultStateId = String(list[0].stateId || list[0].id);

            // Simplify fields for State Prabhari
            const districtField = this.addBoothConfig.fields.find(f => f.id === 'districtId');
            if (districtField) {
              delete districtField.dependsOn;
              districtField.apiUrl = () => `district/getAll?stateId=${this.defaultStateId}`;
            }
          }
        }
      });
    }
    this.loadBooths();
  }

  loadBooths() {
    const params = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      searchTerm: this.searchTerm,
      sortBy: this.sortBy,
      isDescending: this.isDescending,
      mandalId: this.mandalId,
      sectorId: this.sectorId
    };

    this.boothService.getAllBooths(params).subscribe({
      next: (response) => {
        const dataWrap = response.data;
        if (dataWrap && dataWrap.items) {
          this.boothList = dataWrap.items;
          this.totalCount = dataWrap.totalCount || 0;
        } else {
          this.boothList = Array.isArray(dataWrap) ? dataWrap : [];
          this.totalCount = this.boothList.length;
        }
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
        dependsOn: 'vidhanId',
        apiUrl: (vidhanId: any) => `mandal/getall?vidhanId=${vidhanId}&pageSize=1000`,
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data?.items) ? data.data.items : (Array.isArray(data?.items) ? data.items : (Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : [])));
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
        apiUrl: (mandalId: any) => `sector/getByMandalId?id=${mandalId}&pageSize=1000`,
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data?.items) ? data.data.items : (Array.isArray(data?.items) ? data.items : (Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : [])));
          return list.map((item: any) => ({
            value: String(item.sectorId),
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
    { key: 'boothNumber', label: 'Booth No.', sortable: true },
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
    { key: 'pollingStationName', label: 'Polling Station', sortable: true },
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
    }
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
    hoverable: true,
    serverSide: true
  };

  actions: TableAction[] = [
    { id: 'edit', label: '', variant: 'default', icon: 'edit', show: () => this.canManage() },
    { id: 'delete', label: '', variant: 'danger', icon: 'delete', show: () => this.canManage() }
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
    }
  }

  handlePageChange(event: any) {
    this.pageNumber = event.currentPage;
    this.pageSize = event.pageSize;
    this.loadBooths();
  }

  handleSortChange(event: any) {
    this.sortBy = event.column;
    this.isDescending = event.direction === 'desc';
    this.pageNumber = 1; // Reset to first page
    this.loadBooths();
  }

  handleSearchChange(term: string) {
    this.searchTerm = term;
    this.pageNumber = 1; // Reset to first page
    this.loadBooths();
  }

  handleFormSubmit(result: FormResult) {
    if (!result.status) return;

    const raw = result.data;
    const isSanyojak = raw.isBoothSanyojak === 'Yes';
    const isUpdate = !!(raw.id || (this.boothModal.initialData && this.boothModal.initialData.id));

    const submitData: any = {
      MandalId: Number(raw.mandalId),
      SectorId: Number(raw.sectorId),
      Villages: [],
      BoothNumber: Number(raw.boothNumber),
      PollingStationName: raw.pollingStationName || "",
      PollingStationLocation: raw.pollingStationLocation || "",
      IsBoothSanyojak: isSanyojak,
      Sanyojak: isSanyojak ? {
        InchargeName: raw.inchargeName || "",
        Age: raw.age ? Number(raw.age) : 0,
        FatherName: raw.fatherName || "",
        CategoryId: raw.categoryId ? Number(raw.categoryId) : 0,
        CastId: raw.castId ? Number(raw.castId) : 0,
        EducationLevel: raw.educationLevel || "",
        PhoneNumber: raw.phoneNumber || "",
        Address: raw.address || ""
      } : null
    };

    // Transform anshikData or villageId to Villages array
    if (raw.anshikData && Array.isArray(raw.anshikData)) {
      submitData.Villages = raw.anshikData.map((v: any) => ({
        VillageId: Number(v.id || v.villageId),
        HasAnshik: v.anshik === 'Yes'
      }));
    } else if (raw.villageId) {
      const ids = Array.isArray(raw.villageId) ? raw.villageId : [raw.villageId];
      submitData.Villages = ids.map((id: any) => ({
        VillageId: Number(id),
        HasAnshik: false
      }));
    }

    if (isUpdate) {
      submitData.Id = Number(raw.id || this.boothModal.initialData.id);
    }

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
