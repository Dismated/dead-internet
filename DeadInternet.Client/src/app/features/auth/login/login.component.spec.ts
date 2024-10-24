import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { LoginComponent } from './login.component';
import { AuthService } from '../../../core/auth/auth.service';
import { ErrorService } from '../../../core/error-handling/error.service';
import { Component, EventEmitter, Input, Output } from '@angular/core';


@Component({
  selector: 'app-register-input',
  template: ''
})
class FormInputComponentStub {
  @Input() label!: string;
  @Input() type: string = 'text';
  @Input() error!: string;
  @Input() showError: boolean | undefined = false;
}


@Component({
  selector: 'app-auth-form',
  template: ''
})
class AuthFormComponentStub {
  @Input() form!: FormGroup;
  @Output() submitForm = new EventEmitter<void>();
}


@Component({
  selector: 'app-button',
  template: ''
})
class ButtonComponentStub {
  @Input() submitType: boolean = false;
  @Input() btnClass: string = '';
  @Input() btnStyles: { [key: string]: string } = {};
  @Input() disabled: boolean = false;

  @Output() clickEvent = new EventEmitter<Event>();
}

@Component({
  selector: 'app-loading',
  template: ''
})
class LoadingComponentStub { }

@Component({
  selector: 'app-text-button',
  template: ''
})
class TextButtonComponentStub { }

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  let authService: jasmine.SpyObj<AuthService>;
  let router: jasmine.SpyObj<Router>;
  let errorService: jasmine.SpyObj<ErrorService>;

  beforeEach(async () => {
    const authServiceSpy = jasmine.createSpyObj('AuthService', ['login', 'guestLogin']);
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    const errorServiceSpy = jasmine.createSpyObj('ErrorService', ['setErrorMessage', 'getError']);

    await TestBed.configureTestingModule({
      declarations: [LoginComponent, AuthFormComponentStub, TextButtonComponentStub, LoadingComponentStub, ButtonComponentStub, FormInputComponentStub],
      imports: [ReactiveFormsModule],
      providers: [
        { provide: AuthService, useValue: authServiceSpy },
        { provide: Router, useValue: routerSpy },
        { provide: ErrorService, useValue: errorServiceSpy }
      ]
    }).compileComponents();

    authService = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;
    router = TestBed.inject(Router) as jasmine.SpyObj<Router>;
    errorService = TestBed.inject(ErrorService) as jasmine.SpyObj<ErrorService>;
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    errorService.getError.and.returnValue(of(null));
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize the form with empty fields', () => {
    expect(component.loginForm.get('username')?.value).toBe('');
    expect(component.loginForm.get('password')?.value).toBe('');
  });

  it('should mark form as invalid when empty', () => {
    expect(component.loginForm.valid).toBeFalsy();
  });

  it('should mark form as valid when all fields are filled', () => {
    component.loginForm.patchValue({
      username: 'testuser',
      password: 'password123'
    });
    expect(component.loginForm.valid).toBeTruthy();
  });

  it('should call AuthService.login when form is valid and submitted', () => {
    const mockResponse = { data: { token: 'testToken' } };
    authService.login.and.returnValue(of(mockResponse));
    component.loginForm.patchValue({
      username: 'testuser',
      password: 'password123'
    });
    component.onSubmit();
    expect(authService.login).toHaveBeenCalledWith('testuser', 'password123');
  });

  it('should navigate to home page on successful login', () => {
    const mockResponse = { data: { token: 'testToken' } };
    authService.login.and.returnValue(of(mockResponse));
    component.loginForm.patchValue({
      username: 'testuser',
      password: 'password123'
    });
    component.onSubmit();
    expect(router.navigate).toHaveBeenCalledWith(['/home']);
  });

  it('should set error message when login fails due to network error', () => {
    const error = { status: 0 };
    authService.login.and.returnValue(throwError(() => error));
    component.loginForm.patchValue({
      username: 'testuser',
      password: 'password123'
    });
    component.onSubmit();
    expect(errorService.setErrorMessage).toHaveBeenCalledWith('Unable to connect to the server. Please try again later.', 'error');
  });

  it('should set error message when login fails due to invalid input', () => {
    const error = { status: 400 };
    authService.login.and.returnValue(throwError(() => error));
    component.loginForm.patchValue({
      username: 'testuser',
      password: 'password123'
    });
    component.onSubmit();
    expect(errorService.setErrorMessage).toHaveBeenCalledWith('Invalid input. Please check your details and try again.', 'warning');
  });

  it('should set error message when login fails due to unexpected error', () => {
    const error = { status: 500 };
    authService.login.and.returnValue(throwError(() => error));
    component.loginForm.patchValue({
      username: 'testuser',
      password: 'password123'
    });
    component.onSubmit();
    expect(errorService.setErrorMessage).toHaveBeenCalledWith('An unexpected error occurred. Please try again.', 'error');
  });

  it('should set error message when form is invalid on submit', () => {
    component.onSubmit();
    expect(errorService.setErrorMessage).toHaveBeenCalledWith('Please fill in all required fields correctly.', 'warning');
  });

  it('should call AuthService.guestLogin and navigate to home on guest login', () => {
    const mockResponse = { data: { token: 'guestToken' } };
    authService.guestLogin.and.returnValue(of(mockResponse));
    component.onGuestLoginClick();
    expect(authService.guestLogin).toHaveBeenCalled();
    expect(localStorage.getItem('token')).toBe('guestToken');
    expect(router.navigate).toHaveBeenCalledWith(['/home']);
  });

  it('should navigate to register page on register click', () => {
    component.onRegisterClick();
    expect(router.navigate).toHaveBeenCalledWith(['/register']);
  });

  it('should unsubscribe from error subscription on component destroy', () => {
    const unsubscribeSpy = spyOn(component['errorSubscription'] as any, 'unsubscribe');
    component.ngOnDestroy();
    expect(unsubscribeSpy).toHaveBeenCalled();
  });
});
