import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { User } from '../models/user.model';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  // Backend API URL for authentication
  private baseUrl = 'https://localhost:7021/api/Auth'; // Adjust if your backend runs on a different port

  // BehaviorSubject to store and emit the current user's state
  private currentUserSubject: BehaviorSubject<User | null>;
  public currentUser: Observable<User | null>;

  constructor(private http: HttpClient, private router: Router) {
    // Initialize currentUserSubject with data from localStorage if available
    const storedUser = localStorage.getItem('currentUser');
    this.currentUserSubject = new BehaviorSubject<User | null>(
      storedUser ? JSON.parse(storedUser) : null
    );
    this.currentUser = this.currentUserSubject.asObservable();
  }

  // Getter for current user value
  public get currentUserValue(): User | null {
    return this.currentUserSubject.value;
  }

  // User registration
  register(firstName: string, middleName: string, lastName: string, username: string, password: string): Observable<User> {
    return this.http.post<User>(`${this.baseUrl}/register`, { firstName, middleName, lastName, username, password })
      .pipe(
        map(user => {
          // No direct login after registration in this example, just return user info
          return user;
        }),
        catchError(error => {
          console.error('Registration failed:', error);
          return throwError(() => new Error('Registration failed.'));
        })
      );
  }

  // User login
  login(username: string, password: string): Observable<User> {
    return this.http.post<User>(`${this.baseUrl}/login`, { username, password })
      .pipe(
        map(user => {
          // Store user details and JWT token in localStorage
          localStorage.setItem('currentUser', JSON.stringify(user));
          this.currentUserSubject.next(user); // Emit the new user state
          return user;
        }),
        catchError(error => {
          console.error('Login failed:', error);
          return throwError(() => new Error('Invalid username or password.'));
        })
      );
  }

  // User logout
  logout(): void {
    // Remove user from localStorage
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null); // Clear the user state
    this.router.navigate(['/login']); // Redirect to login page
  }

  // Check if user is authenticated (has a token)
  isAuthenticated(): boolean {
    return this.currentUserSubject.value !== null;
  }

  // Check if user has a specific role
  hasRole(role: 'User' | 'Admin'): boolean {
    return this.currentUserSubject.value?.role === role;
  }

  // Get current user's token
  getToken(): string | null {
    return this.currentUserSubject.value?.token || null;
  }
}
