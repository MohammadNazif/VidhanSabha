import { Component, EventEmitter, Input, OnInit, Output, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { FormConfig, FormField, FormResult, DropdownOption } from './generic-form.types';
import { FormDataService } from './form-data.service';
import { Subscription, isObservable, of, Observable } from 'rxjs';

@Component({
  selector: 'app-dynamic-form-modal',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './dynamic-form-modal.component.html',
  styleUrl: './dynamic-form-modal.component.css'
})
export class DynamicFormModalComponent implements OnInit, OnDestroy {
  @Input({ required: true }) config!: FormConfig;
  @Input() initialData: any; // Add this
  @Output() close = new EventEmitter<void>();
  @Output() submitForm = new EventEmitter<FormResult>();

  form!: FormGroup;
  fieldOptions: { [key: string]: DropdownOption[] } = {};
  fieldVisibility: { [key: string]: boolean } = {};
  fileData: { [key: string]: File } = {};
  previews: { [key: string]: string | ArrayBuffer | null } = {};

  private subscriptions: Subscription = new Subscription();

  constructor(
    private fb: FormBuilder,
    private formDataService: FormDataService
  ) { }

  ngOnInit(): void {
    this.createForm();
    this.initializeOptions();
    this.setupConditionalVisibility();
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  private createForm(): void {
    const group: any = {};

    this.config.fields.forEach(field => {
      const value = (this.initialData && this.initialData[field.id] !== undefined) 
        ? this.initialData[field.id] 
        : (field.defaultValue ?? '');
      group[field.id] = [value, field.validations || []];
      this.fieldVisibility[field.id] = !field.visibleIf;
    });

    this.form = this.fb.group(group);
  }

  private initializeOptions(): void {
    this.config.fields.forEach(field => {
      if (field.type === 'select') {
        const apiUrl = field.apiUrl;
        if (typeof apiUrl === 'string') {
          this.subscriptions.add(
            this.formDataService.getOptionsFromApi(apiUrl, field.apiMapper, this.form.value).subscribe(options => {
              this.fieldOptions[field.id] = options;
            })
          );
        } else if (field.options) {
          if (isObservable(field.options)) {
            this.subscriptions.add(
              field.options.subscribe(options => {
                this.fieldOptions[field.id] = options;
              })
            );
          } else {
            this.fieldOptions[field.id] = field.options;
          }
        }
      }
    });
  }

  private setupConditionalVisibility(): void {
    const dependentFields = this.config.fields.filter(f => f.visibleIf);

    if (dependentFields.length === 0) return;

    // Initial check
    this.updateVisibility();

    // Monitor changes
    this.subscriptions.add(
      this.form.valueChanges.subscribe(() => {
        this.updateVisibility();
        this.updateCascadingOptions();
      })
    );
  }

  private updateVisibility(): void {
    this.config.fields.forEach(field => {
      if (!field.visibleIf) {
        this.fieldVisibility[field.id] = true;
        return;
      }

      const { field: targetFieldId, operator, value: targetValue } = field.visibleIf;
      const actualValue = this.form.get(targetFieldId)?.value;

      let isVisible = false;
      switch (operator) {
        case '==':
          isVisible = actualValue === targetValue;
          break;
        case '!=':
          isVisible = actualValue !== targetValue;
          break;
        case 'contains':
          isVisible = Array.isArray(actualValue) ? actualValue.includes(targetValue) : actualValue?.toString().includes(targetValue);
          break;
        case 'notContains':
          isVisible = Array.isArray(actualValue) ? !actualValue.includes(targetValue) : !actualValue?.toString().includes(targetValue);
          break;
        case 'in':
          isVisible = Array.isArray(targetValue) ? targetValue.includes(actualValue) : false;
          break;
        case 'notIn':
          isVisible = Array.isArray(targetValue) ? !targetValue.includes(actualValue) : true;
          break;
      }

      this.fieldVisibility[field.id] = isVisible;

      // If hidden, clear value and validation error
      if (!isVisible && this.form.get(field.id)?.value) {
        this.form.get(field.id)?.setValue('', { emitEvent: false });
      }
    });
  }

  private updateCascadingOptions(): void {
    this.config.fields.forEach(field => {
      if (!field.dependsOn) return;

      const parentValue = this.form.get(field.dependsOn)?.value;

      // Scenario A: Static Mapping (optionsMap)
      if (field.optionsMap) {
        const newOptions = parentValue ? (field.optionsMap[parentValue] || []) : [];
        const currentOptions = this.fieldOptions[field.id];

        if (JSON.stringify(currentOptions) !== JSON.stringify(newOptions)) {
          this.fieldOptions[field.id] = newOptions;
          this.handleChildValueReset(field.id, newOptions);
        }
      }

      // Scenario B: API call (apiUrl is a function OR a string with dependency)
      else if (field.apiUrl && parentValue) {
        const url = typeof field.apiUrl === 'function' ? field.apiUrl(parentValue) : field.apiUrl;

        // Prevent redundant API calls if URL is same as last one (only for dynamic URLs)
        if (typeof field.apiUrl === 'function' && this.lastApiUrl[field.id] === url) return;
        this.lastApiUrl[field.id] = url;

        const formValues = this.form.value;
        this.subscriptions.add(
          this.formDataService.getOptionsFromApi(url, field.apiMapper, formValues).subscribe(options => {
            this.fieldOptions[field.id] = options;
            this.handleChildValueReset(field.id, options);
          })
        );
      }
    });
  }

  private handleChildValueReset(fieldId: string, newOptions: DropdownOption[]): void {
    const currentValue = this.form.get(fieldId)?.value;
    if (currentValue && !newOptions.find(o => o.value === currentValue)) {
      this.form.get(fieldId)?.setValue('', { emitEvent: false });
    }
  }

  // To track last called URLs and avoid loops
  private lastApiUrl: { [key: string]: string } = {};

  onFileChange(event: any, fieldId: string): void {
    const file = event.target.files[0];
    if (file) {
      this.fileData[fieldId] = file;

      // Create preview
      const reader = new FileReader();
      reader.onload = e => this.previews[fieldId] = reader.result;
      reader.readAsDataURL(file);
    }
  }

  onSubmit(): void {
    if (this.form.valid) {
      this.submitForm.emit({
        data: this.form.value,
        files: this.fileData,
        status: true
      });
    } else {
      this.form.markAllAsTouched();
    }
  }

  onCancel(): void {
    this.close.emit();
  }

  getFieldColumnClass(field: FormField): string {
    const colSpanMap: { [key: number]: string } = {
      1: 'md:col-span-1',
      2: 'md:col-span-2',
      3: 'md:col-span-3',
      4: 'md:col-span-4',
      5: 'md:col-span-5',
      6: 'md:col-span-6',
      7: 'md:col-span-7',
      8: 'md:col-span-8',
      9: 'md:col-span-9',
      10: 'md:col-span-10',
      11: 'md:col-span-11',
      12: 'md:col-span-12',
    };

    if (field.gridColSpan && colSpanMap[field.gridColSpan]) {
      return colSpanMap[field.gridColSpan];
    }

    if (field.width === 'full' || field.type === 'textarea' || field.type === 'file') {
      return 'md:col-span-12';
    }

    if (field.width === '1/2') {
      return 'md:col-span-6';
    }

    return 'md:col-span-6';
  }
}
