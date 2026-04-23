import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PageHeaderComponent } from '../../shared/page-header/page-header.component';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';
import { TableColumn, TableConfig } from '../../shared/generic-table/generic-table.types';
import { DesignationService } from '../../../Services/Admin/designation/designation.service';
import { SectorService } from '../../../Services/Admin/sector/sector.service';
import { BoothService } from '../../../Services/Admin/booth/booth.service';
import { AccessService } from '../../../Services/Admin/access/access.service';
import { ToastService } from '../../../Services/common/toast/toast.service';
import { ModulePermission } from '../../../models/module-permission.enum';
import { CrudHandlerService } from '../../../Services/common/crud-handler.service';

export interface SelectionField {
  id: string;
  name: string;
  label: string;
  placeholder: string;
  type: 'designation' | 'sector' | 'booth';
  options: any[];
  value: string;
  visible: boolean;
  valueKey: string;
  labelKey: string | ((item: any) => string);
}

@Component({
  selector: 'app-allow-access',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    PageHeaderComponent,
    GenericTableComponent
  ],
  template: `
    <div class="h-full flex flex-col p-4 gap-4 overflow-hidden">
      <app-page-header title="Allow Access" subtitle="Manage module-level permissions for different roles">
        <button 
          class="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors shadow-sm font-medium flex items-center gap-2" 
          (click)="savePermissions()">
          <span>💾</span> Save Permissions
        </button>
      </app-page-header>

      <div class="flex-1 min-h-0 bg-white rounded-xl shadow-sm border border-slate-200 overflow-hidden flex flex-col p-6 gap-6">
        <!-- Dynamic Selection Fields -->
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4 max-w-4xl">
          <ng-container *ngFor="let field of selectionFields">
            <div *ngIf="field.visible" class="flex flex-col gap-2">
              <label [for]="field.id" class="text-sm font-semibold text-slate-700">{{ field.label }}</label>
              <select 
                [id]="field.id"
                [(ngModel)]="field.value" 
                (change)="onFieldChange(field)"
                class="w-full bg-slate-50 border border-slate-200 rounded-lg px-3 py-2 text-sm text-slate-900 font-medium focus:outline-none focus:border-blue-500 focus:ring-1 focus:ring-blue-500 transition-all">
                <option value="">-- {{ field.placeholder }} --</option>
                <option *ngFor="let opt of field.options" [value]="opt[field.valueKey]">
                  {{ getOptionLabel(field, opt) }}
                </option>
              </select>
            </div>
          </ng-container>
        </div>

        <!-- Permissions Table -->
        <div class="flex-1 min-h-0 border border-slate-100 rounded-lg overflow-hidden">
          <app-generic-table
            [config]="config"
            [columns]="columns"
            [data]="permissionData"
            [loading]="loading">
            
            <ng-template #cellTemplate let-value let-row="row" let-column="column">
              <ng-container *ngIf="column.key === 'hasPermission'">
                <div class="flex items-center justify-center">
                  <label class="relative inline-flex items-center cursor-pointer">
                    <input type="checkbox" [(ngModel)]="row.hasPermission" class="sr-only peer">
                    <div class="w-9 h-5 bg-slate-200 peer-focus:outline-none rounded-full peer peer-checked:after:translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:left-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-4 after:w-4 after:transition-all peer-checked:bg-blue-600"></div>
                  </label>
                </div>
              </ng-container>
              <ng-container *ngIf="column.key !== 'hasPermission'">
                <span class="font-medium text-slate-700">{{ value }}</span>
              </ng-container>
            </ng-template>
          </app-generic-table>
        </div>
      </div>
    </div>
  `,
  styles: [`
    :host { display: block; height: 100%; }
  `]
})
export class AllowAccessComponent implements OnInit {
  selectionFields: SelectionField[] = [
    {
      id: 'type',
      name: 'Id',
      label: 'Select Category',
      placeholder: 'Choose Type',
      type: 'designation', // Category/Type selector
      options: [
        { id: 'sector', name: 'Sector' },
        { id: 'booth', name: 'Booth' }

      ],
      value: '',
      visible: true,
      valueKey: 'id',
      labelKey: (opt: any) => opt.name
    },
    {
      id: 'entity',
      name: 'entityId',
      label: 'Select Record',
      placeholder: 'Choose Record',
      type: 'sector',
      options: [],
      value: '',
      visible: false,
      valueKey: 'id',
      labelKey: (opt: any) => opt.designationName || opt.sectorName || opt.boothNumber || opt.name
    }
  ];

  loading = false;
  permissionData: any[] = [
    { id: ModulePermission.PannaPramukh, moduleName: 'Create Panna Pramukh', hasPermission: false },
    { id: ModulePermission.NewVoter, moduleName: 'Create New Voters', hasPermission: false },
    { id: ModulePermission.BoothVoterDescrition, moduleName: 'Create Booth Voter Description', hasPermission: false },
    { id: ModulePermission.DoubleVoter, moduleName: 'Create Double Voters', hasPermission: false },
    { id: ModulePermission.PravashiVoter, moduleName: 'Create Pravasi Voters', hasPermission: false },
    { id: ModulePermission.BoothSamiti, moduleName: 'Create BoothSamiti', hasPermission: false },
    { id: ModulePermission.EffectivePersion, moduleName: 'Create EffectivePerson', hasPermission: false },
    { id: ModulePermission.Activity, moduleName: 'Create Activity', hasPermission: false },
    { id: ModulePermission.SeniororDisabled, moduleName: 'Create Senior Or Disabled', hasPermission: false }
  ];

  columns: TableColumn[] = [
    { key: 'moduleName', label: 'Allow Permission', sortable: false },
    { key: 'hasPermission', label: 'Allow', sortable: false, type: 'custom' }
  ];

  config: TableConfig = {
    selectable: false,
    paginated: false,
    searchable: false,
    showRowNumbers: true,
    hoverable: true,
    striped: true
  };

  constructor(
    private designationService: DesignationService,
    private sectorService: SectorService,
    private boothService: BoothService,
    private accessService: AccessService,
    private toastService: ToastService,
    private crudHandler: CrudHandlerService
  ) { }

  ngOnInit() {
    // Selection is manual starting with Category
  }

  getOptionLabel(field: SelectionField, opt: any): string {
    if (typeof field.labelKey === 'function') {
      return field.labelKey(opt);
    }
    return opt[field.labelKey];
  }

  onFieldChange(field: SelectionField) {
    if (field.id === 'type') {
      this.handleTypeChange();
    } else {
      this.handleEntityChange();
    }
  }

  handleTypeChange() {
    const typeField = this.selectionFields.find(f => f.id === 'type');
    const entityField = this.selectionFields.find(f => f.id === 'entity');

    if (!typeField || !entityField) return;

    // Reset entity field
    entityField.visible = false;
    entityField.value = '';
    entityField.options = [];
    this.resetPermissions();

    if (!typeField.value) return;

    entityField.visible = true;
    entityField.label = `Select ${typeField.options.find(o => o.id === typeField.value)?.name || 'Entity'}`;
    entityField.placeholder = `-- Choose ${entityField.label.split(' ')[1]} --`;

    this.loading = true;
    if (typeField.value === 'designation') {
      this.designationService.getAllDesignations().subscribe({
        next: (res) => {
          const list = res.isSuccess ? res.data : (Array.isArray(res) ? res : (res.data || []));
          entityField.options = list.map((o: any) => ({
            id: o.id,
            name: o.designationName || o.name
          }));
          this.loading = false;
        },
        error: () => this.loading = false
      });
    } else if (typeField.value === 'sector') {
      this.sectorService.getAllSectors({ pageSize: 1000 }).subscribe({
        next: (res) => {
          const list = res.data?.items || res.data || [];
          entityField.options = list.map((o: any) => ({
            id: o.id,
            name: o.sectorName || o.name
          }));
          this.loading = false;
        },
        error: () => this.loading = false
      });
    } else if (typeField.value === 'booth') {
      this.boothService.getBoothIncharge().subscribe({
        next: (res) => {
          this.loading = false;
          const list = res.data || [];
          entityField.options = list.map((o: any) => ({
            id: o.userId || String(o.boothId),
            name: o.boothInchargeName
          }));
        },
        error: () => this.loading = false
      });
    }
  }

  handleEntityChange() {
    const entityField = this.selectionFields.find(f => f.id === 'entity');
    if (entityField?.value) {
      this.fetchPermissions(entityField.value);
    } else {
      this.resetPermissions();
    }
  }

  fetchPermissions(id: string | number) {
    this.loading = true;
    this.accessService.getPermissionByUserId(id).subscribe({
      next: (response) => {
        this.loading = false;
        if (response && response.isSuccess && Array.isArray(response.data)) {
          this.mapPermissions(response.data);
        } else {
          this.resetPermissions();
        }
      },
      error: (err) => {
        this.loading = false;
        this.resetPermissions();
        console.error('Error fetching permissions:', err);
      }
    });
  }

  resetPermissions() {
    this.permissionData = this.permissionData.map(p => ({ ...p, hasPermission: false }));
  }

  mapPermissions(savedPermissions: any[]) {
    this.permissionData = this.permissionData.map(p => {
      const saved = savedPermissions.find(s => s.module === p.id);
      return {
        ...p,
        hasPermission: saved ? saved.hasPermission : false
      };
    });
  }

  savePermissions() {
    const typeField = this.selectionFields.find(f => f.id === 'type');
    const entityField = this.selectionFields.find(f => f.id === 'entity');

    if (!typeField?.value || !entityField?.value) {
      this.toastService.showWarning('Validation', 'Please select both category and record.');
      return;
    }

    const memberId = isNaN(Number(entityField.value)) ? entityField.value : Number(entityField.value);

    // Construct payload as an array of permission objects
    const payload = this.permissionData.map(p => ({
      memberId: memberId,
      module: Number(p.id),
      hasPermission: p.hasPermission
    }));

    this.crudHandler.handleRequest(
      this.accessService.createPermission(payload),
      'Success',
      'Permissions updated successfully!',
      () => { }
    );
  }
}
