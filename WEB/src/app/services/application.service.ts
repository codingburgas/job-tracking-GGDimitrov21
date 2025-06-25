import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Application, CreateApplicationDto, UpdateApplicationStatusDto } from '../models/application.model';

@Injectable({
  providedIn: 'root'
})
export class ApplicationService {
  private baseUrl = 'https://localhost:7021/api/Applications'; // Adjust to your backend URL

  constructor(private http: HttpClient) { }

  // Get applications for the current user
  getMyApplications(): Observable<Application[]> {
    return this.http.get<Application[]>(`${this.baseUrl}/my`);
  }

  // Get all applications (Admin only)
  getAllApplications(): Observable<Application[]> {
    return this.http.get<Application[]>(this.baseUrl);
  }

  // Submit a new application
  submitApplication(application: CreateApplicationDto): Observable<Application> {
    return this.http.post<Application>(this.baseUrl, application);
  }

  // Update application status (Admin only)
  updateApplicationStatus(id: number, statusDto: UpdateApplicationStatusDto): Observable<Application> {
    return this.http.put<Application>(`${this.baseUrl}/${id}/status`, statusDto);
  }
}
