import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { JobListing, CreateJobListingDto, UpdateJobListingDto } from '../../../models/job-listing.model';
import { JobListingService } from '../../../services/job-listing.service';

@Component({
  selector: 'app-job-form-modal',
  templateUrl: './job-form-modal.component.html',
  standalone: false,
  styleUrls: ['./job-form-modal.component.css']
})
export class JobFormModalComponent implements OnInit {
  @Input() jobListing: JobListing | null = null; // Input for editing existing job
  jobForm!: FormGroup;
  loading = false;
  submitted = false;
  error = '';
  modalTitle = 'Create New Job Listing';

  constructor(
    public activeModal: NgbActiveModal,
    private fb: FormBuilder,
    private jobListingService: JobListingService
  ) { }

  ngOnInit(): void {
    this.jobForm = this.fb.group({
      title: ['', Validators.required],
      company: ['', Validators.required],
      description: ['', Validators.required],
      status: ['Active', Validators.required] // Default status for new jobs
    });

    if (this.jobListing) {
      this.modalTitle = 'Edit Job Listing';
      this.jobForm.patchValue({
        title: this.jobListing.title,
        company: this.jobListing.company,
        description: this.jobListing.description,
        status: this.jobListing.status
      });
    }
  }

  get f() { return this.jobForm.controls; }

  onSubmit(): void {
    this.submitted = true;
    this.error = '';

    if (this.jobForm.invalid) {
      return;
    }

    this.loading = true;

    if (this.jobListing) {
      // Update existing job
      const updateDto: UpdateJobListingDto = {
        title: this.f['title'].value,
        company: this.f['company'].value,
        description: this.f['description'].value,
        status: this.f['status'].value
      };
      this.jobListingService.updateJobListing(this.jobListing.id, updateDto).subscribe({
        next: () => {
          this.activeModal.close('saved');
        },
        error: (err) => {
          this.error = 'Failed to update job listing.';
          console.error('Update job failed:', err);
          this.loading = false;
        }
      });
    } else {
      // Create new job
      const createDto: CreateJobListingDto = {
        title: this.f['title'].value,
        company: this.f['company'].value,
        description: this.f['description'].value
      };
      this.jobListingService.createJobListing(createDto).subscribe({
        next: () => {
          this.activeModal.close('saved');
        },
        error: (err) => {
          this.error = 'Failed to create job listing.';
          console.error('Create job failed:', err);
          this.loading = false;
        }
      });
    }
  }

  cancel(): void {
    this.activeModal.dismiss('cancel');
  }
}
