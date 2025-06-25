import { Component, OnInit } from '@angular/core';
import { Application } from '../../../models/application.model';
import { ApplicationService } from '../../../services/application.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ApplicationStatusModalComponent } from '../application-status-modal/application-status-modal.component';

@Component({
  selector: 'app-manage-applications',
  templateUrl: './manage-applications.component.html',
  styleUrls: ['./manage-applications.component.css']
})
export class ManageApplicationsComponent implements OnInit {
  applications: Application[] = [];
  loading = true;
  error: string | null = null;

  constructor(
    private applicationService: ApplicationService,
    private modalService: NgbModal
  ) { }

  ngOnInit(): void {
    this.loadApplications();
  }

  loadApplications(): void {
    this.loading = true;
    this.error = null;
    this.applicationService.getAllApplications().subscribe({
      next: (data) => {
        this.applications = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Failed to load all applications for admin:', err);
        this.error = 'Failed to load applications. Please ensure you have admin rights.';
        this.loading = false;
      }
    });
  }

  openStatusUpdateModal(application: Application): void {
    const modalRef = this.modalService.open(ApplicationStatusModalComponent);
    modalRef.componentInstance.application = { ...application }; // Pass a copy

    modalRef.result.then((result) => {
      if (result === 'updated') {
        this.loadApplications(); // Reload applications after status update
      }
    }, (reason) => {
      console.log(`Modal dismissed: ${reason}`);
    });
  }

  getStatusClass(status: string): string {
    switch (status) {
      case 'Submitted': return 'badge bg-info text-dark';
      case 'ApprovedForInterview': return 'badge bg-success';
      case 'Rejected': return 'badge bg-danger';
      default: return 'badge bg-secondary';
    }
  }
}
