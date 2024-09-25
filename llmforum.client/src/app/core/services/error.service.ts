import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

export interface ErrorMessage {
  message: string;
  type: 'error' | 'warning' | 'info';
  details?: string;
}

@Injectable({
  providedIn: 'root'
})
export class ErrorService {
  private errorSubject = new BehaviorSubject<ErrorMessage | null>(null);
  errorMessage$ = this.errorSubject.asObservable();


  setError(error: ErrorMessage): void {
    this.errorSubject.next(error);
  }

  setErrorMessage(message: string, type: 'error' | 'warning' | 'info' = 'error', details?: string): void {
    this.setError({ message, type, details });
  }

  clearErrorMessage(): void {
    this.errorSubject.next(null);
  }

  getError(): Observable<ErrorMessage | null> {
    return this.errorSubject.asObservable();
  }

  handleHttpError(error: any): void {
    let errorMessage: string;
    let errorDetails: string | undefined;

    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = `Client Error: ${error.error.message}`;
    } else {
      // Server-side error
      errorMessage = `Server Error: ${error.status} ${error.statusText}`;
      errorDetails = error.error?.message || error.message || 'Unknown error';
    }

    this.setError({ message: errorMessage, type: 'error', details: errorDetails });
  }

  logError(error: any): void {
    console.error('Error logged:', error);
  }
}
