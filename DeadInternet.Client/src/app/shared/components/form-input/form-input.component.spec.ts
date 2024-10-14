import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormInputComponent } from './form-input.component';
import { FormsModule } from '@angular/forms';
import { Component, Input } from '@angular/core';
import { By } from '@angular/platform-browser';

// Mock ErrorPopupComponent
@Component({
  selector: 'app-error-popup',
  template: ''
})
class ErrorPopupComponentStub {
  @Input() label!: string;
  @Input() type: string = 'text';
  @Input() error!: string;
  @Input() showError: boolean | undefined = false;
}

describe('FormInputComponent', () => {
  let component: FormInputComponent;
  let fixture: ComponentFixture<FormInputComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [FormInputComponent, ErrorPopupComponentStub],
      imports: [FormsModule]
    }).compileComponents();

    fixture = TestBed.createComponent(FormInputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should set input properties correctly', () => {
    component.label = 'Email Address';
    component.type = 'email';
    component.error = 'Invalid email';
    component.showError = true;
    fixture.detectChanges();

    const labelElement = fixture.debugElement.query(By.css('label')).nativeElement;
    const inputElement = fixture.debugElement.query(By.css('input')).nativeElement;
    const errorPopup = fixture.debugElement.query(By.css('app-error-popup'));

    expect(labelElement.textContent).toContain('Email Address');
    expect(inputElement.type).toBe('email');
    expect(errorPopup).toBeTruthy();
    expect(errorPopup.componentInstance.error).toBe('Invalid email');
  });

  it('should call onInput and update value when input changes', () => {
    const inputElement = fixture.debugElement.query(By.css('input')).nativeElement;
    inputElement.value = 'test@example.com';
    inputElement.dispatchEvent(new Event('input'));
    fixture.detectChanges();

    expect(component.value).toBe('test@example.com');
  });

  it('should call onBlur when input loses focus', () => {
    spyOn(component, 'onTouched');
    const inputElement = fixture.debugElement.query(By.css('input')).nativeElement;
    inputElement.dispatchEvent(new Event('blur'));
    fixture.detectChanges();

    expect(component.onTouched).toHaveBeenCalled();
  });

  it('should implement ControlValueAccessor methods', () => {
    const testValue = 'test@example.com';
    const changeFn = jasmine.createSpy('changeFn');
    const touchedFn = jasmine.createSpy('touchedFn');

    component.writeValue(testValue);
    expect(component.value).toBe(testValue);

    component.registerOnChange(changeFn);
    component.registerOnTouched(touchedFn);

    component.onInput('new@example.com');
    expect(changeFn).toHaveBeenCalledWith('new@example.com');

    component.onBlur();
    expect(touchedFn).toHaveBeenCalled();
  });

  it('should not show error popup when showError is false', () => {
    component.error = 'Test Error';
    component.showError = false;
    fixture.detectChanges();

    const errorPopup = fixture.debugElement.query(By.css('app-error-popup'));
    expect(errorPopup).toBeFalsy();
  });

  it('should show error popup when showError is true and error is present', () => {
    component.error = 'Test Error';
    component.showError = true;
    fixture.detectChanges();

    const errorPopup = fixture.debugElement.query(By.css('app-error-popup'));
    expect(errorPopup).toBeTruthy();
    expect(errorPopup.componentInstance.error).toBe('Test Error');
  });
});
