import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { NavItem } from './sidebar.types';
import { AuthServiceService } from '../../Services/Auth/auth.service';
import { ModulePermission } from '../../models/module-permission.enum';

import {
  LucideAngularModule
} from 'lucide-angular';
import { PermissionService } from '../../Services/common/permission.service';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    LucideAngularModule
  ],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent implements OnInit {
  @Input() collapsed = false;
  @Input() isMobile = false;
  @Output() collapsedChange = new EventEmitter<boolean>();

  constructor(
    private router: Router,
    private authService: AuthServiceService,
    private permissionService: PermissionService
  ) { }

  profileMenuOpen = false;

  toggleProfileMenu() {
    this.profileMenuOpen = !this.profileMenuOpen;
  }

  onLogout(event?: Event) {
    if (event) {
      event.stopPropagation();
    }
    console.log('User logging out... redirecting to /login');
    this.authService.clearRole();
    this.router.navigate(['/login']);
  }

  navItems: NavItem[] = [
    { icon: 'layout-dashboard', label: 'Dashboard', route: '/', roles: ['ADMIN', 'BoothSanyojak'] },
    {
      icon: 'database', label: 'Master Data',
      badge: 543,
      expanded: false,
      roles: ['ADMIN', 'BoothSanyojak'],
      children: [
        { label: 'Mandal', route: '/mandal', roles: ['ADMIN'] },
        { label: 'Sector', route: '/sector', roles: ['ADMIN'] },
        { label: 'Booth', route: '/booth', roles: ['ADMIN'] },
        { label: 'PannaPramukh', route: '/panna-pramukh', roles: ['ADMIN', 'BoothSanyojak'], moduleId: ModulePermission.PannaPramukh },
        { label: 'Pravasi Voter', route: '/pravasi-voter', roles: ['ADMIN', 'BoothSanyojak'], moduleId: ModulePermission.PravashiVoter },
        { label: 'New Voter', route: '/new-voter', roles: ['ADMIN', 'BoothSanyojak'], moduleId: ModulePermission.NewVoter },
        { label: 'Varisth Naagarik/Viklaang', route: '/varisth-naagarik-viklaang', roles: ['ADMIN', 'BoothSanyojak'], moduleId: ModulePermission.SeniororDisabled },
        { label: 'Booth Voter Description', route: '/booth-voter-description', roles: ['ADMIN', 'BoothSanyojak'], moduleId: ModulePermission.BoothVoterDescrition },
        { label: 'Sahmat/Asahmat', route: '/sahmat-asahmat', roles: ['ADMIN', 'BoothSanyojak'], moduleId: ModulePermission.Sahmat },
        { label: 'Double Voter/Married', route: '/double-voter', roles: ['ADMIN', 'BoothSanyojak'], moduleId: ModulePermission.DoubleVoter },
        { label: 'Booth Samiti', route: '/booth-samiti', roles: ['ADMIN', 'BoothSanyojak'], moduleId: ModulePermission.BoothSamiti },
        { label: 'Prabhavshali Vyakt', route: '/prabhavshali-vyakt', roles: ['ADMIN', 'BoothSanyojak'], moduleId: ModulePermission.EffectivePersion },
        { label: 'Influencer Person', route: '/influencer-person', roles: ['ADMIN'] },
        { label: 'Block', route: '/block', roles: ['ADMIN'] },
        { label: 'BDC', route: '/bdc', roles: ['ADMIN'] },
        { label: 'Pradhan', route: '/pradhan', roles: ['ADMIN'] }
      ]
    },
    {
      icon: 'bar-chart-3', label: 'Reports',
      badge: 0,
      expanded: false,
      roles: ['ADMIN'],
      children: [
        { label: 'Combined Report', route: '/combined-report' },
        { label: 'SectorWithBooth Report', route: '/sector-with-booth-report' },
        { label: 'Mandal Report', route: '/mandal-report' },
        { label: 'Sector Report', route: '/sector-report' },
        { label: 'Booth Report', route: '/booth-report' }
      ]
    },
    {
      icon: 'shield-check', label: 'Access',
      badge: 28,
      expanded: false,
      roles: ['ADMIN'],
      children: [
        { label: 'Allow Access', route: '/allow-access' },
        { label: 'Allow Access List', route: '/allow-access-list' },
      ]
    },
    { icon: 'layout-dashboard', label: 'Dashboard', route: '/superadmin/dashboard', roles: ['SUPERADMIN'] },
    { icon: 'layout-dashboard', label: 'Dashboard', route: '/state-prabhari/dashboard', roles: ['StatePrabhari'] },
    { icon: 'users', label: 'Vidhan Sabha Prabhari', route: '/state-prabhari/vidhansabha-prabhari', roles: ['StatePrabhari'] },
    { icon: 'user-cog', label: 'Designation', route: '/superadmin/designation', roles: ['Adhyaksh', 'StatePrabhari'] },
    { icon: 'map', label: 'State', route: '/superadmin/state', roles: ['SUPERADMIN', 'StatePrabhari'] },
    { icon: 'clipboard-list', label: 'State Prabhari', route: '/superadmin/state-prabhari', roles: ['SUPERADMIN'] },
    { icon: 'navigation', label: 'District', route: '/superadmin/district', roles: ['Adhyaksh', 'StatePrabhari'] },
    { icon: 'landmark', label: 'Vidhan Sabha', route: '/superadmin/vidhansabha', roles: ['Adhyaksh', 'StatePrabhari'] },
    {
      icon: 'clipboard-list', label: 'Lists',
      badge: 0,
      expanded: false,
      roles: ['ADMIN'],
      children: [
        { label: 'Booth List', route: '/booth-list' },
        { label: 'Pravasi Voter List', route: '/pravasi-voter-list' },
        { label: 'Double Voter List', route: '/double-voter-list' },
        { label: 'New Voter List', route: '/new-voter-list' },
        { label: 'Sahmat List', route: '/sahmat-list' },
        { label: 'Asahmat List', route: '/asahmat-list' },
        { label: 'Doctor List', route: '/doctor-list' },
        { label: 'Advocate List', route: '/advocate-list' },
        { label: 'Government Employee List', route: '/government-employee-list' },
        { label: 'Pradhan List', route: '/pradhan-list' },
        { label: 'BDC List', route: '/bdc-list' },
        { label: 'BlockPramukh List', route: '/block-pramukh-list' },
        { label: 'Senior Citizen List', route: '/senior-citizen-list' },
        { label: 'Disabled List', route: '/disabled-list' },
        { label: 'Varisth/Viklaang List', route: '/varisth-naagarik-viklaang-list' },
        { label: 'Panna Pramukh List', route: '/panna-pramukh-list' },
        { label: 'Influencer Person List', route: '/influencer-person-list' },
      ]
    },
    { icon: 'share-2', label: 'Socail Media', route: '/socail-media', roles: ['ADMIN', 'BoothSanyojak'], moduleId: ModulePermission.SocialMedia },
    { icon: 'calendar', label: 'Activity', route: '/activity', roles: ['ADMIN', 'BoothSanyojak'], moduleId: ModulePermission.Activity },

    {
      icon: 'clipboard-list', label: 'Lists',
      badge: 0,
      expanded: false,
      roles: ['BoothSanyojak'],
      children: [
        { label: 'Booth Voters', route: '/booth-list' },
        { label: 'Panna Pramukh', route: '/panna-pramukh-list' },
        { label: 'Activities', route: '/activity' },
        { label: 'New Voters', route: '/new-voter-list' },
        { label: 'Pravasi Voter', route: '/pravasi-voter-list' },
        { label: 'Sahmat', route: '/sahmat-list' },
        { label: 'Asahmat', route: '/asahmat-list' },
        { label: 'Double Voter/Married', route: '/double-voter-list' },
        { label: 'Booth Samiti', route: '/booth-samiti-list' },
        { label: 'Prabhavshali Vyakti', route: '/influencer-person-list' },
        { label: 'Varishth Nagrik', route: '/senior-citizen-list' },
        { label: 'Viklaang', route: '/disabled-list' },
        { label: 'Social Media', route: '/socail-media' },
      ]
    }
  ];

  renderedNavItems: NavItem[] = [];
  searchTerm = '';

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

    // First filter by role and permission
    let filteredItems = this.navItems.filter(item => {
      // Role check
      const hasRole = !item.roles || item.roles.length === 0 ||
        (currentRole && item.roles.some(role => role.toUpperCase() === currentRole.toUpperCase()));
      if (!hasRole) return false;

      // Permission check (skip for ADMIN/SUPERADMIN usually, but keep it generic)
      if (item.moduleId !== undefined && !this.permissionService.hasPermission(item.moduleId)) return false;

      return true;
    }).map(item => {
      if (item.children) {
        return {
          ...item,
          children: item.children.filter(child => {
            // Role check
            const hasRole = !child.roles || child.roles.length === 0 ||
              (currentRole && child.roles.some(role => role.toUpperCase() === currentRole.toUpperCase()));
            if (!hasRole) return false;

            // Permission check
            if (child.moduleId !== undefined && !this.permissionService.hasPermission(child.moduleId)) return false;

            return true;
          })
        };
      }
      return item;
    });

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
}
