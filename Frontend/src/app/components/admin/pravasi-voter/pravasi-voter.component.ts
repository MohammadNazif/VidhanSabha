import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Validators } from '@angular/forms';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';
import { TableAction, TableColumn, TableConfig } from '../../shared/generic-table/generic-table.types';
import { GenericModalButtonComponent } from '../../shared/generic-modal-form/generic-modal-button.component';
import { FormConfig, FormResult } from '../../shared/generic-modal-form/generic-form.types';
import { PageHeaderComponent } from '../../shared/page-header/page-header.component';
import { PravasivoterService } from '../../../Services/Admin/pravasivoter/pravasivoter.service';
import { ToastService } from '../../../Services/common/toast/toast.service';
import { CrudHandlerService } from '../../../Services/common/crud-handler.service';

@Component({
  selector: 'app-pravasi-voter',
  standalone: true,
  imports: [CommonModule, PageHeaderComponent, GenericTableComponent, GenericModalButtonComponent],
  templateUrl: './pravasi-voter.component.html',
  styleUrl: './pravasi-voter.component.css'
})
export class PravasiVoterComponent implements OnInit {
  @ViewChild('voterModal') voterModal!: GenericModalButtonComponent;

  voterList: any[] = [];

  columns: TableColumn[] = [
    { key: 'boothNumber', label: 'Booth No.', sortable: true },
    { key: 'villageName', label: 'Village', sortable: true },
    { key: 'name', label: 'Name', sortable: true },
    { key: 'mobileNo', label: 'Mobile No.', sortable: true },
    { key: 'categoryName', label: 'Category', sortable: true },
    { key: 'castName', label: 'Caste', sortable: true },
    { key: 'occupationName', label: 'Occupation', sortable: true },
    { key: 'voterId', label: 'Voter ID', sortable: true }
  ];

  config: TableConfig = {
    searchable: true,
    paginated: true,
    showRowNumbers: true,
    striped: true,
    hoverable: true
  };

  actions: TableAction[] = [
    { id: 'edit', label: '', variant: 'default', icon: 'edit' },
    { id: 'delete', label: '', variant: 'danger', icon: 'delete' }
  ];

  addVoterConfig: FormConfig = {
    title: 'Add Pravasi Voter',
    submitLabel: 'Save',
    fields: [
      { id: 'id', name: 'id', label: 'ID', type: 'hidden' },
      {
        id: 'boothId',
        name: 'boothId',
        label: 'Booth No',
        type: 'select',
        placeholder: '-- Select Booth No --',
        apiUrl: 'booth/getall',
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : []);
          return list.map((item: any) => ({
            value: String(item.id),
            label: `${item.boothNumber} - ${item.pollingStationName || ''}`
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
        placeholder: '-- Select Village --',
        dependsOn: 'boothId',
        apiUrl: (boothId: string) => `common/villagesByBoothId?boothId=${boothId}`,
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
        id: 'name',
        name: 'name',
        label: 'Name',
        type: 'text',
        placeholder: 'Enter Name',
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'mobileNo',
        name: 'mobileNo',
        label: 'Mobile No',
        type: 'text',
        placeholder: 'Enter Mobile No',
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
        apiUrl: (categoryId: string) => `common/cast?id=${categoryId}`,
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
        id: 'occupationId',
        name: 'occupationId',
        label: 'Occupation',
        type: 'select',
        placeholder: '-- Select Occupation --',
        apiUrl: 'common/occupation',
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
        id: 'voterId',
        name: 'voterId',
        label: 'Voter ID',
        type: 'text',
        placeholder: 'Enter Voter ID',
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'currentAddress',
        name: 'currentAddress',
        label: 'Current Address',
        type: 'textarea',
        placeholder: 'Enter Current Address',
        validations: [Validators.required],
        gridColSpan: 12
      }
    ]
  };

  constructor(
    private voterService: PravasivoterService,
    private toastService: ToastService,
    private crudHandler: CrudHandlerService
  ) { }

  ngOnInit() {
    this.loadVoters();
  }

  loadVoters() {
    this.voterService.getAllPravasivoters().subscribe({
      next: (response) => {
        this.voterList = response.data || (Array.isArray(response) ? response : []);
      },
      error: (err) => {
        console.error('Error fetching pravasi voters:', err);
      }
    });
  }

  handleAction(event: any) {
    const { action, row } = event;
    if (action.id === 'delete') {
      this.crudHandler.handleRequest(
        this.voterService.deletePravasivoter(row.id),
        'Deleted',
        'Pravasi Voter deleted successfully!',
        () => this.loadVoters()
      );
    } else if (action.id === 'edit') {
      const editData = { ...row };
      ['id', 'boothId', 'villageId', 'categoryId', 'castId', 'occupationId'].forEach(key => {
        if (editData[key]) editData[key] = String(editData[key]);
      });
      this.voterModal.openModal(editData);
    }
  }

  handleFormSubmit(result: FormResult) {
    if (!result.status) return;

    const raw = result.data;
    const submitData: any = {
      ...raw,
      id: raw.id ? Number(raw.id) : null,
      boothId: Number(raw.boothId),
      villageId: Number(raw.villageId),
      categoryId: Number(raw.categoryId),
      castId: Number(raw.castId),
      occupationId: Number(raw.occupationId)
    };

    const isUpdate = !!submitData.id;
    const request = isUpdate
      ? this.voterService.updatePravasivoter(submitData)
      : this.voterService.createPravasivoter(submitData);

    this.crudHandler.handleRequest(
      request,
      isUpdate ? 'Updated' : 'Success',
      `Pravasi Voter ${isUpdate ? 'updated' : 'created'} successfully!`,
      () => this.loadVoters()
    );
  }
}
