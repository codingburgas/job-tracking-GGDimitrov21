import { Component, OnInit } from '@angular/core';
import { JobListing } from '../../../models/job-listing.model';
import { JobListingService } from '../../../services/job-listing.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { JobFormModalComponent } from '../job-form-modal/job-form-modal.component';

@Component({
  selector: 'app-manage-jobs',
  templateUrl: './manage-jobs.component.html',
  styleUrls: ['./manage-jobs.component.css']
})
export class ManageJobsComponent implements OnInit {
  jobListings: JobListing[] = [];
  loading = true;
  error: string | null = null;

  constructor(
    private jobListingService: JobListingService,
    private modalService: NgbModal
  ) { }

  ngOnInit(): void {
    this.loadJobListings();
  }

  loadJobListings(): void {
    this.loading = true;
    this.error = null;
    this.jobListingService.getAllJobListings().subscribe({
      next: (data) => {
        this.jobListings = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Failed to load job listings for admin:', err);
        this.error = 'Failed to load job listings. Please ensure you have admin rights.';
        this.loading = false;
      }
    });
  }

  openJobFormModal(job?: JobListing): void {
    const modalRef = this.modalService.open(JobFormModalComponent, { size: 'lg' });
    modalRef.componentInstance.jobListing = job ? { ...job } : null; // Pass a copy to avoid direct mutation

    modalRef.result.then((result) => {
      if (result === 'saved') {
        this.loadJobListings(); // Reload list after save/edit
      }
    }, (reason) => {
      console.log(`Modal dismissed: ${reason}`);
    });
  }

  deleteJob(id: number): void {
    if (confirm('Are you sure you want to delete this job listing?')) {
      this.jobListingService.deleteJobListing(id).subscribe({
        next: () => {
          this.loadJobListings(); // Reload list after deletion
        },
        error: (err) => {
          console.error('Failed to delete job listing:', err);
          this.error = 'Failed to delete job listing.';
        }
      });
    }
  }

  getStatusClass(status: string): string {
    return status === 'Active' ? 'badge bg-success' : 'badge bg-secondary';
  }
}
