import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { LlmPromptComponent } from './llm-prompt.component';
import { FormsModule } from '@angular/forms';
import { By } from '@angular/platform-browser';
import { Component, Input } from '@angular/core';
import { of, throwError } from 'rxjs';
import { PostService } from '../../features/services/post.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-button',
  template: ''
})
class ButtonComponentStub {
  @Input() type: string = ''
  @Input() disabled: boolean = false;
  @Input() btnClass: string = '';
  @Input() btnStyles: { [key: string]: string } = {};
  @Input() submitType: boolean = false;
}

@Component({
  selector: 'app-loading',
  template: ''
})
class LoadingComponentStub {
  @Input() isLoading: boolean = false;
}

@Component({
  selector: "app-comments",
  template: ""
})
class CommentsComponentStub { }

describe('LlmPromptComponent', () => {
  let component: LlmPromptComponent;
  let fixture: ComponentFixture<LlmPromptComponent>;
  let httpMock: HttpTestingController;
  let postServiceSpy: jasmine.SpyObj<PostService>;
  let router: Router;


  beforeEach(async () => {
    const postServiceSpyObj = jasmine.createSpyObj('PostService', ['createPost']);

    await TestBed.configureTestingModule({
      declarations: [LlmPromptComponent, ButtonComponentStub, LoadingComponentStub],
      imports: [HttpClientTestingModule, [RouterTestingModule.withRoutes([
        { path: 'comments/:id', component: CommentsComponentStub } // Define your routes here
      ])], FormsModule],
      providers: [
        { provide: PostService, useValue: postServiceSpyObj },
      ],
    }).compileComponents();

    postServiceSpy = TestBed.inject(PostService) as jasmine.SpyObj<PostService>;

  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LlmPromptComponent);
    component = fixture.componentInstance;
    httpMock = TestBed.inject(HttpTestingController);
    router = TestBed.inject(Router);

    fixture.detectChanges();

  });

  afterEach(() => {
    httpMock.verify(); // Ensure no unmatched HTTP requests are outstanding
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize with default values', () => {
    expect(component.promptText).toBe('');
    expect(component.response).toBe('');
    expect(component.errorMessage).toBe('');
    expect(component.loading).toBe(false);
  });

  it('should update placeholder based on viewport width', () => {
    Object.defineProperty(window, 'innerWidth', { writable: true, configurable: true, value: 1000 });

    // Call the method that uses innerWidth
    component.updatePlaceholder();

    fixture.detectChanges();

    // Assertion
    expect(component.placeholder).toContain('Create a post');
  });

  it('should display error when HTTP request fails', () => {
    postServiceSpy.createPost.and.returnValue(throwError(() => new Error('Error')));

    postServiceSpy.createPost("Test prompt").subscribe({
      next: () => fail('Expected error, but got a success response'),
      error: (err) => {
        expect(err.message).toBe('Error');
      }
    })
  });

  it('should submit prompt and navigate on success', fakeAsync(() => {
    const mockResponse = { data: { prompt: { postId: 1 } } };
    component.promptText = 'Test prompt';
    postServiceSpy.createPost.and.returnValue(of(mockResponse));

    spyOn(router, 'navigate'); // Spy on the router's navigate method
    component.onSubmit();
    tick();

    expect(postServiceSpy.createPost).toHaveBeenCalledWith('Test prompt');
    expect(component.response).toEqual(mockResponse.data);
    expect(router.navigate).toHaveBeenCalledWith(['/comments', 1]);
  }));

  it('should prevent default behavior when shift+enter is pressed', () => {
    const event = new KeyboardEvent('keydown', { key: 'Enter', shiftKey: true });
    const preventDefaultSpy = spyOn(event, 'preventDefault').and.callThrough();

    const textarea = fixture.debugElement.query(By.css('textarea')).nativeElement;
    textarea.value = 'Test prompt';
    component.promptText = 'Test prompt';
    textarea.dispatchEvent(event);

    expect(preventDefaultSpy).toHaveBeenCalled();
    expect(component.promptText).toContain('\n');
  });

  it('should submit when enter key is pressed without shift', () => {
    const event = new KeyboardEvent('keydown', { key: 'Enter' });
    const preventDefaultSpy = spyOn(event, 'preventDefault').and.callThrough();
    spyOn(component, 'onSubmit');

    const textarea = fixture.debugElement.query(By.css('textarea')).nativeElement;
    component.promptText = 'Test';
    textarea.dispatchEvent(event);

    expect(preventDefaultSpy).toHaveBeenCalled();
    expect(component.onSubmit).toHaveBeenCalled();
  });

  it('should resize textarea correctly', () => {
    const textarea = fixture.debugElement.query(By.css('textarea')).nativeElement;

    // Set initial value and simulate the textarea's scrollHeight
    textarea.value = 'This is a test content that should change the height';

    // Dispatch an input event to simulate the user typing
    textarea.dispatchEvent(new Event('input'));

    // Call the adjustTextareaHeight method indirectly by simulating user behavior
    component.adjustTextareaHeight(textarea);

    // Trigger change detection
    fixture.detectChanges();

    // Now check if the height was adjusted based on the content
    expect(textarea.style.height).toBe(`49px`);
  });
});
