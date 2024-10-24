import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { ButtonComponent } from './button.component';

describe('ButtonComponent', () => {
  let component: ButtonComponent;
  let fixture: ComponentFixture<ButtonComponent>;
  let debugEl: any;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ButtonComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ButtonComponent);
    component = fixture.componentInstance;
    debugEl = fixture.debugElement;
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should emit clickEvent when button is clicked', () => {
    spyOn(component.clickEvent, 'emit'); // Spy on the EventEmitter

    const buttonEl = debugEl.query(By.css('button'));
    buttonEl.triggerEventHandler('click', new Event('click')); // Simulate click

    expect(component.clickEvent.emit).toHaveBeenCalled(); // Ensure emit is called
  });


  it('should not emit clickEvent if disabled is true', () => {
    spyOn(component.clickEvent, 'emit');

    component.disabled = true; // Set disabled to true

    const buttonEl = debugEl.query(By.css('button'));
    buttonEl.triggerEventHandler('click', new Event('click'));

    expect(component.clickEvent.emit).not.toHaveBeenCalled(); // Ensure emit is NOT called
  });

  it('should apply dynamic classes and styles', () => {
    component.btnClass = 'btn btn-primary';
    component.btnStyles = { "border-top-left-radius": "0", "border-bottom-left-radius": "0" };

    fixture.detectChanges();
    const buttonEl = debugEl.query(By.css('button'))
    if (!buttonEl) {
      fail('Button element not found');
      return;
    }
    const nativeButtonEl = buttonEl.nativeElement;

    expect(nativeButtonEl.classList.contains('btn')).withContext('Should have "btn" class').toBe(true);
    expect(nativeButtonEl.classList.contains('btn-primary')).withContext('Should have "btn-primary" class').toBe(true);

    // Check styles
    const computedStyle = window.getComputedStyle(nativeButtonEl);

    expect(computedStyle.borderTopLeftRadius).withContext('borderTopLeftRadius should be "0px"').toBe('0px');
    expect(computedStyle.borderBottomLeftRadius).withContext('borderBottomLeftRadius should be "0px"').toBe('0px');
  });

  it('should set the button type attribute correctly', () => {
    const expectedType = 'submit';
    component.submitType = true;
    fixture.detectChanges();

    const buttonEl = debugEl.query(By.css('button'))
    const nativeButtonEl = buttonEl.nativeElement;

    expect(nativeButtonEl.getAttribute('type')).toBe(expectedType);
  });
});
