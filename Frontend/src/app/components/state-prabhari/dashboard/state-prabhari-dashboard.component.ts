import { Component, OnInit, AfterViewInit, ViewChild, ElementRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Chart, registerables } from 'chart.js';
import { Router } from '@angular/router';
import { LucideAngularModule } from 'lucide-angular';
import { AuthServiceService } from '../../../Services/Auth/auth.service';
import { VidhanSabhaCountService } from '../../../Services/Admin/vidhansabha-count/vidhansabha-count.service';

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
  private chart: Chart | null = null;

  statCards: StatCard[] = [
    {
      title: 'Vidhan Sabha',
      value: '-',
      change: '0',
      changeType: 'up',
      icon: 'landmark',
      gradient: 'linear-gradient(135deg, #f59e0b, #d97706)',
      route: '/superadmin/vidhansabha',
      description: 'Total constituencies'
    },
    {
      title: 'Districts',
      value: 75,
      change: '+5',
      changeType: 'up',
      icon: 'navigation',
      gradient: 'linear-gradient(135deg, #0ea5e9, #0284c7)',
      route: '/superadmin/district',
      description: 'Active districts'
    },
    {
      title: 'Designations',
      value: 12,
      change: '+2',
      changeType: 'up',
      icon: 'user-cog',
      gradient: 'linear-gradient(135deg, #6366f1, #4f46e5)',
      route: '/superadmin/designation',
      description: 'Manage roles'
    },
    {
      title: 'State Prabhari',
      value: 1,
      change: '0',
      changeType: 'up',
      icon: 'clipboard-list',
      gradient: 'linear-gradient(135deg, #22c55e, #16a34a)',
      route: '/superadmin/state-prabhari',
      description: 'Assigned states'
    }
  ];

  quickActions = [
    { title: 'Manage Vidhan Sabha', description: 'Add or update constituencies', icon: 'landmark', route: '/superadmin/vidhansabha', gradient: 'linear-gradient(135deg, #f59e0b, #d97706)' },
    { title: 'Update Designations', description: 'Manage administrative roles', icon: 'user-cog', route: '/superadmin/designation', gradient: 'linear-gradient(135deg, #6366f1, #4f46e5)' },
    { title: 'Review Districts', description: 'View assigned district details', icon: 'navigation', route: '/superadmin/district', gradient: 'linear-gradient(135deg, #0ea5e9, #0284c7)' },
    { title: 'State Prabhari List', description: 'View all state prabharis', icon: 'clipboard-list', route: '/superadmin/state-prabhari', gradient: 'linear-gradient(135deg, #22c55e, #16a34a)' }
  ];

  constructor(
    private router: Router,
    private authService: AuthServiceService,
    private vidhanSabhaCountService: VidhanSabhaCountService
  ) { }

  ngOnInit() {
    this.authService.userId$.subscribe(userId => {
      console.log('[StatePrabhariDashboard] userId:', userId);
      if (userId) {
        this.loadVidhanSabhaData(userId);
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
