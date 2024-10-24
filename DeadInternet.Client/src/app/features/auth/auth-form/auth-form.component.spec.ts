import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule, FormBuilder, Validators, ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { AuthFormComponent } from './auth-form.component';
import { Component, EventEmitter, Input, Output, forwardRef } from '@angular/core';

@Component({
  selector: 'app-form-input',
  template: '',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => FormInputComponentStub),
      multi: true
    }
  ]
})
class FormInputComponentStub implements ControlValueAccessor {
  @Input() label!: string;
  @Input() formControl!: string;
  @Input() error!: string;
  @Input() showError!: boolean;

  writeValue(obj: any): void { }
  registerOnChange(fn: any): void { }
  registerOnTouched(fn: any): void { }
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

describe('AuthFormComponent', () => {
  let component: AuthFormComponent;
  let fixture: ComponentFixture<AuthFormComponent>;
  let formBuilder: FormBuilder;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [
        AuthFormComponent,
        FormInputComponentStub,
        ButtonComponentStub
      ],
      imports: [ReactiveFormsModule],
      providers: [FormBuilder]
    }).compileComponents();

    formBuilder = TestBed.inject(FormBuilder);
    fixture = TestBed.createComponent(AuthFormComponent);
    component = fixture.componentInstance;

    // Create the form group
    const form = formBuilder.group({
      username: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8), Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/)]]
    });

    // Assign the form group to the component
    component.form = form;

    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have an invalid form when fields are empty', () => {
    expect(component.form.valid).toBeFalsy();
  });

  it('should have a valid form when all fields are filled correctly', () => {
    component.form.patchValue({
      username: 'testuser',
      email: 'test@example.com',
      password: 'Test1234!'
    });
    expect(component.form.valid).toBeTruthy();
  });

  it('should return correct error messages', () => {
    component.form.get('username')?.setErrors({ required: true });
    expect(component.getErrorMessage('username')).toBe('Username is required.');

    component.form.get('email')?.setErrors({ email: true });
    expect(component.getErrorMessage('email')).toBe('Please enter a valid email address.');

    component.form.get('password')?.setErrors({ minlength: true });
    expect(component.getErrorMessage('password')).toBe('Password must be at least 8 characters long.');

    component.form.get('password')?.setErrors({ pattern: true });
    expect(component.getErrorMessage('password')).toBe('Password must contain at least one uppercase letter, one lowercase letter, one number, one special character, and be at least 8 characters long.');
  });
});
