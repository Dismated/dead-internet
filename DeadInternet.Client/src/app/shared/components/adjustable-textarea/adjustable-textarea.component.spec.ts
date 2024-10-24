import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { AdjustableTextareaComponent } from './adjustable-textarea.component';

describe('AdjustableTextareaComponent', () => {
  let component: AdjustableTextareaComponent;
  let fixture: ComponentFixture<AdjustableTextareaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdjustableTextareaComponent],
      imports: [FormsModule]
    }).compileComponents();

    fixture = TestBed.createComponent(AdjustableTextareaComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('Initialization', () => {
    it('should initialize with default values', () => {
      expect(component.placeholderStart).toBe('');
      expect(component.promptText).toBe('');
      expect(component.textareaStyles).toEqual({});
    });

    it('should set correct placeholderStartLength based on placeholderStart', () => {
      component.placeholderStart = 'Create a post';
      component.ngOnInit();
      expect(component['placeholderStartLength']).toBe(370);

      component.placeholderStart = 'Other text';
      component.ngOnInit();
      expect(component['placeholderStartLength']).toBe(310);
    });
  });



  describe('Key handling', () => {
    it('should emit textSubmit on Enter without Shift', () => {
      spyOn(component.textSubmit, 'emit');
      const event = new KeyboardEvent('keydown', {
        key: 'Enter',
        shiftKey: false
      });

      component.handleKeyDown(event);
      expect(component.textSubmit.emit).toHaveBeenCalled();
    });

    it('should not emit textSubmit on Enter with Shift', () => {
      spyOn(component.textSubmit, 'emit');
      const event = new KeyboardEvent('keydown', {
        key: 'Enter',
        shiftKey: true
      });

      component.handleKeyDown(event);
      expect(component.textSubmit.emit).not.toHaveBeenCalled();
    });
  });

  describe('ControlValueAccessor implementation', () => {
    beforeEach(() => {
      // Create and set up textarea element
      const textarea = document.createElement('textarea');
      component.textareaRef = { nativeElement: textarea };
      fixture.detectChanges();
    });

    it('should call onChange when model changes', fakeAsync(() => {
      const onChangeSpy = jasmine.createSpy('onChange');
      component.registerOnChange(onChangeSpy);

      component.onModelChange('new value');
      tick();

      expect(onChangeSpy).toHaveBeenCalledWith('new value');
    }));

    it('should update promptText when writeValue is called', () => {
      component.writeValue('test value');
      expect(component.promptText).toBe('test value');
    });
  });

  describe('Lifecycle hooks', () => {
    beforeEach(() => {
      // Create and set up textarea element
      const textarea = document.createElement('textarea');
      component.textareaRef = { nativeElement: textarea };
      fixture.detectChanges();
    });

    it('should setup ResizeObserver in ngAfterViewInit', () => {
      spyOn(window, 'ResizeObserver').and.returnValue({
        observe: jasmine.createSpy('observe'),
        disconnect: jasmine.createSpy('disconnect')
      } as any);

      component.ngAfterViewInit();
      expect(window.ResizeObserver).toHaveBeenCalled();
    });

    it('should clean up ResizeObserver in ngOnDestroy', () => {
      const disconnectSpy = jasmine.createSpy('disconnect');
      component['resizeObserver'] = { disconnect: disconnectSpy } as any;

      component.ngOnDestroy();
      expect(disconnectSpy).toHaveBeenCalled();
    });

    it('should remove resize event listener in ngOnDestroy', () => {
      spyOn(window, 'removeEventListener');
      component.ngOnDestroy();
      expect(window.removeEventListener).toHaveBeenCalledWith(
        'resize',
        jasmine.any(Function)
      );
    });
  });
});
