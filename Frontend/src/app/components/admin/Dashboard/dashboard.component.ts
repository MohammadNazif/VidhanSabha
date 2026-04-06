import { Component, OnInit, AfterViewInit, ViewChild, ElementRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Chart, registerables } from 'chart.js';

Chart.register(...registerables);

interface StatCard {
  title: string;
  value: string;
  change: string;
  changeType: 'up' | 'down';
  icon: string;
  gradient: string;
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
  imports: [CommonModule],
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
    {
      title: 'Total Members',
      value: '543',
      change: '+12',
      changeType: 'up',
      icon: '👥',
      gradient: 'linear-gradient(135deg, #6366f1, #8b5cf6)'
    },
    {
      title: 'Active Sessions',
      value: '24',
      change: '+3',
      changeType: 'up',
      icon: '📅',
      gradient: 'linear-gradient(135deg, #06b6d4, #3b82f6)'
    },
    {
      title: 'Bills Passed',
      value: '187',
      change: '+28',
      changeType: 'up',
      icon: '📋',
      gradient: 'linear-gradient(135deg, #10b981, #059669)'
    },
    {
      title: 'Pending Bills',
      value: '43',
      change: '-5',
      changeType: 'down',
      icon: '⏳',
      gradient: 'linear-gradient(135deg, #f59e0b, #ef4444)'
    }
  ];

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

  ngOnInit() {}

  ngAfterViewInit() {
    this.createRevenueChart();
    this.createSessionChart();
    this.createPartyChart();
    this.createAttendanceChart();
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

  private createSessionChart() {
    const ctx = this.sessionChartRef.nativeElement.getContext('2d')!;

    new Chart(ctx, {
      type: 'bar',
      data: {
        labels: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
        datasets: [{
          label: 'Hours in Session',
          data: [6.5, 8.2, 7.8, 9.1, 5.5, 3.2],
          backgroundColor: [
            'rgba(29, 78, 216, 0.8)',
            'rgba(234, 88, 12, 0.8)',
            'rgba(22, 163, 74, 0.8)',
            'rgba(2, 132, 199, 0.8)',
            'rgba(147, 51, 234, 0.8)',
            'rgba(71, 85, 105, 0.8)',
          ],
          borderColor: [
            '#1d4ed8',
            '#ea580c',
            '#16a34a',
            '#0284c7',
            '#9333ea',
            '#475569',
          ],
          borderWidth: 1,
          borderRadius: 6,
          borderSkipped: false,
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: { display: false },
          tooltip: {
            backgroundColor: '#ffffff',
            titleColor: '#0f172a',
            bodyColor: '#475569',
            borderColor: '#e2e8f0',
            borderWidth: 1,
            cornerRadius: 8,
            padding: 12,
          }
        },
        scales: {
          x: {
            grid: { display: false },
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

  private createAttendanceChart() {
    const ctx = this.attendanceChartRef.nativeElement.getContext('2d')!;

    const gradient = ctx.createLinearGradient(0, 0, 0, 200);
    gradient.addColorStop(0, 'rgba(22, 163, 74, 0.2)');
    gradient.addColorStop(1, 'rgba(22, 163, 74, 0.0)');

    new Chart(ctx, {
      type: 'line',
      data: {
        labels: ['W1', 'W2', 'W3', 'W4', 'W5', 'W6', 'W7', 'W8'],
        datasets: [{
          label: 'Attendance %',
          data: [82, 86, 79, 91, 88, 94, 87, 92],
          borderColor: '#16a34a',
          backgroundColor: gradient,
          borderWidth: 2.5,
          fill: true,
          tension: 0.4,
          pointBackgroundColor: '#16a34a',
          pointBorderColor: '#ffffff',
          pointBorderWidth: 2,
          pointRadius: 4,
          pointHoverRadius: 7,
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: { display: false },
          tooltip: {
            backgroundColor: '#ffffff',
            titleColor: '#0f172a',
            bodyColor: '#475569',
            borderColor: '#e2e8f0',
            borderWidth: 1,
            cornerRadius: 8,
            padding: 12,
          }
        },
        scales: {
          x: {
            grid: { display: false },
            ticks: { color: '#64748b', font: { family: 'Inter', size: 10 } },
            border: { display: false }
          },
          y: {
            grid: { color: 'rgba(0,0,0,0.05)' },
            ticks: { color: '#64748b', font: { family: 'Inter', size: 10 } },
            border: { display: false },
            min: 70,
            max: 100
          }
        }
      }
    });
  }
}
