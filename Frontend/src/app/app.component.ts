import { Component, OnInit, HostListener } from '@angular/core';
import { RouterOutlet, Router, NavigationEnd } from '@angular/router';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { CommonModule } from '@angular/common';
import { LoaderComponent } from './components/shared/loader/loader.component';
import { ToastComponent } from './components/shared/toast/toast.component';
import { LoaderService } from './Services/loader/loader.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, SidebarComponent, CommonModule, LoaderComponent, ToastComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'VidhanSabha';
  sidebarCollapsed = false;
  isLoginPage = false;
  isMobile = false;

  constructor(private router: Router, public loaderService: LoaderService) {}

  ngOnInit() {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.isLoginPage = event.urlAfterRedirects.includes('/login');
      }
    });
    this.checkScreenSize();
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    this.checkScreenSize();
  }

  private checkScreenSize() {
    this.isMobile = typeof window !== 'undefined' ? window.innerWidth < 768 : false;
    if (this.isMobile) {
      this.sidebarCollapsed = true; // Auto-collapse on mobile
    }
  }

  onSidebarToggle(collapsed: boolean) {
    this.sidebarCollapsed = collapsed;
  }
}
