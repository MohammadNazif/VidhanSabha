import { Component, EventEmitter, Input, OnInit, Output, OnDestroy, HostListener, ViewChild, ElementRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormArray, ReactiveFormsModule, Validators } from '@angular/forms';
import { FormConfig, FormField, FormResult, DropdownOption } from './generic-form.types';
import { FormDataService } from './form-data.service';
import { Subscription, isObservable, of, Observable } from 'rxjs';
import { AuthServiceService } from '../../../Services/Auth/auth.service';

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
  @ViewChild('scrollContainer') scrollContainer!: ElementRef;

  form!: FormGroup;
  fieldOptions: { [key: string]: DropdownOption[] } = {};
  fieldVisibility: { [key: string]: boolean } = {};
  fileData: { [key: string]: any } = {};
  previews: { [key: string]: any } = {};

  private subscriptions: Subscription = new Subscription();
  private isInitializing = true;
  activeMultiDropdown: string | null = null;
  dropdownPosition: { top: number; left: number; width: number } = { top: 0, left: 0, width: 0 };

  constructor(
    private fb: FormBuilder,
    private formDataService: FormDataService,
    private authService: AuthServiceService
  ) { }

  ngOnInit(): void {
    this.handleBoothSanyojakRole();
    this.createForm();
    this.initializeOptions();
    this.setupFieldInteractions();
    this.updateCascadingOptions();

    // Disable reset logic for first 500ms to allow APIs to load
    setTimeout(() => {
      this.isInitializing = false;
      // Re-trigger cascading once for BoothSanyojak if needed
      this.updateCascadingOptions();
    }, 500);
  }

  private handleBoothSanyojakRole(): void {
    const role = (this.authService.getRole() || '').toUpperCase().trim();
    if (role === 'BOOTHSANYOJAK') {
      const boothId = this.authService.getBoothId();
      if (boothId) {
        this.config.fields.forEach(field => {
          if (field.id === 'boothId') {
            field.defaultValue = boothId;
            field.type = 'hidden';
            this.fieldVisibility[field.id] = false;
          }
        });
      }
    }
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  private createForm(): void {
    const group: any = {};

    this.config.fields.forEach(field => {
      if (field.type === 'form-array') {
        const array: any = this.fb.array([]);
        const initialArrayData = this.initialData?.[field.id] || (field.defaultValue ? [field.defaultValue] : [{}]);
        if (Array.isArray(initialArrayData)) {
          initialArrayData.forEach(item => {
            array.push(this.createGroupFromFields(field.subFields || [], item));
          });
        } else {
          array.push(this.createGroupFromFields(field.subFields || [], {}));
        }
        group[field.id] = array;
      } else {
        const defaultValue = field.multiple ? [] : (field.defaultValue ?? '');
        const value = (this.initialData && this.initialData[field.id] !== undefined)
          ? (field.multiple && !Array.isArray(this.initialData[field.id])
            ? [this.initialData[field.id]]
            : this.initialData[field.id])
          : defaultValue;

        group[field.id] = [{ value, disabled: !!(field.disabledOnEdit && this.initialData?.id) }, field.validations || []];
      }
      this.fieldVisibility[field.id] = !field.visibleIf;
    });

    this.form = this.fb.group(group);
  }

  private createGroupFromFields(fields: FormField[], data: any): FormGroup {
    const group: any = {};
    fields.forEach(field => {
      const value = data[field.id] !== undefined ? data[field.id] : (field.defaultValue ?? '');
      group[field.id] = [value, field.validations || []];
    });
    return this.fb.group(group);
  }

  private initializeOptions(): void {
    const processFields = (fields: FormField[]) => {
      fields.forEach(field => {
        if (field.type === 'select') {
          const apiUrl = field.apiUrl;
          if (apiUrl) {
            const url = typeof apiUrl === 'function' ? apiUrl(this.form.value) : apiUrl;
            this.subscriptions.add(
              this.formDataService.getOptionsFromApi(url, field.apiMapper, this.form.value).subscribe(options => {
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
        if (field.subFields) {
          processFields(field.subFields);
        }
      });
    };

    processFields(this.config.fields);
  }

  private setupFieldInteractions(): void {
    const hasDependencies = this.config.fields.some(f => f.visibleIf || f.dependsOn);

    if (!hasDependencies) return;

    // Initial check
    this.updateVisibility();
    this.updateCascadingOptions();

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
      const control = this.form.get(field.id);

      if (isVisible) {
        if (control?.disabled) control.enable({ emitEvent: false });
      } else {
        if (control?.enabled) {
          control.disable({ emitEvent: false });
          control.setValue('', { emitEvent: false });
        }
      }
    });
  }

  private updateCascadingOptions(): void {
    const processFields = (fields: FormField[], parentGroup: FormGroup | any = this.form) => {
      fields.forEach(field => {
        // Scenario A/B: Select dependency
        if (field.dependsOn) {
          const parentValue = parentGroup.get(field.dependsOn)?.value;

          if (field.optionsMap) {
            const newOptions = parentValue ? (field.optionsMap[parentValue] || []) : [];
            this.fieldOptions[field.id] = newOptions;
          } else if (field.apiUrl && parentValue) {
            const url = typeof field.apiUrl === 'function' ? field.apiUrl(parentValue) : field.apiUrl;
            if (this.lastApiUrl[field.id] !== url) {
              this.lastApiUrl[field.id] = url;
              this.subscriptions.add(
                this.formDataService.getOptionsFromApi(url, field.apiMapper, this.form.value).subscribe(options => {
                  this.fieldOptions[field.id] = options;
                  if (field.type === 'text' || field.type === 'textarea') {
                    const newValue = options.length > 0 ? options[0].value : '';
                    this.form.get(field.id)?.setValue(newValue);
                  } else {
                    this.handleChildValueReset(field.id, options);
                  }
                })
              );
            }
          }
        }

        // Scenario C: Selection table dependency
        if (field.type === 'selection-table' && field.dependsOn) {
          const parentValues = parentGroup.get(field.dependsOn)?.value;
          if (Array.isArray(parentValues)) {
            const parentOptions = this.fieldOptions[field.dependsOn] || [];
            const currentTableData = parentGroup.get(field.id)?.value || [];
            const newTableData = parentValues.map(val => {
              const existing = currentTableData.find((row: any) => String(row.id) === String(val));
              const option = parentOptions.find(o => String(o.value) === String(val));
              return { id: val, name: option?.label || existing?.name || 'Unknown', anshik: existing ? existing.anshik : 'No' };
            });
            if (JSON.stringify(newTableData) !== JSON.stringify(currentTableData)) {
              parentGroup.get(field.id)?.setValue(newTableData, { emitEvent: false });
            }
          }
        }

        // Recurse into FormArrays
        if (field.type === 'form-array') {
          const array = parentGroup.get(field.id) as FormArray;
          if (array && field.subFields) {
            array.controls.forEach(control => {
              if (control instanceof FormGroup) {
                processFields(field.subFields || [], control);
              }
            });
          }
        }
      });
    };

    processFields(this.config.fields);
  }

  setTableCellValue(fieldId: string, rowId: any, key: string, value: any): void {
    const control = this.form.get(fieldId);
    const currentData = control?.value || [];
    const newData = currentData.map((row: any) => {
      if (String(row.id) === String(rowId)) {
        return { ...row, [key]: value };
      }
      return row;
    });

    control?.setValue(newData);
    control?.markAsDirty();
  }

  private handleChildValueReset(fieldId: string, newOptions: DropdownOption[]): void {
    if (this.isInitializing) return;

    const field = this.config.fields.find(f => f.id === fieldId);
    const currentValue = this.form.get(fieldId)?.value;

    if (!currentValue) return;

    if (field?.multiple && Array.isArray(currentValue)) {
      const validValues = currentValue.filter(val =>
        newOptions.some(o => String(o.value) == String(val))
      );
      if (validValues.length !== currentValue.length) {
        this.form.get(fieldId)?.setValue(validValues, { emitEvent: false });
      }
    } else if (!newOptions.find(o => String(o.value) == String(currentValue))) {
      this.form.get(fieldId)?.setValue('', { emitEvent: false });
    }
  }

  toggleMultiSelectOption(fieldId: string, value: any): void {
    const control = this.form.get(fieldId);
    const currentValues = Array.isArray(control?.value) ? [...control.value] : [];

    // Convert to string for consistent comparison if needed, but try to keep original for binding
    const index = currentValues.findIndex(v => String(v) === String(value));

    if (index > -1) {
      currentValues.splice(index, 1);
    } else {
      currentValues.push(value);
    }

    control?.setValue(currentValues);
    control?.markAsDirty();
    control?.markAsTouched();
  }

  isOptionSelected(fieldId: string, value: any): boolean {
    const currentValues = this.form.get(fieldId)?.value;
    if (!Array.isArray(currentValues)) return false;
    return currentValues.some(v => String(v) === String(value));
  }

  getSelectedLabels(fieldId: string): string {
    const currentValues = this.form.get(fieldId)?.value;
    if (!Array.isArray(currentValues) || currentValues.length === 0) return '';

    const options = this.fieldOptions[fieldId] || [];
    return currentValues
      .map(val => options.find(o => String(o.value) === String(val))?.label)
      .filter(l => l)
      .join(', ');
  }

  getOptionLabel(fieldId: string, value: any): string {
    const options = this.fieldOptions[fieldId] || [];
    const option = options.find(o => String(o.value) === String(value));
    return option ? option.label : 'Loading...';
  }

  closeActiveDropdown(): void {
    if (this.activeMultiDropdown) {
      this.activeMultiDropdown = null;
    }
  }

  toggleDropdown(fieldId: string, event: Event): void {
    event.stopPropagation();
    if (this.activeMultiDropdown === fieldId) {
      this.activeMultiDropdown = null;
    } else {
      this.activeMultiDropdown = fieldId;
      const trigger = (event.currentTarget as HTMLElement);
      const rect = trigger.getBoundingClientRect();
      this.dropdownPosition = {
        top: rect.bottom + 8,
        left: rect.left,
        width: rect.width
      };
    }
  }

  // To track last called URLs and avoid loops
  private lastApiUrl: { [key: string]: string } = {};

  onFileChange(event: any, fieldId: string): void {
    const field = this.config.fields.find(f => f.id === fieldId);
    const files = Array.from(event.target.files) as File[];
    
    if (files.length === 0) return;

    if (field?.multiple) {
      this.fileData[fieldId] = files;
      this.previews[fieldId] = [];
      
      files.forEach(file => {
        const reader = new FileReader();
        reader.onload = e => {
          if (!Array.isArray(this.previews[fieldId])) this.previews[fieldId] = [];
          this.previews[fieldId].push(reader.result);
        };
        reader.readAsDataURL(file);
      });
    } else {
      const file = files[0];
      this.fileData[fieldId] = file;
      const reader = new FileReader();
      reader.onload = e => this.previews[fieldId] = reader.result;
      reader.readAsDataURL(file);
    }
  }

  onSubmit(): void {
    if (this.form.valid) {
      this.submitForm.emit({
        data: this.form.getRawValue(),
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

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    if (this.activeMultiDropdown) {
      this.activeMultiDropdown = null;
    }
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

  isRequired(field: FormField | any): boolean {
    if (!field.validations) return false;
    return field.validations.some((v: any) => v.name === 'required' || v === Validators.required);
  }

  getFormArray(fieldId: string): FormArray {
    return this.form.get(fieldId) as FormArray;
  }

  addArrayItem(fieldId: string): void {
    const field = this.config.fields.find(f => f.id === fieldId);
    if (field && field.subFields) {
      this.getFormArray(fieldId).push(this.createGroupFromFields(field.subFields, {}));
    }
  }

  removeArrayItem(fieldId: string, index: number): void {
    const array = this.getFormArray(fieldId);
    if (array.length > 1) {
      array.removeAt(index);
    }
  }
}
