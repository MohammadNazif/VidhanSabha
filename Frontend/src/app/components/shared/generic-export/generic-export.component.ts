import { Component, EventEmitter, Input, Output, OnInit, OnDestroy, HostListener, ElementRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TableColumn } from '../generic-table/generic-table.types';

@Component({
  selector: 'app-generic-export',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div *ngIf="show" class="relative inline-block text-left" (click)="$event.stopPropagation()">
      <div class="relative group">
        <button type="button" [disabled]="isExporting" (click)="toggleDropdown()"
          class="flex items-center gap-2 pl-3 pr-8 py-1.5 rounded-lg text-[11px] font-bold text-slate-600 bg-white border border-slate-200 hover:border-blue-300 hover:bg-slate-50 transition-all shadow-sm outline-none focus:border-blue-500 focus:ring-2 focus:ring-blue-500/10 disabled:opacity-50 disabled:cursor-wait min-w-[110px]">
          
          <!-- Download Icon -->
          <svg class="w-3.5 h-3.5 text-slate-400 group-hover:text-blue-500" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2.5" d="M4 16v1a3 3 0 003 3h10a3 3 0 003-3v-1m-4-4l-4 4m0 0l-4-4m4 4V4"/>
          </svg>

          <span>{{ isExporting ? animatedLabel : label }}</span>

          <!-- Arrow Icon (Right) -->
          <div *ngIf="!isExporting" class="absolute right-2 top-1/2 -translate-y-1/2 text-slate-400">
            <svg class="h-3 w-3" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="3">
              <path stroke-linecap="round" stroke-linejoin="round" d="M19 9l-7 7-7-7" />
            </svg>
          </div>

          <!-- Bouncing Dots (Right) -->
          <div *ngIf="isExporting" class="absolute right-2 top-1/2 -translate-y-1/2">
            <div class="flex gap-0.5">
              <div class="w-1 h-1 bg-blue-500 rounded-full animate-bounce [animation-delay:-0.3s]"></div>
              <div class="w-1 h-1 bg-blue-500 rounded-full animate-bounce [animation-delay:-0.15s]"></div>
              <div class="w-1 h-1 bg-blue-500 rounded-full animate-bounce"></div>
            </div>
          </div>
        </button>

        <!-- Dropdown Menu (Perfectly Symmetrical Width) -->
        <div *ngIf="isOpen && !isExporting" 
          class="absolute right-0 z-50 mt-1 w-full origin-top-right rounded-lg bg-white shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none overflow-hidden">
          <div class="py-0.5">
            <button *ngFor="let option of options" (click)="selectOption(option.value)"
              class="flex items-center w-full px-3 py-1 text-[10px] font-semibold text-slate-600 hover:bg-slate-50 hover:text-blue-600 transition-colors gap-1.5 border-b border-slate-50 last:border-0">
              
              <svg *ngIf="option.value === 'pdf'" class="w-3.5 h-3.5 text-red-500" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M7 21h10a2 2 0 002-2V9.414a1 1 0 00-.293-.707l-5.414-5.414A1 1 0 0012.586 3H7a2 2 0 00-2 2v14a2 2 0 002 2z"/>
              </svg>
              <svg *ngIf="option.value === 'excel' || option.value === 'csv'" class="w-3.5 h-3.5 text-green-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 17v-2m3 2v-4m3 4v-6m2 10H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"/>
              </svg>

              {{ option.label.replace('Export ', '') }}
            </button>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    @keyframes bounce {
      0%, 100% { transform: translateY(0); }
      50% { transform: translateY(-2px); }
    }
    .animate-bounce {
      animation: bounce 0.6s infinite alternate;
    }
  `]
})
export class GenericExportComponent implements OnInit, OnDestroy {
  @Input() show: boolean = true;
  @Input() isExporting: boolean = false;
  @Input() label: string = 'Export';
  @Input() data: any[] = [];
  @Input() columns: TableColumn[] = [];
  @Input() fileName: string = 'Export';
  @Input() options: { label: string; value: string; icon?: string }[] = [
    { label: 'Export PDF', value: 'pdf' },
    { label: 'Export Excel', value: 'excel' }
  ];

  @Output() export = new EventEmitter<string>();

  isOpen = false;
  animatedLabel: string = 'Generating...';
  private dotInterval: any;
  private dots = 0;

  constructor(private elementRef: ElementRef) { }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    if (!this.elementRef.nativeElement.contains(event.target)) {
      this.isOpen = false;
    }
  }

  ngOnInit() {
    this.dotInterval = setInterval(() => {
      if (this.isExporting) {
        this.dots = (this.dots + 1) % 4;
        this.animatedLabel = 'Generating' + '.'.repeat(this.dots);
      } else {
        this.animatedLabel = 'Generating...';
        this.dots = 3;
      }
    }, 500);
  }

  ngOnDestroy() {
    if (this.dotInterval) {
      clearInterval(this.dotInterval);
    }
  }

  toggleDropdown() {
    this.isOpen = !this.isOpen;
  }

  selectOption(value: string) {
    if (value === 'csv') {
      this.downloadCSV();
    } else {
      this.export.emit(value);
    }
    this.isOpen = false;
  }

  private downloadCSV() {
    if (!this.data || this.data.length === 0) return;

    // Filter out columns that don't have a label or key (like actions)
    const validColumns = this.columns.filter(col => col.label && col.key);
    
    const headers = validColumns.map(col => col.label).join(',');
    const rows = this.data.map(row => {
      return validColumns.map(col => {
        const val = row[col.key] ?? '';
        return `"${String(val).replace(/"/g, '""')}"`;
      }).join(',');
    });

    const csvContent = [headers, ...rows].join('\n');
    const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
    const link = document.createElement('a');
    const url = URL.createObjectURL(blob);
    
    link.setAttribute('href', url);
    link.setAttribute('download', `${this.fileName}_${new Date().getTime()}.csv`);
    link.style.visibility = 'hidden';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  }
}
