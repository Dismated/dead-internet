import { TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { AuthGuard } from './auth.guard';
import { AuthService } from './auth.service';
import { ErrorService } from '../error-handling/error.service';
import { RouterTestingModule } from '@angular/router/testing';
import { of } from 'rxjs';

describe('AuthGuard', () => {
  let authGuard: AuthGuard;
  let authService: jasmine.SpyObj<AuthService>;
  let router: jasmine.SpyObj<Router>;
  let errorService: jasmine.SpyObj<ErrorService>;

  beforeEach(() => {
    const authServiceSpy = jasmine.createSpyObj('AuthService', ['isAuthenticated']);
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    const errorServiceSpy = jasmine.createSpyObj('ErrorService', ['setErrorMessage']);

    TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      providers: [
        AuthGuard,
        { provide: AuthService, useValue: authServiceSpy },
        { provide: Router, useValue: routerSpy },
        { provide: ErrorService, useValue: errorServiceSpy }
      ]
    });

    authGuard = TestBed.inject(AuthGuard);
    authService = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;
    router = TestBed.inject(Router) as jasmine.SpyObj<Router>;
    errorService = TestBed.inject(ErrorService) as jasmine.SpyObj<ErrorService>;
  });

  it('should allow the activation if the user is authenticated', () => {
    // Arrange
    authService.isAuthenticated.and.returnValue(true);

    // Act
    const result = authGuard.canActivate();

    // Assert
    expect(result).toBe(true);
    expect(router.navigate).not.toHaveBeenCalled();
    expect(errorService.setErrorMessage).not.toHaveBeenCalled();
  });

  it('should block the activation and navigate to login if the user is not authenticated', () => {
    // Arrange
    authService.isAuthenticated.and.returnValue(false);

    // Act
    const result = authGuard.canActivate();

    // Assert
    expect(result).toBe(false);
    expect(router.navigate).toHaveBeenCalledWith(['/login']);
    expect(errorService.setErrorMessage).toHaveBeenCalledWith(
      'You are not authorized to access this page.',
      'error'
    );
  });
});
