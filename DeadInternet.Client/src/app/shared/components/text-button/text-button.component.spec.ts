import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { TextButtonComponent } from './text-button.component';

describe('ButtonComponent', () => {
  let component: TextButtonComponent;
  let fixture: ComponentFixture<TextButtonComponent>;
  let debugEl: any;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TextButtonComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TextButtonComponent);
    component = fixture.componentInstance;
    debugEl = fixture.debugElement;
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should apply dynamic classes and styles', () => {
    component.btnClass = 'text-button';
    component.btnStyles = { 'color': 'var(--bs-secondary-color)' };

    fixture.detectChanges();
    const buttonEl = debugEl.query(By.css('span')).nativeElement;

    expect(buttonEl.classList).toContain('text-button');
    expect(buttonEl.style.color).toBe('var(--bs-secondary-color)');
  });
});
