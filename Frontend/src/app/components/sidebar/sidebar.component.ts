import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { NavItem } from './sidebar.types';
import { AuthServiceService } from '../../Services/Auth/auth.service';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent implements OnInit {
  @Input() collapsed = false;
  @Input() isMobile = false;
  @Output() collapsedChange = new EventEmitter<boolean>();

  constructor(private router: Router, private authService: AuthServiceService) { }

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
    { icon: '📊', label: 'Dashboard', route: '/' },
    {
      icon: '👥', label: 'Master Data',
      badge: 543,
      expanded: false,

      children: [
        { label: 'Mandal', route: '/mandal' },
        { label: 'Sector', route: '/sector' },
        { label: 'Booth', route: '/booth' },
        { label: 'PannaPramukh', route: '/panna-pramukh' },
        { label: 'Pravasi Voter', route: '/pravasi-voter' },
        { label: 'New Voter', route: '/new-voter' },
        { label: 'Varisth Naagarik/Viklaang', route: '/varisth-naagarik-viklaang' },
        { label: 'Booth Voter Description', route: '/booth-voter-description' },
        { label: 'Sahmat/Asahmat', route: '/sahmat-asahmat' },
        { label: 'DoubleVoter/Married', route: '/double-voter-married' },
        { label: 'Booth Samiti', route: '/booth-samiti' },
        { label: 'Prabhavshali Vyakt', route: '/prabhavshali-vyakt' },
        { label: 'Influencer Person', route: '/influencer-person' },
        { label: 'Block', route: '/block' },
        { label: 'BDC', route: '/bdc' },
        { label: 'Pradhan', route: '/pradhan' }

      ]
    },
    {
      icon: '📋', label: 'Reports',
      badge: 0,
      expanded: false,
      // roles: ['ADMIN'],
      children: [
        { label: 'Combined Report', route: '/combined-report' },
        { label: 'SectorWithBooth Report', route: '/sector-with-booth-report' },
        { label: 'Mandal Report', route: '/mandal-report' },
        { label: 'Sector Report', route: '/sector-report' },
        { label: 'Booth Report', route: '/booth-report' }
      ]
    },
    {
      icon: '📋', label: 'Access',
      badge: 28,
      expanded: false,
      // roles: ['ADMIN'],
      children: [
        { label: 'Allow Access', route: '/allow-access' },
        { label: 'Allow Access List', route: '/allow-access-list' },

      ]
    },
    {
      icon: '📋', label: 'Lists',
      badge: 0,
      expanded: false,
      // roles: ['ADMIN'],
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
        { label: 'Influencer Person List', route: '/influencer-person-list' },
      ]
    },
    { icon: '🗳️', label: 'Socail Media', route: '/socail-media' },
    { icon: '📅', label: 'Activity', route: '/activity' }
  ];

  renderedNavItems: NavItem[] = [];
  searchTerm = '';

  ngOnInit() {
    this.authService.userRole$.subscribe(() => {
      this.updateRenderedItems();
    });
  }

  onSearch(event: Event) {
    this.searchTerm = (event.target as HTMLInputElement).value;
    this.updateRenderedItems();
  }

  updateRenderedItems() {
    const currentRole = this.authService.getRole();

    // First filter by role
    let filteredItems = this.navItems.filter(item => {
      // If item has no roles defined, it's visible to everyone
      if (!item.roles || item.roles.length === 0) return true;
      // Check if current role is allowed
      return currentRole && item.roles.includes(currentRole);
    }).map(item => {
      // Filter children by role as well
      if (item.children) {
        return {
          ...item,
          children: item.children.filter(child => {
            if (!child.roles || child.roles.length === 0) return true;
            return currentRole && child.roles.includes(currentRole);
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
