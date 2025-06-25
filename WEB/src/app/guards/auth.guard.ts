import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router) { }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {

    return this.authService.currentUser.pipe(
      map(user => {
        // Check if user is logged in
        if (user) {
          // Check if route has specific role requirements
          if (route.data['roles']) {
            const requiredRoles = route.data['roles'] as Array<'User' | 'Admin'>;
            // Check if the user's role is included in the required roles
            if (requiredRoles.includes(user.role)) {
              return true; // User has the required role
            } else {
              // User is logged in but doesn't have the required role, redirect to unauthorized or home
              this.router.navigate(['/']); // Or a dedicated unauthorized page
              return false;
            }
          }
          return true; // User is logged in and no specific role required
        }

        // If not logged in, redirect to login page with return URL
        this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
        return false;
      })
    );
  }
}
