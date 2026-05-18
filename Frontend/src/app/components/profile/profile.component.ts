import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BaseApiService } from '../../Services/common/base-api.service';
import { ToastService } from '../../Services/common/toast/toast.service';
import { PageHeaderComponent } from '../shared/page-header/page-header.component';
import { environment } from '../../../environments/environment';
import { AuthServiceService } from '../../Services/Auth/auth.service';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, PageHeaderComponent],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit {
  profileData: any = null;
  isLoading: boolean = false;
  isPasswordVisible: boolean = false;

  getProfileImage(): string {
    if (this.profileData && this.profileData.profile && this.profileData.profile !== 'null' && this.profileData.profile.trim() !== '') {
      const baseUrl = environment.apiUrl.replace('/api', '');
      return `${baseUrl}/${this.profileData.profile}`;
    }
    return '';
  }

  constructor(
    private baseApi: BaseApiService,
    private toast: ToastService,
    private authService: AuthServiceService
  ) {}

  ngOnInit(): void {
    this.loadProfile();
  }

  loadProfile(): void {
    // Subscribe to shared state updates
    this.authService.profileData$.subscribe(data => {
      if (data) {
        this.profileData = data;
      }
    });

    // Check cache
    const cachedData = this.authService.getProfileData();
    if (cachedData) {
      this.profileData = cachedData;
    } else {
      this.isLoading = true;
      this.baseApi.postCustom<any>('common/profile', {}).subscribe({
        next: (res) => {
          if (res.isSuccess) {
            this.authService.setProfileData(res.data);
          } else {
            this.toast.showError('Error', res.title || 'Failed to load profile');
          }
          this.isLoading = false;
        },
        error: (err: any) => {
          this.toast.showError('Error', 'An error occurred while fetching profile');
          this.isLoading = false;
          console.error(err);
        }
      });
    }
  }
}
