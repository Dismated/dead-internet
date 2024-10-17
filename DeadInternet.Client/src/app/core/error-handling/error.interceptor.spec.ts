import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { HTTP_INTERCEPTORS, HttpClient } from '@angular/common/http';
import { ErrorInterceptor } from './error.interceptor';
import { ErrorService } from './error.service';

describe('ErrorInterceptor', () => {
  let httpMock: HttpTestingController;
  let httpClient: HttpClient;
  let errorServiceSpy: jasmine.SpyObj<ErrorService>;

  beforeEach(() => {
    const spy = jasmine.createSpyObj('ErrorService', ['setErrorMessage']);

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
        { provide: ErrorService, useValue: spy }
      ]
    });

    httpMock = TestBed.inject(HttpTestingController);
    httpClient = TestBed.inject(HttpClient);
    errorServiceSpy = TestBed.inject(ErrorService) as jasmine.SpyObj<ErrorService>;
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should handle client-side error and call setErrorMessage with correct message', () => {
    const mockErrorEvent = new ErrorEvent('Network error', {
      message: 'Client-side error',
    });

    httpClient.get('/test').subscribe({
      next: () => fail('should have failed with the client-side error'),
      error: () => {
        expect(errorServiceSpy.setErrorMessage).toHaveBeenCalledWith('Error: Client-side error');
      }
    });

    const req = httpMock.expectOne('/test');
    req.error(mockErrorEvent);
  });

  it('should handle server-side error and call setErrorMessage with correct message', () => {

    httpClient.get('/test').subscribe({
      next: () => fail('should have failed with the server-side error'),
      error: () => {
        expect(errorServiceSpy.setErrorMessage).toHaveBeenCalledWith('Error Code: 500,  Message: Http failure response for /test: 500 Internal Server Error');
      }
    });

    const req = httpMock.expectOne('/test');
    req.flush({ message: 'Server-side error' }, { status: 500, statusText: 'Internal Server Error' });
  });
});
