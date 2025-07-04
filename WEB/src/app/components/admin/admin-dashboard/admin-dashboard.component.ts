﻿import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  standalone: false,
  styleUrls: ['./admin-dashboard.component.css']
})
export class AdminDashboardComponent implements OnInit {

  constructor(private router: Router) { }

  ngOnInit(): void {
  }

  navigateToManageJobs(): void {
    this.router.navigate(['/admin/manage-jobs']);
  }

  navigateToManageApplications(): void {
    this.router.navigate(['/admin/manage-applications']);
  }
}
