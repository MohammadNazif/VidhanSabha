import { Component, EventEmitter, Input, Output, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormConfig, FormResult } from './generic-form.types';
import { DynamicFormModalComponent } from './dynamic-form-modal.component';

@Component({
  selector: 'app-generic-modal-button',
  standalone: true,
  imports: [CommonModule, DynamicFormModalComponent],
  template: `
    <button *ngIf="!hideButton"
            (click)="openModal()" 
            [class]="buttonClass"
            class="flex items-center gap-2 px-3.5 py-1.5 rounded-lg font-medium text-sm transition-all duration-200 active:scale-95 shadow-sm hover:shadow-md"
    >
      <span *ngIf="icon" class="text-base">{{ icon }}</span>
      <span>{{ label }}</span>
    </button>

    <app-dynamic-form-modal 
      *ngIf="isModalOpen"
      [config]="config"
      [initialData]="initialData"
      (close)="closeModal()"
      (submitForm)="handleSubmit($event)"
    ></app-dynamic-form-modal>
  `,
  styles: [`
    :host {
      display: inline-block;
    }
  `]
})
export class GenericModalButtonComponent {
  @Input({ required: true }) config!: FormConfig;
  @Input() label: string = 'Open Form';
  @Input() icon?: string;
  @Input() variant: 'primary' | 'secondary' | 'danger' | 'success' | 'outline' = 'primary';
  @Input() hideButton = false;

  @Output() formSubmit = new EventEmitter<FormResult>();

  isModalOpen = false;
  @Input() initialData: any;

  constructor(private cdr: ChangeDetectorRef) { }

  get buttonClass(): string {
    const base = '';
    const variants = {
      primary: 'bg-blue-600 text-white hover:bg-blue-700',
      secondary: 'bg-gray-600 text-white hover:bg-gray-700',
      danger: 'bg-red-600 text-white hover:bg-red-700',
      success: 'bg-green-600 text-white hover:bg-green-700',
      outline: 'bg-white text-gray-700 border border-gray-200 hover:bg-gray-50'
    };
    return variants[this.variant] || variants.primary;
  }

  openModal(data?: any): void {
    if (data !== undefined) {
      this.initialData = data;
    }
    console.log('Modal Opening Data:', this.initialData);
    this.isModalOpen = true;
    this.cdr.detectChanges();
    // Prevent body scrolling when modal is open
    document.body.style.overflow = 'hidden';
  }

  closeModal(): void {
    this.isModalOpen = false;
    document.body.style.overflow = 'auto';
  }

  handleSubmit(result: FormResult): void {
    this.formSubmit.emit(result);
    this.closeModal();
  }
}
