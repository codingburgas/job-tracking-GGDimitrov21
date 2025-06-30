import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../../services/auth.service';
import { Router } from '@angular/router';

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    standalone: false,
    styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  registerForm!: FormGroup;
  loading = false;
  submitted = false;
  success = false;
  error = '';

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    // Redirect to home if already logged in
    if (this.authService.currentUserValue) {
      this.router.navigate(['/']);
    }
  }

  ngOnInit(): void {
    this.registerForm = this.formBuilder.group({
      firstName: ['', Validators.required],
      middleName: [''], // Optional
      lastName: ['', Validators.required],
      username: ['', [Validators.required, Validators.minLength(4)]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required]
    }, {
      validator: this.mustMatch('password', 'confirmPassword')
    });
  }

  // Convenience getter for easy access to form fields
  get f() { return this.registerForm.controls; }

  // Custom validator to check if two fields match
  mustMatch(controlName: string, matchingControlName: string) {
    return (formGroup: FormGroup) => {
      const control = formGroup.controls[controlName];
      const matchingControl = formGroup.controls[matchingControlName];

      if (matchingControl.errors && !matchingControl.errors['mustMatch']) {
        // Return if another validator has already found an error on the matchingControl
        return;
      }

      // Set error on matchingControl if validation fails
      if (control.value !== matchingControl.value) {
        matchingControl.setErrors({ mustMatch: true });
      } else {
        matchingControl.setErrors(null);
      }
    };
  }

  onSubmit(): void {
    this.submitted = true;
    this.success = false;
    this.error = '';

    // Stop here if form is invalid
    if (this.registerForm.invalid) {
      return;
    }

    this.loading = true;
    this.authService.register(
      this.f['firstName'].value,
      this.f['middleName'].value,
      this.f['lastName'].value,
      this.f['username'].value,
      this.f['password'].value
    )
      .subscribe({
        next: () => {
          this.success = true;
          this.loading = false;
          // Optionally, auto-login or redirect to login page
          // this.authService.login(this.f['username'].value, this.f['password'].value).subscribe(() => {
          //   this.router.navigate(['/']);
          // });
          this.router.navigate(['/login']); // Redirect to login page after successful registration
        },
        error: error => {
          this.error = error.message || 'Registration failed. Please try again.';
          this.loading = false;
        }
      });
  }
}
