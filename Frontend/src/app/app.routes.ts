import { Routes } from '@angular/router';
import { DashboardComponent } from './components/admin/Dashboard/dashboard.component';
import { MembersComponent } from './components/members/members.component';
import { LoginComponent } from './components/auth/login/login.component';
import { MandalComponent } from './components/admin/mandal/mandal.component';
import { SectorComponent } from './components/admin/sector/sector.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: '', component: DashboardComponent },
  { path: 'members', component: MembersComponent },
  { path: 'mandal', component: MandalComponent },
  { path: 'sector', component: SectorComponent },
  { path: '**', redirectTo: '' }
];
