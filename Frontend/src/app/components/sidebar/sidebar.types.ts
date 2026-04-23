export interface NavItem {
  icon: string;
  label: string;
  route?: string;
  badge?: number;
  children?: { label: string; route: string; roles?: string[]; moduleId?: number }[];
  expanded?: boolean;
  roles?: string[];
  moduleId?: number;
}
