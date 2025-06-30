import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { JobListing } from '../../../models/job-listing.model';
import { JobListingService } from '../../../services/job-listing.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ApplyJobModalComponent } from '../apply-job-modal/apply-job-modal.component';
import { ApplicationService } from '../../../services/application.service'; // To check if already applied
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-job-listing-detail',
  templateUrl: './job-listing-detail.component.html',
  standalone: false,
  styleUrls: ['./job-listing-detail.component.css']
})
export class JobListingDetailComponent implements OnInit {
  jobListing: JobListing | null = null;
  loading = true;
  error: string | null = null;
  hasApplied = false;
  isUserRole = false; // Flag to determine if the current user is a 'User'

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private jobListingService: JobListingService,
    private applicationService: ApplicationService,
    private authService: AuthService,
    private modalService: NgbModal
  ) { }

  ngOnInit(): void {
    // Check user role to conditionally show 'Apply' button
    this.authService.currentUser.subscribe(user => {
      this.isUserRole = user?.role === 'User';
    });

    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.loadJobListing(parseInt(id, 10));
      } else {
        this.error = 'Job listing ID not provided.';
        this.loading = false;
      }
    });
  }

  loadJobListing(id: number): void {
    this.loading = true;
    this.error = null;
    this.jobListingService.getJobListingById(id).subscribe({
      next: (data) => {
        this.jobListing = data;
        this.loading = false;
        if (this.isUserRole && this.jobListing.status === 'Active') {
          this.checkIfUserApplied(id); // Check application status only if it's an active job and user role
        }
      },
      error: (err) => {
        console.error('Failed to load job listing details', err);
        this.error = 'Failed to load job listing details. It might not exist or you don\'t have access.';
        this.loading = false;
      }
    });
  }

  // Check if the current user has already applied for this job
  checkIfUserApplied(jobListingId: number): void {
    this.applicationService.getMyApplications().subscribe({
      next: (applications) => {
        this.hasApplied = applications.some(app => app.jobListingId === jobListingId);
      },
      error: (err) => {
        console.error('Failed to check application status:', err);
        // Do not block UI, just assume not applied or show a message
      }
    });
  }

  // Open the modal to apply for the job
  applyForJob(): void {
    if (!this.jobListing) return;

    const modalRef = this.modalService.open(ApplyJobModalComponent);
    modalRef.componentInstance.jobListingId = this.jobListing.id;
    modalRef.componentInstance.jobTitle = this.jobListing.title;

    modalRef.result.then((result) => {
      if (result === 'applied') {
        this.hasApplied = true; // Update UI to reflect application
        // Optionally show a success message
        console.log('Application submitted successfully!');
      }
    }, (reason) => {
      console.log(`Modal dismissed: ${reason}`);
    });
  }

  goBack(): void {
    this.router.navigate(['/job-listings']);
  }
}
