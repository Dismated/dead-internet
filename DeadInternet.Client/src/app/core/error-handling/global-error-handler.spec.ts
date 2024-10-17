import { TestBed } from '@angular/core/testing';
import { GlobalErrorHandler } from './global-error-handler';
import { ErrorService } from './error.service';

describe('GlobalErrorHandler', () => {
  let errorHandler: GlobalErrorHandler;
  let mockErrorService: jasmine.SpyObj<ErrorService>;

  beforeEach(() => {
    const errorServiceSpy = jasmine.createSpyObj('ErrorService', ['setError']);

    TestBed.configureTestingModule({
      providers: [
        GlobalErrorHandler,
        { provide: ErrorService, useValue: errorServiceSpy }
      ]
    });

    errorHandler = TestBed.inject(GlobalErrorHandler);
    mockErrorService = TestBed.inject(ErrorService) as jasmine.SpyObj<ErrorService>;
  });

  it('should call setError on ErrorService and log the error', () => {
    const consoleSpy = spyOn(console, 'error');
    const testError = new Error('Test error');

    errorHandler.handleError(testError);

    expect(mockErrorService.setError).toHaveBeenCalledWith({
      message: 'An unexpected error occurred',
      type: 'error'
    });
    expect(consoleSpy).toHaveBeenCalledWith('Error from global error handler', testError);
  });
});
