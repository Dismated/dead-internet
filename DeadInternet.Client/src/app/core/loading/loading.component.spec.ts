import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LoadingComponent } from './loading.component';
import { By } from '@angular/platform-browser';

describe('LoadingComponent', () => {
  let component: LoadingComponent;
  let fixture: ComponentFixture<LoadingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [LoadingComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LoadingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should not show spinner when isLoading is false', () => {
    component.isLoading = false;
    fixture.detectChanges();
    const spinnerElement = fixture.debugElement.query(By.css('.spinner-border'));
    expect(spinnerElement).toBeNull();
  });

  it('should show spinner when isLoading is true', () => {
    component.isLoading = true;
    fixture.detectChanges();
    const spinnerElement = fixture.debugElement.query(By.css('.spinner-border'));
    expect(spinnerElement).toBeTruthy();
  });

  it('should have correct accessibility attributes', () => {
    component.isLoading = true;
    fixture.detectChanges();
    const spinnerElement = fixture.debugElement.query(By.css('.spinner-border'));
    expect(spinnerElement.attributes['role']).toBe('status');
    const hiddenTextElement = fixture.debugElement.query(By.css('.visually-hidden'));
    expect(hiddenTextElement.nativeElement.textContent).toBe('Loading...');
  });
});
