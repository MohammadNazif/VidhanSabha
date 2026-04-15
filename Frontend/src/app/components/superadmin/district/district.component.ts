import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Validators } from '@angular/forms';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';
import { TableAction, TableColumn, TableConfig } from '../../shared/generic-table/generic-table.types';
import { GenericModalButtonComponent } from '../../shared/generic-modal-form/generic-modal-button.component';
import { FormConfig, FormResult } from '../../shared/generic-modal-form/generic-form.types';

import { PageHeaderComponent } from '../../shared/page-header/page-header.component';
import { DistrictService } from '../../../Services/Admin/district/district.service';
import { StateService } from '../../../Services/Admin/state/state.service';
import { ToastService } from '../../../Services/common/toast/toast.service';
import { CrudHandlerService } from '../../../Services/common/crud-handler.service';

@Component({
  selector: 'app-district',
  standalone: true,
  imports: [CommonModule, PageHeaderComponent, GenericTableComponent, GenericModalButtonComponent],
  templateUrl: './district.component.html',
  styleUrl: './district.component.css'
})
export class DistrictComponent implements OnInit {
  @ViewChild('districtModal') districtModal!: GenericModalButtonComponent;

  districtList: any[] = [];

  columns: TableColumn[] = [
    { key: 'id', label: 'ID', sortable: true },
    { key: 'name', label: 'District Name', sortable: true },
    { key: 'stateName', label: 'State', sortable: true }
  ];

  config: TableConfig = {
    selectable: false,
    filterable: true,
    paginated: true,
    defaultPageSize: 10,
    pageSizeOptions: [10, 20, 50],
    searchable: true,
    searchPlaceholder: 'Search districts...',
    showRowNumbers: true,
    striped: true,
    hoverable: true
  };

  actions: TableAction[] = [
    { id: 'edit', label: '', variant: 'default', icon: 'edit' },
    { id: 'delete', label: '', variant: 'danger', icon: 'delete' }
  ];

  addDistrictConfig: FormConfig = {
    title: 'Add New District',
    submitLabel: 'Create District',
    fields: [
      {
        id: 'stateId',
        name: 'stateId',
        label: 'Select State',
        type: 'select',
        apiUrl: () => `state/getAll`,
        apiMapper: (list: any[]) => list.map(item => ({ value: String(item.id), label: item.name })),
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'name',
        name: 'name',
        label: 'District Name',
        type: 'text',
        placeholder: 'Enter district name',
        validations: [Validators.required],
        gridColSpan: 6
      }
    ]
  };

  constructor(
    private districtService: DistrictService,
    private stateService: StateService,
    private toastService: ToastService,
    private crudHandler: CrudHandlerService
  ) { }

  ngOnInit() {
    this.loadDistricts();
  }

  loadDistricts() {
    this.districtService.getAllDistricts().subscribe({
      next: (response) => {
        if (response && response.isSuccess) {
          this.districtList = response.data;
        } else if (Array.isArray(response)) {
          this.districtList = response;
        } else {
          this.districtList = [];
        }
      },
      error: (err) => console.error('Error fetching districts:', err)
    });
  }

  handleAction(event: any) {
    const { action, row } = event;
    if (action.id === 'delete') {
      this.crudHandler.handleRequest(
        this.districtService.deleteDistrict(row.id),
        'Deleted',
        'District deleted successfully!',
        () => this.loadDistricts()
      );
    } else if (action.id === 'edit') {
      this.districtModal.openModal({
        ...row,
        stateId: String(row.stateId)
      });
    }
  }

  handleFormSubmit(result: FormResult) {
    if (!result.status) return;

    const raw = result.data;
    const submitData = {
      ...raw,
      id: raw.id ? Number(raw.id) : null,
      stateId: Number(raw.stateId)
    };

    const isUpdate = !!submitData.id;
    const request = isUpdate
      ? this.districtService.updateDistrict(submitData)
      : this.districtService.createDistrict(submitData);

    this.crudHandler.handleRequest(
      request,
      isUpdate ? 'Updated' : 'Success',
      `District ${isUpdate ? 'updated' : 'created'} successfully!`,
      () => this.loadDistricts()
    );
  }

  handleExport(format: string) {
    if (!format) return;
    this.toastService.showSuccess('Export Started', `Successfully generated ${format.toUpperCase()} export!`);
  }
}
