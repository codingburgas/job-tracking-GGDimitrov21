import { Component, OnInit } from '@angular/core';
import { JobListing } from '../../../models/job-listing.model';
import { JobListingService } from '../../../services/job-listing.service';
import { Router } from '@angular/router';

@Component({
    selector: 'app-job-listing-list',
    templateUrl: './job-listing-list.component.html',
    standalone: false,
    styleUrls: ['./job-listing-list.component.css']
})
export class JobListingListComponent implements OnInit {
  jobListings: JobListing[] = [];
  loading = true;
  error: string | null = null;

  constructor(
    private jobListingService: JobListingService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadJobListings();
  }

  loadJobListings(): void {
    this.loading = true;
    this.error = null;
    this.jobListingService.getAllJobListings().subscribe({
      next: (data) => {
        // Filter to show only active jobs to regular users if needed,
        // but for admin, all are shown. Backend handles authorization.
        this.jobListings = data.filter(job => job.status === 'Active'); // Only show active jobs to regular users here
        this.loading = false;
      },
      error: (err) => {
        console.error('Failed to load job listings', err);
        this.error = 'Failed to load job listings. Please try again later.';
        this.loading = false;
      }
    });
  }

  viewDetails(id: number): void {
    this.router.navigate(['/job-listings', id]);
  }
}
