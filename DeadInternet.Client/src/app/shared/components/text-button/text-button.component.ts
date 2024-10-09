import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-text-button',
  templateUrl: './text-button.component.html',
  styleUrl: './text-button.component.css'
})
export class TextButtonComponent {
  @Input() btnClass: string = '';
  @Input() btnStyles: { [key: string]: any } = {};
  @Input() disabled: boolean = false;

  @Output() clickEvent = new EventEmitter<Event>();

  onClick(event: Event) {
    if (!this.disabled) {
      this.clickEvent.emit(event);
    }
  }
}
