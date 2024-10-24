import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Router, NavigationStart } from '@angular/router';
import { Subject } from 'rxjs';
import { ErrorMessageComponent } from './error-display.component';
import { ErrorService } from './error.service';

describe('ErrorMessageComponent', () => {
  let component: ErrorMessageComponent;
  let fixture: ComponentFixture<ErrorMessageComponent>;
  let errorService: jasmine.SpyObj<ErrorService>;
  let errorSubject: Subject<{ message: string; type: string } | null>;
  let routerEventsSubject: Subject<any>;

  beforeEach(async () => {
    errorSubject = new Subject<{ message: string; type: string } | null>();
    routerEventsSubject = new Subject<any>();

    const errorServiceSpy = jasmine.createSpyObj('ErrorService', ['clearErrorMessage'], {
      errorMessage$: errorSubject.asObservable()
    });
    const routerSpy = jasmine.createSpyObj('Router', [], {
      events: routerEventsSubject.asObservable()
    });

    await TestBed.configureTestingModule({
      declarations: [ErrorMessageComponent],
      providers: [
        { provide: ErrorService, useValue: errorServiceSpy },
        { provide: Router, useValue: routerSpy }
      ]
    }).compileComponents();

    errorService = TestBed.inject(ErrorService) as jasmine.SpyObj<ErrorService>;
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ErrorMessageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should update errorMessage and errorType when errorService emits an error', () => {
    const errorData = { message: 'Test error', type: 'error' };
    errorSubject.next(errorData);
    expect(component.errorMessage).toBe('Test error');
    expect(component.errorType).toBe('error');
  });

  it('should clear errorMessage and errorType when errorService emits null', () => {
    component.errorMessage = 'Existing error';
    component.errorType = 'error';
    errorSubject.next(null);
    expect(component.errorMessage).toBeNull();
    expect(component.errorType).toBeNull();
  });

  it('should call errorService.clearErrorMessage when closeError is called', () => {
    component.closeError();
    expect(errorService.clearErrorMessage).toHaveBeenCalled();
  });

  it('should call errorService.clearErrorMessage on NavigationStart event', () => {
    routerEventsSubject.next(new NavigationStart(0, 'test'));
    expect(errorService.clearErrorMessage).toHaveBeenCalled();
  });
});
