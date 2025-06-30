import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../services/auth.service';
import { Observable } from 'rxjs';
import { User } from '../../../models/user.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  standalone: false,
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  currentUser$: Observable<User | null>;
  isNavbarCollapsed = true; // For responsive navbar in Angular Bootstrap

  constructor(private authService: AuthService, private router: Router) {
    this.currentUser$ = this.authService.currentUser;
  }

  ngOnInit(): void {
  }

  logout(): void {
    this.authService.logout();
  }

  // Check if current user is an admin
  isAdmin(): boolean {
    return this.authService.hasRole('Admin');
  }

  // Check if current user is a regular user
  isUser(): boolean {
    return this.authService.hasRole('User');
  }
}
