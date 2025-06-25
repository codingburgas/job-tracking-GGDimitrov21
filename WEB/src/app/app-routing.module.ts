import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/auth/login/login.component';
import { RegisterComponent } from './components/auth/register/register.component';
import { JobListingListComponent } from './components/job-listings/job-listing-list/job-listing-list.component';
import { JobListingDetailComponent } from './components/job-listings/job-listing-detail/job-listing-detail.component';
import { MyApplicationsComponent } from './components/applications/my-applications/my-applications.component';
import { AdminDashboardComponent } from './components/admin/admin-dashboard/admin-dashboard.component';
import { AuthGuard } from './guards/auth.guard';
import { ManageJobsComponent } from './components/admin/manage-jobs/manage-jobs.component';
import { ManageApplicationsComponent } from './components/admin/manage-applications/manage-applications.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'job-listings', component: JobListingListComponent, canActivate: [AuthGuard] },
  { path: 'job-listings/:id', component: JobListingDetailComponent, canActivate: [AuthGuard] },
  { path: 'my-applications', component: MyApplicationsComponent, canActivate: [AuthGuard, AuthGuard], data: { roles: ['User'] } },
  { path: 'admin-dashboard', component: AdminDashboardComponent, canActivate: [AuthGuard], data: { roles: ['Admin'] } },
  { path: 'admin/manage-jobs', component: ManageJobsComponent, canActivate: [AuthGuard], data: { roles: ['Admin'] } },
  { path: 'admin/manage-applications', component: ManageApplicationsComponent, canActivate: [AuthGuard], data: { roles: ['Admin'] } },
  { path: '', redirectTo: '/job-listings', pathMatch: 'full' }, // Default route after login
  { path: '**', redirectTo: '/job-listings' } // Wildcard route for any unmatched URLs
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
