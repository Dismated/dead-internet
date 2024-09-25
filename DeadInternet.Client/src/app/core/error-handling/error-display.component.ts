import { Component, OnInit } from '@angular/core';
import { ErrorService } from './error.service';
import { NavigationStart, Router } from '@angular/router';

@Component({
  selector: 'app-error-message',
  template: `
    <div *ngIf="errorMessage" class="alert alert-danger" role="alert">
      <strong>Error!</strong> {{ errorMessage }}
      <button (click)="closeError()">Close</button>
    </div>
  `,
  styles: []
})
export class ErrorMessageComponent implements OnInit {
  errorMessage: string | null = null;
  errorType: string | null = null;

  constructor(private errorService: ErrorService, private router: Router) { }

  ngOnInit() {
    this.errorService.errorMessage$.subscribe((errorData) => {
      if (errorData) {
        this.errorMessage = errorData.message;
        this.errorType = errorData.type;
      } else {
        this.errorMessage = null;
        this.errorType = null;
      }
    });

    this.router.events.subscribe((event) => {
      if (event instanceof NavigationStart) {
        this.errorService.clearErrorMessage();
      }
    });
  }

  closeError() {
    this.errorService.clearErrorMessage();
  }
}
