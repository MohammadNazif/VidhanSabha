import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Validators } from '@angular/forms';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';
import { TableAction, TableColumn, TableConfig } from '../../shared/generic-table/generic-table.types';
import { GenericModalButtonComponent } from '../../shared/generic-modal-form/generic-modal-button.component';
import { FormConfig, FormResult } from '../../shared/generic-modal-form/generic-form.types';

import { PageHeaderComponent } from '../../shared/page-header/page-header.component';
import { VidhanSabhaPrabhariService } from '../../../Services/Admin/vidhansabha-prabhari/vidhansabha-prabhari.service';
import { StateService } from '../../../Services/Admin/state/state.service';
import { AuthServiceService } from '../../../Services/Auth/auth.service';
import { ToastService } from '../../../Services/common/toast/toast.service';
import { CrudHandlerService } from '../../../Services/common/crud-handler.service';
import { ActivatedRoute } from '@angular/router';
import { GenericExportComponent } from '../../shared/generic-export/generic-export.component';

@Component({
  selector: 'app-vidhansabha-prabhari-list',
  standalone: true,
  imports: [CommonModule, PageHeaderComponent, GenericTableComponent, GenericModalButtonComponent, GenericExportComponent],
  templateUrl: './vidhansabha-prabhari-list.component.html',
  styleUrl: './vidhansabha-prabhari-list.component.css'
})
export class VidhanSabhaPrabhariListComponent implements OnInit {
  @ViewChild('prabhariModal') prabhariModal!: GenericModalButtonComponent;

  prabhariList: any[] = [];
  defaultStateId: string | null = null;
  loading = false;
  totalCount = 0;
  isListMode = false;
  listTitle = 'Vidhan Sabha Prabhari Management';

  // Pagination state
  pageNumber = 1;
  pageSize = 50;
  searchTerm = '';
  sortBy = 'id';
  isDescending = true;

  isStatePrabhari(): boolean {
    return (this.authService.getRole() || '').toUpperCase().trim() === 'STATEPRABHARI';
  }

  columns: TableColumn[] = [
    { key: 'districtName', label: 'District', sortable: true },
    { key: 'vidhanSabhaName', label: 'Vidhan Sabha', sortable: true },
    { key: 'prabhariName', label: 'Prabhari', sortable: true },
    { key: 'prabhariEmail', label: 'Email', sortable: true },
    {
      key: 'gender',
      label: 'Gender',
      type: 'badge',
      sortable: true,
      badgeVariant: (val: string) => {
        if (val === 'Male') return 'info';
        if (val === 'Female') return 'success';
        return 'default';
      }
    },
    { key: 'contactNumber', label: 'Contact', sortable: true },
    { key: 'categoryName', label: 'Category', sortable: true },
    { key: 'castName', label: 'Caste', sortable: true },
    { key: 'education', label: 'Education', sortable: true },
    { key: 'profession', label: 'Profession', sortable: true }
  ];

  config: TableConfig = {
    selectable: false,
    // filterable: true,
    paginated: true,
    serverSide: true,
    defaultPageSize: 50,
    pageSizeOptions: [10, 25, 50, 100],
    searchable: true,
    searchPlaceholder: 'Search prabharis...',
    showRowNumbers: true,
    striped: true,
    hoverable: true
  };

  actions: TableAction[] = [
    { id: 'edit', label: '', variant: 'default', icon: 'edit', show: () => true },
    { id: 'delete', label: '', variant: 'danger', icon: 'delete', show: () => true }
  ];

  addPrabhariConfig: FormConfig = {
    title: 'Register Vidhan Sabha Prabhari',
    submitLabel: 'Save Prabhari',
    fields: [
      {
        id: 'stateId',
        name: 'stateId',
        label: 'Select State',
        type: 'select',
        apiUrl: 'common/getstates',
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : []);
          return list.map((item: any) => ({
            value: String(item.id),
            label: item.stateName
          }));
        },
        validations: [Validators.required],
        gridColSpan: 6,
        disabledOnEdit: true
      },
      {
        id: 'districtId',
        name: 'districtId',
        label: 'District',
        type: 'select',
        placeholder: '-- Select District --',
        apiUrl: () => `vidhansabhacount/districtwise/getAll?userId=${this.authService.getUserId()}`,
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : []);
          return list.map((item: any) => ({
            value: String(item.districtId || item.id),
            label: item.dsitrictName || item.districtName || item.name
          }));
        },
        validations: [Validators.required],
        gridColSpan: 6,
        disabledOnEdit: true
      },
      {
        id: 'vidhanSabhaId',
        name: 'vidhanSabhaId',
        label: 'Vidhan Sabha',
        type: 'select',
        placeholder: '-- Select Vidhan Sabha --',
        dependsOn: 'districtId',
        apiUrl: (distId: any) => `stateprabhari/vidhansabha/getAll?districtId=${distId}&pageNumber=1&pageSize=100000&sortBy=id&IsDescending=true`,
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : []);
          return list.map((item: any) => ({
            value: String(item.id),
            label: item.vidhanSabhaName + (item.hasPrabhari ? ' (Already Assigned)' : ''),
            disabled: item.hasPrabhari
          }));
        },
        validations: [Validators.required],
        gridColSpan: 6,
        disabledOnEdit: true
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
        id: 'id',
        name: 'id',
        label: 'ID',
        type: 'hidden'
      }
    ]
  };

  constructor(
    private prabhariService: VidhanSabhaPrabhariService,
    private stateService: StateService,
    private authService: AuthServiceService,
    private toastService: ToastService,
    private crudHandler: CrudHandlerService,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.isListMode = data['mode'] === 'list';
      if (this.isListMode) {
        this.listTitle = 'Vidhan Sabha Prabhari List';
        this.actions = [];
      }
    });

    if (this.isStatePrabhari()) {
      const savedStateId = this.authService.getStateId();
      if (savedStateId) {
        this.defaultStateId = savedStateId;
        this.addPrabhariConfig.fields = this.addPrabhariConfig.fields.filter(f => f.id !== 'stateId');
        this.loadPrabharis();
      } else {
        this.stateService.getAllStates().subscribe({
          next: (response) => {
            const list = response?.data || response || [];
            if (list.length > 0) {
              this.defaultStateId = String(list[0].stateId || list[0].id);
              this.addPrabhariConfig.fields = this.addPrabhariConfig.fields.filter(f => f.id !== 'stateId');
              this.loadPrabharis();
            }
          },
          error: () => this.loadPrabharis()
        });
      }
    } else {
      this.loadPrabharis();
    }
  }

  loadPrabharis() {
    this.loading = true;
    const params: any = {
      PageNumber: this.pageNumber,
      PageSize: this.pageSize,
      SearchTerm: this.searchTerm,
      SortBy: this.sortBy,
      IsDescending: this.isDescending
    };

    if (this.defaultStateId) {
      params.stateId = this.defaultStateId;
    }

    this.prabhariService.getAllPrabharis(params).subscribe({
      next: (response) => {
        const dataWrap = response.data;
        if (dataWrap && dataWrap.items) {
          this.prabhariList = dataWrap.items;
          this.totalCount = dataWrap.totalCount || 0;
        } else {
          this.prabhariList = Array.isArray(dataWrap) ? dataWrap : (Array.isArray(response) ? response : []);
          this.totalCount = this.prabhariList.length;
        }
        this.loading = false;
      },
      error: (err) => {
        console.error('Error fetching prabharis:', err);
        this.loading = false;
      }
    });
  }

  handlePageChange(event: any) {
    this.pageNumber = event.currentPage;
    this.pageSize = event.pageSize;
    this.loadPrabharis();
  }

  handleSortChange(event: any) {
    this.sortBy = event.column;
    this.isDescending = event.direction === 'desc';
    this.pageNumber = 1;
    this.loadPrabharis();
  }

  handleSearchChange(term: string) {
    this.searchTerm = term;
    this.pageNumber = 1;
    this.loadPrabharis();
  }

  handleAction(event: any) {
    const { action, row } = event;
    if (action.id === 'delete') {
      this.crudHandler.handleRequest(
        this.prabhariService.deletePrabhari(row.id),
        'Deleted',
        'Prabhari deleted successfully!',
        () => this.loadPrabharis()
      );
    } else if (action.id === 'edit') {
      this.prabhariModal.openModal({
        ...row,
        stateId: String(row.stateId || this.defaultStateId || ''),
        districtId: row.districtId ? String(row.districtId) : '',
        vidhanSabhaId: String(row.vidhanSabhaId),
        categoryId: String(row.categoryId),
        castId: String(row.castId)
      });
    }
  }

  handleFormSubmit(result: FormResult) {
    if (!result.status) return;

    const raw = result.data;
    const initialId = this.prabhariModal.initialData?.id;
    const isUpdate = !!(raw.id || initialId);

    let submitData: any;

    if (isUpdate) {
      submitData = {
        Id: raw.id ?? initialId,
        UserId: raw.userId || this.prabhariModal.initialData?.userId || this.authService.getUserId(),
        PrabhariName: raw.prabhariName,
        PrabhariEmail: raw.prabhariEmail,
        Gender: raw.gender,
        ContactNumber: raw.contactNumber,
        CategoryId: Number(raw.categoryId),
        CastId: Number(raw.castId),
        Education: raw.education,
        Profession: raw.profession,
        CurrentAddress: raw.currentAddress
      };
    } else {
      submitData = {
        id: null,
        stateId: Number(raw.stateId || this.defaultStateId),
        districtId: Number(raw.districtId),
        vidhanSabhaId: Number(raw.vidhanSabhaId),
        isPrabhari: true,
        Prabhari: {
          stateId: Number(raw.stateId || this.defaultStateId),
          PrabhariName: raw.prabhariName,
          PrabhariEmail: raw.prabhariEmail,
          Gender: raw.gender,
          ContactNumber: raw.contactNumber,
          CategoryId: Number(raw.categoryId),
          CastId: Number(raw.castId),
          Education: raw.education,
          Profession: raw.profession,
          CurrentAddress: raw.currentAddress
        }
      };
    }

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
