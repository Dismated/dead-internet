import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { RegisterComponent } from './register.component';
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

describe('RegisterComponent', () => {
  let component: RegisterComponent;
  let fixture: ComponentFixture<RegisterComponent>;
  let authServiceMock: any;
  let routerMock: any;
  let errorServiceMock: any;

  beforeEach(async () => {
    authServiceMock = jasmine.createSpyObj('AuthService', ['register', 'guestLogin']);
    routerMock = jasmine.createSpyObj('Router', ['navigate']);
    errorServiceMock = jasmine.createSpyObj('ErrorService', ['setErrorMessage']);

    await TestBed.configureTestingModule({
      declarations: [RegisterComponent, FormInputComponentStub, ButtonComponentStub, LoadingComponentStub, TextButtonComponentStub, AuthFormComponentStub],
      imports: [ReactiveFormsModule],
      providers: [
        { provide: AuthService, useValue: authServiceMock },
        { provide: Router, useValue: routerMock },
        { provide: ErrorService, useValue: errorServiceMock }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(RegisterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize the form with empty fields', () => {
    const form = component.registerForm;
    expect(form).toBeTruthy();
    expect(form.controls['username'].value).toEqual('');
    expect(form.controls['email'].value).toEqual('');
    expect(form.controls['password'].value).toEqual('');
  });

  it('should invalidate the form if required fields are empty or invalid', () => {
    const form = component.registerForm;
    form.controls['username'].setValue('');
    form.controls['email'].setValue('invalid-email');
    form.controls['password'].setValue('123');
    expect(form.invalid).toBeTrue();
  });

  it('should submit the form if valid and call AuthService', () => {
    const form = component.registerForm;
    form.controls['username'].setValue('validUser');
    form.controls['email'].setValue('valid@example.com');
    form.controls['password'].setValue('ValidPass123!');

    authServiceMock.register.and.returnValue(of({}));

    component.onSubmit();
    expect(authServiceMock.register).toHaveBeenCalledWith('validUser', 'valid@example.com', 'ValidPass123!');
    expect(routerMock.navigate).toHaveBeenCalledWith(['/login']);
  });

  it('should show error message if the form is invalid on submit', () => {
    component.registerForm.controls['username'].setValue('short');
    component.onSubmit();
    expect(errorServiceMock.setErrorMessage).toHaveBeenCalledWith('Please fill in all required fields correctly.', 'warning');
    expect(authServiceMock.register).not.toHaveBeenCalled();
  });

  it('should handle registration error and set error message', () => {
    component.registerForm.controls['username'].setValue('validUser');
    component.registerForm.controls['email'].setValue('valid@example.com');
    component.registerForm.controls['password'].setValue('ValidPass123!');

    const errorResponse = { status: 400 };
    authServiceMock.register.and.returnValue(throwError(() => errorResponse));

    component.onSubmit();
    expect(errorServiceMock.setErrorMessage).toHaveBeenCalledWith('Invalid input. Please check your details and try again.', 'warning');
  });

  it('should navigate to home after guest login', () => {
    authServiceMock.guestLogin.and.returnValue(of({ data: { token: 'guestToken' } }));

    component.onGuestLoginClick();
    expect(authServiceMock.guestLogin).toHaveBeenCalled();
    expect(localStorage.getItem('token')).toBe('guestToken');
    expect(routerMock.navigate).toHaveBeenCalledWith(['/home']);
  });

  it('should navigate to login page on Login button click', () => {
    component.onLoginClick();
    expect(routerMock.navigate).toHaveBeenCalledWith(['/login']);
  });
});
