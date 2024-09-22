import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthService } from '../../../core/auth/auth.service';
import { Router } from '@angular/router';
import { AuthResponse } from '../../../models/auth-response.interface';
import { Subscription, catchError, throwError } from 'rxjs';
import { ErrorService } from '../../../core/error-handling/error.service';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit, OnDestroy {
  loginForm: FormGroup;
  private errorSubscription: Subscription | undefined;


  constructor(private formBuilder: FormBuilder, private authService: AuthService,
    private router: Router, private errorService: ErrorService
  ) {
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  ngOnInit() {
    this.errorSubscription = this.errorService.getError().subscribe();
  }

  ngOnDestroy() {
    if (this.errorSubscription) {
      this.errorSubscription.unsubscribe();
    }
  }

  onSubmit() {
    if (this.loginForm.valid) {
      const { username, password } = this.loginForm.value;
      this.authService.login(username, password)
        .pipe(
          catchError(error => {
            if (error.status === 0) {
              this.errorService.setErrorMessage('Unable to connect to the server. Please try again later.', 'error');
            } else if (error.status === 400) {
              this.errorService.setErrorMessage('Invalid input. Please check your details and try again.', 'warning');
            } else {
              this.errorService.setErrorMessage('An unexpected error occurred. Please try again.', 'error');
            }
            return throwError(() => error);
          })
        )
        .subscribe({
          next: () => {
            this.router.navigate(['/home']);
          },
          error: (error) => {
            console.error("Login failed:", error);
          }
        });
    } else {
      this.errorService.setErrorMessage('Please fill in all required fields correctly.', 'warning');
    }
  }

  onGuestLoginClick() {
    this.authService.guestLogin().subscribe((res) => {
      localStorage.setItem('token', res.token);
      this.router.navigate(['/home']);
    })
  }

  onRegisterClick() {
    this.router.navigate(['/register']);
  }

}
