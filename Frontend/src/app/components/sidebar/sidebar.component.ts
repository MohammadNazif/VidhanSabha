import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { LucideAngularModule } from 'lucide-angular';
import { AuthServiceService } from '../../Services/Auth/auth.service';
import { ModulePermission } from '../../models/module-permission.enum';
import { PermissionService } from '../../Services/common/permission.service';

interface NavItem {
  icon?: string;
  label: string;
  route?: string;
  badge?: number;
  expanded?: boolean;
  roles?: string[];
  children?: NavItem[];
  moduleId?: number;
}

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule, LucideAngularModule],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent implements OnInit {
  @Input() collapsed = false;
  @Output() collapsedChange = new EventEmitter<boolean>();

  @Input() isMobile = false;
  profileMenuOpen = false;

  navItems: NavItem[] = [
    // --- DASHBOARDS ---
    { icon: 'layout-dashboard', label: 'Dashboard', route: '/dashboard', roles: ['VidhanSabhaPrabhari', 'BoothSanyojak', 'SectorSanyojak'] },
    { icon: 'layout-dashboard', label: 'Dashboard', route: '/superadmin/dashboard', roles: ['SUPERADMIN'] },
    { icon: 'layout-dashboard', label: 'Dashboard', route: '/state-prabhari/dashboard', roles: ['StatePrabhari'] },

    // --- MASTER DATA ---
    {
      icon: 'database', label: 'Master Data',
      expanded: false,
      roles: ['SUPERADMIN', 'StatePrabhari', 'VidhanSabhaPrabhari', 'BoothSanyojak', 'SectorSanyojak', 'Adhyaksh'],
      children: [
        // State & District (Admin Levels)
        { label: 'State', route: '/superadmin/state', roles: ['SUPERADMIN', 'StatePrabhari'] },
        { label: 'District', route: '/superadmin/district', roles: ['Adhyaksh', 'StatePrabhari'] },
        { label: 'Vidhan Sabha', route: '/superadmin/vidhansabha', roles: ['Adhyaksh', 'StatePrabhari'] },
        { label: 'State Prabhari', route: '/superadmin/state-prabhari', roles: ['SUPERADMIN'] },

        // Members & Prabhari
        { label: 'Samiti Member', route: '/superadmin/state-member', roles: ['StatePrabhari'] },
        { label: 'Vidhan Sabha Prabhari', route: '/state-prabhari/vidhansabha-prabhari', roles: ['StatePrabhari'] },
        { label: 'Designation', route: '/superadmin/designation', roles: ['Adhyaksh', 'StatePrabhari'] },

        // Requested Order
        { label: 'Mandal', route: '/mandal', roles: ['VidhanSabhaPrabhari'] },
        { label: 'Sector', route: '/sector', roles: ['VidhanSabhaPrabhari'] },
        { label: 'Booth', route: '/booth', roles: ['VidhanSabhaPrabhari'] },
        { label: 'PannaPramukh', route: '/panna-pramukh', roles: ['VidhanSabhaPrabhari', 'BoothSanyojak', 'SectorSanyojak'], moduleId: ModulePermission.PannaPramukh },
        { label: 'Pravasi Voter', route: '/pravasi-voter', roles: ['VidhanSabhaPrabhari', 'BoothSanyojak', 'SectorSanyojak'], moduleId: ModulePermission.PravashiVoter },
        { label: 'New Voter', route: '/new-voter', roles: ['VidhanSabhaPrabhari', 'BoothSanyojak', 'SectorSanyojak'], moduleId: ModulePermission.NewVoter },
        { label: 'Varisth/Viklaang', route: '/varisth-naagarik-viklaang', roles: ['VidhanSabhaPrabhari', 'BoothSanyojak', 'SectorSanyojak'], moduleId: ModulePermission.SeniororDisabled },
        { label: 'Booth Voter Description', route: '/booth-voter-description', roles: ['VidhanSabhaPrabhari', 'BoothSanyojak', 'SectorSanyojak'], moduleId: ModulePermission.BoothVoterDescrition },
        { label: 'Sahmat/Asahmat', route: '/sahmat-asahmat', roles: ['VidhanSabhaPrabhari', 'BoothSanyojak', 'SectorSanyojak'], moduleId: ModulePermission.Sahmat },
        { label: 'Double Voter/Married', route: '/double-voter', roles: ['VidhanSabhaPrabhari', 'BoothSanyojak', 'SectorSanyojak'], moduleId: ModulePermission.DoubleVoter },
        { label: 'Booth Samiti', route: '/booth-samiti', roles: ['VidhanSabhaPrabhari', 'BoothSanyojak', 'SectorSanyojak'], moduleId: ModulePermission.BoothSamiti },
        { label: 'Mandal Samiti', route: '/mandal-samiti', roles: ['VidhanSabhaPrabhari'] },
        { label: 'Prabhavshali Vyakti', route: '/prabhavshali-vyakt', roles: ['VidhanSabhaPrabhari', 'BoothSanyojak', 'SectorSanyojak'], moduleId: ModulePermission.EffectivePersion },
        { label: 'Influencer Person', route: '/influencer-person', roles: ['VidhanSabhaPrabhari'] },
        { label: 'Block', route: '/block', roles: ['VidhanSabhaPrabhari'] },
        { label: 'BDC', route: '/bdc', roles: ['VidhanSabhaPrabhari'] },
        { label: 'Pradhan', route: '/pradhan', roles: ['VidhanSabhaPrabhari'] },
      ]
    },

    // --- LISTS & REGISTERS ---
    {
      icon: 'clipboard-list', label: 'Lists',
      expanded: false,
      roles: ['SUPERADMIN', 'StatePrabhari', 'VidhanSabhaPrabhari', 'BoothSanyojak', 'SectorSanyojak'],
      children: [
        // SUPERADMIN / StatePrabhari Lists
        { label: 'Vidhan Sabha List', route: '/superadmin/vidhansabha-list', roles: ['StatePrabhari'] },
        { label: 'District List', route: '/superadmin/district-list', roles: ['StatePrabhari'] },
        { label: 'Samiti Member List', route: '/state-prabhari/state-member-list', roles: ['StatePrabhari'] },
        { label: 'Pradesh Samiti List', route: '/state-prabhari/pradesh-samiti-list', roles: ['StatePrabhari'] },
        { label: 'Pradesh Karyakarini List', route: '/state-prabhari/pradesh-karyakarini-list', roles: ['StatePrabhari'] },

        // Ground Level Lists (Synced with Dashboard Cards)
        { label: 'Mandal List', route: '/mandal-list', roles: ['dfs'] },
        { label: 'Sector List', route: '/sector-list', roles: ['sds'] },
        { label: 'Booth List', route: '/booth-list', roles: ['SectorSanyojak', 'VidhanSabhaPrabhari'] },
        { label: 'Panna Pramukh List', route: '/panna-pramukh-list', roles: ['VidhanSabhaPrabhari', 'BoothSanyojak', 'SectorSanyojak'], moduleId: ModulePermission.PannaPramukh },
        { label: 'Sahmat List', route: '/sahmat-list', roles: ['VidhanSabhaPrabhari', 'BoothSanyojak', 'SectorSanyojak'], moduleId: ModulePermission.Sahmat },
        { label: 'Asahmat List', route: '/asahmat-list', roles: ['VidhanSabhaPrabhari', 'BoothSanyojak', 'SectorSanyojak'], moduleId: ModulePermission.Asahmat },
        { label: 'Activity List', route: '/activity', roles: ['BoothSanyojak', 'SectorSanyojak'], moduleId: ModulePermission.Activity },
        { label: 'Pravasi Voter List', route: '/pravasi-voter-list', roles: ['VidhanSabhaPrabhari', 'BoothSanyojak', 'SectorSanyojak'], moduleId: ModulePermission.PravashiVoter },
        { label: 'New Voter List', route: '/new-voter-list', roles: ['VidhanSabhaPrabhari', 'BoothSanyojak', 'SectorSanyojak'], moduleId: ModulePermission.NewVoter },
        { label: 'Double Voter List', route: '/double-voter-list', roles: ['VidhanSabhaPrabhari', 'BoothSanyojak', 'SectorSanyojak'], moduleId: ModulePermission.DoubleVoter },
        { label: 'Prabhavshali Vyakti List', route: '/prabhavshali-vyakt-list', roles: ['BoothSanyojak', 'SectorSanyojak', 'VidhanSabhaPrabhari'], moduleId: ModulePermission.EffectivePersion },
        { label: 'Block List', route: '/block-list', roles: ['VidhanSabhaPrabhari'] },
        { label: 'BDC List', route: '/bdc-list', roles: ['VidhanSabhaPrabhari'] },
        { label: 'Pradhan List', route: '/pradhan-list', roles: ['VidhanSabhaPrabhari'] },
        { label: 'Influencer Person List', route: '/influencer-person-list', roles: ['VidhanSabhaPrabhari'] },
        { label: 'Booth Voter Desc. List', route: '/booth-voter-description-list', roles: ['BoothSanyojak', 'SectorSanyojak',], moduleId: ModulePermission.BoothVoterDescrition },
        { label: 'Booth Samiti List', route: '/booth-samiti-list', roles: ['BoothSanyojak', 'SectorSanyojak'], moduleId: ModulePermission.BoothSamiti },
        { label: 'Mandal Samiti List', route: '/mandal-samiti-list', roles: [''] },
        { label: 'Varishth  List', route: '/senior-citizen-list', roles: ['BoothSanyojak', 'SectorSanyojak', 'VidhanSabhaPrabhari'], moduleId: ModulePermission.SeniororDisabled },
        { label: 'Viklang List', route: '/disabled-list', roles: ['BoothSanyojak', 'SectorSanyojak', 'VidhanSabhaPrabhari'], moduleId: ModulePermission.SeniororDisabled },
        { label: 'Doctor List', route: '/doctor-list', roles: ['VidhanSabhaPrabhari'] },
        { label: 'Advocate List', route: '/advocate-list', roles: ['VidhanSabhaPrabhari'] },
        { label: 'Government Employee List', route: '/government-employee-list', roles: ['VidhanSabhaPrabhari'] },
        { label: 'Social Media List', route: '/social-media-list', roles: ['BoothSanyojak', 'SectorSanyojak', ''], moduleId: ModulePermission.SocialMedia },

      ]
    },

    {
      icon: 'bar-chart-2', label: 'Reports',
      expanded: false,
      roles: ['VidhanSabhaPrabhari'],
      children: [
        { label: 'Combined Report', route: '/combined-report' },
        { label: 'Mandal Report', route: '/mandal-report' },
        { label: 'Sector With Booth Report', route: '/sector-with-booth-report' },
        { label: 'Booth Report', route: '/booth-report' },
        { label: 'Sector Report', route: '/sector-report' }
      ]
    },

    {
      icon: 'shield-check', label: 'Access Control',
      expanded: false,
      roles: ['VidhanSabhaPrabhari'],
      children: [
        { label: 'Allow Access', route: '/allow-access' },
        { label: 'Allow Access List', route: '/allow-access-list' },
      ]
    },

    { icon: 'share-2', label: 'Social Media', route: '/social-media', roles: ['VidhanSabhaPrabhari', 'BoothSanyojak', 'SectorSanyojak'], moduleId: ModulePermission.SocialMedia },
    { icon: 'calendar', label: 'Activity', route: '/activity', roles: ['VidhanSabhaPrabhari', 'BoothSanyojak', 'SectorSanyojak'], moduleId: ModulePermission.Activity },
  ];

  renderedNavItems: NavItem[] = [];
  searchTerm = '';

  constructor(
    private authService: AuthServiceService,
    private permissionService: PermissionService,
    private router: Router
  ) { }

  ngOnInit() {
    this.authService.userRole$.subscribe(() => {
      this.updateRenderedItems();
    });

    // Also update when permissions change
    this.permissionService.permissions$.subscribe(() => {
      this.updateRenderedItems();
    });
  }

  onSearch(event: Event) {
    this.searchTerm = (event.target as HTMLInputElement).value;
    this.updateRenderedItems();
  }

  updateRenderedItems() {
    const currentRole = this.authService.getRole();

    const isSectorSanyojak = currentRole && String(currentRole).toUpperCase() === 'SECTORSANYOJAK';
    const isBoothSanyojak = currentRole && String(currentRole).toUpperCase() === 'BOOTHSANYOJAK';
    const isVidhanSabhaPrabhari = currentRole && String(currentRole).toUpperCase() === 'VIDHANSABHAPRABHARI';

    // First filter by role and permission
    let filteredItems = this.navItems.filter(item => {
      // Role check
      const hasRole = !item.roles || item.roles.length === 0 ||
        (currentRole && item.roles.some(role => String(role).toUpperCase() === String(currentRole).toUpperCase()));
      if (!hasRole) return false;

      // For top-level items, we check permission normally (unless Prabhari)
      if (item.moduleId !== undefined && !isVidhanSabhaPrabhari && !this.permissionService.hasPermission(item.moduleId)) return false;

      return true;
    }).map(item => {
      if (item.children) {
        const isListSection = item.label === 'Lists';

        return {
          ...item,
          children: item.children.filter(child => {
            // Role check
            const hasRole = !child.roles || child.roles.length === 0 ||
              (currentRole && child.roles.some(role => String(role).toUpperCase() === String(currentRole).toUpperCase()));
            if (!hasRole) return false;

            if (child.moduleId !== undefined) {
              const shouldBypass = (isListSection && (isSectorSanyojak || isBoothSanyojak)) || isVidhanSabhaPrabhari;
              if (!shouldBypass && !this.permissionService.hasPermission(child.moduleId)) return false;
            }

            return true;
          })
        };
      }
      return item;
    });

    // Filter out items that have an empty children array after filtering
    filteredItems = filteredItems.filter(item => !item.children || item.children.length > 0);

    // Then filter by search term if any
    if (!this.searchTerm) {
      this.renderedNavItems = filteredItems;
      return;
    }

    const term = this.searchTerm.toLowerCase();

    this.renderedNavItems = filteredItems.map(item => {
      const matchMain = item.label.toLowerCase().includes(term);
      const matchedChildren = item.children ? item.children.filter(child => child.label.toLowerCase().includes(term)) : [];

      if (matchMain) {
        return {
          ...item,
          expanded: true
        };
      } else if (matchedChildren.length > 0) {
        return {
          ...item,
          children: matchedChildren,
          expanded: true
        };
      }
      return null;
    }).filter(item => item !== null) as NavItem[];
  }

  toggleSubmenu(item: NavItem, event: Event) {
    if (item.children) {
      event.preventDefault();
      item.expanded = !item.expanded;

      if (item.expanded && this.collapsed) {
        this.collapsed = false;
        this.collapsedChange.emit(this.collapsed);
      }
    }
  }

  toggleCollapse() {
    this.collapsed = !this.collapsed;
    this.collapsedChange.emit(this.collapsed);
  }

  toggleProfileMenu(event?: Event) {
    if (event) {
      event.stopPropagation();
    }
    this.profileMenuOpen = !this.profileMenuOpen;
  }

  onLogout(event: Event) {
    event.stopPropagation();
    this.authService.clearRole();
    this.router.navigate(['/']);
  }
}
