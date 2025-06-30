import { Component, OnInit } from '@angular/core';
import { AuthService } from './services/auth.service';
import { User } from './models/user.model';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-root',
  template: `
    <app-header></app-header>
    <div class="container mt-4 mb-4">
      <router-outlet></router-outlet>
    </div>
  `,
  standalone: false,
  styles: []
})
export class AppComponent implements OnInit {
  title = 'job-tracking-frontend';
  currentUser$: Observable<User | null>;

  constructor(private authService: AuthService) {
    this.currentUser$ = this.authService.currentUser;
  }

  ngOnInit(): void {
    // You might want to check for token expiration here or in an interceptor
  }
}
