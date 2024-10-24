import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse, HTTP_INTERCEPTORS, HttpResponse } from '@angular/common/http';
import { AuthService } from './auth.service';
import { AuthInterceptor } from './auth.interceptor';
import { of, throwError } from 'rxjs';

describe('AuthInterceptor', () => {
  let authServiceSpy: jasmine.SpyObj<AuthService>;
  let interceptor: AuthInterceptor;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    const authServiceSpyObject = jasmine.createSpyObj('AuthService', ['getToken', 'logout']);

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        { provide: AuthService, useValue: authServiceSpyObject },
        { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
      ]
    });

    authServiceSpy = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;
    interceptor = TestBed.inject(HTTP_INTERCEPTORS).find(i => i instanceof AuthInterceptor) as AuthInterceptor;
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();  // Ensure no outstanding requests are pending
  });

  it('should add an Authorization header if token is present', () => {
    const token = 'test-token';
    authServiceSpy.getToken.and.returnValue(token);

    // Trigger HTTP call
    interceptor.intercept(new HttpRequest('GET', '/test'), {
      handle: (req: HttpRequest<any>) => {
        expect(req.headers.get('Authorization')).toBe(`Bearer ${token}`);
        return of(new HttpResponse({ status: 200 })); // Simulate response
      }
    } as HttpHandler).subscribe();

    // Ensure the token was added in headers
    expect(authServiceSpy.getToken).toHaveBeenCalled();
  });

  it('should not add Authorization header if no token is present', () => {
    authServiceSpy.getToken.and.returnValue(null);


    interceptor.intercept(new HttpRequest('GET', '/test'), {
      handle: (req: HttpRequest<any>) => {
        expect(req.headers.has('Authorization')).toBeFalse();
        return of(new HttpResponse({ status: 200 })); // Simulate response
      }
    } as HttpHandler).subscribe();

    // Ensure getToken was called
    expect(authServiceSpy.getToken).toHaveBeenCalled();
  });

  it('should call logout if 401 error is received', () => {
    authServiceSpy.getToken.and.returnValue('test-token');
    const errorResponse = new HttpErrorResponse({ status: 401, statusText: 'Unauthorized' });

    interceptor.intercept(new HttpRequest('GET', '/test'), {
      handle: () => throwError(() => errorResponse) // Simulate 401 error
    } as HttpHandler).subscribe({
      error: (error) => {
        expect(error.status).toBe(401);
      }
    });

    // Ensure logout was called
    expect(authServiceSpy.logout).toHaveBeenCalled();
  });

  it('should not call logout for non-401 errors', () => {
    authServiceSpy.getToken.and.returnValue('test-token');
    const errorResponse = new HttpErrorResponse({ status: 500, statusText: 'Server Error' });

    interceptor.intercept(new HttpRequest('GET', '/test'), {
      handle: () => throwError(() => errorResponse) // Simulate 500 error
    } as HttpHandler).subscribe({
      error: (error) => {
        expect(error.status).toBe(500);
      }
    });

    // Ensure logout was NOT called for 500 error
    expect(authServiceSpy.logout).not.toHaveBeenCalled();
  });
});
