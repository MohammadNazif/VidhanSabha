import { Component, OnInit, AfterViewInit, ViewChild, ElementRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable } from 'rxjs';
import { Chart, registerables } from 'chart.js';
import {
  LucideAngularModule,
  Building,
  Map,
  Vote,
  BookOpen,
  ShieldCheck,
  ShieldAlert,
  Calendar,
  Users,
  Plus,
  Database,
  Share2,
  ClipboardList,
  BarChart2
} from 'lucide-angular';

import { Router } from '@angular/router';
import { AuthServiceService } from '../../../Services/Auth/auth.service';
import { DashboardService } from '../../../Services/Admin/dashboard.service';
import { BaseApiService } from '../../../Services/common/base-api.service';

Chart.register(...registerables);

interface StatCard {
  title: string;
  value: string | number;
  change: string;
  changeType: 'up' | 'down';
  icon: string;
  gradient: string;
  route?: string;
}

interface RecentActivity {
  icon: string;
  title: string;
  description: string;
  time: string;
  color: string;
}

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    LucideAngularModule
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit, AfterViewInit {
  @ViewChild('revenueChart') revenueChartRef!: ElementRef<HTMLCanvasElement>;
  @ViewChild('sessionChart') sessionChartRef!: ElementRef<HTMLCanvasElement>;
  @ViewChild('partyChart') partyChartRef!: ElementRef<HTMLCanvasElement>;
  @ViewChild('attendanceChart') attendanceChartRef!: ElementRef<HTMLCanvasElement>;
  @ViewChild('chart2019LS') chart2019Ref!: ElementRef<HTMLCanvasElement>;
  @ViewChild('chart2024LS') chart2024Ref!: ElementRef<HTMLCanvasElement>;

  currentDate = new Date();
  isRestrictedRole = false;
  userRole = '';
  statCards: StatCard[] = [];
  loadingCounts = true;
  vidhanSabhaName = '';
  vidhanSabhaNumber = '';

  // All available cards with their default configurations
  private allCards: StatCard[] = [
    { title: 'Mandal', value: 0, icon: 'building', gradient: 'linear-gradient(135deg, #6366f1, #4f46e5)', change: '+0%', changeType: 'up', route: '/mandal-list' },
    { title: 'Sector', value: 0, icon: 'map', gradient: 'linear-gradient(135deg, #0ea5e9, #0284c7)', change: '+0%', changeType: 'up', route: '/sector-list' },
    { title: 'Booth', value: 0, icon: 'vote', gradient: 'linear-gradient(135deg, #22c55e, #16a34a)', change: '+0%', changeType: 'up', route: '/booth-list' },
    { title: 'PannaPramukh', value: 0, icon: 'book-open', gradient: 'linear-gradient(135deg, #f59e0b, #d97706)', change: '+0%', changeType: 'up', route: '/panna-pramukh-list' },
    { title: 'Sahmat', value: 0, icon: 'shield-check', gradient: 'linear-gradient(135deg, #10b981, #059669)', change: '+0%', changeType: 'up', route: '/sahmat-list' },
    { title: 'Asahmat', value: 0, icon: 'shield-alert', gradient: 'linear-gradient(135deg, #ef4444, #dc2626)', change: '+0%', changeType: 'down', route: '/asahmat-list' },
    // { title: 'Activities', value: 0, icon: 'calendar', gradient: 'linear-gradient(135deg, #8b5cf6, #7c3aed)', change: '+0%', changeType: 'up', route: '/activity' },
    { title: 'Pravasi', value: 0, icon: 'users', gradient: 'linear-gradient(135deg, #06b6d4, #0891b2)', change: '+0%', changeType: 'up', route: '/pravasi-voter-list' },
    { title: 'New Voters', value: 0, icon: 'plus', gradient: 'linear-gradient(135deg, #84cc16, #65a30d)', change: '+0%', changeType: 'up', route: '/new-voter-list' },
    { title: 'Double Voters', value: 0, icon: 'shield-alert', gradient: 'linear-gradient(135deg, #f97316, #ea580c)', change: '+0%', changeType: 'down', route: '/double-voter-list' },
    { title: 'Prabhavsali Vyakti', value: 0, icon: 'users', gradient: 'linear-gradient(135deg, #eab308, #ca8a04)', change: '+0%', changeType: 'up', route: '/prabhavshali-vyakt-list' },
    { title: 'Block', value: 0, icon: 'building', gradient: 'linear-gradient(135deg, #64748b, #475569)', change: '+0%', changeType: 'up', route: '/block-list' },
    { title: 'BDC', value: 0, icon: 'database', gradient: 'linear-gradient(135deg, #14b8a6, #0f766e)', change: '+0%', changeType: 'up', route: '/bdc-list' },
    { title: 'Influencer Person', value: 0, icon: 'users', gradient: 'linear-gradient(135deg, #ec4899, #db2777)', change: '+0%', changeType: 'up', route: '/influencer-person-list' },
    { title: 'Booth Voter', value: 0, icon: 'vote', gradient: 'linear-gradient(135deg, #22c55e, #16a34a)', change: '+0%', changeType: 'up', route: '/booth-voter-description-list' },
    { title: 'Booth Samiti', value: 0, icon: 'building', gradient: 'linear-gradient(135deg, #6366f1, #4f46e5)', change: '+0%', changeType: 'up', route: '/booth-samiti-list' },
    { title: 'Senior Citizen', value: 0, icon: 'users', gradient: 'linear-gradient(135deg, #f59e0b, #d97706)', change: '+0%', changeType: 'up', route: '/senior-citizen-list' },
    { title: 'Vikalaang', value: 0, icon: 'users', gradient: 'linear-gradient(135deg, #ef4444, #dc2626)', change: '+0%', changeType: 'up', route: '/disabled-list' },
    { title: 'Post', value: 0, icon: 'share-2', gradient: 'linear-gradient(135deg, #8b5cf6, #7c3aed)', change: '+0%', changeType: 'up', route: '/social-media-list' },
    { title: 'Combined Report', value: 'View', icon: 'bar-chart-2', gradient: 'linear-gradient(135deg, #6366f1, #4f46e5)', change: 'Full Summary', changeType: 'up', route: '/combined-report' },
    { title: 'Mandal Report', value: 'View', icon: 'bar-chart-2', gradient: 'linear-gradient(135deg, #0ea5e9, #0284c7)', change: 'Regional Summary', changeType: 'up', route: '/mandal-report' },
    { title: 'Booth Voter Report', value: 'View', icon: 'bar-chart-2', gradient: 'linear-gradient(135deg, #22c55e, #16a34a)', change: 'Voter Stats', changeType: 'up', route: '/booth-voter-description-list' },
    { title: 'Booth Samiti Report', value: 'View', icon: 'bar-chart-2', gradient: 'linear-gradient(135deg, #8b5cf6, #7c3aed)', change: 'Samiti Stats', changeType: 'up', route: '/booth-samiti-list' },
    { title: 'Pradhan', value: 0, icon: 'users', gradient: 'linear-gradient(135deg, #ec4899, #db2777)', change: '+0%', changeType: 'up', route: '/pradhan-list' },
  ];

  constructor(
    private router: Router,
    private authService: AuthServiceService,
    private dashboardService: DashboardService,
    private baseApi: BaseApiService
  ) { }

  navigateTo(route?: string) {
    if (route) {
      this.router.navigate([route]);
    }
  }

  recentActivities: RecentActivity[] = [
    {
      icon: '📋',
      title: 'New Bill Introduced',
      description: 'Education Reform Act 2026 introduced in Lok Sabha',
      time: '2 min ago',
      color: '#6366f1'
    },
    {
      icon: '🗳️',
      title: 'Voting Completed',
      description: 'Finance Bill passed with 342 votes in favor',
      time: '15 min ago',
      color: '#10b981'
    },
    {
      icon: '👤',
      title: 'Member Update',
      description: 'New member onboarded from Karnataka constituency',
      time: '1 hour ago',
      color: '#06b6d4'
    },
    {
      icon: '📅',
      title: 'Session Scheduled',
      description: 'Budget session scheduled for April 15, 2026',
      time: '2 hours ago',
      color: '#f59e0b'
    },
    {
      icon: '📊',
      title: 'Report Generated',
      description: 'Q1 2026 Legislative performance report is ready',
      time: '5 hours ago',
      color: '#ec4899'
    }
  ];

  topMembers = [
    { name: 'Rajesh Kumar', party: 'BJP', attendance: 96, bills: 14, avatar: 'R' },
    { name: 'Priya Sharma', party: 'INC', attendance: 94, bills: 11, avatar: 'P' },
    { name: 'Amit Patel', party: 'AAP', attendance: 92, bills: 9, avatar: 'A' },
    { name: 'Sneha Reddy', party: 'TMC', attendance: 90, bills: 8, avatar: 'S' },
    { name: 'Vikram Singh', party: 'BJP', attendance: 89, bills: 7, avatar: 'V' },
  ];

  ngOnInit() {
    this.authService.userRole$.subscribe(role => {
      const r = (role || '').toUpperCase().trim();
      if (r === 'BOOTHSANYOJAK') {
        this.userRole = 'Booth Adhyaksh';
      } else {
        this.userRole = role || '';
      }
      this.isRestrictedRole = r === 'BOOTHSANYOJAK' || r === 'SECTORSANYOJAK';
      console.log('Dashboard detected role:', r);
      if (r === 'SUPERADMIN') {
        this.router.navigate(['/superadmin/dashboard']);
      } else if (r === 'STATEPRABHARI') {
        this.router.navigate(['/state-prabhari/dashboard']);
      }

      this.initializeCards(r);
    });

    this.loadCounts();
    this.loadProfile();
  }

  loadProfile() {
    this.authService.profileData$.subscribe(data => {
      if (data) {
        this.vidhanSabhaName = data.vidhanSabhaName || '';
        this.vidhanSabhaNumber = data.vidhanSabhaNumber?.toString() || '';
      }
    });
  }

  initializeCards(role: string) {
    const r = role.toUpperCase().trim();
    if (r === 'BOOTHSANYOJAK' || r === 'SECTORSANYOJAK') {
      const allowedTitles = [
        'Booth Voter', 'PannaPramukh', 'Activities', 'Double Voters',
        'Pravasi', 'Sahmat', 'Asahmat', 'New Voters', 'Booth Samiti',
        'Prabhavsali Vyakti', 'Senior Citizen', 'Vikalaang', 'Post'
      ];

      if (r === 'SECTORSANYOJAK') {
        allowedTitles.unshift('Booth');
      }

      this.statCards = this.allCards.filter(card => allowedTitles.includes(card.title));
      // Re-sort to maintain order if needed, or just let filter handle it
    } else {
      // Default cards for other roles (like VidhanSabhaPrabhari)
      const defaultTitles = [
        'Mandal', 'Sector', 'Booth', 'PannaPramukh', 'Sahmat', 'Asahmat',
        'Activities', 'Pravasi', 'New Voters', 'Double Voters',
        'Prabhavsali Vyakti', 'Block', 'BDC', 'Influencer Person', 'Pradhan'
      ];
      this.statCards = this.allCards.filter(card => defaultTitles.includes(card.title));
    }
  }

  loadCounts() {
    const role = (this.authService.getRole() || '').toUpperCase().trim();
    const isBoothOnly = role === 'BOOTHSANYOJAK';

    let obs: Observable<any>;
    if (role === 'BOOTHSANYOJAK') {
      obs = this.dashboardService.getBoothCounts();
    } else if (role === 'SECTORSANYOJAK') {
      obs = this.dashboardService.getSectorCounts();
    } else {
      obs = this.dashboardService.getGlobalCounts();
    }

    obs.subscribe({
      next: (res: any) => {
        if (res.isSuccess && res.data) {
          const counts = res.data;
          if (counts.boothId) {
            this.authService.setBoothId(String(counts.boothId));
          }
          if (counts.sectorId) {
            this.authService.setSectorId(String(counts.sectorId));
          }
          if (counts.mandalId) {
            this.authService.setMandalId(String(counts.mandalId));
          }
          this.statCards = this.statCards.map(card => {
            const key = this.getMapKey(card.title);
            if (counts[key] !== undefined) {
              return { ...card, value: counts[key] };
            }
            return card;
          });
        }
        this.loadingCounts = false;
      },
      error: (err: any) => {
        console.error('Error fetching dashboard counts:', err);
        this.loadingCounts = false;
      }
    });
  }

  private getMapKey(title: string): string {
    const mapping: { [key: string]: string } = {
      'Mandal': 'mandal',
      'Sector': 'sector',
      'Booth': 'booth',
      'PannaPramukh': 'pannaPramukh',
      'Sahmat': 'sahmat',
      'Asahmat': 'asahmat',
      'Activities': 'activities',
      'Pravasi': 'pravasi',
      'New Voters': 'newVoters',
      'Double Voters': 'doubleVoter',
      'Prabhavsali Vyakti': 'prabhavshaliVyakti',
      'Block': 'block',
      'BDC': 'bdc',
      'Influencer Person': 'influencerPerson',
      'Booth Voter': 'boothVoter',
      'Booth Samiti': 'boothSamiti',
      'Senior Citizen': 'varisthNagrik',
      'Vikalaang': 'viklaang',
      'Post': 'post',
      'Pradhan': 'pradhan'
    };
    return mapping[title] || title.toLowerCase();
  }

  ngAfterViewInit() {
    this.createRevenueChart();
    this.createPartyChart();
    this.create2019LSChart();
    this.create2024LSChart();
  }

  private createRevenueChart() {
    if (!this.revenueChartRef || !this.revenueChartRef.nativeElement) return;
    const ctx = this.revenueChartRef.nativeElement.getContext('2d')!;

    const gradient = ctx.createLinearGradient(0, 0, 0, 300);
    gradient.addColorStop(0, 'rgba(29, 78, 216, 0.2)'); // Navy blue
    gradient.addColorStop(1, 'rgba(29, 78, 216, 0.0)');

    const gradient2 = ctx.createLinearGradient(0, 0, 0, 300);
    gradient2.addColorStop(0, 'rgba(22, 163, 74, 0.2)'); // Green
    gradient2.addColorStop(1, 'rgba(22, 163, 74, 0.0)');

    new Chart(ctx, {
      type: 'bar',
      plugins: [{
        id: 'datalabels',
        afterDatasetsDraw(chart) {
          const { ctx, data } = chart;
          ctx.save();
          chart.data.datasets.forEach((dataset, i) => {
            chart.getDatasetMeta(i).data.forEach((bar, index) => {
              const value = dataset.data[index] as number;
              ctx.fillStyle = '#475569';
              ctx.font = 'bold 12px Inter';
              ctx.textAlign = 'center';
              ctx.textBaseline = 'bottom';
              ctx.fillText(value.toString(), bar.x, bar.y - 5);
            });
          });
          ctx.restore();
        }
      }],
      data: {
        labels: ['BJP', 'SP', 'ADS', 'RLD', 'NISHAD', 'Others'],
        datasets: [
          {
            label: 'Seats Obtained',
            data: [255, 111, 12, 8, 6, 11],
            backgroundColor: [
              '#FF9933', // BJP - Saffron
              '#ef4444', // SP - Red
              '#eab308', // ADS - Gold
              '#16a34a', // RLD - Green
              '#1d4ed8', // NISHAD - Blue
              '#64748b', // Others - Slate
            ],
            borderRadius: 8,
            barThickness: 45,
          }
        ]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        layout: {
          padding: {
            top: 20,
            bottom: 10,
            left: 10,
            right: 10
          }
        },
        plugins: {
          legend: { display: false },
          tooltip: { enabled: true }
        },
        scales: {
          x: {
            grid: { display: false },
            ticks: { color: '#64748b', font: { family: 'Inter', size: 11, weight: 600 } }
          },
          y: {
            beginAtZero: true,
            max: 300,
            grid: {
              color: 'rgba(0,0,0,0.05)',
              lineWidth: 1
            },
            ticks: {
              color: '#64748b',
              font: { family: 'Inter', size: 11 },
              padding: 10
            }
          }
        }
      }
    });
  }


  private createPartyChart() {
    if (!this.partyChartRef || !this.partyChartRef.nativeElement) return;
    const ctx = this.partyChartRef.nativeElement.getContext('2d')!;

    new Chart(ctx, {
      type: 'doughnut',
      data: {
        labels: [
          'BJP (Dinesh Khatik): 1,07,587',
          'SP (Yogesh Verma): 1,00,275',
          'BSP (Sanjeev Kumar): 14,240',
          'AIMIM (Vinod Jatav): 4,290',
          'Others/NOTA: 1,513'
        ],
        datasets: [{
          data: [107587, 100275, 14240, 4290, 1513],
          backgroundColor: [
            '#FF9933', // BJP
            '#ef4444', // SP
            '#1d4ed8', // BSP
            '#06b6d4', // AIMIM
            '#64748b', // Others/NOTA
          ],
          borderColor: '#ffffff',
          borderWidth: 2,
          hoverBorderColor: '#ffffff',
          hoverOffset: 8,
        }]
      },
      options: {
        animation: {
          duration: 1200,
          easing: 'easeOutBounce',
          delay: 200
        },
        responsive: true,
        maintainAspectRatio: false,
        layout: {
          padding: 10
        },
        cutout: '70%',
        plugins: {
          legend: {
            display: true,
            position: 'bottom',
            labels: {
              color: '#475569',
              font: { family: 'Inter', size: 10, weight: 600 },
              usePointStyle: true,
              pointStyle: 'circle',
              padding: 12,
            }
          },
          tooltip: {
            backgroundColor: '#ffffff',
            titleColor: '#0f172a',
            bodyColor: '#475569',
            borderColor: '#e2e8f0',
            borderWidth: 1,
            cornerRadius: 8,
            padding: 12,
          }
        }
      }
    });
  }

  private create2019LSChart() {
    if (!this.chart2019Ref || !this.chart2019Ref.nativeElement) return;
    const ctx = this.chart2019Ref.nativeElement.getContext('2d')!;

    new Chart(ctx, {
      type: 'doughnut',
      data: {
        labels: [
          'BJP: 62 (50%)',
          'BSP: 10 (19.4%)',
          'SP: 5 (18.1%)',
          'ADS: 2 (1.2%)',
          'INC: 1 (6.4%)'
        ],
        datasets: [{
          data: [62, 10, 5, 2, 1],
          backgroundColor: ['#FF9933', '#1d4ed8', '#ef4444', '#eab308', '#0ea5e9'],
          borderColor: '#ffffff',
          borderWidth: 2,
          hoverOffset: 8,
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        cutout: '70%',
        plugins: {
          legend: {
            display: true,
            position: 'bottom',
            labels: {
              color: '#475569',
              font: { family: 'Inter', size: 10, weight: 600 },
              usePointStyle: true,
              pointStyle: 'circle',
              padding: 8,
            }
          }
        }
      }
    });
  }

  private create2024LSChart() {
    if (!this.chart2024Ref || !this.chart2024Ref.nativeElement) return;
    const ctx = this.chart2024Ref.nativeElement.getContext('2d')!;

    new Chart(ctx, {
      type: 'doughnut',
      data: {
        labels: [
          'SP: 37 (33.8%)',
          'BJP: 33 (41.7%)',
          'INC: 6 (9.5%)',
          'RLD: 2 (1.0%)',
          'ASPKR: 1 (0.7%)',
          'Others: 1'
        ],
        datasets: [{
          data: [37, 33, 6, 2, 1, 1],
          backgroundColor: ['#ef4444', '#FF9933', '#0ea5e9', '#16a34a', '#1d4ed8', '#64748b'],
          borderColor: '#ffffff',
          borderWidth: 2,
          hoverOffset: 8,
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        cutout: '70%',
        plugins: {
          legend: {
            display: true,
            position: 'bottom',
            labels: {
              color: '#475569',
              font: { family: 'Inter', size: 10, weight: 600 },
              usePointStyle: true,
              pointStyle: 'circle',
              padding: 8,
            }
          }
        }
      }
    });
  }
}
