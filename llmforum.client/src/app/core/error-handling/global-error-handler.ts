import { ErrorHandler, Injectable } from '@angular/core';
import { ErrorService } from '../services/error.service';

@Injectable()
export class GlobalErrorHandler implements ErrorHandler {
  constructor(private errorService: ErrorService) { }

  handleError(error: Error) {
    this.errorService.setError({ message: 'An unexpected error occurred', type: 'error' });
    console.error('Error from global error handler', error);
  }
}
