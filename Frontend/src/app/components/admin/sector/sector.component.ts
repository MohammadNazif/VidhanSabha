import { Component } from '@angular/core';
import { TableAction, TableColumn, TableConfig } from '../../shared/generic-table/generic-table.types';
import { Validators } from '@angular/forms';
import { FormConfig, FormResult } from '../../shared/generic-modal-form/generic-form.types';
import { CommonModule } from '@angular/common';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';
import { GenericModalButtonComponent } from '../../shared/generic-modal-form/generic-modal-button.component';

import { PageHeaderComponent } from '../../shared/page-header/page-header.component';

@Component({
  selector: 'app-sector',
  standalone: true,
  imports: [CommonModule, GenericTableComponent, GenericModalButtonComponent, PageHeaderComponent],
  templateUrl: './sector.component.html',
  styleUrl: './sector.component.css'
})
export class SectorComponent {

  addSectorConfig: FormConfig = {
    title: 'Register New Sector',
    submitLabel: 'Register Sector',
    fields: [
      {
        id: 'MandalId',
        name: 'Mandal',
        label: 'Mandal',
        type: 'select',
        options: [
          { label: 'Select Mandal', value: '' },
          { label: 'Mandawar', value: 'Mandawar' },
          { label: 'Mohammadpur Deomal', value: 'Mohammadpur Deomal' },
          { label: 'Jhalu', value: 'Jhalu' },
          { label: 'Bijnor City', value: 'Bijnor City' },
          { label: 'Adampur', value: 'Adampur' },
        ],
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'VillageId',
        name: 'Village',
        label: 'Village',
        type: 'select',
        options: [
          { label: 'Select Village', value: '' },
          { label: 'Mandawar', value: 'Mandawar' },
          { label: 'Mohammadpur Deomal', value: 'Mohammadpur Deomal' },
          { label: 'Jhalu', value: 'Jhalu' },
          { label: 'Bijnor City', value: 'Bijnor City' },
          { label: 'Adampur', value: 'Adampur' },
        ],
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'sectorName',
        name: 'sectorName',
        label: 'Sector Name',
        type: 'text',
        placeholder: 'Enter sector name',
        validations: [Validators.required],
        gridColSpan: 6
      },
      {
        id: 'isSanyojak',
        name: 'isSanyojak',
        label: 'Sector Sanyojak',
        type: 'select',
        placeholder: '-- Select Yes/No --',
        options: [
          { label: 'Yes', value: 'Yes' },
          { label: 'No', value: 'No' }
        ],
        validations: [Validators.required],
        gridColSpan: 6
      },
      // Conditional Fields (Visible if isSanyojak == 'Yes')
      {
        id: 'inchargeName',
        name: 'inchargeName',
        label: 'Incharge Name',
        type: 'text',
        placeholder: 'Enter incharge full name',
        visibleIf: { field: 'isSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 6
      },
      {
        id: 'age',
        name: 'age',
        label: 'Age',
        type: 'number',
        placeholder: 'Age',
        visibleIf: { field: 'isSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 2
      },
      {
        id: 'fatherName',
        name: 'fatherName',
        label: 'Father Name',
        type: 'text',
        placeholder: "Enter father's full name",
        visibleIf: { field: 'isSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 4
      },
      {
        id: 'category',
        name: 'category',
        label: 'Category',
        type: 'select',
        placeholder: '-- Select Category --',
        apiUrl: '/common/category',
        apiMapper: (data: any) => {
          if (Array.isArray(data?.data)) {
            return data.data.map((item: any) => ({
              value: item.id,
              label: item.name
            }));
          }
          return [];
        },
        visibleIf: { field: 'isSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 6
      },
      {
        id: 'caste',
        name: 'caste',
        label: 'Caste',
        type: 'select',
        placeholder: '-- Select Caste --',
        dependsOn: 'category',
        apiUrl: (catId: any) => `/common/subcaste?categoryId=${catId}`,
        apiMapper: (data: any) => {
          if (Array.isArray(data?.data)) {
            return data.data.map((item: any) => ({
              value: item.name,
              label: item.name
            }));
          }
          return [];
        },
        visibleIf: { field: 'isSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 6
      },
      {
        id: 'education',
        name: 'education',
        label: 'Education Level',
        type: 'text',
        placeholder: 'Enter highest education (e.g., B.A., 12th)',
        visibleIf: { field: 'isSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 6
      },
      {
        id: 'phone',
        name: 'phone',
        label: 'Phone Number',
        type: 'text',
        placeholder: 'Enter 10-digit mobile number',
        validations: [Validators.pattern('^[0-9]{10}$')],
        visibleIf: { field: 'isSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 6
      },
      {
        id: 'address',
        name: 'address',
        label: 'Address',
        type: 'textarea',
        placeholder: 'Enter full address with landmark',
        visibleIf: { field: 'isSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 12
      },
      {
        id: 'profileImage',
        name: 'profileImage',
        label: 'Profile Image',
        type: 'file',
        visibleIf: { field: 'isSanyojak', operator: '==', value: 'Yes' },
        gridColSpan: 12
      }
    ]
  };

  membersData = [
    { id: 1, name: 'Mandawar', },
    { id: 2, name: 'Mohammadpur Deomal' },
    { id: 3, name: 'Jhalu' },
    { id: 4, name: 'Bijnor City' },
    { id: 5, name: 'Adampur' },
  ];

  columns: TableColumn[] = [
    { key: 'mandal', label: 'Mandal', type: 'avatar', sortable: true, avatarFallbackKey: 'name' },
    { key: 'village', label: 'Village ', sortable: true },
    { key: 'sector', label: 'Sector', sortable: true },
    { key: 'sanyojak', label: 'Sector Sanyojak', sortable: true },
    { key: 'contact', label: 'Contact', sortable: true },
    { key: 'profileImage', label: 'Profile Image', sortable: true, avatarFallbackKey: 'name' },

  ];

  config: TableConfig = {
    selectable: false,
    filterable: true,
    paginated: true,
    defaultPageSize: 10,
    pageSizeOptions: [10, 20, 50],
    searchable: true,
    searchPlaceholder: 'Search members...',
    showRowNumbers: true,
    striped: true,
    hoverable: true
  };

  actions: TableAction[] = [
    { id: 'edit', label: '', variant: 'default', icon: '✏️' },
    { id: 'delete', label: '', variant: 'danger', icon: '🗑️' }
  ];



  handleNewMember(result: FormResult) {
    console.log('New Sector Data:', result.data);
    const newId = this.membersData.length + 1;
    const newSector = {
      id: newId,
      name: result.data.SectorName,
      mandal: result.data.MandalId,
      village: result.data.VillageId,
      sanyojak: result.data['Sector Sanyojak']
    };
    this.membersData = [newSector, ...this.membersData];
    alert('Sector registered successfully!');
  }

  handleAction(event: any) {
    console.log('Action clicked:', event);
    alert(`Action ${event.action.id} clicked for ${event.row.name}`);
  }

  handleSelection(selected: any[]) {
    console.log('Selected rows:', selected);
  }

  handleExport(format: string) {
    if (!format) return;
    console.log(`Generating ${format.toUpperCase()} export...`);
    alert(`Successfully generated ${format.toUpperCase()} export!`);
  }

}