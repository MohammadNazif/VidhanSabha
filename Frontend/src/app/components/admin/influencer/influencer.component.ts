import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, Validators } from '@angular/forms';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';
import { TableAction, TableColumn, TableConfig } from '../../shared/generic-table/generic-table.types';
import { GenericModalButtonComponent } from '../../shared/generic-modal-form/generic-modal-button.component';
import { FormConfig, FormResult } from '../../shared/generic-modal-form/generic-form.types';
import { PageHeaderComponent } from '../../shared/page-header/page-header.component';
import { InfluencerService } from '../../../Services/Admin/influencer/influencer.service';
import { ToastService } from '../../../Services/common/toast/toast.service';
import { CrudHandlerService } from '../../../Services/common/crud-handler.service';
import { AuthServiceService } from '../../../Services/Auth/auth.service';
import { ModulePermission } from '../../../models/module-permission.enum';
import { ActivatedRoute } from '@angular/router';
import { GenericExportComponent } from '../../shared/generic-export/generic-export.component';

@Component({
  selector: 'app-influencer',
  standalone: true,
  imports: [CommonModule, GenericTableComponent, GenericModalButtonComponent, PageHeaderComponent, ReactiveFormsModule, GenericExportComponent],
  templateUrl: './influencer.component.html',
  styleUrl: './influencer.component.css'
})
export class InfluencerComponent implements OnInit {
  @ViewChild('influencerModal') influencerModal!: GenericModalButtonComponent;

  personList: any[] = [];
  cachedPersons: any[] = [];
  totalCount = 0;
  loading = false;
  isExporting = false;

  pageNumber = 1;
  pageSize = 50;
  searchTerm = '';
  sortBy = '';
  isDescending = false;
  isListView = false;
  pageTitle = 'Influencer Person Management';
  pageSubtitle = 'Manage master data for influential people';
  exportOptions = [
    { label: 'Export PDF', value: 'pdf' },
    { label: 'Export Excel', value: 'excel' },
    { label: 'Import Excel', value: 'import' }
  ];

  canManage(): boolean {
    if (this.isListView) return false;
    const role = (this.authService.getRole() || '').toUpperCase().trim();
    if (role === 'VIDHANSABHAPRABHARI') return true;
    return true; // Influencer Master Data is usually manageable by authorized roles
  }

  columns: TableColumn[] = [
    { key: 'boothNumber', label: 'Booth No.', sortable: true },
    { key: 'villageName', label: 'Village', sortable: true },
    { key: 'name', label: 'Name', sortable: true },
    // { key: 'designationName', label: 'Designation', sortable: true },
    { key: 'mobile', label: 'Mobile No.', sortable: true },
    { key: 'categoryName', label: 'Category', sortable: true },
    { key: 'castName', label: 'Caste', sortable: true },
    { key: 'description', label: 'Description', sortable: true }
  ];

  config: TableConfig = {
    searchable: true,
    searchPlaceholder: 'Search...',
    paginated: true,
    showRowNumbers: true,
    striped: true,
    hoverable: true,
    serverSide: true,
    defaultPageSize: 50,
    filterable: false
  };

  actions: TableAction[] = [
    { id: 'edit', label: '', variant: 'default', icon: 'edit', show: () => this.canManage() },
    { id: 'delete', label: '', variant: 'danger', icon: 'delete', show: () => this.canManage() }
  ];

  formConfig: FormConfig = {
    title: 'Influencer Person Registration',
    submitLabel: 'Save Entry',
    fields: [
      {
        id: 'isEffectivePerson',
        name: 'isEffectivePerson',
        label: 'Effective Person',
        type: 'select',
        options: [
          { value: 'Yes', label: 'Yes' },
          { value: 'No', label: 'No' }
        ],
        defaultValue: 'No',
        validations: [Validators.required],
        gridColSpan: 12
      },
      // Conditional Fields for "Yes"
      {
        id: 'designationId',
        name: 'designationId',
        label: 'Designation',
        type: 'select',
        placeholder: '-- Select Designation --',
        apiUrl: 'common/getadmindesignation',
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data?.items) ? data.data.items : (Array.isArray(data?.items) ? data.items : (Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : [])));
          return list.map((item: any) => ({
            value: String(item.id),
            label: item.designationName || item.name
          }));
        },
        visibleIf: { field: 'isEffectivePerson', operator: '==', value: 'Yes' },
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'personId',
        name: 'personId',
        label: 'Person',
        type: 'select',
        placeholder: '-- Select Person --',
        dependsOn: 'designationId',
        apiUrl: (desgId: string) => `prabhavshali/getDesgById?desgId=${desgId}`,
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : []);
          this.cachedPersons = list;
          return list.map((item: any) => ({
            value: String(item.id),
            label: item.name
          }));
        },
        visibleIf: { field: 'isEffectivePerson', operator: '==', value: 'Yes' },
        validations: [Validators.required],
        gridColSpan: 6
      },
      // Conditional Fields for "No"
      {
        id: 'boothId',
        name: 'boothId',
        label: 'Booth',
        type: 'select',
        placeholder: '-- Select Booth --',
        apiUrl: () => {
          const role = (this.authService.getRole() || '').toUpperCase().trim();
          if (role === 'SECTORSANYOJAK') {
            return `booth/getAllBoothBySectorid?sectorid=${this.authService.getUserId()}`;
          }
          return 'common/boothNumber';
        },
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : []);
          return list.map((item: any) => ({
            value: String(item.boothId || item.id),
            label: `Booth No. ${item.boothNumber}`
          }));
        },
        visibleIf: { field: 'isEffectivePerson', operator: '==', value: 'No' },
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'villageId',
        name: 'villageId',
        label: 'Village',
        type: 'select',
        placeholder: '-- Select Village --',
        dependsOn: 'boothId',
        apiUrl: (boothId: string) => `common/villagesByBoothId?boothId=${boothId}`,
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data?.items) ? data.data.items : (Array.isArray(data?.items) ? data.items : (Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : [])));
          return list.map((item: any) => ({
            value: String(item.id),
            label: item.name || item.villageName
          }));
        },
        visibleIf: { field: 'isEffectivePerson', operator: '==', value: 'No' },
        validations: [Validators.required],
        gridColSpan: 6,
        multiple: true
      },
      {
        id: 'name',
        name: 'name',
        label: 'Name',
        type: 'text',
        placeholder: 'Enter Name',
        visibleIf: { field: 'isEffectivePerson', operator: '==', value: 'No' },
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
          const list = Array.isArray(data?.data?.items) ? data.data.items : (Array.isArray(data?.items) ? data.items : (Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : [])));
          return list.map((item: any) => ({
            value: String(item.id),
            label: item.name || item.category
          }));
        },
        visibleIf: { field: 'isEffectivePerson', operator: '==', value: 'No' },
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
          const list = Array.isArray(data?.data?.items) ? data.data.items : (Array.isArray(data?.items) ? data.items : (Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : [])));
          return list.map((item: any) => ({
            value: String(item.id),
            label: item.name || item.castName
          }));
        },
        visibleIf: { field: 'isEffectivePerson', operator: '==', value: 'No' },
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'mobile',
        name: 'mobile',
        label: 'Mobile',
        type: 'text',
        placeholder: 'Enter Mobile Number',
        visibleIf: { field: 'isEffectivePerson', operator: '==', value: 'No' },
        validations: [Validators.required, Validators.pattern('^[0-9]{10}$')],
        gridColSpan: 6
      },
      {
        id: 'description',
        name: 'description',
        label: 'Description',
        type: 'textarea',
        placeholder: 'Enter Description',
        visibleIf: { field: 'isEffectivePerson', operator: '==', value: 'No' },
        gridColSpan: 12
      }
    ]
  };

  constructor(
    private influencerService: InfluencerService,
    private toastService: ToastService,
    private crudHandler: CrudHandlerService,
    private authService: AuthServiceService,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.route.url.subscribe(url => {
      const path = url[0]?.path || '';
      this.isListView = path.includes('-list');

      if (this.isListView) {
        this.pageTitle = 'Influencer Person List';
        this.pageSubtitle = 'View and export influential people list';
      } else {
        this.pageTitle = 'Influencer Person Management';
        this.pageSubtitle = 'Manage master data for influential people';
      }

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
      isDescending: this.isDescending,
      userId: this.authService.getUserId()
    };

    this.influencerService.getAllInfluencer(params).subscribe({
      next: (response) => {
        const dataWrap = response.data;
        const items = dataWrap?.items || (Array.isArray(dataWrap) ? dataWrap : []);
        this.personList = items.map((item: any) => ({
          ...item,
          villageName: Array.isArray(item.villages) ? item.villages.map((v: any) => v.villageName).join(', ') : '',
          villageId: Array.isArray(item.villages) ? item.villages.map((v: any) => v.villageId || v.villageIds) : []
        }));
        this.totalCount = dataWrap?.totalCount || this.personList.length;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error fetching influencers:', err);
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
        this.influencerService.deleteInfluencer(row.id),
        'Deleted',
        'Entry deleted successfully!',
        () => this.loadData(),
        true,
        ModulePermission.EffectivePersion
      );
    } else if (action.id === 'edit') {
      if (!this.canManage()) return;
      const editData = { ...row };
      ['id', 'boothId', 'designationId', 'categoryId', 'castId'].forEach(key => {
        if (editData[key]) editData[key] = String(editData[key]);
      });
      if (editData.villageId) {
        editData.villageId = Array.isArray(editData.villageId) ? editData.villageId.map(String) : [String(editData.villageId)];
      }
      this.influencerModal.openModal(editData);
    }
  }

  handleFormSubmit(result: FormResult) {
    if (!result.status) return;

    const raw = result.data;
    const rowId = raw.id || (this.influencerModal.initialData && this.influencerModal.initialData.id);

    const submitData: any = {
      id: rowId ? Number(rowId) : null,
      boothId: Number(raw.boothId || 0),
      designationId: Number(raw.designationId || 0),
      villageIds: Array.isArray(raw.villageId) ? raw.villageId.map((v: any) => Number(v)) : (raw.villageId ? [Number(raw.villageId)] : []),
      name: raw.name,
      categoryId: Number(raw.categoryId || 0),
      castId: Number(raw.castId || 0),
      mobile: raw.mobile,
      description: raw.description,
      userId: this.authService.getUserId()
    };

    if (raw.isEffectivePerson === 'Yes') {
      const selectedPerson = this.cachedPersons.find(p => String(p.id) === String(raw.personId));
      if (selectedPerson) {
        submitData.boothId = selectedPerson.boothId;
        submitData.name = selectedPerson.name;
        submitData.categoryId = selectedPerson.categoryId;
        submitData.castId = selectedPerson.castId;
        submitData.mobile = selectedPerson.mobile;
        submitData.description = selectedPerson.description;
        submitData.villageIds = Array.isArray(selectedPerson.villages)
          ? selectedPerson.villages.map((v: any) => v.villageId)
          : (selectedPerson.villageId ? [Number(selectedPerson.villageId)] : []);
      }
    }

    const isUpdate = !!submitData.id;
    const request = isUpdate
      ? this.influencerService.updateInfluencer(submitData)
      : this.influencerService.createInfluencer(submitData);

    this.crudHandler.handleRequest(
      request,
      isUpdate ? 'Updated' : 'Success',
      `Influencer ${isUpdate ? 'updated' : 'created'} successfully!`,
      () => this.loadData(),
      true,
      ModulePermission.EffectivePersion
    );
  }

  handleExport(format: string) {
    if (!format) return;
    if (format === 'import') {
      const fileInput = document.getElementById('importFileInput') as HTMLInputElement;
      if (fileInput) fileInput.click();
      return;
    }

    this.isExporting = true;
    const exportFormat = format as 'excel' | 'pdf';
    const request = exportFormat === 'excel' ? this.influencerService.exportToExcel() : this.influencerService.exportToPdf();

    request.subscribe({
      next: (blob: Blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `Influencer_List.${format === 'excel' ? 'xlsx' : 'pdf'}`;
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
        document.body.removeChild(a);
        this.isExporting = false;
        this.toastService.showSuccess('Success', `List exported to ${format.toUpperCase()} successfully!`);
      },
      error: (err: any) => {
        console.error(`Error exporting:`, err);
        this.toastService.showError('Error', `Failed to export list`);
        this.isExporting = false;
      }
    });
  }

  onFileImport(event: any) {
    const file = event.target.files[0];
    if (!file) return;

    this.loading = true;
    this.influencerService.importExcel(file).subscribe({
      next: (res: any) => {
        this.toastService.showSuccess('Success', 'Influencer data imported successfully!');
        this.loadData();
        event.target.value = '';
      },
      error: (err: any) => {
        console.error('Import error:', err);
        this.toastService.showError('Error', 'Failed to import influencer data');
        this.loading = false;
        event.target.value = '';
      }
    });
  }
}
