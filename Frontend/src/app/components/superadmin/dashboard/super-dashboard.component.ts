import { Component, OnInit, AfterViewInit, ViewChild, ElementRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Chart, registerables } from 'chart.js';
import { Router } from '@angular/router';
import { LucideAngularModule } from 'lucide-angular';
import { AuthServiceService } from '../../../Services/Auth/auth.service';

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
  selector: 'app-super-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    LucideAngularModule
  ],
  templateUrl: './super-dashboard.component.html',
  styleUrl: './super-dashboard.component.css'
})
export class SuperDashboardComponent implements OnInit, AfterViewInit {
  @ViewChild('prabhariChart') prabhariChartRef!: ElementRef<HTMLCanvasElement>;

  currentDate = new Date();
  loadingCounts = true;
  userRole = '';

  ngOnInit() {
    this.authService.userRole$.subscribe(role => {
      this.userRole = role || '';
    });

    // Simulate loading for consistency
    setTimeout(() => {
      this.loadingCounts = false;
    }, 500);
  }

  statCards: StatCard[] = [
    {
      title: 'Vidhan Sabha',
      value: 403,
      change: '0',
      changeType: 'up',
      icon: 'landmark',
      gradient: 'linear-gradient(135deg, #f59e0b, #d97706)',
      route: '/superadmin/vidhansabha',
      description: 'Total constituencies'
    },

    {
      title: 'Total States',
      value: 1,
      change: '0',
      changeType: 'up',
      icon: 'map',
      gradient: 'linear-gradient(135deg, #0ea5e9, #0284c7)',
      route: '/superadmin/state',
      description: 'Operational states'
    },
    {
      title: 'Total Districts',
      value: 75,
      change: '+5',
      changeType: 'up',
      icon: 'navigation',
      gradient: 'linear-gradient(135deg, #22c55e, #16a34a)',
      route: '/superadmin/district',
      description: 'Active districts'
    },
    {
      title: 'Designation',
      value: 2,
      change: '+2',
      changeType: 'up',
      icon: 'user-cog',
      gradient: 'linear-gradient(135deg, #6366f1, #4f46e5)',
      route: '/superadmin/designation',
      description: 'Manage administrative roles'
    },
    {
      title: 'Samiti Members',
      value: '-',
      change: '+0',
      changeType: 'up',
      icon: 'users',
      gradient: 'linear-gradient(135deg, #ec4899, #db2777)',
      route: '/superadmin/state-member',
      description: 'Manage state members'
    }

  ];

  constructor(private router: Router, private authService: AuthServiceService) { }

  ngAfterViewInit() {
    this.createPrabhariChart();
  }

  navigateTo(route?: string) {
    if (route) {
      this.router.navigate([route]);
    }
  }

  private createPrabhariChart() {
    if (!this.prabhariChartRef || !this.prabhariChartRef.nativeElement) return;
    const ctx = this.prabhariChartRef.nativeElement.getContext('2d')!;

    new Chart(ctx, {
      type: 'doughnut',
      data: {
        labels: ['With Prabhari', 'Without Prabhari'],
        datasets: [{
          data: [312, 91],
          backgroundColor: [
            '#4f46e5', // Indigo
            '#e2e8f0', // Slate 200
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
