import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from './auth.service';
import { ErrorService } from '../error-handling/error.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router,
    private errorService: ErrorService
  ) { }

  canActivate(): boolean {
    if (this.authService.isAuthenticated()) {
      return true;
    } else {
      this.errorService.setErrorMessage('You are not authorized to access this page.', 'error');
      this.router.navigate(['/login']);
      return false;
    }
  }
}
