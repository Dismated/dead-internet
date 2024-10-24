import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { LlmPromptComponent } from './llm-prompt.component';
import { FormsModule } from '@angular/forms';
import { By } from '@angular/platform-browser';
import { Component, ElementRef, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { of, throwError } from 'rxjs';
import { PostService } from '../../features/services/post.service';
import { Router } from '@angular/router';
import { AdjustableTextareaComponent } from '../../shared/components/adjustable-textarea/adjustable-textarea.component';

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

@Component({
  selector: "app-adjustable-textarea",
  template: ""
})
class AdjustableTextareaComponentStub {
  @ViewChild('textarea') textareaRef!: ElementRef;
  @Input() placeholderStart = '';
  @Input() promptText = '';
  @Input() textareaStyles: { [key: string]: string } = {};
  @Output() textSubmit = new EventEmitter<void>();
}

describe('LlmPromptComponent', () => {
  let component: LlmPromptComponent;
  let fixture: ComponentFixture<LlmPromptComponent>;
  let httpMock: HttpTestingController;
  let postServiceSpy: jasmine.SpyObj<PostService>;
  let router: Router;
  let debugEl: any;


  beforeEach(async () => {
    const postServiceSpyObj = jasmine.createSpyObj('PostService', ['createPost']);

    await TestBed.configureTestingModule({
      declarations: [LlmPromptComponent, ButtonComponentStub, LoadingComponentStub, AdjustableTextareaComponentStub],
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

    debugEl = fixture.debugElement

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

    expect(postServiceSpy.createPost).toHaveBeenCalledWith({ prompt: 'Test prompt' });
    expect(component.response).toEqual(mockResponse.data);
    expect(router.navigate).toHaveBeenCalledWith(['/comments', 1]);
  }));

});
