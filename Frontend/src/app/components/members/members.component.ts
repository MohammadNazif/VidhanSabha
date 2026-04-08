import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Validators } from '@angular/forms';
import { GenericTableComponent } from '../shared/generic-table/generic-table.component';
import { TableColumn, TableConfig, TableAction, BadgeVariant } from '../shared/generic-table/generic-table.types';
import { GenericModalButtonComponent } from '../shared/generic-modal-form/generic-modal-button.component';
import { FormConfig, FormResult } from '../shared/generic-modal-form/generic-form.types';
import { ToastService } from '../../Services/common/toast/toast.service';

@Component({
  selector: 'app-members',
  standalone: true,
  imports: [CommonModule, GenericTableComponent, GenericModalButtonComponent],
  templateUrl: './members.component.html',
  styleUrl: './members.component.css'
})
export class MembersComponent {
  constructor(private toastService: ToastService) { }

  addMemberConfig: FormConfig = {
    title: 'Register New Member',
    submitLabel: 'Register Member',
    fields: [
      { id: 'profile', name: 'profile', label: 'Photo', type: 'file', width: 'full' },
      { id: 'firstName', name: 'firstName', label: 'First Name', type: 'text', placeholder: 'Enter first name', validations: [Validators.required] },
      { id: 'lastName', name: 'lastName', label: 'Last Name', type: 'text', placeholder: 'Enter last name', validations: [Validators.required] },
      {
        id: 'party',
        name: 'party',
        label: 'Political Party',
        type: 'select',
        options: [
          { value: 'BJP', label: 'Bharatiya Janata Party' },
          { value: 'INC', label: 'Indian National Congress' },
          { value: 'AAP', label: 'Aam Aadmi Party' },
          { value: 'TMC', label: 'Trinamool Congress' },
          { value: 'OTH', label: 'Others (Independent)' }
        ],
        validations: [Validators.required]
      },
      {
        id: 'hasBio',
        name: 'hasBio',
        label: 'Include Biographic Details?',
        type: 'select',
        options: [
          { value: 'yes', label: 'Yes - I want to provide details' },
          { value: 'no', label: 'No - Skip for now' }
        ],
        defaultValue: 'no'
      },
      // Fields when Yes
      {
        id: 'profession',
        name: 'profession',
        label: 'Current Profession',
        type: 'text',
        placeholder: 'e.g. Lawyer, Doctor',
        visibleIf: { field: 'hasBio', operator: '==', value: 'yes' }
      },
      {
        id: 'education',
        name: 'education',
        label: 'Education Level',
        type: 'select',
        options: [
          { value: 'grad', label: 'Graduate' },
          { value: 'postgrad', label: 'Post Graduate' },
          { value: 'phd', label: 'PhD' }
        ],
        visibleIf: { field: 'hasBio', operator: '==', value: 'yes' }
      },
      // Fields when No
      {
        id: 'reason',
        name: 'reason',
        label: 'Reason for skipping',
        type: 'textarea',
        placeholder: 'Optional: why skip bio?',
        width: 'full',
        visibleIf: { field: 'hasBio', operator: '==', value: 'no' }
      },
      { id: 'email', name: 'email', label: 'Email ID', type: 'email', placeholder: 'member@vidhansabha.gov' },
      { id: 'joinDate', name: 'joinDate', label: 'Election Date', type: 'date', defaultValue: new Date().toISOString().split('T')[0] },
      { id: 'bio', name: 'bio', label: 'Detailed Biography', type: 'textarea', width: 'full', placeholder: 'Enter biography here...', visibleIf: { field: 'hasBio', operator: '==', value: 'yes' } }
    ]
  };

  handleNewMember(result: FormResult) {
    console.log('New Member Data:', result.data);
    if (result.files?.['profile']) {
      console.log('Profile photo uploaded:', result.files['profile'].name);
    }

    // Simulating adding to table
    const newId = this.membersData.length + 1;
    const newMember = {
      id: newId,
      name: `${result.data.firstName} ${result.data.lastName}`,
      party: result.data.party,
      constituency: result.data.constituency || result.data.otherPartyName || 'N/A',
      attendance: 0,
      status: 'Active',
      joinDate: result.data.joinDate
    };

    this.membersData = [newMember, ...this.membersData];
    this.toastService.showSuccess('Success', 'Member registered successfully!');
  }
  columns: TableColumn[] = [
    { key: 'name', label: 'Name', type: 'avatar', sortable: true, avatarFallbackKey: 'name' },
    { key: 'party', label: 'Party', type: 'badge', sortable: true, badgeVariant: this.getPartyBadge },
    { key: 'constituency', label: 'Constituency', type: 'text', sortable: true },
    { key: 'attendance', label: 'Attendance', type: 'progress', sortable: true, progressMax: 100 },
    { key: 'status', label: 'Status', type: 'badge', sortable: true, badgeVariant: this.getStatusBadge },
    { key: 'joinDate', label: 'Elected Date', type: 'date', sortable: true },
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

  membersData = [
    { id: 1, name: 'Rajesh Kumar', party: 'BJP', constituency: 'Varanasi', attendance: 96, status: 'Active', joinDate: '2024-06-04' },
    { id: 2, name: 'Priya Sharma', party: 'INC', constituency: 'Raebareli', attendance: 85, status: 'Active', joinDate: '2024-06-04' },
    { id: 3, name: 'Amit Patel', party: 'AAP', constituency: 'New Delhi', attendance: 92, status: 'Active', joinDate: '2024-06-04' },
    { id: 4, name: 'Sneha Reddy', party: 'TMC', constituency: 'Kolkata South', attendance: 78, status: 'Warning', joinDate: '2024-06-04' },
    { id: 5, name: 'Vikram Singh', party: 'BJP', constituency: 'Lucknow', attendance: 99, status: 'Active', joinDate: '2024-06-04' },
    { id: 6, name: 'Arjun Das', party: 'INC', constituency: 'Amethi', attendance: 45, status: 'Inactive', joinDate: '2024-06-04' },
    { id: 7, name: 'Meera Bai', party: 'DMK', constituency: 'Chennai Central', attendance: 88, status: 'Active', joinDate: '2024-06-04' },
    { id: 8, name: 'Kiran Rao', party: 'AAP', constituency: 'Chandni Chowk', attendance: 91, status: 'Active', joinDate: '2024-06-04' },
  ];

  getPartyBadge(party: string): BadgeVariant {
    switch (party) {
      case 'BJP': return 'warning';
      case 'INC': return 'info';
      case 'AAP': return 'success';
      case 'TMC': return 'danger';
      case 'DMK': return 'danger';
      default: return 'default';
    }
  }

  getStatusBadge(status: string): BadgeVariant {
    switch (status) {
      case 'Active': return 'success';
      case 'Warning': return 'warning';
      case 'Inactive': return 'danger';
      default: return 'default';
    }
  }

  handleAction(event: any) {
    console.log('Action clicked:', event);
    this.toastService.showWarning('Action Selected', `Action ${event.action.id} clicked for ${event.row.name}`);
  }

  handleSelection(selected: any[]) {
    console.log('Selected rows:', selected);
  }

  handleExport(format: string) {
    if (!format) return;

    // In a real application, you would generate and download the file here
    console.log(`Generating ${format.toUpperCase()} export for ${this.membersData.length} records...`);
    this.toastService.showSuccess('Export Started', `Successfully generated ${format.toUpperCase()} export!`);
  }
}
