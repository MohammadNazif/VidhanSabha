import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { PageHeaderComponent } from '../../shared/page-header/page-header.component';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';
import { TableAction, TableColumn, TableConfig } from '../../shared/generic-table/generic-table.types';
import { AccessService } from '../../../Services/Admin/access/access.service';
import { ToastService } from '../../../Services/common/toast/toast.service';
import { CrudHandlerService } from '../../../Services/common/crud-handler.service';

@Component({
  selector: 'app-allow-access-list',
  standalone: true,
  imports: [CommonModule, RouterModule, PageHeaderComponent, GenericTableComponent],
  template: `
    <div class="h-full flex flex-col p-4 gap-4 overflow-hidden">
      <app-page-header title="Access Management List" subtitle="Overview of module permissions granted to different roles">
        <button 
          class="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors shadow-sm font-medium flex items-center gap-2"
          [routerLink]="['/allow-access']">
          <span>➕</span> New Permission
        </button>
      </app-page-header>

      <div class="flex-1 min-h-0 bg-white rounded-xl shadow-sm border border-slate-200 overflow-hidden flex flex-col p-2">
        <app-generic-table
          [config]="tableConfig"
          [columns]="columns"
          [actions]="actions"
          [data]="accessList"
          [loading]="loading"
          (actionClick)="handleAction($event)">
        </app-generic-table>
      </div>
    </div>
  `,
  styles: [`
    :host { display: block; height: 100%; }
  `]
})
export class AllowAccessListComponent implements OnInit {
  accessList: any[] = [];
  loading = false;

  columns: TableColumn[] = [
    { key: 'designationName', label: 'Designation', sortable: true },
    { 
      key: 'permissionCount', 
      label: 'Active Permissions', 
      type: 'badge',
      badgeVariant: (val: number) => val > 0 ? 'success' : 'default'
    },
    { key: 'updatedAt', label: 'Last Updated', type: 'date' }
  ];

  actions: TableAction[] = [
    { id: 'edit', label: '', icon: 'edit', variant: 'primary' },
    { id: 'delete', label: '', icon: 'delete', variant: 'danger' }
  ];

  tableConfig: TableConfig = {
    selectable: false,
    paginated: true,
    searchable: true,
    defaultPageSize: 10
  };

  constructor(
    private accessService: AccessService,
    private router: Router,
    private toastService: ToastService,
    private crudHandler: CrudHandlerService
  ) {}

  ngOnInit() {
    this.loadAccessList();
  }

  loadAccessList() {
    this.loading = true;
    this.accessService.getAllAccess().subscribe({
      next: (res: any) => {
        // Mapping mock data or API response
        const data = res.data?.items || res.data || [];
        this.accessList = data.map((item: any) => ({
          ...item,
          permissionCount: item.permissions?.length || 0
        }));
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.toastService.showError('Error', 'Failed to load access list');
      }
    });
  }

  handleAction(event: { action: TableAction; row: any }) {
    const { action, row } = event;
    if (action.id === 'edit') {
      this.router.navigate(['/allow-access'], { queryParams: { id: row.designationId || row.id } });
    } else if (action.id === 'delete') {
      this.crudHandler.handleRequest(
        this.accessService.deleteAccess(row.id),
        'Deleted',
        'Access permissions removed!',
        () => this.loadAccessList()
      );
    }
  }
}
