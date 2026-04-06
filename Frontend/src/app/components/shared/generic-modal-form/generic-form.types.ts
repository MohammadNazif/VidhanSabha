import { ValidatorFn, AbstractControl } from '@angular/forms';
import { Observable } from 'rxjs';

export type FieldType =
  | 'text'
  | 'number'
  | 'email'
  | 'password'
  | 'select'
  | 'date'
  | 'file'
  | 'textarea'
  | 'checkbox'
  | 'radio';

export interface DropdownOption {
  value: any;
  label: string;
}

export interface ConditionalLogic {
  field: string;
  operator: '==' | '!=' | 'contains' | 'notContains' | 'in' | 'notIn';
  value: any | any[];
}

export interface FormField {
  id: string;
  label: string;
  name: string;
  type: FieldType;
  placeholder?: string;
  defaultValue?: any;
  options?: DropdownOption[] | Observable<DropdownOption[]>;
  validations?: ValidatorFn[];
  visibleIf?: ConditionalLogic;
  width?: '1/2' | 'full'; // Legacy: 1/2 -> col-span-6, full -> col-span-12
  gridColSpan?: number; // Modern: 1-12 span mapping
  apiUrl?: string | ((val: any) => string);
  apiMapper?: (data: any, formValues?: any) => DropdownOption[];
  dependsOn?: string; // ID of the field this field depends on
  optionsMap?: { [key: string]: DropdownOption[] }; // Map of parent values to child options
}

export interface FormConfig {
  title: string;
  fields: FormField[];
  submitLabel: string;
  cancelLabel?: string;
  modalSize?: 'sm' | 'md' | 'lg' | 'xl';
}

export interface FormResult {
  status: boolean;
  data: any;
  files?: { [key: string]: File };
}
