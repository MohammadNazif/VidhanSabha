import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, Validators } from '@angular/forms';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';
import { TableAction, TableColumn, TableConfig } from '../../shared/generic-table/generic-table.types';
import { GenericModalButtonComponent } from '../../shared/generic-modal-form/generic-modal-button.component';
import { FormConfig, FormResult } from '../../shared/generic-modal-form/generic-form.types';
import { PageHeaderComponent } from '../../shared/page-header/page-header.component';
import { StateMemberService } from '../../../Services/Admin/state-member/state-member.service';
import { ToastService } from '../../../Services/common/toast/toast.service';
import { CrudHandlerService } from '../../../Services/common/crud-handler.service';
import { AuthServiceService } from '../../../Services/Auth/auth.service';
import { ActivatedRoute } from '@angular/router';
import { GenericExportComponent } from '../../shared/generic-export/generic-export.component';

@Component({
  selector: 'app-state-member-mgmt',
  standalone: true,
  imports: [CommonModule, GenericTableComponent, GenericModalButtonComponent, PageHeaderComponent, ReactiveFormsModule, GenericExportComponent],
  templateUrl: './state-member.component.html',
  styleUrl: './state-member.component.css'
})
export class StateMemberMgmtComponent implements OnInit {
  @ViewChild('memberModal') memberModal!: GenericModalButtonComponent;

  memberList: any[] = [];
  totalCount = 0;
  loading = false;
  isExporting = false;

  // Table state
  pageNumber = 1;
  pageSize = 50;
  searchTerm = '';
  sortBy = 'id';
  isDescending = true;
  isListMode = false;
  filterDesignationId: number | null = null;
  filterSamitiType: any = null;
  listTitle = 'Samiti Member Management';

  columns: TableColumn[] = [
    { key: 'name', label: 'Name', sortable: true },
    { key: 'designationName', label: 'Designation', sortable: true },
    { key: 'designationypeName', label: 'Type', sortable: true },
    { key: 'email', label: 'Email', sortable: true },
    { key: 'mobile', label: 'Mobile', sortable: true },
    { key: 'education', label: 'Education', sortable: true },
    { key: 'proffesion', label: 'Profession', sortable: true }
  ];

  config: TableConfig = {
    searchable: true,
    searchPlaceholder: 'Search...',
    paginated: true,
    showRowNumbers: true,
    striped: true,
    hoverable: true,
    serverSide: true,
    defaultPageSize: 50
  };

  actions: TableAction[] = [
    { id: 'edit', label: '', variant: 'default', icon: 'edit', show: () => true },
    { id: 'delete', label: '', variant: 'danger', icon: 'delete', show: () => true }
  ];

  formConfig: FormConfig = {
    title: 'Samiti Member Details',
    submitLabel: 'Save Member',
    fields: [
      {
        id: 'designationId',
        name: 'designationId',
        label: 'Designation',
        type: 'select',
        placeholder: '-- Select Designation --',
        apiUrl: 'designation/getAll',
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data?.items) ? data.data.items : (Array.isArray(data?.items) ? data.items : (Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : [])));
          return list.map((item: any) => ({
            value: String(item.id),
            label: item.designationName || item.name
          }));
        },
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'designationTypeId',
        name: 'designationTypeId',
        label: 'Designation Type',
        type: 'select',
        placeholder: '-- Select Type --',
        dependsOn: 'designationId',
        apiUrl: 'common/designationType',
        apiMapper: (data: any, formValues: any) => {
          const list = Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : []);
          const selectedDesignationId = formValues?.designationId;

          return list.map((item: any) => {
            // Check if this (designationId + designationTypeId) already exists in memberList
            // Note: This check is based on the currently loaded member list
            const isAlreadyTaken = this.memberList.some(m =>
              String(m.designationId) === String(selectedDesignationId) &&
              String(m.designationTypeId) === String(item.id)
            );

            return {
              value: String(item.id),
              label: item.designationName || item.name,
              disabled: isAlreadyTaken
            };
          });
        },
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'name',
        name: 'name',
        label: 'Full Name',
        type: 'text',
        placeholder: 'Enter full name',
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'email',
        name: 'email',
        label: 'Email',
        type: 'email',
        placeholder: 'Enter email address',
        validations: [Validators.required, Validators.email],
        gridColSpan: 6
      },
      {
        id: 'mobile',
        name: 'mobile',
        label: 'Mobile No',
        type: 'text',
        placeholder: 'Enter 10-digit mobile no',
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
        id: 'dob',
        name: 'dob',
        label: 'Date of Birth',
        type: 'date',
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'education',
        name: 'education',
        label: 'Education',
        type: 'text',
        placeholder: 'Enter education',
        gridColSpan: 6
      },
      {
        id: 'proffesion',
        name: 'proffesion',
        label: 'Profession',
        type: 'text',
        placeholder: 'Enter profession',
        gridColSpan: 6
      },
      {
        id: 'profile',
        name: 'profile',
        label: 'Profile Picture',
        type: 'file',
        gridColSpan: 6
      },
      {
        id: 'address',
        name: 'address',
        label: 'Address',
        type: 'textarea',
        placeholder: 'Enter address',
        gridColSpan: 6
      }
    ]
  };

  constructor(
    private memberService: StateMemberService,
    private toastService: ToastService,
    private crudHandler: CrudHandlerService,
    private authService: AuthServiceService,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.isListMode = data['mode'] === 'list';
      if (this.isListMode) {
        this.listTitle = 'Samiti Member List';
        this.actions = [];
      }
      this.filterDesignationId = data['designationId'] || null;
      this.filterSamitiType = data['samitiType'] || null;

      if (this.filterSamitiType === 1 || this.filterSamitiType === '1') this.listTitle = 'Pradesh Samiti List';
      else if (this.filterSamitiType === 2 || this.filterSamitiType === '2') this.listTitle = 'Pradesh Karyakarini Samiti List';
      else if (this.isListMode) this.listTitle = 'Samiti Member List';
    });
    this.loadData();
  }

  loadData() {
    this.loading = true;
    const params = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      searchTerm: this.searchTerm,
      sortBy: this.sortBy,
      isDescending: this.isDescending,
      designationId: this.filterDesignationId,
      samitiType: this.filterSamitiType
    };

    this.memberService.getAllMembers(params).subscribe({
      next: (response) => {
        const dataWrap = response.data;
        if (dataWrap && dataWrap.items) {
          this.memberList = dataWrap.items;
          this.totalCount = dataWrap.totalCount || 0;
        } else {
          this.memberList = Array.isArray(dataWrap) ? dataWrap : [];
          this.totalCount = this.memberList.length;
        }
        this.loading = false;
      },
      error: (err) => {
        console.error('Error fetching state members:', err);
        this.loading = false;
      }
    });
  }

  handlePageChange(event: any) {
    this.pageNumber = event.currentPage;
    this.pageSize = event.pageSize;
    this.loadData();
  }

  handleSortChange(event: any) {
    this.sortBy = event.column;
    this.isDescending = event.direction === 'desc';
    this.pageNumber = 1;
    this.loadData();
  }

  handleSearchChange(term: string) {
    this.searchTerm = term;
    this.pageNumber = 1;
    this.loadData();
  }

  handleAction(event: any) {
    const { action, row } = event;
    if (action.id === 'delete') {
      this.crudHandler.handleRequest(
        this.memberService.deleteMember(row.id),
        'Deleted',
        'Member deleted successfully!',
        () => this.loadData()
      );
    } else if (action.id === 'edit') {
      const editData = { ...row };
      ['designationId', 'designationTypeId', 'categoryId', 'castId'].forEach(key => {
        if (editData[key]) editData[key] = String(editData[key]);
      });
      this.memberModal.openModal(editData);
    }
  }

  handleFormSubmit(result: FormResult) {
    if (!result.status) return;

    const raw = result.data;
    const formData = new FormData();

    Object.keys(raw).forEach(key => {
      if (raw[key] !== null && raw[key] !== undefined && key !== 'profile') {
        formData.append(key, raw[key]);
      }
    });

    if (result.files && result.files['profile']) {
      formData.append('Profile', result.files['profile']);
    }

    const rowId = raw.id || (this.memberModal.initialData && this.memberModal.initialData.id);
    const isUpdate = !!rowId;
    if (isUpdate) formData.append('id', rowId);

    const request = isUpdate
      ? this.memberService.updateMember(formData)
      : this.memberService.createMember(formData);

    this.crudHandler.handleRequest(
      request,
      isUpdate ? 'Updated' : 'Success',
      `Member ${isUpdate ? 'updated' : 'created'} successfully!`,
      () => this.loadData()
    );
  }

  handleExport(format: string) {
    if (!format) return;
    this.isExporting = true;

    const exportFormat = format as 'excel' | 'pdf';
    this.memberService.export('statemembers', exportFormat).subscribe({
      next: (blob: Blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `State_Members_List.${format === 'excel' ? 'xlsx' : 'pdf'}`;
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
        document.body.removeChild(a);
        this.isExporting = false;
        this.toastService.showSuccess('Success', `List exported to ${format.toUpperCase()} successfully!`);
      },
      error: (err: any) => {
        console.error(`Error exporting to ${format}:`, err);
        this.toastService.showError('Error', `Failed to export list to ${format.toUpperCase()}`);
        this.isExporting = false;
      }
    });
  }
}
