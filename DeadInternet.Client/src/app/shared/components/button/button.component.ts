import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-button',
  templateUrl: './button.component.html',
  styleUrl: './button.component.css'
})
export class ButtonComponent implements OnInit {
  @Input() submitType: boolean = false;
  @Input() btnClass: string = '';
  @Input() btnStyles: { [key: string]: string } = {};
  @Input() disabled: boolean = false;

  @Output() clickEvent = new EventEmitter<Event>();

  attrType: 'submit' | 'button' = 'button';

  ngOnInit(): void {
    this.attrType = this.submitType ? 'submit' : 'button';
    console.log(this.btnStyles, this.btnClass);
  }
  onClick(event: Event) {
    if (!this.disabled) {
      this.clickEvent.emit(event);
    }
  }
}
