import {
  Component,
  Input,
  Output,
  EventEmitter,
  OnInit,
  OnChanges,
  SimpleChanges,
  TemplateRef,
  ContentChild,
  TrackByFunction,
  inject
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import {
  TableColumn,
  TableAction,
  TableConfig,
  SortState,
  PageState,
  SortDirection,
  BadgeVariant
} from './generic-table.types';

/** Built-in Lucide-style SVG icons — pass the key as `action.icon` */
const ICON_MAP: Record<string, string> = {
  edit: `<svg xmlns="http://www.w3.org/2000/svg" width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.2" stroke-linecap="round" stroke-linejoin="round"><path d="M12 20h9"/><path d="M16.5 3.5a2.121 2.121 0 0 1 3 3L7 19l-4 1 1-4L16.5 3.5z"/></svg>`,
  delete: `<svg xmlns="http://www.w3.org/2000/svg" width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.2" stroke-linecap="round" stroke-linejoin="round"><polyline points="3 6 5 6 21 6"/><path d="M19 6l-1 14a2 2 0 0 1-2 2H8a2 2 0 0 1-2-2L5 6"/><path d="M10 11v6"/><path d="M14 11v6"/><path d="M9 6V4a1 1 0 0 1 1-1h4a1 1 0 0 1 1 1v2"/></svg>`,
  add: `<svg xmlns="http://www.w3.org/2000/svg" width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round"><line x1="12" y1="5" x2="12" y2="19"/><line x1="5" y1="12" x2="19" y2="12"/></svg>`,
  view: `<svg xmlns="http://www.w3.org/2000/svg" width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"/><circle cx="12" cy="12" r="3"/></svg>`,
  download: `<svg xmlns="http://www.w3.org/2000/svg" width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M21 15v4a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-4"/><polyline points="7 10 12 15 17 10"/><line x1="12" y1="15" x2="12" y2="3"/></svg>`,
  user: `<svg xmlns="http://www.w3.org/2000/svg" width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"/><circle cx="12" cy="7" r="4"/></svg>`,
  layout: `<svg xmlns="http://www.w3.org/2000/svg" width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><rect x="3" y="3" width="18" height="18" rx="2" ry="2"/><line x1="3" y1="9" x2="21" y2="9"/><line x1="9" y1="21" x2="9" y2="9"/></svg>`,
  map: `<svg xmlns="http://www.w3.org/2000/svg" width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M1 6v16l7-4 8 4 7-4V2l-7 4-8-4-7 4z"/><line x1="8" y1="2" x2="8" y2="18"/><line x1="16" y1="6" x2="16" y2="22"/></svg>`
};

/** Pre-sanitized SafeHtml icon cache — built once per app lifecycle, never re-created */
const _iconCache = new Map<string, SafeHtml>();

@Component({
  selector: 'app-generic-table',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './generic-table.component.html',
  styleUrl: './generic-table.component.css'
})
export class GenericTableComponent implements OnInit, OnChanges {
  private readonly sanitizer = inject(DomSanitizer);

  /** Returns the cached SafeHtml SVG for the given icon key. Created once per key. */
  getSafeIcon(icon: string | undefined): SafeHtml {
    const key = icon ?? '';
    if (!_iconCache.has(key)) {
      const svg = key ? (ICON_MAP[key] ?? key) : '';
      _iconCache.set(key, this.sanitizer.bypassSecurityTrustHtml(svg));
    }
    return _iconCache.get(key)!;
  }
  // ── Inputs ──
  @Input() data: any[] = [];
  @Input() columns: TableColumn[] = [];
  @Input() actions: TableAction[] = [];
  @Input() config: TableConfig = {};
  @Input() loading = false;
  @Input() totalItems = 0;

  // ── Custom Templates ──
  @ContentChild('cellTemplate') cellTemplate!: TemplateRef<any>;
  @ContentChild('headerTemplate') headerTemplate!: TemplateRef<any>;
  @ContentChild('expandedRowTemplate') expandedRowTemplate!: TemplateRef<any>;

  // ── Outputs ──
  @Output() rowClick = new EventEmitter<{ row: any; index: number }>();
  @Output() rowDoubleClick = new EventEmitter<{ row: any; index: number }>();
  @Output() actionClick = new EventEmitter<{ action: TableAction; row: any; index: number }>();
  @Output() selectionChange = new EventEmitter<any[]>();
  @Output() sortChange = new EventEmitter<SortState>();
  @Output() pageChange = new EventEmitter<PageState>();
  @Output() searchChange = new EventEmitter<string>();

  // ── Internal State ──
  processedData: any[] = [];
  displayedData: any[] = [];
  searchTerm = '';
  sortState: SortState = { column: '', direction: null };
  selectedRows: Set<number> = new Set();
  allSelected = false;

  pageState: PageState = {
    currentPage: 1,
    pageSize: 50,
    totalItems: 0,
    totalPages: 0
  };

  // Default config
  private defaultConfig: TableConfig = {
    selectable: false,
    paginated: true,
    pageSizeOptions: [5, 10, 25, 50],
    defaultPageSize: 50,
    searchable: true,
    searchPlaceholder: 'Search records...',
    hoverable: true,
    striped: false,
    showRowNumbers: false,
    emptyMessage: 'No records found',
    emptyIcon: '📭',
    loading: false,
    compact: false,
    serverSide: false,
    exportable: false
  };

  get mergedConfig(): TableConfig {
    return { ...this.defaultConfig, ...this.config };
  }

  get visibleColumns(): TableColumn[] {
    return this.columns.filter(c => c.visible !== false);
  }

  get totalColumnCount(): number {
    let count = this.visibleColumns.length;
    if (this.mergedConfig.selectable) count++;
    if (this.mergedConfig.showRowNumbers) count++;
    if (this.hasVisibleActions) count++;
    return count;
  }

  get hasVisibleActions(): boolean {
    if (!this.actions || this.actions.length === 0) return false;
    if (this.displayedData.length === 0) {
      try {
        return this.actions.some(action => !action.show || action.show(null));
      } catch {
        return true;
      }
    }
    return this.actions.some(action => 
      this.displayedData.some(row => this.isActionVisible(action, row))
    );
  }

  trackByIndex: TrackByFunction<any> = (index: number) => index;

  ngOnInit() {
    this.pageState.pageSize = this.mergedConfig.defaultPageSize || 10;
    this.processData();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['data'] || changes['columns']) {
      this.processData();
    }
    if (changes['loading']) {
      this.loading = changes['loading'].currentValue;
    }
  }

  // ── Data Processing ──
  processData() {
    if (this.mergedConfig.serverSide) {
      this.processedData = [...this.data];
      this.pageState.totalItems = this.totalItems || this.data.length;
      this.pageState.totalPages = Math.ceil(this.pageState.totalItems / this.pageState.pageSize) || 1;
      this.displayedData = this.processedData;
      return;
    }

    let result = [...this.data];

    // Search / Filter
    if (this.searchTerm.trim()) {
      const term = this.searchTerm.toLowerCase();
      const searchCols = this.mergedConfig.searchableColumns || this.columns.map(c => c.key);
      result = result.filter(row =>
        searchCols.some(key => {
          const val = this.getNestedValue(row, key);
          return val != null && String(val).toLowerCase().includes(term);
        })
      );
    }

    // Sort
    if (this.sortState.column && this.sortState.direction) {
      const col = this.sortState.column;
      const dir = this.sortState.direction === 'asc' ? 1 : -1;
      result.sort((a, b) => {
        const aVal = this.getNestedValue(a, col);
        const bVal = this.getNestedValue(b, col);
        if (aVal == null && bVal == null) return 0;
        if (aVal == null) return 1;
        if (bVal == null) return -1;
        if (typeof aVal === 'number' && typeof bVal === 'number') {
          return (aVal - bVal) * dir;
        }
        return String(aVal).localeCompare(String(bVal)) * dir;
      });
    }

    this.processedData = result;
    this.pageState.totalItems = result.length;
    this.pageState.totalPages = Math.ceil(result.length / this.pageState.pageSize) || 1;

    // Reset to page 1 if current page exceeds total
    if (this.pageState.currentPage > this.pageState.totalPages) {
      this.pageState.currentPage = 1;
    }

    this.updateDisplayedData();
  }

  updateDisplayedData() {
    if (this.mergedConfig.serverSide) {
      this.displayedData = [...this.data];
      return;
    }

    if (this.mergedConfig.paginated) {
      const start = (this.pageState.currentPage - 1) * this.pageState.pageSize;
      const end = start + this.pageState.pageSize;
      this.displayedData = this.processedData.slice(start, end);
    } else {
      this.displayedData = this.processedData;
    }
  }

  // ── Sorting ──
  onSort(column: TableColumn) {
    if (!column.sortable) return;

    if (this.sortState.column === column.key) {
      // Toggle: asc -> desc -> null
      if (this.sortState.direction === 'asc') {
        this.sortState.direction = 'desc';
      } else if (this.sortState.direction === 'desc') {
        this.sortState = { column: '', direction: null };
      }
    } else {
      this.sortState = { column: column.key, direction: 'asc' };
    }

    this.sortChange.emit(this.sortState);
    
    if (!this.mergedConfig.serverSide) {
      this.processData();
    }
  }

  getSortIcon(column: TableColumn): string {
    if (this.sortState.column !== column.key || !this.sortState.direction) return '⇅';
    return this.sortState.direction === 'asc' ? '↑' : '↓';
  }

  // ── Search ──
  onSearch() {
    this.pageState.currentPage = 1;
    this.searchChange.emit(this.searchTerm);
    this.processData();
  }

  clearSearch() {
    this.searchTerm = '';
    this.onSearch();
  }

  // ── Pagination ──
  goToPage(page: number) {
    if (page < 1 || page > this.pageState.totalPages) return;
    this.pageState.currentPage = page;
    this.pageChange.emit(this.pageState);
    
    if (!this.mergedConfig.serverSide) {
      this.updateDisplayedData();
    }
  }

  onPageSizeChange(size: number) {
    this.pageState.pageSize = size;
    this.pageState.currentPage = 1;
    this.pageChange.emit(this.pageState);
    
    if (!this.mergedConfig.serverSide) {
      this.processData();
    }
  }

  get pageNumbers(): number[] {
    const total = this.pageState.totalPages;
    const current = this.pageState.currentPage;
    const pages: number[] = [];

    if (total <= 5) {
      for (let i = 1; i <= total; i++) pages.push(i);
    } else {
      pages.push(1);
      if (current > 3) pages.push(-1); // ellipsis
      const start = Math.max(2, current - 1);
      const end = Math.min(total - 1, current + 1);
      for (let i = start; i <= end; i++) pages.push(i);
      if (current < total - 2) pages.push(-1); // ellipsis
      pages.push(total);
    }
    return pages;
  }

  get showingFrom(): number {
    return (this.pageState.currentPage - 1) * this.pageState.pageSize + 1;
  }

  get showingTo(): number {
    return Math.min(
      this.pageState.currentPage * this.pageState.pageSize,
      this.pageState.totalItems
    );
  }

  // ── Selection ──
  toggleSelectAll() {
    if (this.allSelected) {
      this.selectedRows.clear();
      this.allSelected = false;
    } else {
      this.displayedData.forEach((_, i) => {
        const globalIndex = (this.pageState.currentPage - 1) * this.pageState.pageSize + i;
        this.selectedRows.add(globalIndex);
      });
      this.allSelected = true;
    }
    this.emitSelection();
  }

  toggleRowSelect(index: number) {
    const globalIndex = (this.pageState.currentPage - 1) * this.pageState.pageSize + index;
    if (this.selectedRows.has(globalIndex)) {
      this.selectedRows.delete(globalIndex);
    } else {
      this.selectedRows.add(globalIndex);
    }
    this.allSelected = this.displayedData.every((_, i) => {
      const gi = (this.pageState.currentPage - 1) * this.pageState.pageSize + i;
      return this.selectedRows.has(gi);
    });
    this.emitSelection();
  }

  isRowSelected(index: number): boolean {
    const globalIndex = (this.pageState.currentPage - 1) * this.pageState.pageSize + index;
    return this.selectedRows.has(globalIndex);
  }

  private emitSelection() {
    const selected = Array.from(this.selectedRows).map(i => this.processedData[i]).filter(Boolean);
    this.selectionChange.emit(selected);
  }

  // ── Row Events ──
  onRowClick(row: any, index: number) {
    this.rowClick.emit({ row, index });
  }

  onRowDoubleClick(row: any, index: number) {
    this.rowDoubleClick.emit({ row, index });
  }

  onActionClick(action: TableAction, row: any, index: number, event: Event) {
    event.stopPropagation();
    if (action.disabled && action.disabled(row)) return;
    this.actionClick.emit({ action, row, index });
  }

  // ── Helpers ──
  getNestedValue(obj: any, path: string): any {
    return path.split('.').reduce((acc, part) => acc && acc[part], obj);
  }

  getCellValue(row: any, column: TableColumn): any {
    const value = this.getNestedValue(row, column.key);
    if (column.formatter) {
      return column.formatter(value, row);
    }
    return value;
  }

  getBadgeVariant(value: any, row: any, column: TableColumn): BadgeVariant {
    if (column.badgeVariant) {
      return column.badgeVariant(value, row);
    }
    return 'default';
  }

  getProgressColor(value: number, row: any, column: TableColumn): string {
    if (column.progressColor) {
      return column.progressColor(value, row);
    }
    if (value >= 80) return 'linear-gradient(90deg, #10b981, #059669)';
    if (value >= 50) return 'linear-gradient(90deg, #f59e0b, #d97706)';
    return 'linear-gradient(90deg, #ef4444, #dc2626)';
  }

  getProgressPercent(value: number, column: TableColumn): number {
    const max = column.progressMax || 100;
    return Math.min((value / max) * 100, 100);
  }

  getInitials(name: string): string {
    if (!name) return '?';
    return name
      .split(' ')
      .map(n => n[0])
      .join('')
      .slice(0, 2);
  }

  getRowNumber(index: number): number {
    return (this.pageState.currentPage - 1) * this.pageState.pageSize + index + 1;
  }

  isActionVisible(action: TableAction, row: any): boolean {
    return action.show ? action.show(row) : true;
  }

  isActionDisabled(action: TableAction, row: any): boolean {
    return action.disabled ? action.disabled(row) : false;
  }
}
