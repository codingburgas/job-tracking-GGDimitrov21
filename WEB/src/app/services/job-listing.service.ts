import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { JobListing, CreateJobListingDto, UpdateJobListingDto } from '../models/job-listing.model';

@Injectable({
  providedIn: 'root'
})
export class JobListingService {
  private baseUrl = 'https://localhost:7021/api/JobListings'; // Adjust to your backend URL

  constructor(private http: HttpClient) { }

  // Get all job listings (for users and admins)
  getAllJobListings(): Observable<JobListing[]> {
    return this.http.get<JobListing[]>(this.baseUrl);
  }

  // Get a single job listing by ID
  getJobListingById(id: number): Observable<JobListing> {
    return this.http.get<JobListing>(`${this.baseUrl}/${id}`);
  }

  // Create a new job listing (Admin only)
  createJobListing(jobListing: CreateJobListingDto): Observable<JobListing> {
    return this.http.post<JobListing>(this.baseUrl, jobListing);
  }

  // Update an existing job listing (Admin only)
  updateJobListing(id: number, jobListing: UpdateJobListingDto): Observable<JobListing> {
    return this.http.put<JobListing>(`${this.baseUrl}/${id}`, jobListing);
  }

  // Delete a job listing (Admin only)
  deleteJobListing(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }
}
