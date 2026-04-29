/**
 * Generic Table Component - Interfaces & Types
 * Reusable across the entire project
 */

export type ColumnType = 'text' | 'number' | 'date' | 'badge' | 'avatar' | 'progress' | 'actions' | 'custom';
export type SortDirection = 'asc' | 'desc' | null;
export type BadgeVariant = 'success' | 'warning' | 'danger' | 'info' | 'default';

export interface TableFilterOption {
  label: string;
  value: any;
}

export interface TableFilter {
  key: string;
  label: string;
  type: 'select' | 'text';
  options?: TableFilterOption[]; // For select types
  placeholder?: string;
  value?: any; // Current selected value
  multiple?: boolean; // Enable multi-select for select type
}

export interface TableColumn {
  /** Unique key matching the data property */
  key: string;
  /** Display label in the header */
  label: string;
  /** Column type for built-in rendering */
  type?: ColumnType;
  /** Allow sorting on this column */
  sortable?: boolean;
  /** Custom width (e.g., '200px', '20%') */
  width?: string;
  /** Minimum width */
  minWidth?: string;
  /** Text alignment */
  align?: 'left' | 'center' | 'right';
  /** Whether the column is visible */
  visible?: boolean;
  /** Badge variant mapping function (for badge type) */
  badgeVariant?: (value: any, row: any) => BadgeVariant;
  /** Format function for display value */
  formatter?: (value: any, row: any) => string;
  /** Avatar fallback field (for avatar type, uses initials) */
  avatarFallbackKey?: string;
  /** Progress max value (for progress type, default 100) */
  progressMax?: number;
  /** Progress color function */
  progressColor?: (value: number, row: any) => string;
  /** Whether column is sticky */
  sticky?: boolean;
  /** Custom CSS class for the column */
  cssClass?: string;
}

export interface TableAction {
  /** Unique identifier */
  id: string;
  /** Display label */
  label: string;
  /** Emoji or icon text */
  icon?: string;
  /** Color variant */
  variant?: 'primary' | 'danger' | 'warning' | 'default';
  /** Condition to show the action */
  show?: (row: any) => boolean;
  /** Whether action is disabled */
  disabled?: (row: any) => boolean;
}

export interface TableConfig {
  /** Enable row selection (checkbox) */
  selectable?: boolean;
  /** Enable pagination */
  paginated?: boolean;
  /** Items per page options */
  pageSizeOptions?: number[];
  /** Default page size */
  defaultPageSize?: number;
  /** Enable search/filter */
  searchable?: boolean;
  /** Search placeholder text */
  searchPlaceholder?: string;
  /** Searchable column keys (defaults to all) */
  searchableColumns?: string[];
  /** Enable row hover effect */
  hoverable?: boolean;
  /** Enable striped rows */
  striped?: boolean;
  /** Show row numbers */
  showRowNumbers?: boolean;
  /** Empty state message */
  emptyMessage?: string;
  /** Empty state icon */
  emptyIcon?: string;
  /** Loading state */
  loading?: boolean;
  /** Table height for scrollable body (e.g., '400px') */
  maxHeight?: string;
  /** Compact mode */
  compact?: boolean;
  /** Enable advanced filtering */
  filterable?: boolean;
  /** Custom generic filters to render */
  filters?: TableFilter[];
  /** Enable server-side pagination, sorting, and filtering */
  serverSide?: boolean;
  /** Debounce time for search input in milliseconds (default: 400) */
  searchDebounceTime?: number;
  /** Enable export (future) */
  exportable?: boolean;
}

export interface SortState {
  column: string;
  direction: SortDirection;
}

export interface PageState {
  currentPage: number;
  pageSize: number;
  totalItems: number;
  totalPages: number;
}
