import { Component, OnInit, AfterViewInit, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { RouterModule } from '@angular/router';

export interface NavLink     { label: string; href: string; }
export interface Feature     { icon: string; title: string; desc: string; }
export interface Role        { icon: string; level: string; title: string; sub: string; color: string; bg: string; }
export interface ReportCard  { mockBg: string; title: string; desc: string; }
export interface SecurityPt  { text: string; }
export interface Permission  { label: string; enabled: boolean; locked?: boolean; }
export interface FooterBadge { icon: string; text: string; }

@Component({
  selector: 'app-global-landing',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './global-landing.component.html',
  styleUrls: ['./global-landing.component.css']
})
export class GlobalLandingComponent implements OnInit, AfterViewInit {

  scrolled       = false;
  mobileMenuOpen = false;

  navLinks: NavLink[] = [
    { label: 'Features',  href: 'features'  },
    { label: 'Roles',     href: 'roles'     },
    { label: 'Analytics', href: 'analytics' },
    { label: 'Security',  href: 'security'  },
  ];

  features: Feature[] = [
    { icon: 'fa-chart-pie',          title: 'Multi-Level Dashboards',         desc: 'Superadmin, State Prabhari, Vidhan Sabha, Sector and Booth dashboards with role-specific KPIs and quick filters.' },
    { icon: 'fa-sitemap',            title: 'Administrative Hierarchy',       desc: 'Create and manage States, Districts, Mandals, Sectors, Booths and committee assignments with full audit trails.' },
    { icon: 'fa-users',              title: 'Voter Categorization & Tracking',desc: 'Tag New, Migrant, Senior, Sammat and Avahmat voters with movement and contact history at your fingertips.' },
    { icon: 'fa-star',               title: 'Influencer & Leader Management', desc: 'Record Book Pramukhs, social influencers and engagement history for targeted, measurable outreach.' },
    { icon: 'fa-chart-bar',          title: 'Analytical Reporting',           desc: 'Booth, Sector, Mandal and combined reports with activity logs, comparison views, and export options.' },
    { icon: 'fa-shield-halved',      title: 'Security & Compliance',          desc: 'Role-based permissions, encrypted data at rest and in transit, and audit trails for every sensitive action.' },
  ];

  roles: Role[] = [
    { icon: 'fa-building',      level: 'L1', title: 'State Prabhari', sub: 'Statewide oversight', color: '#0369a1', bg: '#f0f9ff' },
    { icon: 'fa-landmark',      level: 'L2', title: 'Vidhan Sabha Prabhari', sub: 'Constituency management', color: '#0f766e', bg: '#f0fdfa' },
    { icon: 'fa-map-location',  level: 'L3', title: 'Sector Sanyojak', sub: 'Sector coordination', color: '#047857', bg: '#ecfdf5' },
    { icon: 'fa-location-dot',  level: 'L4', title: 'Booth Sanyojak', sub: 'Booth-level operations', color: '#0284c7', bg: '#f0f9ff' }
  ];

  reports: ReportCard[] = [
    { mockBg: 'mandal', title: 'Mandal Report',  desc: 'Trend lines, voter category breakdown and turnout forecast in one view.' },
    { mockBg: 'booth',  title: 'Booth Report',   desc: 'Heatmap, contact activity timeline and sentiment indicator at booth resolution.' },
  ];

  securityPoints: SecurityPt[] = [
    { text: 'Granular module-level permissions' },
    { text: 'End-to-end TLS + AES-256 encryption' },
    { text: 'Audit logs for every administrative action' },
    { text: 'SSO and 2FA ready' },
  ];

  permissions: Permission[] = [
    { label: 'Voter records',   enabled: true,  locked: false },
    { label: 'Booth reports',   enabled: true,  locked: false },
    { label: 'Hierarchy edits', enabled: false, locked: true  },
    { label: 'Influencer log',  enabled: true,  locked: false },
    { label: 'System settings', enabled: false, locked: true  },
  ];

  footerBadges: FooterBadge[] = [
    { icon: 'fa-bolt',       text: 'Real-time updates'      },
    { icon: 'fa-arrow-right',text: 'Actionable next steps'  },
    { icon: 'fa-share-nodes',text: 'Export & share'         },
  ];

  boothTurnout = '+12%';

  ngOnInit(): void {}

  ngAfterViewInit(): void {
    setTimeout(() => {
      const obs = new IntersectionObserver(entries => {
        entries.forEach(e => { if (e.isIntersecting) { e.target.classList.add('visible'); obs.unobserve(e.target); } });
      }, { threshold: 0.08 });
      document.querySelectorAll('.reveal').forEach(el => obs.observe(el));
    }, 80);
  }

  @HostListener('window:scroll')
  onScroll(): void { this.scrolled = window.scrollY > 20; }

  toggleMenu(): void  { this.mobileMenuOpen = !this.mobileMenuOpen; }
  closeMenu(): void   { this.mobileMenuOpen = false; }

  scrollTo(id: string): void {
    this.closeMenu();
    document.getElementById(id)?.scrollIntoView({ behavior: 'smooth' });
  }

  togglePerm(p: Permission): void {
    if (!p.locked) p.enabled = !p.enabled;
  }
}
