import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../../core/auth/auth.service';
import { Router } from '@angular/router';
import { ErrorService } from '../../../core/error-handling/error.service';
import { Subscription, catchError, throwError } from 'rxjs';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit, OnDestroy {
  registerForm: FormGroup;
  errorMessage: string | undefined;
  private errorSubscription: Subscription | undefined;
  usernameFocused: boolean = false;
  emailFocused: boolean = false;
  passwordFocused: boolean = false;

  constructor(private formBuilder: FormBuilder, private authService: AuthService, private router: Router, private errorService: ErrorService) {
    this.registerForm = this.formBuilder.group({
      username: ['', [Validators.required, Validators.minLength(4)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.pattern(/^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*(),.?":{}|<>]).{8,}$/)]],
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
    if (this.registerForm.valid) {
      const { username, email, password } = this.registerForm.value;
      this.authService.register(username, email, password)
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
            this.router.navigate(['/login']);
          },
          error: (err) => {
            console.error('Registration failed', err);
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

  onLoginClick() {
    this.router.navigate(['/login']);
  }

  get username() {
    return this.registerForm.get('username');
  }
  get email() {
    return this.registerForm.get('email');
  }
  get password() {
    return this.registerForm.get('password');
  }
}
