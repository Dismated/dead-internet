import { TestBed } from '@angular/core/testing';
import { ErrorService, ErrorMessage } from './error.service';
import { skip } from 'rxjs/operators';

describe('ErrorService', () => {
  let service: ErrorService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ErrorService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should set and get error message', (done) => {
    const errorMessage: ErrorMessage = { message: 'Test error', type: 'error' };
    service.setError(errorMessage);
    service.getError().subscribe((error) => {
      expect(error).toEqual(errorMessage);
      done();
    });
  });

  it('should set error message with default type', (done) => {
    service.setErrorMessage('Test error');
    service.getError().subscribe((error) => {
      expect(error).toEqual({ message: 'Test error', type: 'error', details: undefined });
      done();
    });
  });

  it('should set error message with custom type and details', (done) => {
    service.setErrorMessage('Test warning', 'warning', 'Additional details');
    service.getError().subscribe((error) => {
      expect(error).toEqual({ message: 'Test warning', type: 'warning', details: 'Additional details' });
      done();
    });
  });

  it('should clear error message', (done) => {
    service.setErrorMessage('Test error');
    service.clearErrorMessage();
    service.getError().subscribe((error) => {
      expect(error).toBeNull();
      done();
    });
  });

  it('should emit new error messages', (done) => {
    let emissionCount = 0;
    service.errorMessage$.pipe(skip(1)).subscribe((error) => {
      emissionCount++;
      if (emissionCount === 1) {
        expect(error).toEqual({ message: 'First error', type: 'error', details: undefined });
      } else if (emissionCount === 2) {
        expect(error).toEqual({ message: 'Second error', type: 'warning', details: undefined });
        done();
      }
    });

    service.setErrorMessage('First error');
    service.setErrorMessage('Second error', 'warning');
  });
});
