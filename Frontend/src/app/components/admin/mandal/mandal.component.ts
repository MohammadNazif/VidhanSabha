import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Validators } from '@angular/forms';
import { GenericTableComponent } from '../../shared/generic-table/generic-table.component';
import { TableAction, TableColumn, TableConfig } from '../../shared/generic-table/generic-table.types';
import { GenericModalButtonComponent } from '../../shared/generic-modal-form/generic-modal-button.component';
import { FormConfig, FormResult } from '../../shared/generic-modal-form/generic-form.types';

import { PageHeaderComponent } from '../../shared/page-header/page-header.component';
import { MandalService } from '../../../Services/mandal/mandal.service';


@Component({
  selector: 'app-mandal',
  standalone: true,
  imports: [CommonModule, PageHeaderComponent, GenericTableComponent, GenericModalButtonComponent],
  templateUrl: './mandal.component.html',
  styleUrl: './mandal.component.css'
})
export class MandalComponent implements OnInit {
  constructor(private mandalService: MandalService) { }

  ngOnInit() {
    this.loadMandals();
  }

  loadMandals() {
    this.mandalService.getAllMandals().subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.mandalList = response.data;
        } else if (Array.isArray(response)) {
          this.mandalList = response;
        } else if (response && Array.isArray(response.data)) {
          this.mandalList = response.data;
        } else {
          console.warn('Unexpected response format:', response);
          this.mandalList = [];
        }
      },
      error: (err) => {
        console.error('Error fetching mandals:', err);
      }
    });
  }

  addMandalConfig: FormConfig = {
    title: 'Add New Mandal',
    submitLabel: 'Create Mandal',
    fields: [
      {
        id: 'name',
        name: 'name',
        label: 'Mandal Name',
        type: 'text',
        placeholder: 'Enter mandal name',
        validations: [Validators.required],
        gridColSpan: 12
      }
    ]
  };

  mandalList: any[] = []; // Will be populated from API

  columns: TableColumn[] = [
    { key: 'name', label: 'Name', type: 'avatar', sortable: true, avatarFallbackKey: 'name' },

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
    console.log('Form submitted:', result);
    if (result.status) {
      this.mandalService.createMandal(result.data).subscribe({
        next: (response) => {
          console.log('Mandal created successfully:', response);
          alert('Mandal created successfully!');
          this.loadMandals();
        },
        error: (err) => {
          console.error('Error creating mandal:', err);
          alert('Failed to create Mandal. Please try again.');
        }
      });
    }
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
