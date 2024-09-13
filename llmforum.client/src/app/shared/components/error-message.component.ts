import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-error-message',
  template: `
    <div *ngIf="message" class="alert alert-danger" role="alert">
      <strong>Error!</strong> {{ message }}
    </div>
  `,
  styles: []
})
export class ErrorMessageComponent {
  @Input() message: string = '';
}
