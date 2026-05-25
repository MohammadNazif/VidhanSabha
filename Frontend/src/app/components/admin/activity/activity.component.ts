import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, Validators } from '@angular/forms';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';
import { TableConfig, TableColumn, TableAction } from '../../shared/generic-table/generic-table.types';
import { GenericModalButtonComponent } from '../../shared/generic-modal-form/generic-modal-button.component';
import { FormConfig, FormResult } from '../../shared/generic-modal-form/generic-form.types';
import { PageHeaderComponent } from '../../shared/page-header/page-header.component';
import { ActivityService } from '../../../Services/Admin/activity/activity.service';
import { ToastService } from '../../../Services/common/toast/toast.service';
import { CrudHandlerService } from '../../../Services/common/crud-handler.service';
import { AuthServiceService } from '../../../Services/Auth/auth.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-activity',
  standalone: true,
  imports: [CommonModule, FormsModule, GenericTableComponent, GenericModalButtonComponent, PageHeaderComponent],
  template: `
    <div class="h-full flex flex-col p-4 gap-4 overflow-hidden">
      <app-page-header [title]="canManage() ? 'Activity Management' : 'Activity List'" subtitle="Manage and publish legislative activities">
        <app-generic-modal-button 
            *ngIf="canManage()"
            #activityModal 
            [config]="addActivityConfig" 
            label="Add Activity" 
            icon="+"
            variant="primary" 
            (formSubmit)="handleFormSubmit($event)">
        </app-generic-modal-button>
      </app-page-header>

      <div class="flex-1 min-h-0 bg-white rounded-xl shadow-sm border border-slate-200 overflow-hidden flex flex-col p-2">
        <app-generic-table
          [config]="tableConfig"
          [columns]="columns"
          [actions]="actions"
          [data]="activities"
          [loading]="loading"
          [totalItems]="totalItems"
          (actionClick)="handleAction($event)"
          (searchChange)="handleSearchChange($event)"
          (pageChange)="handlePageChange($event)"
          (sortChange)="handleSortChange($event)">
        </app-generic-table>
      </div>

      <!-- Gallery Modal -->
      <div *ngIf="isGalleryOpen" class="fixed inset-0 z-[60] flex items-center justify-center bg-black/60 backdrop-blur-sm p-4 animate-in fade-in duration-300" (click)="isGalleryOpen = false">
        <div class="relative max-w-4xl w-full bg-white rounded-2xl overflow-hidden shadow-2xl animate-in zoom-in-95 duration-300" (click)="$event.stopPropagation()">
          <div class="flex justify-between items-center p-4 border-b bg-slate-50">
            <div>
              <h3 class="text-lg font-bold text-slate-800">Activity Gallery</h3>
              <p class="text-xs text-slate-500">{{ selectedImages.length }} images found</p>
            </div>
            <button (click)="isGalleryOpen = false" class="w-8 h-8 flex items-center justify-center rounded-full hover:bg-slate-200 transition-colors text-slate-500">
               <span class="text-2xl leading-none">&times;</span>
            </button>
          </div>
          <div class="p-6 max-h-[70vh] overflow-y-auto">
            <div class="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 gap-4">
               <div *ngFor="let img of selectedImages" class="group relative aspect-square rounded-xl overflow-hidden border border-slate-200 bg-slate-100 cursor-pointer shadow-sm hover:shadow-md transition-all">
                  <img [src]="getImageUrl(img)" class="w-full h-full object-cover transition-transform duration-500 group-hover:scale-110" />
                  <div class="absolute inset-0 bg-black/0 group-hover:bg-black/10 transition-colors"></div>
               </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  `
})
export class ActivityComponent implements OnInit {
  @ViewChild('activityModal') activityModal!: GenericModalButtonComponent;

  activities: any[] = [];
  loading = false;
  totalItems = 0;
  searchTerm = '';
  currentPage = 1;
  pageSize = 10;
  sortState: any = { column: 'id', direction: 'desc' };

  isGalleryOpen = false;
  selectedImages: string[] = [];

  canManage(): boolean {
    const role = (this.authService.getRole() || '').toUpperCase().trim();
    return ['SUPERADMIN', 'ADMIN', 'VIDHANSABHAPRABHARI'].includes(role);
  }

  columns: TableColumn[] = [

    { key: 'title', label: 'Title', sortable: true },
    { key: 'date', label: 'Date', sortable: true, formatter: (val) => val ? new Date(val).toLocaleDateString() : 'N/A' },
    { key: 'description', label: 'Description', truncate: 50 },

  ];

  actions: TableAction[] = [
    { id: 'view', label: '', icon: 'view', variant: 'default', show: (row) => !!(row.youTubeLink || row.videoPath) },
    { id: 'gallery', label: '', icon: 'images', variant: 'warning', show: (row) => !!(row.imagePaths?.length) },
    { id: 'edit', label: '', icon: 'edit', variant: 'primary', show: () => this.canManage() },
    { id: 'delete', label: '', icon: 'delete', variant: 'danger', show: () => this.canManage() }
  ];

  tableConfig: TableConfig = {
    selectable: false,
    paginated: true,
    searchable: true,
    searchPlaceholder: 'Search activities...',
    serverSide: true,
    defaultPageSize: 10
  };

  addActivityConfig: FormConfig = {
    title: 'Add Activity',
    submitLabel: 'Save Activity',
    fields: [
      {
        id: 'Title',
        name: 'Title',
        label: 'Title',
        type: 'text',
        placeholder: 'Enter activity title',
        gridColSpan: 6,
        validations: [Validators.required]
      },
      {
        id: 'Date',
        name: 'Date',
        label: 'Date',
        type: 'date',
        gridColSpan: 6,
        validations: [Validators.required]
      },
      {
        id: 'Description',
        name: 'Description',
        label: 'Description',
        type: 'textarea',
        placeholder: 'Enter detailed description',
        gridColSpan: 12,
        validations: [Validators.required]
      },
      {
        id: 'videoType',
        name: 'videoType',
        label: 'Video Content Type',
        type: 'select',
        defaultValue: 'none',
        options: [
          { value: 'none', label: 'No Video' },
          { value: 'youtube', label: 'YouTube Link' },
          { value: 'upload', label: 'Video File' }
        ],
        gridColSpan: 6
      },
      {
        id: 'YouTubeLink',
        name: 'YouTubeLink',
        label: 'YouTube URL',
        type: 'text',
        placeholder: 'https://youtube.com/...',
        visibleIf: { field: 'videoType', operator: '==', value: 'youtube' },
        gridColSpan: 6
      },
      {
        id: 'VideoFile',
        name: 'VideoFile',
        label: 'Upload Video',
        type: 'file',
        visibleIf: { field: 'videoType', operator: '==', value: 'upload' },
        gridColSpan: 6
      },
      {
        id: 'Images',
        name: 'Images',
        label: 'Images (Up to 4)',
        type: 'file',
        multiple: true,
        gridColSpan: 12
      }
    ]
  };

  constructor(
    private activityService: ActivityService,
    private toastService: ToastService,
    private crudHandler: CrudHandlerService,
    private authService: AuthServiceService,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.loadActivities();
  }

  loadActivities() {
    this.loading = true;
    const params = {
      searchTerm: this.searchTerm,
      pageNumber: this.currentPage,
      pageSize: this.pageSize,
      sortColumn: this.sortState.column,
      isDescending: this.sortState.direction === 'desc'
    };

    this.activityService.getAllActivities(params).subscribe({
      next: (res: any) => {
        this.activities = res.data?.items || res.data || [];
        this.totalItems = res.data?.totalCount || res.meta?.totalCount || this.activities.length;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.toastService.showError('Error', 'Failed to load activities');
      }
    });
  }

  handleAction(event: { action: TableAction; row: any; index: number }) {
    const { action, row } = event;
    if (action.id === 'view') {
      const url = row.youTubeLink || this.getImageUrl(row.videoPath);
      if (url) window.open(url, '_blank');
    } else if (action.id === 'gallery') {
      this.selectedImages = row.imagePaths || [];
      this.isGalleryOpen = true;
    } else if (action.id === 'delete') {
      this.crudHandler.handleRequest(
        this.activityService.deleteActivity(row.id),
        'Deleted',
        'Activity deleted successfully!',
        () => this.loadActivities()
      );
    } else if (action.id === 'edit') {
      const editData = {
        id: row.id,
        Title: row.title,
        Date: row.date ? row.date.split('T')[0] : '',
        Description: row.description,
        YouTubeLink: row.youTubeLink,
        videoType: row.youTubeLink ? 'youtube' : (row.videoPath ? 'upload' : 'none')
      };
      this.activityModal.openModal(editData);
    }
  }

  handleSearchChange(term: string) {
    this.searchTerm = term;
    this.currentPage = 1;
    this.loadActivities();
  }

  handlePageChange(state: any) {
    this.currentPage = state.currentPage;
    this.pageSize = state.pageSize;
    this.loadActivities();
  }

  handleSortChange(state: any) {
    this.sortState = state;
    this.loadActivities();
  }

  handleFormSubmit(result: FormResult) {
    if (!result.status) return;

    const raw = result.data;
    const files = result.files;
    const rowId = this.activityModal.initialData?.id || raw.id;

    const formData = new FormData();
    if (rowId) formData.append('Id', String(rowId));

    // Core fields - matching PascalCase DTO properties
    formData.append('Title', raw.Title || '');
    formData.append('Date', raw.Date || '');
    formData.append('Description', raw.Description || '');

    // Video Source Handling
    if (raw.videoType === 'youtube' && raw.YouTubeLink) {
      formData.append('YouTubeLink', raw.YouTubeLink);
    } else if (raw.videoType === 'upload' && files?.['VideoFile']) {
      formData.append('VideoFile', files['VideoFile']);
    }

    // Images Handling (List<IFormFile>)
    if (files && files['Images']) {
      const images = files['Images'];
      if (Array.isArray(images)) {
        images.forEach(file => {
          formData.append('Images', file);
        });
      } else {
        formData.append('Images', images);
      }
    }

    const isUpdate = !!rowId;
    const request = isUpdate
      ? this.activityService.updateActivity(formData)
      : this.activityService.createActivity(formData);

    this.crudHandler.handleRequest(
      request,
      isUpdate ? 'Updated' : 'Created',
      `Activity ${isUpdate ? 'updated' : 'created'} successfully!`,
      () => this.loadActivities()
    );
  }

  getImageUrl(path: string): string {
    if (!path) return '';
    if (path.startsWith('http')) return path;
    const baseUrl = (this.activityService as any).apiUrl.replace('/api', '');
    return `${baseUrl}/${path}`;
  }
}
