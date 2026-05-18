import { Component, OnInit, AfterViewInit, ViewChild, ElementRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Chart, registerables } from 'chart.js';
import { Router } from '@angular/router';
import { LucideAngularModule } from 'lucide-angular';
import { AuthServiceService } from '../../../Services/Auth/auth.service';
import { VidhanSabhaCountService } from '../../../Services/Admin/vidhansabha-count/vidhansabha-count.service';
import { StatePrabhariService } from '../../../Services/Admin/state-prabhari/state-prabhari.service';

Chart.register(...registerables);

interface StatCard {
  title: string;
  value: string | number;
  change: string;
  changeType: 'up' | 'down';
  icon: string;
  gradient: string;
  route?: string;
  description?: string;
}

@Component({
  selector: 'app-state-prabhari-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    LucideAngularModule
  ],
  templateUrl: './state-prabhari-dashboard.component.html',
  styleUrl: './state-prabhari-dashboard.component.css'
})
export class StatePrabhariDashboardComponent implements OnInit, AfterViewInit {
  @ViewChild('statsChart') statsChartRef!: ElementRef<HTMLCanvasElement>;

  currentDate = new Date();
  vidhanSabhaData: any[] = [];
  loadingCounts = true;
  userRole = '';
  private chart: Chart | null = null;

  statCards: StatCard[] = [
    {
      title: 'Vidhan Sabha',
      value: '-',
      change: '0',
      changeType: 'up',
      icon: 'landmark',
      gradient: 'linear-gradient(135deg, #f59e0b, #d97706)',
      route: '/superadmin/vidhansabha-list',
      description: 'Total constituencies'
    },
    {
      title: 'Districts',
      value: '-',
      change: '0',
      changeType: 'up',
      icon: 'navigation',
      gradient: 'linear-gradient(135deg, #0ea5e9, #0284c7)',
      route: '/superadmin/district-list',
      description: 'Active districts'
    },
    {
      title: 'Pradesh Samiti',
      value: '-',
      change: '0',
      changeType: 'up',
      icon: 'users',
      gradient: 'linear-gradient(135deg, #ec4899, #db2777)',
      route: '/state-prabhari/pradesh-samiti-list',
      description: 'Samiti members'
    },
    {
      title: 'Pradesh Karyakarini',
      value: '-',
      change: '0',
      changeType: 'up',
      icon: 'users',
      gradient: 'linear-gradient(135deg, #6366f1, #4f46e5)',
      route: '/state-prabhari/pradesh-karyakarini-list',
      description: 'Karyakarini members'
    },
    {
      title: 'Designations',
      value: '-',
      change: '0',
      changeType: 'up',
      icon: 'user-cog',
      gradient: 'linear-gradient(135deg, #22c55e, #16a34a)',
      route: '/superadmin/designation',
      description: 'Manage roles'
    }
  ];

  quickActions = [
    { title: 'Manage Vidhan Sabha', description: 'Add or update constituencies', icon: 'landmark', route: '/superadmin/vidhansabha', gradient: 'linear-gradient(135deg, #f59e0b, #d97706)' },
    { title: 'Update Designations', description: 'Manage administrative roles', icon: 'user-cog', route: '/superadmin/designation', gradient: 'linear-gradient(135deg, #6366f1, #4f46e5)' },
    { title: 'Review Districts', description: 'View assigned district details', icon: 'navigation', route: '/superadmin/district', gradient: 'linear-gradient(135deg, #0ea5e9, #0284c7)' },
    { title: 'State Prabhari List', description: 'View all state prabharis', icon: 'clipboard-list', route: '/superadmin/state-prabhari', gradient: 'linear-gradient(135deg, #22c55e, #16a34a)' },
    { title: 'Samiti Member List', description: 'Manage Samiti members', icon: 'users', route: '/superadmin/state-member', gradient: 'linear-gradient(135deg, #ec4899, #db2777)' }
  ];

  constructor(
    private router: Router,
    private authService: AuthServiceService,
    private vidhanSabhaCountService: VidhanSabhaCountService,
    private statePrabhariService: StatePrabhariService
  ) { }

  ngOnInit() {
    this.authService.userRole$.subscribe(role => {
      this.userRole = role || '';
    });

    this.authService.userId$.subscribe(userId => {
      console.log('[StatePrabhariDashboard] userId:', userId);
      if (userId) {
        // this.loadVidhanSabhaData(userId);
        this.loadDashboardCounts(userId);
      }
    });
  }


  ngAfterViewInit() {
    this.createStatsChart([]);
  }

  navigateTo(route?: string) {
    if (route) {
      this.router.navigate([route]);
    }
  }

  private loadVidhanSabhaData(userId: string) {
    this.vidhanSabhaCountService.getAllByUserId(userId).subscribe({
      next: (res: any) => {
        const data = res?.data ?? (Array.isArray(res) ? res : []);
        this.vidhanSabhaData = data;

        // Update Vidhan Sabha stat card
        this.statCards[0] = {
          ...this.statCards[0],
          value: data.length
        };

        // Rebuild chart with real data
        this.rebuildChart(data);
      },
      error: (err) => console.error('Error loading Vidhan Sabha data:', err)
    });
  }

  private loadDashboardCounts(userId: string) {
    this.statePrabhariService.getDashboardCounts(userId).subscribe({
      next: (res: any) => {
        const counts = res?.data || res || {};

        // Update Stat Cards with dynamic data
        this.statCards = this.statCards.map(card => {
          if (card.title === 'Vidhan Sabha') return { ...card, value: counts.vidhanSabha ?? card.value };
          if (card.title === 'Districts') return { ...card, value: counts.district ?? card.value };
          if (card.title === 'Designations') return { ...card, value: counts.designation ?? card.value };
          if (card.title === 'Pradesh Samiti') return { ...card, value: counts.pradeshSamiti ?? card.value };
          if (card.title === 'Pradesh Karyakarini') return { ...card, value: counts.pradeshKaryarkarniSamiti ?? card.value };
          return card;
        });

        if (counts.stateId) {
          this.authService.setStateId(String(counts.stateId));
        }

        this.loadingCounts = false;
      },
      error: (err) => {
        console.error('Error loading dashboard counts:', err);
        this.loadingCounts = false;
      }
    });
  }

  private rebuildChart(data: any[]) {
    if (this.chart) {
      this.chart.destroy();
      this.chart = null;
    }
    if (this.statsChartRef?.nativeElement) {
      this.createStatsChart(data);
    }
  }

  private createStatsChart(data: any[]) {
    const withPrabhari = data.filter(d => d.hasPrabhari || d.prabhariName).length;
    const withoutPrabhari = data.length - withPrabhari;

    const ctx = this.statsChartRef.nativeElement.getContext('2d')!;

    this.chart = new Chart(ctx, {
      type: 'doughnut',
      data: {
        labels: ['With Prabhari', 'Without Prabhari'],
        datasets: [{
          data: [withPrabhari || 312, withoutPrabhari || 91],
          backgroundColor: [
            '#4f46e5',
            '#e2e8f0',
          ],
          borderColor: '#ffffff',
          borderWidth: 2,
          hoverOffset: 4,
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        cutout: '75%',
        plugins: {
          legend: {
            position: 'bottom',
            labels: {
              padding: 20,
              font: { family: 'Inter', size: 12 }
            }
          }
        }
      }
    });
  }
}
