import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms'; // For forms
import { NgbModule } from '@ng-bootstrap/ng-bootstrap'; // Angular Bootstrap

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/authentication/login/login.component';
import { RegisterComponent } from './components/authentication/register/register.component';
import { HeaderComponent } from './components/shared/header/header.component';
import { JobListingListComponent } from './components/job-listings/job-listing-list/job-listing-list.component';
import { JobListingDetailComponent } from './components/job-listings/job-listing-detail/job-listing-detail.component';
import { MyApplicationsComponent } from './components/applications/my-applications/my-applications.component';
import { AdminDashboardComponent } from './components/admin/admin-dashboard/admin-dashboard.component';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { ManageJobsComponent } from './components/admin/manage-jobs/manage-jobs.component';
import { ManageApplicationsComponent } from './components/admin/manage-applications/manage-applications.component';
import { JobFormModalComponent } from './components/admin/job-form-modal/job-form-modal.component';
import { ApplyJobModalComponent } from './components/job-listings/apply-job-modal/apply-job-modal.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    HeaderComponent,
    JobListingListComponent,
    JobListingDetailComponent,
    MyApplicationsComponent,
    AdminDashboardComponent,
    ManageJobsComponent,
    ManageApplicationsComponent,
    JobFormModalComponent,
    ApplyJobModalComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,          // For template-driven forms
    ReactiveFormsModule,  // For reactive forms
    NgbModule             // Angular Bootstrap module
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
