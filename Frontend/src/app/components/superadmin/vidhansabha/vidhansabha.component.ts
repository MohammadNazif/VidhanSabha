import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Validators } from '@angular/forms';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';
import { TableAction, TableColumn, TableConfig } from '../../shared/generic-table/generic-table.types';
import { GenericModalButtonComponent } from '../../shared/generic-modal-form/generic-modal-button.component';
import { FormConfig, FormResult } from '../../shared/generic-modal-form/generic-form.types';

import { PageHeaderComponent } from '../../shared/page-header/page-header.component';
import { VidhanSabhaService } from '../../../Services/Admin/vidhansabha/vidhansabha.service';
import { DistrictService } from '../../../Services/Admin/district/district.service';
import { ToastService } from '../../../Services/common/toast/toast.service';
import { CrudHandlerService } from '../../../Services/common/crud-handler.service';
import { VidhanSabhaPrabhariService } from '../../../Services/Admin/vidhansabha-prabhari/vidhansabha-prabhari.service';
import { StateService } from '../../../Services/Admin/state/state.service';
import { AuthServiceService } from '../../../Services/Auth/auth.service';

@Component({
  selector: 'app-vidhansabha',
  standalone: true,
  imports: [CommonModule, PageHeaderComponent, GenericTableComponent, GenericModalButtonComponent],
  templateUrl: './vidhansabha.component.html',
  styleUrl: './vidhansabha.component.css'
})
export class VidhanSabhaComponent implements OnInit {
  @ViewChild('vidhanModal') vidhanModal!: GenericModalButtonComponent;
  @ViewChild('prabhariModal') prabhariModal!: GenericModalButtonComponent;

  vidhanList: any[] = [];
  defaultStateId: string | null = null;

  isStatePrabhari(): boolean {
    return (this.authService.getRole() || '').toUpperCase().trim() === 'STATEPRABHARI';
  }

  columns: TableColumn[] = [

    { key: 'vidhanSabhaName', label: 'Vidhan Sabha Name', sortable: true },
    { key: 'vidhanSabhaNumber', label: 'Vidhan Sabha Number', sortable: true },
    { key: 'districtName', label: 'District', sortable: true },
    {
      key: 'hasPrabhari',
      label: 'Prabhari',
      type: 'badge',
      sortable: true,
      badgeVariant: (val: any) => val === 'Yes' ? 'success' : 'danger'
    }
  ];

  config: TableConfig = {
    selectable: false,
    filterable: true,
    paginated: true,
    defaultPageSize: 10,
    pageSizeOptions: [10, 20, 50],
    searchable: true,
    searchPlaceholder: 'Search constituencies...',
    showRowNumbers: true,
    striped: true,
    hoverable: true
  };

  actions: TableAction[] = [

    { id: 'edit', label: '', variant: 'default', icon: 'edit' },
    { id: 'delete', label: '', variant: 'danger', icon: 'delete' }
  ];

  addVidhanConfig: FormConfig = {
    title: 'Add New Vidhan Sabha',
    submitLabel: 'Create Vidhan Sabha',
    fields: [
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
            label: item.districtName || item.dsitrictName || item.name
          }));
        },
        validations: [Validators.required],
        gridColSpan: 6,
        disabledOnEdit: true
      },
      {
        id: 'vidhanSabhaName',
        name: 'vidhanSabhaName',
        label: 'Vidhan Sabha Name',
        type: 'text',
        placeholder: 'Enter name',
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'vidhanSabhaNumber',
        name: 'vidhanSabhaNumber',
        label: 'Vidhan Sabha Number',
        type: 'number',
        placeholder: 'Enter number',
        validations: [Validators.required],
        gridColSpan: 6
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
    private vidhanService: VidhanSabhaService,
    private vidhanPrabhariService: VidhanSabhaPrabhariService,
    private districtService: DistrictService,
    private stateService: StateService,
    private authService: AuthServiceService,
    private toastService: ToastService,
    private crudHandler: CrudHandlerService
  ) { }

  ngOnInit() {
    if (this.isStatePrabhari()) {
      this.stateService.getAllStates().subscribe({
        next: (response) => {
          const list = response?.data || response || [];
          if (list.length > 0) {
            this.defaultStateId = String(list[0].stateId || list[0].id);

            const districtField = this.addVidhanConfig.fields.find(f => f.id === 'districtId');
            if (districtField) {
              districtField.apiUrl = () => `vidhansabhacount/districtwise/getAll?userId=${this.authService.getUserId()}`;
            }
            // Load data only after defaultStateId is ready
            this.loadVidhanSabhas();
          }
        },
        error: () => this.loadVidhanSabhas() // Fallback load
      });
    } else {
      this.loadVidhanSabhas();
    }
  }

  loadVidhanSabhas() {
    this.vidhanService.getVidhanSabhasByStateId(this.defaultStateId).subscribe({
      next: (response) => {
        if (response && response.isSuccess) {
          const list = response.data || [];
          this.vidhanList = list.map((item: any) => ({
            ...item,
            hasPrabhari: item.hasPrabhari ? 'Yes' : 'No'
          }));
        } else {
          this.vidhanList = [];
        }
      },
      error: (err) => console.error('Error fetching Vidhan Sabhas:', err)
    });
  }

  handleAction(event: any) {
    const { action, row } = event;
    if (action.id === 'delete') {
      this.crudHandler.handleRequest(
        this.vidhanService.deleteVidhanSabha(row.id),
        'Deleted',
        'Vidhan Sabha deleted successfully!',
        () => this.loadVidhanSabhas()
      );
    } else if (action.id === 'edit') {
      this.vidhanModal.openModal({
        ...row,
        districtId: String(row.districtId),
        stateId: row.stateId,
        assignPrabhari: row.isPrabhariAssigned === 'Yes' ? 'Yes' : 'No'
      });
    } else if (action.id === 'assign_vidhansabha') {
      this.vidhanModal.openModal({
        ...row,
        districtId: String(row.districtId),
        stateId: row.stateId,
        assignPrabhari: row.isPrabhariAssigned === 'Yes' ? 'Yes' : 'No'
      });
    }
  }

  handleFormSubmit(result: FormResult) {
    if (!result.status) return;

    const raw = result.data;
    const userId = this.authService.getUserId();

    const vsData = {
      ...raw,
      id: raw.id ? Number(raw.id) : null,
      districtId: Number(raw.districtId),
      vidhanSabhaNumber: Number(raw.vidhanSabhaNumber),
      stateId: Number(raw.stateId || this.defaultStateId),
      userId: userId ? String(userId) : null
    };

    const isUpdate = !!vsData.id;
    const request = isUpdate
      ? this.vidhanService.updateVidhanSabha(vsData)
      : this.vidhanService.createVidhanSabha(vsData);

    this.crudHandler.handleRequest(
      request,
      isUpdate ? 'Updated' : 'Success',
      `Vidhan Sabha ${isUpdate ? 'updated' : 'created'} successfully!`,
      () => this.loadVidhanSabhas()
    );
  }

  handlePrabhariSubmit(result: FormResult) {
    if (!result.status) return;

    const raw = result.data;
    const vidhanSabhaId = this.prabhariModal.initialData?.vidhanSabhaId;

    if (!vidhanSabhaId) {
      this.toastService.showError('Error', 'Constituency ID missing');
      return;
    }

    const isUpdate = !!(raw.id || this.prabhariModal.initialData?.id);
    const submitData = {
      ...raw,
      vidhanSabhaId: Number(vidhanSabhaId),
      stateId: Number(this.prabhariModal.initialData?.stateId || this.defaultStateId),
      castId: Number(raw.castId),
      categoryId: Number(raw.categoryId)
    };

    const request = isUpdate
      ? this.vidhanPrabhariService.updatePrabhari(submitData)
      : this.vidhanPrabhariService.createPrabhari(submitData);

    this.crudHandler.handleRequest(
      request,
      isUpdate ? 'Updated' : 'Created',
      `Prabhari successfully ${isUpdate ? 'updated' : 'assigned'} to the constituency!`,
      () => this.loadVidhanSabhas()
    );
  }

  handleExport(format: string) {
    if (!format) return;
    this.toastService.showSuccess('Export Started', `Successfully generated ${format.toUpperCase()} export!`);
  }
}
