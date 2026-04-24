import { ApplicationConfig, provideZoneChangeDetection, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { provideAnimations } from '@angular/platform-browser/animations';
import { 
  LucideAngularModule, 
  LayoutDashboard, 
  Users, 
  Database, 
  Building, 
  Map, 
  Vote, 
  BookOpen, 
  BarChart3, 
  ShieldCheck, 
  ClipboardList, 
  Share2, 
  Calendar, 
  ShieldAlert, 
  Landmark, 
  MapPin, 
  Locate, 
  Navigation, 
  UserCog, 
  Search,
  ChevronLeft,
  ChevronRight,
  ChevronDown,
  LogOut,
  Bell,
  Settings,
  Plus
} from 'lucide-angular';

import { routes } from './app.routes';
import { loaderInterceptor } from './interceptors/loader/loader.interceptor';
import { authInterceptor } from './interceptors/auth.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(withInterceptors([loaderInterceptor, authInterceptor])),
    provideAnimations(),
    importProvidersFrom(
      ReactiveFormsModule,
      LucideAngularModule.pick({ 
        LayoutDashboard, 
        Users, 
        Database, 
        Building, 
        Map, 
        Vote, 
        BookOpen, 
        BarChart3, 
        ShieldCheck, 
        ClipboardList, 
        Share2, 
        Calendar, 
        ShieldAlert, 
        Landmark, 
        MapPin, 
        Locate, 
        Navigation, 
        UserCog, 
        Search,
        ChevronLeft,
        ChevronRight,
        ChevronDown,
        LogOut,
        Bell,
        Settings,
        Plus
      })
    )
  ]
};
