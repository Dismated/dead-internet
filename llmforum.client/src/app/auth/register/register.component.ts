import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  registerForm: FormGroup;
  errorMessage: string | null = null;

  constructor(private formBuilder: FormBuilder, private authService: AuthService, private router: Router) {
    this.registerForm = this.formBuilder.group({
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.registerForm.valid) {
      const { username, email, password } = this.registerForm.value;
      this.authService.register(username, email, password)
        .subscribe(
          () => {
            this.router.navigate(['/login']);
          },
          (error) => {
            if (error.status === 0) {
              this.errorMessage = 'Unable to connect to the server. Please try again later.';
            } else {

              this.errorMessage = 'An error occurred during registration. Please check your input and try again.';
            }
          }
        );
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
}
