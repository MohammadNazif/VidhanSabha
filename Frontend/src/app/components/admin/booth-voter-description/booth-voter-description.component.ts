import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Validators } from '@angular/forms';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';
import { TableAction, TableColumn, TableConfig } from '../../shared/generic-table/generic-table.types';
import { GenericModalButtonComponent } from '../../shared/generic-modal-form/generic-modal-button.component';
import { FormConfig, FormResult } from '../../shared/generic-modal-form/generic-form.types';
import { PageHeaderComponent } from '../../shared/page-header/page-header.component';
import { BoothVoterService } from '../../../Services/Admin/booth-voter/booth-voter.service';
import { CasteVoterService } from '../../../Services/Admin/booth-voter/caste-voter.service';
import { BoothSamitiService } from '../../../Services/Admin/booth-voter/booth-samiti.service';
import { ToastService } from '../../../Services/common/toast/toast.service';
import { CrudHandlerService } from '../../../Services/common/crud-handler.service';
import { AuthServiceService } from '../../../Services/Auth/auth.service';
import { ActivatedRoute } from '@angular/router';
import { ModulePermission } from '../../../models/module-permission.enum';

@Component({
  selector: 'app-booth-voter-description',
  standalone: true,
  imports: [CommonModule, PageHeaderComponent, GenericTableComponent, GenericModalButtonComponent],
  templateUrl: './booth-voter-description.component.html',
  styleUrl: './booth-voter-description.component.css'
})
export class BoothVoterDescriptionComponent implements OnInit {
  @ViewChild('voterModal') voterModal!: GenericModalButtonComponent;
  @ViewChild('casteModal') casteModal!: GenericModalButtonComponent;
  @ViewChild('samitiModal') samitiModal!: GenericModalButtonComponent;

  voterList: any[] = [];
  totalCount = 0;

  // Server-side state
  pageNumber = 1;
  pageSize = 50;
  searchTerm = '';
  sortBy = '';
  isDescending = false;

  isListView = false;

  columns: TableColumn[] = [
    { key: 'boothNumber', label: 'Booth No.', sortable: true },
    { key: 'villageName', label: 'Village', sortable: true },
    { key: 'totalVoter', label: 'Total Voters', sortable: true },
    { key: 'male', label: 'Male', sortable: true },
    { key: 'female', label: 'Female', sortable: true },
    { key: 'other', label: 'Other', sortable: true }
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
    { id: 'edit', label: '', variant: 'default', icon: 'edit', show: () => this.canManage() },
    { id: 'delete', label: '', variant: 'danger', icon: 'delete', show: () => this.canManage() },
    { id: 'add_caste', label: 'Caste Wise', variant: 'primary', icon: 'plus', show: () => this.canManage() },
    { id: 'add_samiti', label: 'Add', variant: 'primary', icon: 'users', show: () => this.canManage() }
  ];

  constructor(
    private boothVoterService: BoothVoterService,
    private casteVoterService: CasteVoterService,
    private boothSamitiService: BoothSamitiService,
    private toastService: ToastService,
    private crudHandler: CrudHandlerService,
    private authService: AuthServiceService,
    private route: ActivatedRoute
  ) { }

  canManage(): boolean {
    if (this.isListView) return false;
    const role = (this.authService.getRole() || '').toUpperCase().trim();
    // Allow BoothSanyojak and VidhanSabhaPrabhari as per sidebar.component.ts
    return true;
  }

  addVoterConfig: FormConfig = {
    title: 'Add Booth Voter Description',
    submitLabel: 'Save Description',
    fields: [
      {
        id: 'boothId',
        name: 'boothId',
        label: 'Booth',
        type: 'select',
        placeholder: '--Select Booth--',
        apiUrl: () => `common/boothNumber?PageNumber=1&PageSize=1000&IsDescending=true&SortBy=id&userId=${this.authService.getUserId()}`,
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
        label: 'Village',
        type: 'select',
        placeholder: '--Select Village--',
        dependsOn: 'boothId',
        apiUrl: (boothId: string) => `common/villagesByBoothId?boothId=${boothId}&PageNumber=1&PageSize=1000&IsDescending=true&SortBy=id&userId=${this.authService.getUserId()}`,
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
        id: 'totalVoter',
        name: 'totalVoter',
        label: 'Total Voters',
        type: 'number',
        placeholder: 'Enter Total Voters',
        validations: [Validators.required, Validators.min(0)],
        gridColSpan: 6
      },
      {
        id: 'male',
        name: 'male',
        label: 'Male Voters',
        type: 'number',
        placeholder: 'Enter Male Voters',
        validations: [Validators.required, Validators.min(0)],
        gridColSpan: 6
      },
      {
        id: 'female',
        name: 'female',
        label: 'Female Voters',
        type: 'number',
        placeholder: 'Enter Female Voters',
        validations: [Validators.required, Validators.min(0)],
        gridColSpan: 6
      },
      {
        id: 'other',
        name: 'other',
        label: 'Other Voters',
        type: 'number',
        placeholder: 'Enter Other Voters',
        validations: [Validators.required, Validators.min(0)],
        gridColSpan: 6
      }
    ]
  };

  addCasteConfig: FormConfig = {
    title: 'Add Caste Wise Voters',
    submitLabel: 'Save Caste Voters',
    fields: [
      {
        id: 'casteVoterId',
        name: 'casteVoterId',
        label: 'Booth Voter ID',
        type: 'hidden'
      },
      {
        id: 'casteVoters',
        name: 'casteVoters',
        label: 'Caste Wise Details',
        type: 'form-array',
        gridColSpan: 12,
        subFields: [
          {
            id: 'subCasteId',
            name: 'subCasteId',
            label: 'Select Caste',
            type: 'select',
            apiUrl: 'common/category', // Using category as placeholder if direct cast isn't found
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
            id: 'number',
            name: 'number',
            label: 'Number of Voters',
            type: 'number',
            placeholder: 'Enter number',
            validations: [Validators.required, Validators.min(0)],
            gridColSpan: 6
          }
        ]
      }
    ]
  };

  addSamitiConfig: FormConfig = {
    title: 'Add Booth Samiti Member',
    submitLabel: 'Save Member',
    fields: [
      {
        id: 'boothId',
        name: 'boothId',
        label: 'Booth ID',
        type: 'hidden'
      },
      {
        id: 'name',
        name: 'name',
        label: 'Full Name',
        type: 'text',
        placeholder: 'Enter member name',
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'contact',
        name: 'contact',
        label: 'Contact Number',
        type: 'text',
        placeholder: 'Enter phone number',
        validations: [Validators.required, Validators.pattern('^[0-9]{10}$')],
        gridColSpan: 6
      },
      {
        id: 'age',
        name: 'age',
        label: 'Age',
        type: 'number',
        placeholder: 'Enter age',
        validations: [Validators.required, Validators.min(18)],
        gridColSpan: 6
      },
      {
        id: 'occupation',
        name: 'occupation',
        label: 'Occupation',
        type: 'text',
        placeholder: 'Enter occupation',
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
        id: 'casteId',
        name: 'casteId',
        label: 'Caste',
        type: 'select',
        placeholder: '-- Select Caste --',
        dependsOn: 'categoryId',
        apiUrl: (catId: string) => `common/cast?id=${catId}`,
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
        id: 'designationId',
        name: 'designationId',
        label: 'Designation',
        type: 'select',
        placeholder: '-- Select Designation --',
        apiUrl: 'boothsamiti-designation/getAll',
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : []);
          return list.map((item: any) => ({
            value: String(item.id),
            label: item.designationName
          }));
        },
        validations: [Validators.required],
        gridColSpan: 12
      }
    ]
  };

  ngOnInit() {
    this.route.url.subscribe(url => {
      const path = url[0]?.path || '';
      this.isListView = path.includes('-list');
      this.loadVoters();
    });
  }

  loadVoters() {
    const params: any = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      searchTerm: this.searchTerm,
      sortBy: this.sortBy,
      isDescending: this.isDescending
    };

    const userId = this.authService.getUserId();
    if (userId) {
      params.userId = userId;
    }

    this.boothVoterService.getAllBoothVoters(params).subscribe({
      next: (response) => {
        const dataWrap = response.data;
        if (dataWrap && dataWrap.items) {
          this.voterList = dataWrap.items.map((item: any) => ({
            ...item,
            villageName: Array.isArray(item.villages) ? item.villages.map((v: any) => v.villageName).join(', ') : '',
            villageId: Array.isArray(item.villages) ? item.villages.map((v: any) => v.villageId) : []
          }));
          this.totalCount = dataWrap.totalCount || 0;
        } else {
          const rawList = Array.isArray(dataWrap) ? dataWrap : [];
          this.voterList = rawList.map((item: any) => ({
            ...item,
            villageName: Array.isArray(item.villages) ? item.villages.map((v: any) => v.villageName).join(', ') : '',
            villageId: Array.isArray(item.villages) ? item.villages.map((v: any) => v.villageId) : []
          }));
          this.totalCount = this.voterList.length;
        }
      },
      error: (err) => {
        console.error('Error fetching booth voters:', err);
      }
    });
  }

  handlePageChange(event: any) {
    this.pageNumber = event.currentPage;
    this.pageSize = event.pageSize;
    this.loadVoters();
  }

  handleSortChange(event: any) {
    this.sortBy = event.column;
    this.isDescending = event.direction === 'desc';
    this.pageNumber = 1;
    this.loadVoters();
  }

  handleSearchChange(term: string) {
    this.searchTerm = term;
    this.pageNumber = 1;
    this.loadVoters();
  }

  handleSelection(selected: any[]) {
    console.log('Selected items:', selected);
  }

  handleAction(event: any) {
    const { action, row } = event;
    if (action.id === 'delete') {
      this.crudHandler.handleRequest(
        this.boothVoterService.deleteBoothVoter(row.id),
        'Deleted',
        'Booth Voter Description deleted successfully!',
        () => this.loadVoters(),
        true,
        ModulePermission.BoothVoterDescrition
      );
    } else if (action.id === 'edit') {
      const editData = { ...row };
      ['id', 'boothId'].forEach(key => {
        if (editData[key] !== null && editData[key] !== undefined) editData[key] = String(editData[key]);
      });
      if (editData.villageId !== null && editData.villageId !== undefined) {
        editData.villageId = Array.isArray(editData.villageId) ? editData.villageId.map(String) : [String(editData.villageId)];
      }
      this.voterModal.openModal(editData);
    } else if (action.id === 'add_caste') {
      this.casteModal.openModal({
        casteVoterId: String(row.id),
        casteVoters: [{ subCasteId: '', number: '' }]
      });
    } else if (action.id === 'add_samiti') {
      this.samitiModal.openModal({
        boothId: String(row.boothId)
      });
    }
  }

  handleSamitiSubmit(result: FormResult) {
    if (!result.status) return;

    const raw = result.data;
    const submitData = {
      name: raw.name,
      categoryId: Number(raw.categoryId),
      casteId: Number(raw.casteId),
      age: Number(raw.age),
      contact: raw.contact,
      occupation: raw.occupation,
      designationId: Number(raw.designationId)
    };

    this.crudHandler.handleRequest(
      this.boothSamitiService.createBoothSamiti(submitData),
      'Created',
      'Booth Samiti member added successfully!',
      () => this.loadVoters(),
      true,
      ModulePermission.BoothVoterDescrition
    );
  }

  handleCasteSubmit(result: FormResult) {
    if (!result.status) return;

    const raw = result.data;
    const submitData = {
      casteVoterId: Number(raw.casteVoterId),
      casteVoters: raw.casteVoters.map((cv: any) => ({
        subCasteId: Number(cv.subCasteId),
        number: Number(cv.number)
      }))
    };

    this.crudHandler.handleRequest(
      this.casteVoterService.createCasteVoter(submitData),
      'Created',
      'Caste wise voters added successfully!',
      () => this.loadVoters(),
      true,
      ModulePermission.BoothVoterDescrition
    );
  }

  handleFormSubmit(result: FormResult) {
    if (!result.status) return;

    const raw = result.data;
    const rowId = raw.id || (this.voterModal.initialData && this.voterModal.initialData.id);

    const submitData: any = {
      id: rowId ? Number(rowId) : null,
      boothId: Number(raw.boothId),
      villageId: Array.isArray(raw.villageId) ? raw.villageId.map((v: any) => Number(v)) : (raw.villageId ? [Number(raw.villageId)] : []),
      totalVoter: Number(raw.totalVoter),
      male: Number(raw.male),
      female: Number(raw.female),
      other: Number(raw.other),
      userId: this.authService.getUserId()
    };

    const isUpdate = !!submitData.id;
    const request = isUpdate
      ? this.boothVoterService.updateBoothVoter(submitData)
      : this.boothVoterService.createBoothVoter(submitData);

    this.crudHandler.handleRequest(
      request,
      isUpdate ? 'Updated' : 'Success',
      `Booth Voter Description ${isUpdate ? 'updated' : 'created'} successfully!`,
      () => this.loadVoters(),
      true,
      ModulePermission.BoothVoterDescrition
    );
  }
}
