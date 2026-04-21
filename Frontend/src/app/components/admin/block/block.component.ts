import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Validators } from '@angular/forms';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';
import { TableAction, TableColumn, TableConfig } from '../../shared/generic-table/generic-table.types';
import { GenericModalButtonComponent } from '../../shared/generic-modal-form/generic-modal-button.component';
import { FormConfig, FormResult } from '../../shared/generic-modal-form/generic-form.types';
import { PageHeaderComponent } from '../../shared/page-header/page-header.component';
import { BlockService } from '../../../Services/Admin/block/block.service';
import { ToastService } from '../../../Services/common/toast/toast.service';
import { CrudHandlerService } from '../../../Services/common/crud-handler.service';

@Component({
  selector: 'app-block',
  standalone: true,
  imports: [CommonModule, PageHeaderComponent, GenericTableComponent, GenericModalButtonComponent],
  templateUrl: './block.component.html',
  styleUrl: './block.component.css'
})
export class BlockComponent implements OnInit {
  @ViewChild('blockModal') blockModal!: GenericModalButtonComponent;

  blockList: any[] = [];

  columns: TableColumn[] = [
    { key: 'blockName', label: 'Block Name', sortable: true },
    { key: 'blockPramukh', label: 'Block Pramukh', sortable: true },
    { key: 'mobile', label: 'Mobile No.', sortable: true },
    { key: 'partyName', label: 'Party', sortable: true },
    { key: 'categoryName', label: 'Category', sortable: true },
    { key: 'castName', label: 'Caste', sortable: true },
    { key: 'occupation', label: 'Occupation', sortable: true }
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

  formConfig: FormConfig = {
    title: 'Register Block / Block Pramukh',
    submitLabel: 'Save Block',
    fields: [
      {
        id: 'blockName',
        name: 'blockName',
        label: 'Block Name',
        type: 'text',
        placeholder: 'Enter block name',
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'blockPramukh',
        name: 'blockPramukh',
        label: 'Block Pramukh Name',
        type: 'text',
        placeholder: 'Enter pramukh name',
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'mobile',
        name: 'mobile',
        label: 'Mobile No',
        type: 'text',
        placeholder: 'Enter mobile no',
        validations: [Validators.required, Validators.pattern('^[0-9]{10}$')],
        gridColSpan: 6
      },
      {
        id: 'partyId',
        name: 'partyId',
        label: 'Party',
        type: 'select',
        placeholder: '-- Select Party --',
        apiUrl: 'common/getparty',
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : []);
          return list.map((item: any) => ({
            value: String(item.id),
            label: item.partyName || item.party
          }));
        },
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
            label: item.name || item.category
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
            label: item.name || item.castName
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
        apiUrl: 'common/getoccupation',
        apiMapper: (data: any) => {
          const list = Array.isArray(data?.data) ? data.data : (Array.isArray(data) ? data : []);
          return list.map((item: any) => ({
            value: String(item.id),
            label: item.occupationName || item.occupation
          }));
        },
        validations: [Validators.required],
        gridColSpan: 12
      },
      {
        id: 'address',
        name: 'address',
        label: 'Address',
        type: 'textarea',
        placeholder: 'Enter address...',
        gridColSpan: 12
      }
    ]
  };

  constructor(
    private blockService: BlockService,
    private toastService: ToastService,
    private crudHandler: CrudHandlerService
  ) { }

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    this.blockService.getAllBlocks().subscribe({
      next: (response) => {
        this.blockList = response.data || (Array.isArray(response) ? response : []);
      },
      error: (err) => console.error('Error fetching block list:', err)
    });
  }

  handleAction(event: any) {
    const { action, row } = event;
    if (action.id === 'delete') {
      this.crudHandler.handleRequest(
        this.blockService.deleteBlock(row.id),
        'Deleted',
        'Block deleted successfully!',
        () => this.loadData()
      );
    } else if (action.id === 'edit') {
      const editData = { ...row };
      ['id', 'partyId', 'categoryId', 'castId', 'occupationId'].forEach(key => {
        if (editData[key]) editData[key] = String(editData[key]);
      });
      this.blockModal.openModal(editData);
    }
  }

  handleFormSubmit(result: FormResult) {
    if (!result.status) return;

    const raw = result.data;
    const rowId = raw.id || (this.blockModal.initialData && this.blockModal.initialData.id);

    const submitData: any = {
      id: rowId ? Number(rowId) : null,
      blockName: raw.blockName,
      blockPramukh: raw.blockPramukh,
      partyId: Number(raw.partyId),
      mobile: raw.mobile,
      address: raw.address,
      categoryId: Number(raw.categoryId),
      castId: Number(raw.castId),
      occupationId: Number(raw.occupationId)
    };

    const isUpdate = !!submitData.id;
    const request = isUpdate
      ? this.blockService.updateBlock(submitData)
      : this.blockService.createBlock(submitData);

    this.crudHandler.handleRequest(
      request,
      isUpdate ? 'Updated' : 'Success',
      `Block ${isUpdate ? 'updated' : 'created'} successfully!`,
      () => this.loadData()
    );
  }
}
