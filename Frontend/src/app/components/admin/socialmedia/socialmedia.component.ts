import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Validators } from '@angular/forms';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';
import { TableAction, TableColumn, TableConfig } from '../../shared/generic-table/generic-table.types';
import { GenericModalButtonComponent } from '../../shared/generic-modal-form/generic-modal-button.component';
import { FormConfig, FormResult } from '../../shared/generic-modal-form/generic-form.types';
import { PageHeaderComponent } from '../../shared/page-header/page-header.component';
import { SocialMediaService } from '../../../Services/Admin/socialmedia/socialmedia.service';
import { ToastService } from '../../../Services/common/toast/toast.service';
import { CrudHandlerService } from '../../../Services/common/crud-handler.service';
import { AuthServiceService } from '../../../Services/Auth/auth.service';
import { ActivatedRoute } from '@angular/router';
import { ModulePermission } from '../../../models/module-permission.enum';
import { PermissionService } from '../../../Services/common/permission.service';

@Component({
  selector: 'app-socialmedia',
  standalone: true,
  imports: [CommonModule, PageHeaderComponent, GenericTableComponent, GenericModalButtonComponent],
  templateUrl: './socialmedia.component.html',
  styleUrl: './socialmedia.component.css'
})
export class SocialMediaComponent implements OnInit {
  @ViewChild('socialMediaModal') socialMediaModal!: GenericModalButtonComponent;

  socialMediaList: any[] = [];
  totalCount = 0;
  loading = false;

  // Server-side state
  pageNumber = 1;
  pageSize = 50;
  searchTerm = '';
  sortBy = '';
  isDescending = true;
  isListView = false;

  canManage(): boolean {
    return !this.isListView && this.permissionService.hasPermission(ModulePermission.SocialMedia);
  }

  columns: TableColumn[] = [
    { key: 'title', label: 'Title', sortable: true },
    {
      key: 'platforms', label: 'Platforms', sortable: false, formatter: (val: any, row: any) => {
        return row.platforms && Array.isArray(row.platforms) ? row.platforms.map((p: any) => p.platformName).join(', ') : 'N/A';
      }
    },
    {
      key: 'sectors', label: 'Sectors', sortable: false, formatter: (val: any, row: any) => {
        return row.sectors && Array.isArray(row.sectors) ? row.sectors.map((s: any) => s.sectorName).join(', ') : 'N/A';
      }
    },
    {
      key: 'booths', label: 'Booths', sortable: false, formatter: (val: any, row: any) => {
        return row.booths && Array.isArray(row.booths) ? row.booths.map((b: any) => b.boothNo).join(', ') : 'N/A';
      }
    },
    { key: 'description', label: 'Description', sortable: true }
  ];

  config: TableConfig = {
    searchable: true,
    paginated: true,
    filterable: false,
    showRowNumbers: true,
    striped: true,
    hoverable: true,
    serverSide: true,
    defaultPageSize: 50
  };

  actions: TableAction[] = [
    { id: 'edit', label: '', variant: 'default', icon: 'edit', show: () => this.canManage() },
    { id: 'delete', label: '', variant: 'danger', icon: 'delete', show: () => this.canManage() }
  ];

  addSocialMediaConfig: FormConfig = {
    title: 'Create Social Media Post',
    submitLabel: 'Create',
    fields: [
      {
        id: 'id',
        name: 'id',
        label: 'ID',
        type: 'hidden'
      },
      {
        id: 'title',
        name: 'title',
        label: 'Title',
        type: 'text',
        placeholder: 'Enter Title',
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'platformIds',
        name: 'platformIds',
        label: 'Platforms',
        type: 'select',
        placeholder: '--Select Platforms--',
        apiUrl: 'socialmedia/getAllPlatform',
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : []);
          return list.map((item: any) => ({
            value: String(item.id),
            label: item.platformName
          }));
        },
        validations: [Validators.required],
        gridColSpan: 6,
        multiple: true
      },
      {
        id: 'description',
        name: 'description',
        label: 'Description',
        type: 'textarea',
        placeholder: 'Enter Description',
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'postImagePath',
        name: 'postImagePath',
        label: 'Post Image',
        type: 'file',
        placeholder: 'Upload Post Image',
        validations: [],
        gridColSpan: 6
      },

      {
        id: 'sectorIds',
        name: 'sectorIds',
        label: 'Sectors',
        type: 'select',
        placeholder: '--Select Sectors--',
        apiUrl: 'sector/getAll?PageNumber=1&PageSize=500000&IsDescending=false',
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data?.items) ? data.data.items : (Array.isArray(data?.items) ? data.items : (Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : [])));
          return list.map((item: any) => ({
            value: String(item.sectorId || item.id),
            label: item.sectorName || item.name
          }));
        },
        validations: [],
        gridColSpan: 6,
        multiple: true
      },
      {
        id: 'boothIds',
        name: 'boothIds',
        label: 'Booths',
        type: 'select',
        placeholder: '--Select Booths--',
        apiUrl: 'common/boothNumber',
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : []);
          return list.map((item: any) => ({
            value: String(item.boothId || item.id),
            label: `Booth No. ${item.boothNumber}`
          }));
        },
        validations: [],
        gridColSpan: 6,
        multiple: true
      }
    ]
  };

  constructor(
    private socialMediaService: SocialMediaService,
    private toastService: ToastService,
    private crudHandler: CrudHandlerService,
    private authService: AuthServiceService,
    private route: ActivatedRoute,
    private permissionService: PermissionService
  ) { }

  ngOnInit() {
    this.route.url.subscribe((url: any) => {
      const path = url[0]?.path || '';
      this.isListView = path.includes('-list');
      this.config.filterable = this.isListView;
      this.loadData();
    });
  }

  loadData() {
    this.loading = true;
    const params: any = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      searchTerm: this.searchTerm,
      sortBy: this.sortBy,
      isDescending: this.isDescending
    };

    this.socialMediaService.getAllSocialMedia(params).subscribe({
      next: (response) => {
        const dataWrap = response.data;
        const items = dataWrap?.items || (Array.isArray(dataWrap) ? dataWrap : []);

        this.socialMediaList = items;
        this.totalCount = dataWrap?.totalCount || this.socialMediaList.length;
        this.loading = false;
      },
      error: (err) => {
        console.warn('Error fetching social media list:', err);
        this.socialMediaList = [];
        this.totalCount = 0;
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
        this.socialMediaService.deleteSocialMedia(row.id),
        'Deleted',
        'Social Media deleted successfully!',
        () => this.loadData(),
        true,
        ModulePermission.SocialMedia
      );
    } else if (action.id === 'edit') {
      const editData = { ...row };

      if (editData.id) {
        editData.id = String(editData.id);
      }

      if (editData.platforms && Array.isArray(editData.platforms)) {
        editData.platformIds = editData.platforms.map((p: any) => String(p.platformId));
      }
      if (editData.sectors && Array.isArray(editData.sectors)) {
        editData.sectorIds = editData.sectors.map((s: any) => String(s.sectorId));
      }
      if (editData.booths && Array.isArray(editData.booths)) {
        editData.boothIds = editData.booths.map((b: any) => String(b.boothId));
      }

      this.socialMediaModal.openModal(editData);
    }
  }

  handleFormSubmit(result: FormResult) {
    if (!result.status) return;

    const raw = result.data;
    const formData = new FormData();

    formData.append('title', raw.title || '');
    formData.append('description', raw.description || '');

    // Handle file upload
    if (result.files && result.files['postImagePath']) {
      formData.append('postImagePath', result.files['postImagePath']);
    }

    // Handle array fields
    if (Array.isArray(raw.platformIds)) {
      raw.platformIds.forEach((v: any) => formData.append('platformIds', String(v)));
    } else if (raw.platformIds) {
      formData.append('platformIds', String(raw.platformIds));
    }

    if (Array.isArray(raw.sectorIds)) {
      raw.sectorIds.forEach((v: any) => formData.append('sectorIds', String(v)));
    } else if (raw.sectorIds) {
      formData.append('sectorIds', String(raw.sectorIds));
    }

    if (Array.isArray(raw.boothIds)) {
      raw.boothIds.forEach((v: any) => formData.append('boothIds', String(v)));
    } else if (raw.boothIds) {
      formData.append('boothIds', String(raw.boothIds));
    }

    const isUpdate = !!raw.id && raw.id !== '0';
    const request = isUpdate
      ? this.socialMediaService.updateSocialMedia(formData)
      : this.socialMediaService.createSocialMedia(formData);

    this.crudHandler.handleRequest(
      request,
      isUpdate ? 'Updated' : 'Success',
      `Social Media post ${isUpdate ? 'updated' : 'created'} successfully!`,
      () => this.loadData(),
      true,
      ModulePermission.SocialMedia
    );
  }

  handleExport(format: string) {
    if (!format) return;
    this.toastService.showSuccess('Export Started', `Successfully generated ${format.toUpperCase()} export!`);
  }
}
