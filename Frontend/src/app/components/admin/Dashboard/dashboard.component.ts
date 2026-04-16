import { Component, OnInit, AfterViewInit, ViewChild, ElementRef } from '@angular/core';
import { CommonModule } from '@angular/common';
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
  Database 
} from 'lucide-angular';

import { Router } from '@angular/router';
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

  currentDate = new Date();

  statCards: StatCard[] = [
    { title: 'Mandal', value: 5, icon: 'building', gradient: 'linear-gradient(135deg, #6366f1, #4f46e5)', change: '+0%', changeType: 'up', route: '/mandal' },
    { title: 'Sector', value: 83, icon: 'map', gradient: 'linear-gradient(135deg, #0ea5e9, #0284c7)', change: '+0%', changeType: 'up', route: '/sector' },
    { title: 'Booth', value: 419, icon: 'vote', gradient: 'linear-gradient(135deg, #22c55e, #16a34a)', change: '+0%', changeType: 'up', route: '/booth' },
    { title: 'PannaPramukh', value: 2, icon: 'book-open', gradient: 'linear-gradient(135deg, #f59e0b, #d97706)', change: '+0%', changeType: 'up', route: '/panna-pramukh' },
    { title: 'Sahmat', value: 0, icon: 'shield-check', gradient: 'linear-gradient(135deg, #10b981, #059669)', change: '+0%', changeType: 'up', route: '/sahmat-list' },
    { title: 'Asahmat', value: 0, icon: 'shield-alert', gradient: 'linear-gradient(135deg, #ef4444, #dc2626)', change: '+0%', changeType: 'down', route: '/asahmat-list' },
    { title: 'Activities', value: 1, icon: 'calendar', gradient: 'linear-gradient(135deg, #8b5cf6, #7c3aed)', change: '+0%', changeType: 'up', route: '/activity' },
    { title: 'Pravasi', value: 3, icon: 'users', gradient: 'linear-gradient(135deg, #06b6d4, #0891b2)', change: '+0%', changeType: 'up', route: '/pravasi-voter' },
    { title: 'New Voters', value: 1, icon: 'plus', gradient: 'linear-gradient(135deg, #84cc16, #65a30d)', change: '+0%', changeType: 'up', route: '/new-voter-list' },
    { title: 'Double Voter', value: 1, icon: 'shield-alert', gradient: 'linear-gradient(135deg, #f97316, #ea580c)', change: '+0%', changeType: 'down', route: '/double-voter-list' },
    { title: 'Prabhavshali Vyakti', value: 1, icon: 'users', gradient: 'linear-gradient(135deg, #eab308, #ca8a04)', change: '+0%', changeType: 'up', route: '/prabhavshali-vyakt' },
    { title: 'Block', value: 1, icon: 'building', gradient: 'linear-gradient(135deg, #64748b, #475569)', change: '+0%', changeType: 'up', route: '/block' },
    { title: 'BDC', value: 126, icon: 'database', gradient: 'linear-gradient(135deg, #14b8a6, #0f766e)', change: '+0%', changeType: 'up', route: '/bdc' },
    { title: 'Influencer Person', value: 0, icon: 'users', gradient: 'linear-gradient(135deg, #ec4899, #db2777)', change: '+0%', changeType: 'up', route: '/influencer-person' }
  ];

  constructor(private router: Router, private authService: AuthServiceService) { }

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
      console.log('Dashboard detected role:', r);
      if (r === 'SUPERADMIN') {
        this.router.navigate(['/superadmin/dashboard']);
      } else if (r === 'STATEPRABHARI') {
        this.router.navigate(['/state-prabhari/dashboard']);
      }
    });
  }

  ngAfterViewInit() {
    this.createRevenueChart();
    this.createPartyChart();

  }

  private createRevenueChart() {
    const ctx = this.revenueChartRef.nativeElement.getContext('2d')!;

    const gradient = ctx.createLinearGradient(0, 0, 0, 300);
    gradient.addColorStop(0, 'rgba(29, 78, 216, 0.2)'); // Navy blue
    gradient.addColorStop(1, 'rgba(29, 78, 216, 0.0)');

    const gradient2 = ctx.createLinearGradient(0, 0, 0, 300);
    gradient2.addColorStop(0, 'rgba(22, 163, 74, 0.2)'); // Green
    gradient2.addColorStop(1, 'rgba(22, 163, 74, 0.0)');

    new Chart(ctx, {
      type: 'line',
      data: {
        labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
        datasets: [
          {
            label: 'Bills Introduced',
            data: [12, 19, 15, 25, 22, 30, 28, 35, 32, 40, 38, 45],
            borderColor: '#1d4ed8',
            backgroundColor: gradient,
            borderWidth: 2.5,
            fill: true,
            tension: 0.4,
            pointBackgroundColor: '#1d4ed8',
            pointBorderColor: '#ffffff',
            pointBorderWidth: 2,
            pointRadius: 0,
            pointHoverRadius: 6,
          },
          {
            label: 'Bills Passed',
            data: [8, 14, 11, 18, 16, 24, 20, 28, 25, 32, 30, 38],
            borderColor: '#16a34a',
            backgroundColor: gradient2,
            borderWidth: 2.5,
            fill: true,
            tension: 0.4,
            pointBackgroundColor: '#16a34a',
            pointBorderColor: '#ffffff',
            pointBorderWidth: 2,
            pointRadius: 0,
            pointHoverRadius: 6,
          }
        ]
      },
      options: {
        animation: {
          duration: 1000,
          easing: 'easeOutQuart',
          delay: 500
        },
        responsive: true,
        maintainAspectRatio: false,
        interaction: {
          intersect: false,
          mode: 'index',
        },
        plugins: {
          legend: {
            display: true,
            position: 'top',
            align: 'end',
            labels: {
              color: '#475569',
              font: { family: 'Inter', size: 11, weight: 500 },
              usePointStyle: true,
              pointStyle: 'circle',
              padding: 20,
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
            titleFont: { family: 'Inter', weight: 600 },
            bodyFont: { family: 'Inter' },
            boxPadding: 4
          }
        },
        scales: {
          x: {
            grid: { color: 'rgba(0,0,0,0.05)' },
            ticks: { color: '#64748b', font: { family: 'Inter', size: 11 } },
            border: { display: false }
          },
          y: {
            grid: { color: 'rgba(0,0,0,0.05)' },
            ticks: { color: '#64748b', font: { family: 'Inter', size: 11 } },
            border: { display: false },
            beginAtZero: true
          }
        }
      }
    });
  }


  private createPartyChart() {
    const ctx = this.partyChartRef.nativeElement.getContext('2d')!;

    new Chart(ctx, {
      type: 'doughnut',
      data: {
        labels: ['BJP', 'INC', 'AAP', 'TMC', 'DMK', 'Others'],
        datasets: [{
          data: [303, 52, 28, 23, 22, 115],
          backgroundColor: [
            '#f97316', // Saffron
            '#0284c7', // Sky Blue
            '#06b6d4', // Cyan
            '#16a34a', // Green
            '#db2777', // Rose
            '#64748b', // Slate
          ],
          borderColor: '#ffffff',
          borderWidth: 2,
          hoverBorderColor: '#ffffff',
          hoverOffset: 6,
        }]
      },
      options: {
        animation: {
          duration: 1200,
          easing: 'easeOutBounce',
          delay: 500
        },
        responsive: true,
        maintainAspectRatio: false,
        cutout: '72%',
        plugins: {
          legend: {
            display: true,
            position: 'bottom',
            labels: {
              color: '#475569',
              font: { family: 'Inter', size: 11, weight: 500 },
              usePointStyle: true,
              pointStyle: 'circle',
              padding: 16,
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

}



