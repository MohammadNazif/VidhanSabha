import { Routes } from '@angular/router';
import { DashboardComponent } from './components/admin/Dashboard/dashboard.component';
import { MembersComponent } from './components/members/members.component';
import { LoginComponent } from './components/auth/login/login.component';
import { MandalComponent } from './components/admin/mandal/mandal.component';
import { SectorComponent } from './components/admin/sector/sector.component';
import { BoothComponent } from './components/admin/booth/booth.component';
import { PannapramukhComponent } from './components/admin/pannapramukh/pannapramukh.component';
import { DesignationComponent } from './components/superadmin/designation/designation.component';
import { SuperDashboardComponent } from './components/superadmin/dashboard/super-dashboard.component';
import { StateComponent } from './components/superadmin/state/state.component';
import { DistrictComponent } from './components/superadmin/district/district.component';
import { VidhanSabhaComponent } from './components/superadmin/vidhansabha/vidhansabha.component';
import { PravasiVoterComponent } from './components/admin/pravasi-voter/pravasi-voter.component';
import { NewVoterComponent } from './components/admin/new-voter/new-voter.component';
import { StatePrabhariListComponent } from './components/state-prabhari/list/state-prabhari-list.component';
import { StatePrabhariDashboardComponent } from './components/state-prabhari/dashboard/state-prabhari-dashboard.component';
import { SahmatAsahmatComponent } from './components/admin/sahmat-asahmat/sahmat-asahmat.component';
import { VidhanSabhaPrabhariListComponent } from './components/state-prabhari/vidhansabha-prabhari/vidhansabha-prabhari-list.component';
import { DoubleVoterComponent } from './components/admin/double-voter/double-voter.component';
import { PrabhavshaliComponent } from './components/admin/prabhavshali/prabhavshali.component';
import { BlockComponent } from './components/admin/block/block.component';
import { PradhanComponent } from './components/admin/pradhan/pradhan.component';
import { BdcComponent } from './components/admin/bdc/bdc.component';
import { SeniorDisabledComponent } from './components/admin/senior-disabled/senior-disabled.component';
import { AllowAccessComponent } from './components/admin/access/allow-access.component';
import { AllowAccessListComponent } from './components/admin/access/allow-access-list.component';
import { CombinedReportComponent } from './components/admin/reports/combined-report/combined-report.component';
import { MandalReportComponent } from './components/admin/reports/mandal-report/mandal-report.component';
import { BoothVoterDescriptionComponent } from './components/admin/booth-voter-description/booth-voter-description.component';
import { BoothSamitiComponent } from './components/admin/booth-samiti/booth-samiti.component';
import { SocialMediaComponent } from './components/admin/socialmedia/socialmedia.component';
import { InfluencerPersonComponent } from './components/admin/influencer-person/influencer-person.component';
import { InfluencerComponent } from './components/admin/influencer/influencer.component';
import { StateMemberMgmtComponent } from './components/state-prabhari/state-member/state-member.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'dashboard', component: DashboardComponent },
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'combined-report', component: CombinedReportComponent },
  { path: 'mandal-report', component: MandalReportComponent },
  { path: 'booth-voter-description', component: BoothVoterDescriptionComponent },
  { path: 'members', component: MembersComponent },
  { path: 'mandal', component: MandalComponent },
  { path: 'mandal-list', component: MandalComponent },
  { path: 'sector', component: SectorComponent },
  { path: 'sector-list', component: SectorComponent },
  { path: 'booth', component: BoothComponent },
  { path: 'booth-samiti', component: BoothSamitiComponent },
  { path: 'booth-samiti-list', component: BoothSamitiComponent },
  { path: 'booth-voter-description-list', component: BoothVoterDescriptionComponent },
  { path: 'booth-list', component: BoothComponent },
  { path: 'panna-pramukh', component: PannapramukhComponent },
  { path: 'panna-pramukh-list', component: PannapramukhComponent },
  { path: 'pravasi-voter', component: PravasiVoterComponent },
  { path: 'pravasi-voter-list', component: PravasiVoterComponent },
  { path: 'new-voter', component: NewVoterComponent },
  { path: 'new-voter-list', component: NewVoterComponent },
  { path: 'sahmat-asahmat', component: SahmatAsahmatComponent },
  { path: 'sahmat-list', component: SahmatAsahmatComponent },
  { path: 'asahmat-list', component: SahmatAsahmatComponent },
  { path: 'double-voter', component: DoubleVoterComponent },
  { path: 'double-voter-list', component: DoubleVoterComponent },
  { path: 'prabhavshali-vyakt', component: PrabhavshaliComponent },
  { path: 'prabhavshali-vyakt-list', component: PrabhavshaliComponent },
  { path: 'influencer-person', component: InfluencerComponent },
  { path: 'influencer-person-list', component: InfluencerComponent },
  { path: 'doctor-list', component: PrabhavshaliComponent },
  { path: 'advocate-list', component: PrabhavshaliComponent },
  { path: 'government-employee-list', component: PrabhavshaliComponent },
  { path: 'pradhan-list', component: PrabhavshaliComponent },
  { path: 'block', component: BlockComponent },
  { path: 'block-list', component: BlockComponent },
  { path: 'block-pramukh-list', component: BlockComponent },
  { path: 'pradhan', component: PradhanComponent },
  { path: 'pradhan-list', component: PradhanComponent },
  { path: 'bdc', component: BdcComponent },
  { path: 'bdc-list', component: BdcComponent },
  { path: 'varisth-naagarik-viklaang', component: SeniorDisabledComponent },
  { path: 'varisth-naagarik-viklaang-list', component: SeniorDisabledComponent },
  { path: 'senior-citizen-list', component: SeniorDisabledComponent },
  { path: 'disabled-list', component: SeniorDisabledComponent },
  { path: 'allow-access', component: AllowAccessComponent },
  { path: 'allow-access-list', component: AllowAccessListComponent },
  { path: 'social-media', component: SocialMediaComponent },
  { path: 'social-media-list', component: SocialMediaComponent },
  { path: 'superadmin/dashboard', component: SuperDashboardComponent },
  { path: 'superadmin/designation', component: DesignationComponent },
  { path: 'superadmin/state', component: StateComponent },
  { path: 'superadmin/state-prabhari', component: StatePrabhariListComponent },
  { path: 'superadmin/district', component: DistrictComponent },
  { path: 'superadmin/district-list', component: DistrictComponent, data: { mode: 'list' } },
  { path: 'superadmin/vidhansabha', component: VidhanSabhaComponent },
  { path: 'superadmin/vidhansabha-list', component: VidhanSabhaComponent, data: { mode: 'list' } },
  { path: 'superadmin/state-member', component: StateMemberMgmtComponent },
  { path: 'superadmin/state-member-list', component: StateMemberMgmtComponent, data: { mode: 'list' } },
  { path: 'superadmin/pradesh-samiti-list', component: StateMemberMgmtComponent, data: { samitiType: 1, mode: 'list' } },
  { path: 'superadmin/pradesh-karyakarini-list', component: StateMemberMgmtComponent, data: { samitiType: 2, mode: 'list' } },
  { path: 'state-prabhari/state-member', component: StateMemberMgmtComponent },
  { path: 'state-prabhari/state-member-list', component: StateMemberMgmtComponent, data: { mode: 'list' } },
  { path: 'state-prabhari/pradesh-samiti-list', component: StateMemberMgmtComponent, data: { samitiType: 1, mode: 'list' } },
  { path: 'state-prabhari/pradesh-karyakarini-list', component: StateMemberMgmtComponent, data: { samitiType: 2, mode: 'list' } },
  { path: 'state-prabhari/dashboard', component: StatePrabhariDashboardComponent },
  { path: 'state-prabhari/vidhansabha-prabhari', component: VidhanSabhaPrabhariListComponent },
  { path: 'state-prabhari/vidhansabha-prabhari-list', component: VidhanSabhaPrabhariListComponent, data: { mode: 'list' } },
  { path: '**', redirectTo: '' }
];
