import { Component, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ApplicationService } from '../../../services/application.service';

@Component({
  selector: 'app-apply-job-modal',
  templateUrl: './apply-job-modal.component.html',
  standalone: false,
  styleUrls: ['./apply-job-modal.component.scss']
})
export class ApplyJobModalComponent {
  @Input() jobListingId!: number;
  @Input() jobTitle!: string;
  loading = false;
  success = false;
  error = '';

  constructor(public activeModal: NgbActiveModal, private applicationService: ApplicationService) { }

  submitApplication(): void {
    this.loading = true;
    this.error = '';
    this.success = false;

    this.applicationService.submitApplication({ jobListingId: this.jobListingId }).subscribe({
      next: () => {
        this.success = true;
        this.loading = false;
        // Close modal after a short delay or user action
        setTimeout(() => this.activeModal.close('applied'), 1500);
      },
      error: (err) => {
        console.error('Application submission failed:', err);
        this.error = 'Failed to submit application. You might have already applied or an error occurred.';
        this.loading = false;
      }
    });
  }

  cancel(): void {
    this.activeModal.dismiss('cancel');
  }
}
