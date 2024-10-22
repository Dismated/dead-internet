import { AfterViewInit, Component, ElementRef, EventEmitter, Input, NgZone, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-adjustable-textarea',
  templateUrl: './adjustable-textarea.component.html',
  styleUrl: './adjustable-textarea.component.css',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: AdjustableTextareaComponent,
      multi: true
    }
  ]

})
export class AdjustableTextareaComponent implements AfterViewInit, OnInit, OnDestroy, ControlValueAccessor {
  @ViewChild('textarea') textareaRef!: ElementRef;
  @Input() placeholderStart = '';
  @Input() promptText = '';
  @Input() textareaStyles: { [key: string]: string } = {};
  @Output() textSubmit = new EventEmitter<void>();

  placeholder: string = ''
  placeholderStartLength = 0

  private resizeObserver!: ResizeObserver;
  private onChange: (value: string) => void = () => { };
  private onTouched: () => void = () => { };

  constructor(private elementRef: ElementRef, private ngZone: NgZone) { }

  ngOnInit() {
    this.updatePlaceholder();
    this.placeholderStartLength = this.placeholderStart === 'Create a post' ? 370 : 310
  }

  ngAfterViewInit() {
    this.adjustHeight();
    this.setupResizeObserver();

    this.ngZone.runOutsideAngular(() => {
      window.addEventListener('resize', this.updatePlaceholder.bind(this));
    });
  }

  ngOnDestroy() {
    if (this.resizeObserver) {
      this.resizeObserver.disconnect();
    }
    window.removeEventListener('resize', this.updatePlaceholder.bind(this));
  }

  onModelChange(newValue: string) {
    this.promptText = newValue;
    this.onChange(this.promptText);
    this.adjustHeight();
  }

  handleKeyDown(event: KeyboardEvent) {
    if (event.key === 'Enter' && !event.shiftKey) {
      event.preventDefault();
      this.textSubmit.emit();
    }
  }

  private adjustHeight() {
    const textarea = this.textareaRef.nativeElement;
    textarea.style.height = 'auto';
    if (!textarea.value.trim()) {
      textarea.style.height = '24px';
    } else {
      textarea.style.height = `${textarea.scrollHeight}px`;
    }
  }

  private setupResizeObserver() {
    this.resizeObserver = new ResizeObserver(() => {
      this.ngZone.run(() => {
        this.adjustHeight();
        this.updatePlaceholder();
      });
    });

    this.resizeObserver.observe(this.textareaRef.nativeElement);
    this.resizeObserver.observe(this.elementRef.nativeElement.parentElement);
  }

  updatePlaceholder(): void {
    const parentWidth = this.elementRef.nativeElement.parentElement?.offsetWidth || 0;
    const numberOfSpaces = Math.max(Math.floor((parentWidth - this.placeholderStartLength) / 4.4), 0);
    this.placeholder = `${this.placeholderStart} ${' '.repeat(numberOfSpaces)} (Shift + Enter for new line)`;
  }

  writeValue(value: string): void {
    this.promptText = value;
    this.adjustHeight();
  }

  registerOnChange(fn: (value: string) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

}
