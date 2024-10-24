import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ErrorPopupComponent } from './error-popup.component';
import { By } from '@angular/platform-browser';

describe('ErrorPopupComponent', () => {
  let component: ErrorPopupComponent;
  let fixture: ComponentFixture<ErrorPopupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ErrorPopupComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ErrorPopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have a default empty error message', () => {
    expect(component.error).toBe('');
  });

  it('should display the error message when provided', () => {
    const testError = 'Test error message';
    component.error = testError;
    fixture.detectChanges();

    const errorElement = fixture.debugElement.query(By.css('.popup div:last-child'));
    expect(errorElement.nativeElement.textContent).toContain(testError);
  });

  it('should have a triangle element', () => {
    const triangleElement = fixture.debugElement.query(By.css('.triangle'));
    expect(triangleElement).toBeTruthy();
  });
});
