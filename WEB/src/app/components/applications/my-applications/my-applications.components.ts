import { Component, OnInit } from '@angular/core';
import { Application } from '../../../models/application.model';
import { ApplicationService } from '../../../services/application.service';

@Component({
  selector: 'app-my-applications',
  templateUrl: './my-applications.component.html',
  styleUrls: ['./my-applications.component.css']
})
export class MyApplicationsComponent implements OnInit {
  myApplications: Application[] = [];
  loading = true;
  error: string | null = null;

  constructor(private applicationService: ApplicationService) { }

  ngOnInit(): void {
    this.loadMyApplications();
  }

  loadMyApplications(): void {
    this.loading = true;
    this.error = null;
    this.applicationService.getMyApplications().subscribe({
      next: (data) => {
        this.myApplications = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Failed to load my applications', err);
        this.error = 'Failed to load your applications. Please try again later.';
        this.loading = false;
      }
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
