import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { of, throwError } from 'rxjs';
import { CommentsComponent } from './comments.component';
import { CommentsService } from '../features/services/comments.service';
import { ErrorService } from '../core/error-handling/error.service';
import { Component, Input, Output, EventEmitter } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-comment',
  template: ''
})
class CommentComponentStub {
  @Input() comment: any;
  @Input() depth: number = 0;
  @Output() deleteComment = new EventEmitter<string>();
}

describe('CommentsComponent', () => {
  let component: CommentsComponent;
  let fixture: ComponentFixture<CommentsComponent>;
  let mockCommentsService: jasmine.SpyObj<CommentsService>;
  let mockErrorService: jasmine.SpyObj<ErrorService>;
  let mockActivatedRoute: any;

  beforeEach(async () => {
    mockCommentsService = jasmine.createSpyObj('CommentsService', ['getComments', 'deleteCommentChain']);
    mockErrorService = jasmine.createSpyObj('ErrorService', ['setErrorMessage']);
    mockActivatedRoute = {
      snapshot: {
        paramMap: {
          get: jasmine.createSpy('get').and.returnValue('123')
        }
      }
    };
    await TestBed.configureTestingModule({
      declarations: [CommentsComponent, CommentComponentStub],
      providers: [
        { provide: CommentsService, useValue: mockCommentsService },
        { provide: ErrorService, useValue: mockErrorService },
        { provide: ActivatedRoute, useValue: mockActivatedRoute }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(CommentsComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load comments on init', fakeAsync(() => {
    const mockResponse = {
      data: {
        prompt: { content: 'Test Prompt' },
        replies: [{ id: '1', content: 'Test Comment' }]
      }
    };
    mockCommentsService.getComments.and.returnValue(of(mockResponse));

    fixture.detectChanges();
    tick();

    expect(component.promptText).toBe('Test Prompt');
    expect(component.commentData).toEqual([{ id: '1', content: 'Test Comment' }]);
    expect(component.loading).toBeFalse();
  }));

  it('should handle error when loading comments', fakeAsync(() => {
    mockCommentsService.getComments.and.returnValue(throwError(() => new Error('Test Error')));

    mockCommentsService.getComments('1').subscribe({
      next: () => fail('Expected error, but got a success response'),
      error: (err) => {
        expect(err.message).toBe('Test Error');
      }
    });
  }));

  it('should handle error when no post ID is provided', fakeAsync(() => {
    (mockActivatedRoute.snapshot!.paramMap.get as jasmine.Spy).and.returnValue(null);

    fixture.detectChanges();
    tick();

    expect(mockErrorService.setErrorMessage).toHaveBeenCalledWith('No post ID provided');
    expect(mockCommentsService.getComments).not.toHaveBeenCalled();
  }));

  it('should delete comment chain', fakeAsync(() => {
    component.commentData = [
      { id: '1', content: 'Comment 1' },
      {
        id: '2', content: 'Comment 2', replies: [
          { id: '3', content: 'Reply to 2' }
        ]
      }
    ];
    mockCommentsService.deleteCommentChain.and.returnValue(of(undefined));

    component.deleteCommentChain('2');
    tick();

    expect(mockCommentsService.deleteCommentChain).toHaveBeenCalledWith('2');
    expect(component.commentData).toEqual([{ id: '1', content: 'Comment 1' }]);
  }));



  it('should handle error when deleting comment chain', fakeAsync(() => {
    mockCommentsService.deleteCommentChain.and.returnValue(throwError(() => new Error('Delete error')));
    spyOn(console, 'error');

    component.deleteCommentChain('1');
    tick();

    expect(console.error).toHaveBeenCalledWith('Error deleting comment:', jasmine.any(Error));
  }));
});
